using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockRanking.Benchmarks;

namespace StockRanking
{
    public class BacktestCalculator
    {
        public PortfolioParameters Portfolio = new PortfolioParameters();
        public decimal YearBackPortfolioValue = 0;
        public List<Position> LastDayPositions = new List<Position>();
        public decimal LastDayCash = 0;
        public static bool CancelProcess = false;
        public List<PortfolioSnapshot> MonthlySnapshots { get { return monthlySnapshots; } }
        public PortfolioSnapshot Snapshot6Months;
        public PortfolioSnapshot Snapshot3Months;
        public PortfolioSnapshot Snapshot1Year;
        public PortfolioSnapshot Snapshot2Year;
        public PortfolioSnapshot Snapshot3Year;
        public PortfolioSnapshot Snapshot5Year;
        public PortfolioSnapshot Snapshot10Year;
        public PortfolioSnapshot LastSnapshot;
        public PortfolioSnapshot Snapshot1Month;

        Dictionary<int, Stock> stocks = null;
        List<Stock> stocksList = null;
        List<FeatureWeight> featureWeights = new List<FeatureWeight>();
        List<FilterConditions> filtersList = new List<FilterConditions>();
        int monthsPeriod = 12;
        List<PortfolioSnapshot> snapshots = new List<PortfolioSnapshot>();
        List<PortfolioSnapshot> monthlySnapshots = new List<PortfolioSnapshot>();
        PortfolioSnapshot currentSnapshot = null;
        List<Position> currentPositions = new List<Position>();
        decimal currentAvailableCash = 0;
        Dictionary<String, int> countPerIndustry = new Dictionary<string, int>();
        BenchmarksCalculator benchmarksCalculator;
        MetricsCalculator metricsCalculator;
        List<int> rebalanceDates = new List<int>();

        String BenchmarkSymbol = "";
        String InverseSymbol = "";

        public BenchmarksCalculator BenchmarksCalculator => benchmarksCalculator;
        public MetricsCalculator MetricsCalculator => metricsCalculator;
        public List<FeatureWeight> FeatureWeights => this.featureWeights;

        public BacktestCalculator(PortfolioParameters portfolio, List<FeatureWeight> featureW, List<FilterConditions> filters)
        {
            CancelProcess = false;
            Portfolio = portfolio;
            featureWeights = featureW;
            filtersList = filters;
            monthsPeriod = portfolio.RebalanceFrequencyMonths;
            StrategyView.CancelResultsProcessing = false;
            benchmarksCalculator = new BenchmarksCalculator(portfolio);
            metricsCalculator = new MetricsCalculator(benchmarksCalculator, this);
        }

