using StockRanking.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StockRanking
{
    public class MailingGenerator
    {
        HtmlHelper htmlHelper = new HtmlHelper();
        HtmlHelper htmlHelperMonthly = new HtmlHelper();
        int dateToProcess = 0;
        List<MailingParameters> mailsToSend = new List<MailingParameters>();
        String ProcessError = "";
        
        public void ProcessMails(int idStrategy, ProcessingPanelControl pnlProcessing)
        {
            ZachsSourceReader.CancelProcess = false;
            StockSourcesReader.CancelProcess = false;
            StrategyView.CancelResultsProcessing = false;
            BacktestCalculator.CancelProcess = false;
            ProcessError = "";

            pnlProcessing.StartProcess();
            
            if (Stock.CachedStocks == null)
            {
                Stock.ReadStockSymbolsThreads(true, pnlProcessing);
            }

            var lastSpyPriceMonthly = Stock.GetLastMonthlyPriceDate(-1);
            dateToProcess = Stock.GetLastPriceDate(-1);

            List<Strategy> strategies = Strategy.GetStrategies();
            foreach(var strategy in strategies)
            {
                try
                {
                    if(BacktestCalculator.CancelProcess)
                    {
                        pnlProcessing.StopProcess();
                        return;
                    }

                    htmlHelper = new HtmlHelper();

                    if (idStrategy > 0 && strategy.Id != idStrategy)
                        continue;

                    var mailingParams = MailingParameters.Load(strategy.Id);
                    int date = 0;

                    if (Utils.ConvertDateTimeToInt(mailingParams.LastMailSent) == dateToProcess)
                        continue;

                    if (mailingParams.Destinations.Count == 0)
                        continue;

                    if (!mailingParams.SendRebalance && !mailingParams.SendSells && mailingParams.SendWeekdays == "")
                        continue;

                    //check rebalance and weekdays if SendSells is deactivated
                    bool sendRebalance = false;
                    if (!mailingParams.SendSells)
                    {
                        if (Utils.ConvertIntToDateTime(lastSpyPriceMonthly).Month != mailingParams.LastMailSent.Month)
                        {
                            sendRebalance = true;
                        }
                    }

                    bool sendWeekday = false;
                    if (mailingParams.SendWeekdays.Contains(getDayOfWeek()))
                    {
                        sendWeekday = true;
                    }

                    if (!mailingParams.SendSells && !sendWeekday && !sendRebalance)
                        continue;

                    List<FeatureWeight> featuresW = Feature.generateFeatureWeightsList(strategy);
                    BacktestCalculator backtest = new BacktestCalculator(strategy.PortfolioParameters, featuresW, strategy.Filters);

                    BacktestResult outCash = new BacktestResult();
                    backtest.RunBacktest(pnlProcessing, new DateTime(2005, 01, 01), out outCash);

                    var tradesToSend = backtest.GetTradesForDate(dateToProcess);

                    if (!sendWeekday && !sendRebalance)
                    {
                        //send only if there are trades
                        if (tradesToSend.Count() == 0)
                            continue;
                    }

                    if (backtest.IsRebalanceDate(dateToProcess))
                    {
                        composeRebalanceTables(tradesToSend, strategy, backtest.LastSnapshot.TotalPortfolioValue);
                    }
                    else
                    {
                        composeBuySellTables(tradesToSend, strategy, backtest.LastSnapshot.TotalPortfolioValue);
                    }

                    composePortfolioTable(backtest.LastDayPositions, strategy, backtest.GetLastRebalanceDate());

                    composeWatchlistTable(backtest, dateToProcess, mailingParams.IncludeTopStocks);

                    composeBondsModel(backtest);

                    composeRiskTable(backtest);

                    composePerformanceTable(backtest);

                    mailingParams.MailText = htmlHelper.GetString();
                    mailingParams.StrategyName = strategy.Name;
                    
                    mailsToSend.Add(mailingParams);
                }
                catch (Exception e)
                {
                    ProcessError += "\n\nError generating alert Mail: " + e.Message + " - " + e.StackTrace;
                }
            }

            var mailsSent = sendMails();

            foreach (var mail in mailsSent)
                mail.SaveMailSent(dateToProcess);

            if (ProcessError != "")
                pnlProcessing.ShowError(ProcessError);
            else
                pnlProcessing.StopProcess();
        }

        List<MailingParameters> sendMails()
        {
            var mailsSent = new List<MailingParameters>();

            if (mailsToSend.Count == 0)
                return mailsSent;

            var mailConfig = MailConfiguration.Load();
            if (mailConfig == null)
                return mailsSent;

            foreach (var mailParams in mailsToSend)
            {
                String error = SendMail(mailConfig, String.Join(";", mailParams.Destinations), "Factor Analysis Alert - " + DateTime.Now.ToString("MM-dd-yyyy") + " - " + mailParams.StrategyName, mailParams.MailText);
                if(error != "")
                    ProcessError += "\n\nError Sending alert Mail: " + error;
                else
                    mailsSent.Add(mailParams);
            }

            return mailsSent;
        }

        String getDayOfWeek()
        {
            switch(DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "1";
                case DayOfWeek.Tuesday:
                    return "2";
                case DayOfWeek.Wednesday:
                    return "3";
                case DayOfWeek.Thursday:
                    return "4";
                case DayOfWeek.Friday:
                    return "5";
                case DayOfWeek.Saturday:
                    return "6";
                case DayOfWeek.Sunday:
                    return "7";
            }

            return "-";
        }

        void composePortfolioTable(List<Position> positions, Strategy strategy, int lastRebalanceDate)
        {
            if(positions.Count == 0)
            {
                htmlHelper.AddText("Portfolio is currently empty. " + strategy.Name + " : Date " + Utils.ConvertDateIntToString(dateToProcess));
                return;
            }

            htmlHelper.AddText("Portfolio: " + strategy.Name + " : Date " + Utils.ConvertDateIntToString(dateToProcess));
            htmlHelper.AddText("Model was Rebalanced on " + Utils.ConvertDateIntToString(lastRebalanceDate));

            htmlHelper.OpenTable(8, new List<string>() { "Symbol", "Entry Date", "Entry Price", "Last Price", "% Return", "Stop Price", "% from Stop", "PORT%" }, "Portfolio");
            foreach (var position in positions)
            {
                position.AuxCurrentPrice = Stock.GetLastPrice(position.IdStock);
                
                htmlHelper.OpenRow();
                htmlHelper.AddCell(Stock.GetStock(position.IdStock).Symbol, true);
                htmlHelper.AddCell(Utils.FormatDateTimeToString(position.AuxDateEntered), true);
                htmlHelper.AddCell(position.EntryPrice.ToString("n2"));
                htmlHelper.AddCell(position.AuxCurrentPrice.ToString("n2"));
                htmlHelper.AddCell(position.AuxCurrentPrice == 0 ? "0&nbsp;%" : (((position.EntryPrice - position.AuxCurrentPrice) / position.AuxCurrentPrice) * 100).ToString("n2") + "&nbsp;%");
                htmlHelper.AddCell(position.StopLossPrice.ToString("n2"));
                htmlHelper.AddCell(position.AuxPercAboveStop.ToString("n2") + "&nbsp;%");
                htmlHelper.AddCell(position.AuxPortGain.ToString("n2") + "&nbsp;%");
                htmlHelper.CloseRow();
            }
            htmlHelper.CloseTable();
        }

        void composeWatchlistTable(BacktestCalculator backtest, int lastRebalanceDate, int items)
        {
            if (items == 0)
                return;

            Dictionary<int, RankingResult> stocksRanking = backtest.generateStocksCurrentValues(new DateTime(Utils.ConvertIntToDateTime(lastRebalanceDate).Year, Utils.ConvertIntToDateTime(lastRebalanceDate).Month, 1));

            List<String> columns = new List<string>();
            columns.Add("Position");
            columns.Add("Symbol");
            columns.Add("Sector");
            columns.Add("Closing Price");
            columns.Add("Ytd");
            columns.Add("Volume");
            columns.Add("% Undervalued");
            columns.Add("Total Score");

            int iPosition = 0;
            htmlHelper.OpenTable(columns.Count, columns, "Model Rank Watchlist");

            var rankings = stocksRanking.Values.ToList();
            rankings.Sort(delegate (RankingResult x, RankingResult y)
            {
                return y.RankPosition.CompareTo(x.RankPosition);
            });

            foreach (var rankingResults in rankings)
            {
                iPosition++;
                if (iPosition > items)
                    break;

                Stock stock = Stock.GetStock(rankingResults.IdStock);

                htmlHelper.OpenRow();
                htmlHelper.AddCell(iPosition.ToString());
                htmlHelper.AddCell(stock.Symbol, true);
                htmlHelper.AddCell(stock.getStockSector(), true);
                htmlHelper.AddCell(stock.CurrentPrice.ToString("n2"), false);
                htmlHelper.AddCell(stock.CurrentFilters.GetYTD(stock.CurrentPrice).ToString("n2") + "&nbsp;%", false);
                htmlHelper.AddCell(stock.CurrentVolume.ToString("n2"), false);
                /*
                for(int featureIndex = 0; featureIndex < backtest.FeatureWeights.Count(); featureIndex++)
                {
                    if (backtest.FeatureWeights[featureIndex].IsEnabled)
                    {
                        htmlHelper.AddCell(stock.Features[featureIndex].RankedValue.ToString("n4"));
                    }
                }
                */
                htmlHelper.AddCell((-1*stock.CurrentFilters.GetCompositeForPrice(backtest.Portfolio, (double)stock.CurrentPrice)).ToString("n2") + "%");
                htmlHelper.AddCell(rankingResults.TotalWeight.ToString("n2"));
                htmlHelper.CloseRow();
            }

            htmlHelper.CloseTable();
        }

        void composeRebalanceTables(List<Trade> trades, Strategy strategy, decimal currentPortfolio)
        {
            htmlHelper.AddText(Utils.ConvertDateIntToString(dateToProcess) + " Portfolio Update: **Rebalance Notification");
            htmlHelper.AddText("This Weeks Portfolio Changes");
            var nextRebalance = Utils.ConvertIntToDateTime(dateToProcess).AddMonths(strategy.PortfolioParameters.RebalanceFrequencyMonths);
            nextRebalance = new DateTime(nextRebalance.Year, nextRebalance.Month, 1);
            htmlHelper.AddText("Next rebalance Date: " + nextRebalance.ToString("MM/dd/yyyy"));

            var buyTrades = trades.Where(x => x.RealEntryDate == dateToProcess);

            htmlHelper.OpenTable(9, new List<string>() { "Symbol", "Entry Date", "Entry Price", "Sell Date", "Sell Price", "Return/Loss", "Shares", "%Return Loss", "Note" }, "Sells");
            foreach (var trade in trades.Where(x => x.RealExitDate == dateToProcess && x.RealExitDate != x.RealEntryDate))
            {
                htmlHelper.OpenRow();
                htmlHelper.AddCell(trade.Symbol, true);
                htmlHelper.AddCell(Utils.ConvertDateIntToString(trade.RealEntryDate), true);
                htmlHelper.AddCell(trade.EntryPrice.ToString("n2"));
                htmlHelper.AddCell(Utils.ConvertDateIntToString(trade.RealExitDate));
                htmlHelper.AddCell(trade.ExitPrice.ToString("n2"));
                htmlHelper.AddCell((trade.EntryPrice - trade.ExitPrice).ToString("n2"));
                htmlHelper.AddCell(trade.Shares.ToString("n0"));
                htmlHelper.AddCell((((trade.EntryPrice - trade.ExitPrice) / trade.EntryPrice)*100).ToString("n2") + "&nbsp;%");
                var checkBuyTable = "";
                if (buyTrades.Any(x => x.Symbol == trade.Symbol))
                    checkBuyTable = " - Check Buy Table";

                htmlHelper.AddCell("Model Sell" + checkBuyTable, true);
                htmlHelper.CloseRow();
            }
            htmlHelper.CloseTable();
            
            htmlHelper.OpenTable(6, new List<string>() { "Symbol", "Buy On Close", "Sector", "Note", "Shares", "Portfolio %" }, "Buys");
            foreach (var trade in buyTrades)
            {
                htmlHelper.OpenRow();
                htmlHelper.AddCell(trade.Symbol, true);
                htmlHelper.AddCell(Utils.ConvertDateIntToString(trade.RealEntryDate), true);
                htmlHelper.AddCell(Stock.GetStock(trade.IdStock).getStockSector(), true);
                htmlHelper.AddCell("Model Buy", true);
                htmlHelper.AddCell(trade.Shares.ToString("n0"), false);
                if(currentPortfolio == 0)
                    htmlHelper.AddCell("", false);
                else
                    htmlHelper.AddCell((trade.EntryPrice * trade.Shares / currentPortfolio * 100).ToString("n2") + "&nbsp;%", false);
                htmlHelper.CloseRow();
            }
            htmlHelper.CloseTable();
        }

        void composeBuySellTables(List<Trade> trades, Strategy strategy, decimal currentPortfolio)
        {
            htmlHelper.AddText(Utils.ConvertDateIntToString(dateToProcess) + " Portfolio Update: BUY/SELL ");
            htmlHelper.AddText("Portfolio BUY/SELL in between rebalancing dates");

            var buyTrades = trades.Where(x => x.RealEntryDate == dateToProcess);
            var sellTrades = trades.Where(x => x.RealExitDate == dateToProcess);

            if (sellTrades.Count() > 0)
            {
                htmlHelper.OpenTable(9, new List<string>() { "Symbol", "Entry Date", "Entry Price", "Sell Date", "Sell Price", "Shares", "Return/Loss", "%Return Loss", "Note" }, "Sells");
                foreach (var trade in trades.Where(x => x.RealExitDate == dateToProcess))
                {
                    htmlHelper.OpenRow();
                    htmlHelper.AddCell(trade.Symbol, true);
                    htmlHelper.AddCell(Utils.ConvertDateIntToString(trade.RealEntryDate), true);
                    htmlHelper.AddCell(trade.EntryPrice.ToString("n2"));
                    htmlHelper.AddCell(Utils.ConvertDateIntToString(trade.RealExitDate));
                    htmlHelper.AddCell(trade.ExitPrice.ToString("n2"));
                    htmlHelper.AddCell((trade.EntryPrice - trade.ExitPrice).ToString("n2"));
                    htmlHelper.AddCell(trade.Shares.ToString("n0"));
                    htmlHelper.AddCell((((trade.EntryPrice - trade.ExitPrice) / trade.EntryPrice) * 100).ToString("n2") + "&nbsp;%");
                    var checkBuyTable = "";
                    if (buyTrades.Any(x => x.Symbol == trade.Symbol))
                        checkBuyTable = " - Check Buy Table";

                    if (trade.StopLossSell)
                        htmlHelper.AddCell("Stop Loss Sell" + checkBuyTable, true);
                    else
                        htmlHelper.AddCell("Risk Model Sell" + checkBuyTable, true);
                    htmlHelper.CloseRow();
                }
                htmlHelper.CloseTable();
            }

            if (buyTrades.Count() > 0)
            {
                htmlHelper.OpenTable(5, new List<string>() { "Symbol", "Buy On Close", "Sector", "Note", "Shares", "Portfolio %" }, "Buys");
                foreach (var trade in buyTrades)
                {
                    htmlHelper.OpenRow();
                    htmlHelper.AddCell(trade.Symbol, true);
                    htmlHelper.AddCell(Utils.ConvertDateIntToString(trade.RealEntryDate), true);
                    htmlHelper.AddCell(Stock.GetStock(trade.IdStock).getStockSector(), true);
                    htmlHelper.AddCell("Buy Replacing Stop Loss", true);
                    htmlHelper.AddCell(trade.Shares.ToString("n0"), false);
                    if (currentPortfolio == 0)
                        htmlHelper.AddCell("", false);
                    else
                        htmlHelper.AddCell((trade.EntryPrice * trade.Shares / currentPortfolio * 100).ToString("n2") + "&nbsp;%", false);
                    htmlHelper.CloseRow();
                }
                htmlHelper.CloseTable();
            }
        }

        void composeBondsModel(BacktestCalculator backtest)
        {
            var lastSell = backtest.BenchmarksCalculator.BondsModelBenchmark.LastSell;
            var currentAsset = backtest.BenchmarksCalculator.BondsModelBenchmark.GetCurrentAsset();
            var buyDate = backtest.BenchmarksCalculator.BondsModelBenchmark.LastBuyPositionDate;

            String pos = "CASH";
            if (currentAsset != null)
                pos = currentAsset.StockSymbol;

            String buyDateStr = "";
            if (buyDate != 0)
                buyDateStr = " (since " + Utils.ConvertDateIntToString(buyDate) + ")";

            htmlHelper.OpenTable(4, new List<string>() { "Action", "Symbol", "Sell Date", "Gain/Loss&nbsp;%" }, "Bonds Model: " + pos + buyDateStr);

            if (lastSell != null)
            {
                if (lastSell.RealExitDate == dateToProcess)
                {
                    htmlHelper.OpenRow();
                    htmlHelper.AddCell("Sell", true);
                    htmlHelper.AddCell(lastSell.Symbol, true);
                    htmlHelper.AddCell(Utils.ConvertDateIntToString(lastSell.RealExitDate), true);
                    htmlHelper.AddCell(lastSell.EntryPrice == 0 ? "0" : ((lastSell.ExitPrice - lastSell.EntryPrice) / lastSell.EntryPrice * 100).ToString("n2") + "&nbsp;%");
                    htmlHelper.CloseRow();
                }
            }

            if (buyDate == dateToProcess)
            {
                htmlHelper.OpenRow();
                if (currentAsset == null)
                {
                    htmlHelper.AddCell("CASH", true);
                    htmlHelper.AddCell("", true);
                    htmlHelper.AddCell("");
                    htmlHelper.AddCell("");
                }
                else
                {
                    htmlHelper.AddCell("BUY", true);
                    htmlHelper.AddCell(currentAsset.StockSymbol, true);
                    htmlHelper.AddCell("");
                    htmlHelper.AddCell("");
                }
                htmlHelper.CloseRow();
            }
            else
            {
                if (currentAsset == null)
                {
                    htmlHelper.OpenRow();
                    htmlHelper.AddCell("CASH", true);
                    htmlHelper.AddCell("CASH", true);
                    htmlHelper.AddCell("", true);
                    htmlHelper.AddCell("", true);
                    htmlHelper.CloseRow();
                }
                else
                {
                    htmlHelper.OpenRow();
                    htmlHelper.AddCell("Hold", true);
                    htmlHelper.AddCell(currentAsset.StockSymbol, true);
                    htmlHelper.AddCell("", true);
                    var buyPrice = currentAsset.getPrice(buyDate);
                    var currPrice = currentAsset.getPrice(dateToProcess);
                    htmlHelper.AddCell(buyPrice == 0 ? "0" : ((currPrice - buyPrice) / buyPrice * 100).ToString("n2") + "&nbsp;%");
                    htmlHelper.CloseRow();
                }
            }

            htmlHelper.CloseTable();
        }

        void composeRiskTable(BacktestCalculator backtestCalculator)
        {
            htmlHelper.OpenTable(4, new List<string>() { "Model", "Status", "Components" }, "Market Risk & Hedge Models: " + Utils.ConvertDateIntToString(dateToProcess));

            decimal avg50, avg200;
            bool spyRes = backtestCalculator.checkSPYAvgSignal(Utils.ConvertIntToDateTime(dateToProcess), out avg50, out avg200);

            decimal hiloValue;
            bool hiloRes = backtestCalculator.checkHiLoSignal(Utils.ConvertIntToDateTime(dateToProcess), out hiloValue);

            decimal cape1, cape2;
            bool capeRes = backtestCalculator.checkCapeSignal(Utils.ConvertIntToDateTime(dateToProcess), out cape1, out cape2);

            bool customRes = CustomRiskModelSingleton.Instance.GetSignal(Utils.ConvertIntToDateTime(dateToProcess));

            htmlHelper.OpenRow();
            htmlHelper.AddCell("SPY AVG", true);
            htmlHelper.AddCell(spyRes ? "OFF" : "ON ", true);
            htmlHelper.AddCell("MM1  : " + avg50.ToString("n2") + "  MM2  : " + avg200.ToString("n2"), true);
            htmlHelper.CloseRow();

            htmlHelper.OpenRow();
            htmlHelper.AddCell("HI LO", true);
            htmlHelper.AddCell(hiloRes ? "OFF" : "ON ", true);
            htmlHelper.AddCell("HiLo%: " + hiloValue.ToString("n2"), true);
            htmlHelper.CloseRow();

            htmlHelper.OpenRow();
            htmlHelper.AddCell("CAPE", true);
            htmlHelper.AddCell(capeRes ? "OFF" : "ON ", true);
            htmlHelper.AddCell("CAPE1: " + cape1.ToString("n2") + "  CAPE2: " + cape2.ToString("n2"), true);
            htmlHelper.CloseRow();

            htmlHelper.OpenRow();
            htmlHelper.AddCell("FILE", true);
            htmlHelper.AddCell(customRes ? "OFF" : "ON ", true);
            htmlHelper.AddCell("", true);
            htmlHelper.CloseRow();

            htmlHelper.CloseTable();
        }

        void composePerformanceTable(BacktestCalculator backtestCalculator)
        {
            htmlHelper.OpenTable(8, new List<string>() { "Strategy", "MTD", "QTD", "YTD", "1YR", "3YR", "5YR", "Inception" }, "Performance Table");

            var snapshots = backtestCalculator.MonthlySnapshots;
            PortfolioSnapshot qtdSnapshot = getQuarterSnapshot(backtestCalculator);
            DateTime mtdDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            PortfolioSnapshot mtdSnapshot = snapshots.Find(x => x.Date.Year == mtdDate.Year && x.Date.Month == mtdDate.Month && x.Date.Day == mtdDate.Day);
            PortfolioSnapshot ytdSnapshot = snapshots.Find(x => x.Date.Year == DateTime.Now.Year - 1 && x.Date.Month == 12 && x.Date.Day == 31);
            if (mtdSnapshot == null)
                mtdSnapshot = new PortfolioSnapshot();
            if (ytdSnapshot == null)
                ytdSnapshot = new PortfolioSnapshot();
            if (qtdSnapshot == null)
                qtdSnapshot = new PortfolioSnapshot();

            addPerformanceRow("Equity", mtdSnapshot.TotalPortfolioValue, qtdSnapshot.TotalPortfolioValue, ytdSnapshot.TotalPortfolioValue, backtestCalculator.Snapshot6Months.TotalPortfolioValue, backtestCalculator.Snapshot1Year.TotalPortfolioValue, backtestCalculator.Snapshot3Year.TotalPortfolioValue, backtestCalculator.Snapshot5Year.TotalPortfolioValue, backtestCalculator.Snapshot10Year.TotalPortfolioValue, snapshots[0].TotalPortfolioValue, snapshots[snapshots.Count - 1].TotalPortfolioValue);
            addPerformanceRow("Benchmark", mtdSnapshot.BenchmarkSymbolBenchmark, qtdSnapshot.BenchmarkSymbolBenchmark, ytdSnapshot.BenchmarkSymbolBenchmark, backtestCalculator.Snapshot6Months.BenchmarkSymbolBenchmark, backtestCalculator.Snapshot1Year.BenchmarkSymbolBenchmark, backtestCalculator.Snapshot3Year.BenchmarkSymbolBenchmark, backtestCalculator.Snapshot5Year.BenchmarkSymbolBenchmark, backtestCalculator.Snapshot10Year.BenchmarkSymbolBenchmark, snapshots[0].BenchmarkSymbolBenchmark, snapshots[snapshots.Count - 1].BenchmarkSymbolBenchmark);
            addPerformanceRowAlpha("Alpha", mtdSnapshot.BenchmarkSymbolBenchmark, qtdSnapshot.BenchmarkSymbolBenchmark, ytdSnapshot.BenchmarkSymbolBenchmark, backtestCalculator.Snapshot6Months.BenchmarkSymbolBenchmark, backtestCalculator.Snapshot1Year.BenchmarkSymbolBenchmark, backtestCalculator.Snapshot3Year.BenchmarkSymbolBenchmark, backtestCalculator.Snapshot5Year.BenchmarkSymbolBenchmark, backtestCalculator.Snapshot10Year.BenchmarkSymbolBenchmark, snapshots[0].BenchmarkSymbolBenchmark, snapshots[snapshots.Count - 1].BenchmarkSymbolBenchmark, mtdSnapshot.TotalPortfolioValue, qtdSnapshot.TotalPortfolioValue, ytdSnapshot.TotalPortfolioValue, backtestCalculator.Snapshot6Months.TotalPortfolioValue, backtestCalculator.Snapshot1Year.TotalPortfolioValue, backtestCalculator.Snapshot3Year.TotalPortfolioValue, backtestCalculator.Snapshot5Year.TotalPortfolioValue, backtestCalculator.Snapshot10Year.TotalPortfolioValue, snapshots[0].TotalPortfolioValue, snapshots[snapshots.Count - 1].TotalPortfolioValue);
            addPerformanceRow("Blended Advanced", mtdSnapshot.BlendedAdvancedBenchmark, qtdSnapshot.BlendedAdvancedBenchmark, ytdSnapshot.BlendedAdvancedBenchmark, backtestCalculator.Snapshot6Months.BlendedAdvancedBenchmark, backtestCalculator.Snapshot1Year.BlendedAdvancedBenchmark, backtestCalculator.Snapshot3Year.BlendedAdvancedBenchmark, backtestCalculator.Snapshot5Year.BlendedAdvancedBenchmark, backtestCalculator.Snapshot10Year.BlendedAdvancedBenchmark, snapshots[0].BlendedAdvancedBenchmark, snapshots[snapshots.Count - 1].BlendedAdvancedBenchmark);
            addPerformanceRow("Blended Basic", mtdSnapshot.BlendedBasicBenchmark, qtdSnapshot.BlendedBasicBenchmark, ytdSnapshot.BlendedBasicBenchmark, backtestCalculator.Snapshot6Months.BlendedBasicBenchmark, backtestCalculator.Snapshot1Year.BlendedBasicBenchmark, backtestCalculator.Snapshot3Year.BlendedBasicBenchmark, backtestCalculator.Snapshot5Year.BlendedBasicBenchmark, backtestCalculator.Snapshot10Year.BlendedBasicBenchmark, snapshots[0].BlendedBasicBenchmark, snapshots[snapshots.Count - 1].BlendedBasicBenchmark);
            addPerformanceRow("Bonds Model", mtdSnapshot.BondsBenchmark, qtdSnapshot.BondsBenchmark, ytdSnapshot.BondsBenchmark, backtestCalculator.Snapshot6Months.BondsBenchmark, backtestCalculator.Snapshot1Year.BondsBenchmark, backtestCalculator.Snapshot3Year.BondsBenchmark, backtestCalculator.Snapshot5Year.BondsBenchmark, backtestCalculator.Snapshot10Year.BondsBenchmark, snapshots[0].BondsBenchmark, snapshots[snapshots.Count - 1].BondsBenchmark);

            htmlHelper.CloseTable();
        }
        
        void addPerformanceRow(string name, decimal mtd, decimal qtd, decimal ytd, decimal m6, decimal y1, decimal y3, decimal y5, decimal y10, decimal start, decimal last)
        {
            htmlHelper.OpenRow();
            htmlHelper.AddCell(name, true);
            htmlHelper.AddCell((mtd == 0 ? 0 : (last - mtd) / mtd * 100).ToString("n2") + "&nbsp;%");
            htmlHelper.AddCell((qtd == 0 ? 0 : (last - qtd) / qtd * 100).ToString("n2") + "&nbsp;%");
            htmlHelper.AddCell((ytd == 0 ? 0 : (last - ytd) / ytd * 100).ToString("n2") + "&nbsp;%");
            htmlHelper.AddCell((y1 == 0 ? 0 : (last - y1) / y1 * 100).ToString("n2") + "&nbsp;%");
            htmlHelper.AddCell((y3 == 0 ? 0 : (last - y3) / y3 * 100).ToString("n2") + "&nbsp;%");
            htmlHelper.AddCell((y5 == 0 ? 0 : (last - y5) / y5 * 100).ToString("n2") + "&nbsp;%");
            htmlHelper.AddCell((start == 0 ? 0 : (last - start) / start * 100).ToString("n2") + "&nbsp;%");
            htmlHelper.CloseRow();
        }

        void addPerformanceRowAlpha(string name, decimal mtdA, decimal qtdA, decimal ytdA, decimal m6A, decimal y1A, decimal y3A, decimal y5A, decimal y10A, decimal startA, decimal lastA, decimal mtd, decimal qtd, decimal ytd, decimal m6, decimal y1, decimal y3, decimal y5, decimal y10, decimal start, decimal last)
        {
            htmlHelper.OpenRow();
            htmlHelper.AddCell(name, true);
            htmlHelper.AddCell((mtd == 0 || mtdA == 0 ? 0 : (last - mtd) / mtd * 100 - (lastA - mtdA) / mtdA * 100).ToString("n2") + "&nbsp;%");
            htmlHelper.AddCell((qtd == 0 || qtdA == 0 ? 0 : (last - qtd) / qtd * 100).ToString("n2") + "&nbsp;%");
            htmlHelper.AddCell((ytd == 0 || ytdA == 0 ? 0 : (last - ytd) / ytd * 100 - (lastA - ytdA) / ytdA * 100).ToString("n2") + "&nbsp;%");
            htmlHelper.AddCell((y1 == 0 || y1A == 0 ? 0 : (last - y1) / y1 * 100 - (lastA - y1A) / y1A * 100).ToString("n2") + "&nbsp;%");
            htmlHelper.AddCell((y3 == 0 || y3A == 0 ? 0 : (last - y3) / y3 * 100 - (lastA - y3A) / y3A * 100).ToString("n2") + "&nbsp;%");
            htmlHelper.AddCell((y5 == 0 || y5A == 0 ? 0 : (last - y5) / y5 * 100 - (lastA - y5A) / y5A * 100).ToString("n2") + "&nbsp;%");
            htmlHelper.AddCell((start == 0 || startA == 0 ? 0 : (last - start) / start * 100 - (lastA - startA) / startA * 100).ToString("n2") + "&nbsp;%");
            htmlHelper.CloseRow();
        }

        PortfolioSnapshot getQuarterSnapshot(BacktestCalculator backtestCalculator)
        {
            int quarterStart = 1;

            switch (DateTime.Now.Month)
            {
                case 1:
                case 2:
                case 3:
                    quarterStart = 1;
                    break;
                case 4:
                case 5:
                case 6:
                    quarterStart = 4;
                    break;
                case 7:
                case 8:
                case 9:
                    quarterStart = 7;
                    break;
                case 10:
                case 11:
                case 12:
                    quarterStart = 10;
                    break;
            }

            DateTime quarter = new DateTime(DateTime.Now.Year, quarterStart, 1).AddDays(-1);
            return backtestCalculator.MonthlySnapshots.Find(x => x.Date.Year == quarter.Year && x.Date.Month == quarter.Month && x.Date.Day == quarter.Day);
        }

        public static String SendTestMail(MailConfiguration mailConfig, String sendTo)
        {
            return SendMail(mailConfig, sendTo, "Test Mail", "<body>Test Email automatically sent<br>Alerts will be sent using this configuration.");
        }

        public static String SendMail(MailConfiguration mailConfig, String sendTo, String subject, String content)
        {
            try
            {
                MailMessage mail = new MailMessage(mailConfig.From, sendTo.Replace(";", ","));
                mail.Subject = subject;
                mail.Body = content;
                mail.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient(mailConfig.Smtp, mailConfig.Port);

                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = mailConfig.Username,
                    Password = mailConfig.Password
                };

                smtpClient.EnableSsl = true;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

                smtpClient.Send(mail);
            }
            catch(Exception e)
            {
                return e.Message + " - " + e.StackTrace;
            }

            return "";
        }

    }
}