        public List<PortfolioSnapshot> RunBacktest(ProcessingPanelControl pnlProcessing, DateTime startDate)
        {
            CustomRiskModelSingleton.Instance.SetFile(Portfolio.CustomRiskFile);

            pnlProcessing.StartProcess();
            pnlProcessing.SetTitle("Starting backtest.");
            pnlProcessing.SetMaxValue(10);

            snapshots = new List<PortfolioSnapshot>();
            rebalanceDates = new List<int>();
            currentPositions = new List<Position>();
            currentAvailableCash = 0;
            monthlySnapshots = new List<PortfolioSnapshot>();
            YearBackPortfolioValue = 0;
            LastDayCash = 0;
            benchmarksCalculator.ResetBenchmarks();
            this.Snapshot6Months = null;
            this.Snapshot3Months = null;
            this.Snapshot10Year = null;
            this.Snapshot1Year = null;
            this.Snapshot2Year = null;
            this.Snapshot3Year = null;
            this.Snapshot5Year = null;
            this.LastSnapshot = null;
            this.Snapshot1Month = null;

            stocksList = Stock.ReadStockSymbolsThreads(true, pnlProcessing);

            if (stocksList == null)
                return null;

            stocks = new Dictionary<int, Stock>();
            stocksList.ForEach(x => stocks.Add(x.Id, x));

            if (Stock.GetStock(Portfolio.IdSymbolBenchmark) == null)
            {
                MessageBox.Show("Error processing backtest. Benchmark symbol does not exist.");
                return null;
            }
            BenchmarkSymbol = Stock.GetStock(Portfolio.IdSymbolBenchmark).Symbol;

            if (Stock.GetStock(Portfolio.IdSymbolInverse) == null && Portfolio.UseRiskModel == 1)
            {
                MessageBox.Show("Error processing backtest. Inverse symbol does not exist.");
                return null;
            }
            InverseSymbol = Stock.GetStock(Portfolio.IdSymbolInverse).Symbol;

            /*
             Backtest logic:
             - Sell everything and record trades
             - Record total portfolio value for this date
             - Substract benchmark %
             - Evaluate stocks ranking
             - Evaluate number of stocks and divide total money per stock
             - Cap stock money using max % per position
             - For each position search the first stock from the ranking 
             -    check if industry limit exceded
             -    check if industry is allowed
             - Rebalance:
             -    Calc new stock quantity
             -    BUY to set new quantity
             - Rebalance SPY:
             -    If available cash > 0 then Add more SPY to the benchmark
             - Substract commissions from available cash
             */
            
            DateTime processingDate = new DateTime(startDate.Year, startDate.Month, 1);
            currentAvailableCash = Portfolio.AccountSize;
            pnlProcessing.SetMaxValue((DateTime.Now.Year - processingDate.Year) * (12/monthsPeriod));

            int lastDatePrice = 0;
            Stock.GetLastPrice(Portfolio.IdSymbolBenchmark, out lastDatePrice);

            if (lastDatePrice == 0)
                return null;

            DateTime lastValidDate = Utils.ConvertIntToDateTime(lastDatePrice);
            int currentPeriods = 0;

            benchmarksCalculator.StartBenchmarks(Utils.ConvertDateTimeToInt(processingDate), Portfolio.AccountSize);
            monthlySnapshots.Add(new PortfolioSnapshot(processingDate.AddDays(-1), Portfolio.AccountSize, benchmarksCalculator));

            while (processingDate <= lastValidDate)
            {
                pnlProcessing.SetTitle("Processing Date: " + processingDate.ToString("MM-dd-yyyy"));
                pnlProcessing.PerformStep();

                if (CancelProcess)
                    return null;

                countPerIndustry.Clear();

                //rank stocks for this period
                Dictionary<int, RankingResult> stocksRanking = generateStocksCurrentValues(processingDate);
                
                Stock benchmarkStock = stocks[Portfolio.IdSymbolBenchmark];

                //sell everything, discount commissions, store trades
                decimal currentPortfolioValue = sellPositions(processingDate, false, true, RiskModelResults.Normal);
                decimal totalMoney = currentAvailableCash + currentPortfolioValue;
                
                //discount annual fee
                if (Portfolio.AnnualFee > 0 && currentPeriods == 12)
                {
                    totalMoney -= totalMoney * Portfolio.AnnualFee / 100m;
                    currentPeriods = 0;
                }

                //record total portfolio value
                benchmarksCalculator.RebalanceBenchmarks(Utils.ConvertDateTimeToInt(processingDate), totalMoney);
                storeIntervalSnapshots(processingDate, totalMoney);

                currentSnapshot = new PortfolioSnapshot(processingDate, totalMoney, benchmarksCalculator);

                snapshots.Add(currentSnapshot);
                rebalanceDates.Add(currentSnapshot.RealDate);

                //stocks bought in order to filter them when buying after stop loss
                List<int> stocksAlreadyBought = new List<int>();
                List<RankingResult> orderedResults = new List<RankingResult>();

                if (stocksRanking != null && (int)checkRiskModel(processingDate) < (int)RiskModelResults.NoNewPositions)
                {
                    orderedResults = stocksRanking.Values.ToList<RankingResult>().OrderByDescending(x => x.TotalWeight).ToList();

                    //Substract benchmark %
                    decimal benchmarkMoney = totalMoney * (decimal)Portfolio.BenchmarkPercent / 100m;
                    totalMoney -= benchmarkMoney;

                    //Evaluate number of stocks and divide total money per stock
                    decimal capitalPerPosition = totalMoney / Portfolio.Positions;
                    if (capitalPerPosition > totalMoney * Portfolio.MaxWeightPerPosition)
                    {
                        capitalPerPosition = totalMoney * Portfolio.MaxWeightPerPosition;
                    }

                    //For each position search the first stock from the ranking 
                    // -check if industry limit exceded
                    // -check if industry is allowed
                    List<Stock> stocksSelected = new List<Stock>();
                    while (true)
                    {
                        if (stocksSelected.Count == Portfolio.Positions)
                            break;

                        Stock nextStock = getNextStock(orderedResults, stocksAlreadyBought, countPerIndustry, DateTime.MinValue);

                        if (nextStock == null)
                            break;

                        stocksSelected.Add(nextStock);
                    }

                    //buy each position
                    foreach (Stock selectedStock in stocksSelected)
                    {
                        int shares = (int)Math.Floor(capitalPerPosition / selectedStock.CurrentPrice);
                        if (shares > Portfolio.MaxSharesPennyStocks && selectedStock.CurrentPrice < 1)
                            shares = (int)Portfolio.MaxSharesPennyStocks;

                        Position newPosition = new Position(selectedStock.Id, shares, Utils.ConvertDateTimeToInt(processingDate), selectedStock.CurrentPrice, getInitialStopLossPrice(selectedStock.CurrentPrice), selectedStock.Symbol, selectedStock.CompanyName, selectedStock.getStockSector());

                        totalMoney -= newPosition.EntryPrice * newPosition.Shares;

                        currentPositions.Add(newPosition);
                    }

                    //buy benchmark (SPY)
                    totalMoney += benchmarkMoney;
                    if (Portfolio.BuyBenchmarkLeftoverCash == 1)
                    {
                        benchmarkMoney = totalMoney;
                    }

                    if (benchmarkStock.CurrentPrice != 0)
                    {
                        Position spyPosition = new Position(Portfolio.IdSymbolBenchmark, (int)Math.Floor(benchmarkMoney / benchmarkStock.CurrentPrice), Utils.ConvertDateTimeToInt(processingDate), benchmarkStock.CurrentPrice, 0, BenchmarkSymbol, BenchmarkSymbol, "");
                        if (spyPosition.Shares > 0)
                        {
                            currentPositions.Add(spyPosition);
                            totalMoney -= spyPosition.EntryPrice * spyPosition.Shares;
                        }
                    }
                }


                //current available cash will represent available money for the daily simulation
                currentAvailableCash = totalMoney;


                //run a daily simulation in order to validate stop loss values
                DateTime stopLossCurrentDate = processingDate.AddDays(1);
                List<decimal> soldPositions = new List<decimal>();
                while (stopLossCurrentDate < processingDate.AddMonths(monthsPeriod) && stopLossCurrentDate <= lastValidDate)
                {
                    storeIntervalSnapshots(stopLossCurrentDate, currentAvailableCash);
                    var totalPortfolioValue = valuateCurrentPositions(stopLossCurrentDate) + currentAvailableCash;
                    benchmarksCalculator.UpdateBenchmarksDaily(Utils.ConvertDateTimeToInt(stopLossCurrentDate), totalPortfolioValue);

                    RiskModelResults riskModelResult = checkRiskModel(stopLossCurrentDate);

                    if ((int)riskModelResult == (int)RiskModelResults.SellSPY)
                    {
                        //sell SPY position
                        Position currentSPYPosition = currentPositions.Find(x => x.IdStock == Portfolio.IdSymbolBenchmark);
                        if (currentSPYPosition != null)
                        {
                            currentAvailableCash += sellPosition(currentSPYPosition, stopLossCurrentDate, false, riskModelResult);
                            currentPositions.Remove(currentSPYPosition);
                        }
                    }

                    if ((int)riskModelResult < (int)RiskModelResults.SellAllBuySPXU)
                    {
                        //sell SPXU position
                        Position currentSpxuPosition = currentPositions.Find(x => x.IdStock == Portfolio.IdSymbolInverse);
                        if (currentSpxuPosition != null)
                        {
                            currentAvailableCash += sellPosition(currentSpxuPosition, stopLossCurrentDate, false, riskModelResult);
                            currentPositions.Remove(currentSpxuPosition);
                        }
                    }

                    if ((int)riskModelResult == (int)RiskModelResults.SellAllBuySPXU)
                    {
                        //sell all
                        currentAvailableCash += sellPositions(stopLossCurrentDate, true, false, riskModelResult);

                        soldPositions.Clear();

                        //buy SPXU if not present
                        Position currentSpxuPosition = currentPositions.Find(x => x.IdStock == Portfolio.IdSymbolInverse);
                        if (currentSpxuPosition == null)
                        {
                            Stock spxuStock = stocks[Portfolio.IdSymbolInverse];
                            spxuStock.CurrentPrice = Stock.GetPrice(spxuStock.Id, Utils.ConvertDateTimeToInt(stopLossCurrentDate));
                            if (spxuStock.CurrentPrice != 0)
                            {
                                if ((int)Math.Floor(currentAvailableCash * getSPXUPercent() / spxuStock.CurrentPrice) > 0)
                                {
                                    currentSpxuPosition = new Position(Portfolio.IdSymbolInverse, (int)Math.Floor(currentAvailableCash * getSPXUPercent() / spxuStock.CurrentPrice), Utils.ConvertDateTimeToInt(stopLossCurrentDate), spxuStock.CurrentPrice, 0, InverseSymbol, InverseSymbol, "");
                                    currentPositions.Add(currentSpxuPosition);
                                    currentAvailableCash -= currentSpxuPosition.EntryPrice * currentSpxuPosition.Shares;
                                }
                            }
                        }
                    }


                    List<Position> positionsToRemove = new List<Position>();
                    foreach (Position position in currentPositions)
                    {
                        //ignore Indexes for stop loss
                        if (position.IdStock < 0)
                            continue;

                        decimal stockPrice = Stock.GetPrice(position.IdStock, Utils.ConvertDateTimeToInt(stopLossCurrentDate));
                        position.UpdateStopLossPrice(stockPrice, Portfolio);
                            
                        if (stockPrice <= position.StopLossPrice && Portfolio.EntryStopLoss != 0)
                        { 
                            positionsToRemove.Add(position);

                            currentAvailableCash += sellPosition(position, stopLossCurrentDate, true, RiskModelResults.Normal);

                            soldPositions.Add(stockPrice * position.Shares);

                            //remove from industry count
                            if (Portfolio.MaxPositionsPerIndustry > 0)
                                countPerIndustry[stocks[position.IdStock].getStockSector()]--;
                        }

                    }

                    foreach (Position pos in positionsToRemove)
                        currentPositions.Remove(pos);

                    //if date is monday, buy new stocks for freed up positions
                    if(stopLossCurrentDate.DayOfWeek == DayOfWeek.Monday && riskModelResult == (int)RiskModelResults.Normal)
                    {
                        foreach(decimal positionToBuy in soldPositions)
                        {
                            //select next stock to buy
                            Stock nextStock = getNextStock(orderedResults, stocksAlreadyBought, countPerIndustry, stopLossCurrentDate);
                            if (nextStock == null)
                                break;

                            decimal stockPrice = Stock.GetPrice(nextStock.Id, Utils.ConvertDateTimeToInt(stopLossCurrentDate));

                            int shares = (int)Math.Floor(positionToBuy / stockPrice);
                            if (shares > Portfolio.MaxSharesPennyStocks && stockPrice < 1)
                                shares = (int)Portfolio.MaxSharesPennyStocks;

                            //buy new position
                            Position newPosition = new Position(nextStock.Id, shares, Utils.ConvertDateTimeToInt(stopLossCurrentDate), stockPrice, getInitialStopLossPrice(stockPrice), nextStock.Symbol, nextStock.CompanyName, nextStock.getStockSector());
                            currentAvailableCash -= newPosition.EntryPrice * newPosition.Shares;
                            newPosition.BuyAfterStopLoss = true;
                            currentPositions.Add(newPosition);
                        }

                        soldPositions.Clear();
                    }

                    stopLossCurrentDate = stopLossCurrentDate.AddDays(1);
                }
                    

                processingDate = processingDate.AddMonths(monthsPeriod);
                currentPeriods += monthsPeriod;
            }


            //last snapshot, estimate final portfolio value
            LastDayPositions = currentPositions.ToList();
            LastDayCash = currentAvailableCash;

            foreach (Position pos in currentPositions)
            {
                currentAvailableCash += pos.Shares * Stock.GetLastPrice(pos.IdStock);
            }

            benchmarksCalculator.UpdateBenchmarks(Utils.ConvertDateTimeToInt(DateTime.Now.AddDays(-1)), currentAvailableCash);
            PortfolioSnapshot lastSnap = new PortfolioSnapshot(benchmarksCalculator);

            snapshots.Add(lastSnap);
            lastSnap.Date = DateTime.Now.AddDays(-1);
            Stock.UpdateLastPrice(stocksList);
            lastSnap.TotalPortfolioValue = currentAvailableCash;
            currentSnapshot = lastSnap;
            sellPositions(lastSnap.Date, false, true, RiskModelResults.Normal, true);

            //store last monthly portfolio value (only if not first day of month)
            if (lastSnap.Date.Day != DateTime.DaysInMonth(lastSnap.Date.Year, lastSnap.Date.Month))
                monthlySnapshots.Add(new PortfolioSnapshot(lastSnap.Date, currentAvailableCash, benchmarksCalculator));

            DebugDataLogger.Instance.CloseAll();

            LastSnapshot = lastSnap;
            if (this.Snapshot2Year == null)
                this.Snapshot2Year = this.snapshots[0];
            if (this.Snapshot3Year == null)
                this.Snapshot3Year = this.snapshots[0];
            if (this.Snapshot5Year == null)
                this.Snapshot5Year = this.snapshots[0];
            if (this.Snapshot10Year == null)
                this.Snapshot10Year = this.snapshots[0];
            if (this.Snapshot3Months == null)
                this.Snapshot3Months = this.snapshots[0];
            if (this.Snapshot6Months == null)
                this.Snapshot6Months = this.snapshots[0];
            if (this.Snapshot1Month == null)
                this.Snapshot1Month = this.snapshots[0];

            return snapshots;
        }

        void storeIntervalSnapshots(DateTime date, decimal currentAvailableCash)
        {
            int dateInt = Utils.ConvertDateTimeToInt(date);
            if (date.Day == 1 ||
                dateInt == Utils.ConvertDateTimeToInt(DateTime.Now.AddYears(-1)) ||
                dateInt == Utils.ConvertDateTimeToInt(DateTime.Now.AddYears(-2)) ||
                dateInt == Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-6)) ||
                dateInt == Utils.ConvertDateTimeToInt(DateTime.Now.AddYears(-10)) ||
                dateInt == Utils.ConvertDateTimeToInt(DateTime.Now.AddYears(-3)) ||
                dateInt == Utils.ConvertDateTimeToInt(DateTime.Now.AddYears(-5)) ||
                dateInt == Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-3)) ||
                dateInt == Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-1)))
            {

                var totalPortfolioValue = valuateCurrentPositions(date) + currentAvailableCash;
                benchmarksCalculator.UpdateBenchmarksDaily(Utils.ConvertDateTimeToInt(date), totalPortfolioValue);

                //if first of month store portfolio value
                if (date.Day == 1)
                {
                    benchmarksCalculator.UpdateBenchmarks(Utils.ConvertDateTimeToInt(date), totalPortfolioValue);
                    if(!monthlySnapshots.Last().Date.AddDays(1).Equals(date))
                        monthlySnapshots.Add(new PortfolioSnapshot(date.AddDays(-1), totalPortfolioValue, benchmarksCalculator, true));
                }

                //1 year back portfolio value
                if (dateInt == Utils.ConvertDateTimeToInt(DateTime.Now.AddYears(-1)))
                {
                    YearBackPortfolioValue = totalPortfolioValue;
                    Snapshot1Year = new PortfolioSnapshot(date, totalPortfolioValue, benchmarksCalculator);
                }

                //6 months back
                if (dateInt == Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-6)))
                    Snapshot6Months = new PortfolioSnapshot(date, totalPortfolioValue, benchmarksCalculator);
                //3 months back
                if (dateInt == Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-3)))
                    Snapshot3Months = new PortfolioSnapshot(date, totalPortfolioValue, benchmarksCalculator);
                //1 months back
                if (dateInt == Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-1)))
                    Snapshot1Month = new PortfolioSnapshot(date, totalPortfolioValue, benchmarksCalculator);

                //2 years back
                if (dateInt == Utils.ConvertDateTimeToInt(DateTime.Now.AddYears(-2)))
                    Snapshot2Year = new PortfolioSnapshot(date, totalPortfolioValue, benchmarksCalculator);
                //3 years back
                if (dateInt == Utils.ConvertDateTimeToInt(DateTime.Now.AddYears(-3)))
                    Snapshot3Year = new PortfolioSnapshot(date, totalPortfolioValue, benchmarksCalculator);
                //5 years back
                if (dateInt == Utils.ConvertDateTimeToInt(DateTime.Now.AddYears(-5)))
                    Snapshot5Year = new PortfolioSnapshot(date, totalPortfolioValue, benchmarksCalculator);
                //10 years back
                if (dateInt == Utils.ConvertDateTimeToInt(DateTime.Now.AddYears(-10)))
                    Snapshot10Year = new PortfolioSnapshot(date, totalPortfolioValue, benchmarksCalculator);
            }
        }

        decimal getInitialStopLossPrice(decimal price)
        {
            return price * ((decimal)1-(decimal)Portfolio.EntryStopLoss/100);
        }

        Stock getNextStock(List<RankingResult> orderedResults, List<int> stocksAlreadyBought, Dictionary<String, int> countPerIndustry, DateTime currentDate)
        {
            Stock result = null;

            foreach (RankingResult rankedStock in orderedResults)
            { 
                if (rankedStock.IdStock == Portfolio.IdSymbolBenchmark)
                    continue;

                if (stocksAlreadyBought.Contains(rankedStock.IdStock))
                    continue;

                Stock stock = stocks[rankedStock.IdStock];

                if (currentDate == DateTime.MinValue)
                {
                    if (stock.CurrentPrice <= 0)
                        continue;

                    if (stock.CurrentVolume < Portfolio.FilterMinVolume)
                        continue;

                    if (stock.CurrentPrice * Portfolio.FilterComissionsPerc <= (decimal)Portfolio.CommisionPerShare)
                        continue;
                }
                else
                {
                    int volume = 0;
                    decimal stockPrice = Stock.GetPrice(stock.Id, Utils.ConvertDateTimeToInt(currentDate), out volume);
                    if (stockPrice <= 0)
                        continue;

                    if (volume < Portfolio.FilterMinVolume)
                        continue;

                    if (stockPrice * Portfolio.FilterComissionsPerc <= (decimal)Portfolio.CommisionPerShare)
                        continue;
                }
                
                //check same industry qtty
                if (Portfolio.MaxPositionsPerIndustry > 0)
                {
                    if (countPerIndustry.ContainsKey(stock.getStockSector()))
                    {
                        if (countPerIndustry[stock.getStockSector()] >= Portfolio.MaxPositionsPerIndustry)
                            continue;

                        countPerIndustry[stock.getStockSector()]++;
                    }
                    else
                        countPerIndustry.Add(stock.getStockSector(), 1);
                }

                result = stock;
                break;
            }

            if (result != null)
                stocksAlreadyBought.Add(result.Id);

            return result;

        }


        decimal valuateCurrentPositions(DateTime date)
        {
            decimal totalMoney = 0;
            
            foreach (Position pos in currentPositions)
            {
                Stock stock = stocks[pos.IdStock];
                decimal currPrice = Stock.GetPrice(stock.Id, Utils.ConvertDateTimeToInt(date));
                var dividends = stock.getDividendSum(pos.DateEntered, Utils.ConvertDateTimeToInt(date));

                totalMoney += pos.Shares * (currPrice + dividends);
            }

            return totalMoney;
        }


        decimal sellPositions(DateTime currentDate, bool panicToCash, bool sellSPUX, RiskModelResults riskModel, bool sellForCurrentDateValorization = false)
        {
            decimal totalMoney = 0;
            List<Position> posToRemove = new List<Position>();

            foreach(Position pos in currentPositions)
            {
                //dont sell spxu unless specified
                if (!sellSPUX && pos.IdStock == Portfolio.IdSymbolInverse)
                    continue;

                Stock stock = stocks[pos.IdStock];
                if (stock.CurrentPrice == 0 || riskModel == RiskModelResults.SellAllBuySPXU || pos.IdStock == Portfolio.IdSymbolInverse)
                    stock.CurrentPrice = Stock.GetPrice(stock.Id, Utils.ConvertDateTimeToInt(currentDate));
                decimal currentPrice = stock.CurrentPrice;
                totalMoney += pos.Shares * currentPrice;

                //take dividends in account
                var dividends = stock.getDividendSum(pos.DateEntered, Utils.ConvertDateTimeToInt(currentDate));
                totalMoney += pos.Shares * dividends;

                //record trade
                Trade newTrade = new Trade()
                {
                    RealEntryDate = benchmarksCalculator.GetRealDate(pos.DateEntered),
                    RealExitDate = benchmarksCalculator.GetRealDate(currentDate),
                    EntryDate = pos.DateEntered,
                    ExitDate = Utils.ConvertDateTimeToInt(currentDate),
                    ExitPrice = currentPrice,
                    EntryPrice = pos.EntryPrice,
                    IdStock = pos.IdStock,
                    MultipleEntries = false,
                    Shares = pos.Shares,
                    Profit = pos.Shares * currentPrice - pos.Shares * pos.EntryPrice + pos.Shares * dividends,
                    Commission = (decimal)Portfolio.CommisionPerShare * (decimal)pos.Shares,
                    Symbol = stock.Symbol,
                    StopLossSell = false,
                    BuyAfterStopLoss = pos.BuyAfterStopLoss,
                    RiskModel = riskModel,
                    AuxSector = pos.AuxSector,
                    CurrentDateValorizationSell = sellForCurrentDateValorization
                };

                //discount commission
                totalMoney -= (decimal)Portfolio.CommisionPerShare * (decimal)pos.Shares;

                currentSnapshot.Trades.Add(newTrade);

                posToRemove.Add(pos);
            }

            foreach(Position posR in posToRemove)
                currentPositions.Remove(posR);

            return totalMoney;
        }


        decimal sellPosition(Position position, DateTime currentDate, bool stopLossSell, RiskModelResults riskModel)
        {
            Stock stock = stocks[position.IdStock];
            if (stock.CurrentPrice == 0 || riskModel == RiskModelResults.SellAllBuySPXU || riskModel == RiskModelResults.SellSPY || stopLossSell)
                stock.CurrentPrice = Stock.GetPrice(stock.Id, Utils.ConvertDateTimeToInt(currentDate));
            decimal currentPrice = stock.CurrentPrice;
            decimal moneyVariation = position.Shares * currentPrice;

            //take dividends in account
            var dividends = stock.getDividendSum(position.DateEntered, Utils.ConvertDateTimeToInt(currentDate));
            moneyVariation += position.Shares * dividends;

            //sell position and record trade
            Trade newTrade = new Trade()
            {
                RealEntryDate = benchmarksCalculator.GetRealDate(position.DateEntered),
                RealExitDate = benchmarksCalculator.GetRealDate(currentDate),
                EntryDate = position.DateEntered,
                ExitDate = Utils.ConvertDateTimeToInt(currentDate),
                ExitPrice = currentPrice,
                EntryPrice = position.EntryPrice,
                IdStock = position.IdStock,
                MultipleEntries = false,
                Shares = position.Shares,
                Profit = position.Shares * currentPrice - position.Shares * position.EntryPrice + position.Shares * dividends,
                Commission = (decimal)Portfolio.CommisionPerShare * (decimal)position.Shares,
                Symbol = stocks[position.IdStock].Symbol,
                StopLossSell = stopLossSell,
                BuyAfterStopLoss = position.BuyAfterStopLoss,
                RiskModel = riskModel,
                AuxSector = position.AuxSector
            };

            //discount commission
            moneyVariation -= (decimal)Portfolio.CommisionPerShare * (decimal)position.Shares;
            
            currentSnapshot.Trades.Add(newTrade);

            return moneyVariation;
        }

        public Dictionary<int, RankingResult> generateStocksCurrentValues(DateTime processDate)
        {
            Stock.UpdateCurrentValues(processDate, stocksList);

            RankingCalculator rankingCalculator = new RankingCalculator(Portfolio);

            return rankingCalculator.ProcessRanking(processDate, stocksList, filtersList, featureWeights, monthsPeriod, false);
        }

        RiskModelResults checkRiskModel(DateTime dateToCheck)
        {
            if (Portfolio.UseRiskModel != 1)
                return RiskModelResults.Normal;

            bool spyAVG = false;
            bool cape = false;
            bool hiLo = false;
            bool customSignal = false;
            bool macd = false;
            bool rsi = false;
            bool stochastic = false;

            int inactive = 0;

            if (Portfolio.UseAVGModel == 1)
                spyAVG = checkSPYAvgSignal(dateToCheck);
            else
                inactive++;

            if (Portfolio.UseCapeModel == 1)
                cape = checkCapeSignal(dateToCheck);
            else
                inactive++;

            if (Portfolio.UseHiLoModel == 1)
                hiLo = checkHiLoSignal(dateToCheck);
            else
                inactive++;

            if (Portfolio.UseCustomFileModel == 1)
                customSignal = checkCustomSignal(dateToCheck);
            else
                inactive++;

            if (Portfolio.UseMACDModel == 1)
            {
                macd = checkMacdSignal(dateToCheck);
            }
            else inactive++;

            if (Portfolio.UseRSIModel == 1)
            {
                rsi = checkRSISignal(dateToCheck);
            }
            else inactive++;

            if (Portfolio.UseStochasticModel == 1)
            {
                stochastic = checkStochastic(dateToCheck);
            }
            else inactive++;

            int total = 0;

            if (spyAVG) total++;
            if (cape) total++;
            if (hiLo) total++;
            if (customSignal) total++;
            if (macd) total++;
            if (rsi) total++;
            if (stochastic) total++;

            if(inactive > 1 && total > 0)
            {
                total += inactive-4;
            }

            if (total > 4)
                total = 3;

            return (RiskModelResults)total;
        }

        public bool checkStochastic(DateTime dateToCheck)
        {
            int rangeLength = 14;
            int length = rangeLength + decimal.ToInt32(Portfolio.StochasticLoopback) + 10;

            DataTable data = DatabaseSingleton.Instance.GetData("select close_price from price_history where id_stock = -1 and date <= " + Utils.ConvertDateTimeToInt(dateToCheck) + " order by date desc limit " + length.ToString());
            length = data.Rows.Count;
            double[] close = new double[length];
            double[] high = new double[length];
            double[] low = new double[length];

            double[] outSlowK = new double[length];
            double[] outSlowD = new double[length];
            int outBegIdx = 0, outNBElement = length;

            TALibraryInCSharp.Core.RetCode result;

            for (int i = 0; i < length; i++)
            {
                close[length - i - 1] = Convert.ToDouble(data.Rows[i][0]);
            }
            for (int i = 0; i<length; i++)
            {
                high[i] = close[i];
                low[i] = close[i];
                for (int j = i-rangeLength + 1; j<=i; j++)
                {
                    if (j < 0) continue;
                    if (high[i] < close[j]) high[i] = close[j];
                    if (low[i] > close[j]) low[i] = close[j];
                }
            }

            result = TALibraryInCSharp.Core.Stoch(length - 1, length - 1, high, low, close, decimal.ToInt32(Portfolio.StochasticLoopback), 3, 0, 3, 0,
                ref outBegIdx, ref outNBElement, outSlowK, outSlowD);
                
            if (result != 0) return false;

            double stoValue = outSlowK[0];

            //Console.WriteLine("," + stoValue.ToString() + "," + high[length - 1].ToString() + "," + low[length - 1].ToString() + "," + close[length - 1].ToString());
            //Console.WriteLine(dateToCheck.ToShortDateString() + ", " + outNBElement.ToString() + ", " + stoValue.ToString());

            bool ret = stoValue > decimal.ToDouble(Portfolio.StochasticThreshold);

            if (Portfolio.StochasticCompare == 1) ret = !ret;

            return ret;
        }
        public bool checkRSISignal(DateTime dateToCheck)
        {
            int length = decimal.ToInt32(Portfolio.RSILoopback) + 1;

            DataTable data = DatabaseSingleton.Instance.GetData("select close_price from price_history where id_stock = -1 and date <= " + Utils.ConvertDateTimeToInt(dateToCheck) + " order by date desc limit " + length.ToString());

            length = data.Rows.Count;

            if (length <= decimal.ToInt32(Portfolio.RSILoopback)) return false;

            double[] close = new double[length];
            double[] outRsi = new double[length];
            int outBegIdx = 0, outNBElement = length;
            TALibraryInCSharp.Core.RetCode result;

            for (int i = 0; i < length; i++)
            {
                close[length - i - 1] = Convert.ToDouble(data.Rows[i][0]);
            }

            result = TALibraryInCSharp.Core.Rsi(length - 1, length - 1, close, decimal.ToInt32(Portfolio.RSILoopback),
                ref outBegIdx, ref outNBElement, outRsi);

            if (result != 0) return false;

            double rsivalue = outRsi[0];

            //Console.Write("," + rsivalue.ToString());
            //Console.WriteLine(dateToCheck.ToShortDateString() + ", " + length.ToString() + ", " + rsivalue.ToString());
            bool ret = rsivalue > decimal.ToDouble(Portfolio.RSIThreshold);

            if (Portfolio.RSICompare == 1) ret = !ret;

            return ret;
        }

        public bool checkMacdSignal(DateTime dateToCheck)
        {
            int length = decimal.ToInt32(Portfolio.MACDLoopback);
            if (length < decimal.ToInt32(Portfolio.MACDLoopback1))
            {
                length = decimal.ToInt32(Portfolio.MACDLoopback1);
            }
            if (length < decimal.ToInt32(Portfolio.MACDLoopback2))
            {
                length = decimal.ToInt32(Portfolio.MACDLoopback2);
            }

            length = length + 50;

            DataTable data = DatabaseSingleton.Instance.GetData("select close_price from price_history where id_stock = -1 and date <= " + Utils.ConvertDateTimeToInt(dateToCheck) + " order by date desc limit " + length.ToString() );

            if (data.Rows.Count < length) return false;

            length = data.Rows.Count;

            if (length == 0) return false;

            double[] close = new double[length];
            double[] outMacd = new double[length];
            double[] outMacdSignal = new double[length];
            double[] outMacdHist = new double[length];
            int outBegIdx = 0, outNBElement = length;
            TALibraryInCSharp.Core.RetCode result;

            for (int i = 0; i<length; i++)
            {
                close[length - i - 1] = Convert.ToDouble(data.Rows[i][0]);
            }

            result = TALibraryInCSharp.Core.Macd(length - 1, length - 1 , close, decimal.ToInt32(Portfolio.MACDLoopback), decimal.ToInt32(Portfolio.MACDLoopback1), decimal.ToInt32(Portfolio.MACDLoopback2),
                ref outBegIdx, ref outNBElement, outMacd, outMacdSignal, outMacdHist);

            if (result != 0) return false;

            double macdValue = outMacdHist[0];

            //Console.Write(dateToCheck.ToShortDateString() + "," + macdValue.ToString());
            //Console.WriteLine(dateToCheck.ToShortDateString() + ", " + length.ToString() + ", " + macdValue.ToString());
            bool ret = macdValue > decimal.ToDouble(Portfolio.MACDThreshold);

            if (Portfolio.MACDCompare == 1) ret = !ret;

            return ret;
        }

        public bool checkSPYAvgSignal(DateTime dateToCheck)
        {
            decimal a, b;
            return checkSPYAvgSignal(dateToCheck, out a, out b);
        }

        public bool checkSPYAvgSignal(DateTime dateToCheck, out decimal avg50, out decimal avg200)
        {
            //get 50 days avg and 200 avg comparison
            avg50 = 0;
            avg200 = 0;

            try
            {
                int daysAvg1 = Convert.ToInt32(Portfolio.SPYAvgDays1);
                int daysAvg2 = Convert.ToInt32(Portfolio.SPYAvgDays2);

                avg50 = Convert.ToDecimal(DatabaseSingleton.Instance.GetData("select avg(close_price) from(select close_price from price_history where id_stock = -1 and date <= " + Utils.ConvertDateTimeToInt(dateToCheck) + " order by date desc limit " + daysAvg1.ToString() + ")")
                    .Rows[0][0]);
                avg200 = Convert.ToDecimal(DatabaseSingleton.Instance.GetData("select avg(close_price) from(select close_price from price_history where id_stock = -1 and date <= " + Utils.ConvertDateTimeToInt(dateToCheck) + " order by date desc limit " + daysAvg2.ToString() + ")")
                    .Rows[0][0]);

                if (avg50 < avg200)
                    return true;
            }
            catch (Exception e)
            { }
            
            return false;
        }


        public bool checkHiLoSignal(DateTime dateToCheck)
        {
            decimal a;
            return checkHiLoSignal(dateToCheck, out a);
        }

        public bool checkHiLoSignal(DateTime dateToCheck, out decimal hiLoPercent)
        {
            //get 40 days AVG for HiLow %
            hiLoPercent = 0;
            try
            {
                int daysAvg = Convert.ToInt32(Portfolio.HiLoDays);

                hiLoPercent = Convert.ToDecimal(DatabaseSingleton.Instance.GetData("select CAST(avg(VALUE) AS DOUBLE) from(select VALUE from SPY_HILOW where date <= " + Utils.ConvertDateTimeToInt(dateToCheck) + " order by date desc limit " + daysAvg.ToString() + ")")
                    .Rows[0][0]);

                if (hiLoPercent < Portfolio.HiLoPercent)
                    return true;
            }
            catch (Exception e)
            { }

            return false;
        }

        bool riskErrorMessage = false;
        public bool checkCustomSignal(DateTime dateToCheck)
        {
            try
            {
                return CustomRiskModelSingleton.Instance.GetSignal(dateToCheck);
            }
            catch (Exception e)
            {
                if (riskErrorMessage)
                    return false;

                riskErrorMessage = true;
                MessageBox.Show(e.Message, "Error with Custom File Risk Model", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            return false;
        }



        public bool checkCapeSignal(DateTime dateToCheck)
        {
            decimal a,b;
            return checkCapeSignal(dateToCheck, out a, out b);
        }
         
        public bool checkCapeSignal(DateTime dateToCheck, out decimal cape1, out decimal cape2)
        {
            //YoY Rate of Change of CAPE< 13
            //MoM Rate of Change of the YoY Rate of Change CAPE < 0
            cape1 = 0;
            cape2 = 0;
            try
            {
                DataTable data = DatabaseSingleton.Instance.GetData("SELECT YEAR, MONTH FROM CAPE_VALUE ORDER BY YEAR DESC, MONTH DESC LIMIT 1");
                int year = 1990;
                int month = 1;

                if (data.Rows.Count > 0)
                {
                    year = Convert.ToInt32(data.Rows[0][0]);
                    month = Convert.ToInt32(data.Rows[0][1]);
                }
                if(Convert.ToInt32(dateToCheck.ToString("yyyyMM")) > Convert.ToInt32(year.ToString() + month.ToString().PadLeft(2, '0')))
                    dateToCheck = new DateTime(year, month, 1);


                //get initial CAPE SMA
                DataTable resultsCape = DatabaseSingleton.Instance.GetData(
                    " select CAST(CAPE AS DECIMAL) CAPE, YEAR, MONTH " +
                    " FROM CAPE_VALUE WHERE(YEAR = " + dateToCheck.Year + " and MONTH <= " + dateToCheck.Month + ") OR year < " + dateToCheck.Year + " ORDER BY year DESC, month desc LIMIT 48");

                decimal sma1 = 0;
                decimal sma2 = 0;
                decimal sma3 = 0;
                int iRow = 0;

                List<decimal> capeValues = new List<decimal>();
                foreach (DataRow row in resultsCape.Rows)
                    capeValues.Add(Convert.ToDecimal(row[0]));

                capeValues.Reverse();

                //use first 10 values for initial SMA
                int countDiv = 0;
                for (iRow = 12; iRow < 12+10; iRow++)
                {
                    decimal capeCurr = Convert.ToDecimal(capeValues[iRow]);
                    decimal capeOld = Convert.ToDecimal(capeValues[iRow-12]);

                    if (capeOld != 0)
                    {
                        countDiv++;
                        sma1 += (capeCurr - capeOld) / capeOld * (decimal)100;
                    }
                }
                sma1 = sma1 / countDiv;

                //calc EMA
                for (; iRow < capeValues.Count; iRow++)
                {
                    decimal capeCurr = Convert.ToDecimal(capeValues[iRow]);
                    decimal capeOld = Convert.ToDecimal(capeValues[iRow - 12]);

                    if (capeOld == 0)
                        continue;

                    sma3 = sma2;
                    sma2 = sma1;

                    decimal currentValue = (capeCurr - capeOld) / capeOld * (decimal)100;

                    sma1 = ((currentValue - sma1) * (decimal)0.2) + sma1;
                }

                cape2 = 0;
                if(sma2 != 0)
                    cape2 = (sma1 - sma2) / sma2 * (decimal)100;

                decimal cape2Prev = 0;
                if (sma3 != 0)
                    cape2Prev = (sma2 - sma3) / sma3 * (decimal)100;

                //CAPE votes off when the EMA of YoY ROC is <-13 AND (MoM ROC of the EMA is < 0 OR MoM ROC < MoM ROC of 1 month ago)

                cape1 = sma1;
                if(sma1 < Portfolio.CapeYOY && (cape2 < Portfolio.CapeMOM || cape2 < cape2Prev))
                    return true;
            }
            catch (Exception e)
            { }

            return false;
        }

        public int getMonthsInterval()
        {
            return Portfolio.RebalanceFrequencyMonths;
        }

        public decimal getSPXUPercent()
        {
            return Portfolio.SPXUPercent / (decimal)100;
        }

        public void calcStopLossPrice(Position position)
        {
            //estimate stop loss price starting from purchase date
            position.StopLossPrice = 0;

            try
            {
                position.StopLossPrice = getInitialStopLossPrice(position.EntryPrice);

                DateTime currDate = Utils.ConvertIntToDateTime(position.DateEntered);
                while(Utils.ConvertDateTimeToInt(currDate) < Utils.ConvertDateTimeToInt(DateTime.Now))
                {
                    decimal stockPrice = Stock.GetPrice(position.IdStock, Utils.ConvertDateTimeToInt(currDate));
                    position.UpdateStopLossPrice(stockPrice, Portfolio);
                    currDate = currDate.AddDays(1);
                }
            }
            catch (Exception)
            {

            }
        }

        public PortfolioSnapshot GetLastRealSnapshot()
        {
            if (monthlySnapshots.Count == 0)
                return null;

            var lastPortfolio = monthlySnapshots.Last();
            var portfolioDate = lastPortfolio.Date;
            if (portfolioDate.Date.Day != DateTime.DaysInMonth(portfolioDate.Date.Year, portfolioDate.Date.Month))
                lastPortfolio = monthlySnapshots[monthlySnapshots.Count - 2];

            return lastPortfolio;
        }

        public List<Trade> GetTradesForDate(int date)
        {
            var result = new List<Trade>();

            foreach (var snapshot in snapshots)
                foreach (var trade in snapshot.Trades)
                    if (trade.RealEntryDate == date || (trade.RealExitDate == date && !trade.CurrentDateValorizationSell))
                        result.Add(trade);
                        
            return result;
        }

        public bool IsRebalanceDate(int date)
        {
            return rebalanceDates.Any(x => x == date);
        }

        public int GetLastRebalanceDate()
        {
            if (rebalanceDates.Count == 0)
                return 20050101;

            return rebalanceDates.Last();
        }

    }
}
