using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockRanking
{
    public class PortfolioParameters
    {
        public int Id = 0;
        public int AccountSize = 100000;
        public float BenchmarkPercent = 20;
        public float CommisionPerShare = 0.0035f;
        public int Positions = 5;
        public int LongSelling = 1;
        public int ShortSelling = 0;
        public int ShortPositions = 5;
        public decimal MaxWeightPerPosition = 0.25m;
        public int MaxPositionsPerIndustry = 0;
        public int RebalanceFrequencyMonths = 6;
        public List<String> IndustriesIncluded = new List<string>();
        public decimal EntryStopLoss = 30;
        public decimal TrailingStopLoss = 60;
        public int LossOnlyInitial = 0;

        public int UseRiskModel = 0;
        public decimal CapeYOY = 13;
        public decimal CapeMOM = 0;
        public decimal HiLoDays = 40;
        public decimal HiLoPercent = 5;
        public int SPYAvgDays1 = 50;
        public int SPYAvgDays2 = 200;
        public decimal SPXUPercent = 0;
        public int IdSymbolBenchmark = -1;
        public int IdSymbolInverse = -2;
        public String CustomRiskFile = "";
        public int BuyBenchmarkLeftoverCash = 1;
        public int ReBuyAfterStop = 0;
        public decimal AnnualFee = 2.2m;

        public int UseCapeModel = 0;
        public int UseAVGModel = 0;
        public int UseHiLoModel = 0;
        public int UseCustomFileModel = 0;
        public int UseMACDModel = 0;
        public int UseRSIModel = 0;
        public int UseStochasticModel = 0;

        public decimal MACDLoopback = 12;
        public decimal MACDLoopback1 = 26;
        public decimal MACDLoopback2 = 9;
        public decimal RSILoopback = 14;
        public decimal StochasticLoopback = 5;
        public int MACDCompare = 0;
        public int RSICompare = 0;
        public int StochasticCompare = 0;
        public decimal MACDThreshold = 0.0m;
        public decimal RSIThreshold = 0.0m;
        public decimal StochasticThreshold = 0.0m;

        public decimal MonthDelayEntry = 0;
        
        public decimal CompositePEWeight = 1.6m;
        public decimal CompositePSWeight = 1.2m;
        public decimal CompositePBWeight = 1m;
        public decimal CompositePFCFWeight = 1.4m;
        public int VRHistoryYears = 5;
        public decimal FilterComissionsPerc = 2m;
        public int FilterMinVolume = 1000;
        public long MaxSharesPennyStocks = 10000;

        //blended advanced backtest
        public decimal BABStocksPercent = 75;
        public decimal BABBondsPercent = 22;
        public decimal BABCashPercent = 3;
        public int BABAsset1Id = 0;
        public int BABAsset2Id = 0;
        public int BABAsset3Id = 0;
        public int BABAsset4Id = 0;
        public int BABRiskAssetConfigurable1 = 0;
        public int BABRiskAssetConfigurable2 = 0;
        public String BABCustomRiskFile = "";

        //bonds model
        public int BondsMomentumWindow = 3;
        public int BondsMovingAvg1 = 10;
        public int BondsMovingAvg2 = 20;
        public int BondsAsset1Id = 0;
        public int BondsAsset2Id = 0;
        public int BondsAsset3Id = 0;
        public int BondsAsset4Id = 0;
        public int BondsRiskModelAssetId = 0;

        static int compositeRollingMedianYears = -1;

        public int AuxHistoryId = -1;

        public MailingParameters MailingParameters = new MailingParameters();

        public int GetBondsRiskModelAssetId()
        {
            if (BondsRiskModelAssetId == 0)
                return BondsAsset1Id;

            return BondsRiskModelAssetId;
        }

        private static String getPortfolioFields()
        {
            return
                " ID, " + Utils.AddCastFromDB("PORTFOLIO_VALUE") + ",   " + Utils.AddCastFromDB("BENCHMARK_PERCENT") +
                ", " + Utils.AddCastFromDB("COMMISION_PER_SHARE") + ",  MAX_POSITIONS_PER_IND,  " +
                Utils.AddCastFromDB("MAX_WEIGHT_PER_POS") + ", LONG_SELLING, TOTAL_POSITIONS, SHORT_SELLING, TOTAL_SHORT_POSITIONS, INDUSTRIES, REBALANCING_MONTHS,  " +
                Utils.AddCastFromDB("ENTRY_STOP_LOSS") + ",   " + Utils.AddCastFromDB("TRAILING_STOP_LOSS") +
                ",  USE_RISK_MODEL,  " + Utils.AddCastFromDB("CAPE_YOY") + ",  " + Utils.AddCastFromDB("CAPE_MOM") +
                ",   " + Utils.AddCastFromDB("HILO_DAYS") + ",   " + Utils.AddCastFromDB("HILO_PERCENT") +
                ",  AVG_SPY_DAYS, AVG_SPY_2_DAYS,  " + Utils.AddCastFromDB("SPXU_PERCENT") +
                ",  ID_SYMBOL_BENCHMARK, ID_SYMBOL_INVERSE, USE_CAPE_MODEL, USE_AVG_MODEL, USE_HILO_MODEL " +
                ",  USE_CUSTOM_FILE_MODEL, CUSTOM_RISK_FILE,  " + Utils.AddCastFromDB("ANNUAL_FEE") +
                ",  BUY_BENCHMARK_WITH_CASH,  " + Utils.AddCastFromDB("PE_WEIGHT") + ", " + Utils.AddCastFromDB("PS_WEIGHT") +
                ",   " + Utils.AddCastFromDB("PB_WEIGHT") + ",   " + Utils.AddCastFromDB("PFCF_WEIGHT") + ",   " +
                Utils.AddCastFromDB("FILTER_COMISSION_PERC") +
                ",  FILTER_MIN_VOLUME, MAX_SHARES_PENNY_STOCK, VR_HISTORY_YEARS, " +
                Utils.AddCastFromDB("BAB_STOCK_PERC") + ",   " + Utils.AddCastFromDB("BAB_BOND_PERC") + ",   " +
                Utils.AddCastFromDB("BAB_CASH_PERC") + ", BAB_ASSET_1, BAB_ASSET_2, BAB_ASSET_3, BAB_ASSET_4, BAB_CUSTOM_RISK_FILE " +
                ",  BONDS_ASSET_1, BONDS_ASSET_2, BONDS_ASSET_3, BONDS_ASSET_4, BONDS_MOMENTUM_WINDOW, BONDS_MOVING_AVG1, BONDS_MOVING_AVG2, BAB_ASSET_RISK_1, BAB_ASSET_RISK_2, BONDS_RISK_ASSET_1," +
                " USE_MACD_MODEL, MACD_LOOPBACK, MACD_LOOPBACK1, MACD_LOOPBACK2, MACD_COMPARE, " + Utils.AddCastFromDB("MACD_THRESHOLD") + ", " +
                " USE_RSI_MODEL, RSI_LOOPBACK, RSI_COMPARE, " + Utils.AddCastFromDB("RSI_THRESHOLD") + ", " +
                " USE_STOCHASTIC_MODEL, STOCHASTIC_LOOPBACK, STOCHASTIC_COMPARE, " + Utils.AddCastFromDB("STOCHASTIC_THRESHOLD") + ", " +
                " LOSS_ONLY_INITIAL, MONTH_DELAY_ENTRY, RE_BUY_AFTER_STOP";
        }

        public static PortfolioParameters Load(int idStrategy)
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT " + getPortfolioFields() + " FROM PORFOLIO_PARAMETERS WHERE ID = " + idStrategy);

            if (data.Rows.Count > 0)
                return new PortfolioParameters(data.Rows[0]);

            //if no strategy found return default parameters
            if (idStrategy > 0)
                return Load(0);

            return new PortfolioParameters();
        }


        public static List<PortfolioParameters> LoadAll()
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT " + getPortfolioFields() + " FROM PORFOLIO_PARAMETERS WHERE ID > -1");

            var results = new List<PortfolioParameters>();

            //if no strategy found return default parameters
            foreach(DataRow row in data.Rows)
            {
                results.Add(new PortfolioParameters(row));
            }

            return results;
        }

        public PortfolioParameters()
        {

        }


        public PortfolioParameters(DataRow row)
        {
            Id = Convert.ToInt32(row["ID"]);
            AccountSize = Convert.ToInt32(row["PORTFOLIO_VALUE"]);
            BenchmarkPercent = Convert.ToSingle(row["BENCHMARK_PERCENT"]);
            CommisionPerShare = Convert.ToSingle(row["COMMISION_PER_SHARE"]);
            MaxPositionsPerIndustry = Convert.ToInt32(row["MAX_POSITIONS_PER_IND"]);
            MaxWeightPerPosition = Convert.ToDecimal(row["MAX_WEIGHT_PER_POS"]);
            Positions = Convert.ToInt32(row["TOTAL_POSITIONS"]);
            LongSelling = Convert.ToInt32(row["LONG_SELLING"]);
            ShortSelling = Convert.ToInt32(row["SHORT_SELLING"]);
            ShortPositions = Convert.ToInt32(row["TOTAL_SHORT_POSITIONS"]);
            RebalanceFrequencyMonths = Convert.ToInt32(row["REBALANCING_MONTHS"]);
            IndustriesIncluded = row["INDUSTRIES"].ToString().Split('|').ToList<String>();
             
            if (row["ENTRY_STOP_LOSS"].GetType() != typeof(DBNull))
                EntryStopLoss = Convert.ToDecimal(row["ENTRY_STOP_LOSS"]);

            if (row["TRAILING_STOP_LOSS"].GetType() != typeof(DBNull))
                TrailingStopLoss = Convert.ToDecimal(row["TRAILING_STOP_LOSS"]);

            if (row["USE_RISK_MODEL"].GetType() != typeof(DBNull))
                UseRiskModel= Convert.ToInt32(row["USE_RISK_MODEL"]);

            if (row["USE_CAPE_MODEL"].GetType() != typeof(DBNull))
                UseCapeModel = Convert.ToInt32(row["USE_CAPE_MODEL"]);

            if (row["USE_AVG_MODEL"].GetType() != typeof(DBNull))
                UseAVGModel = Convert.ToInt32(row["USE_AVG_MODEL"]);

            if (row["USE_HILO_MODEL"].GetType() != typeof(DBNull))
                UseHiLoModel = Convert.ToInt32(row["USE_HILO_MODEL"]);
             
            if (row["CAPE_YOY"].GetType() != typeof(DBNull))
                CapeYOY = Convert.ToDecimal(row["CAPE_YOY"]);
            if (row["CAPE_MOM"].GetType() != typeof(DBNull))
                CapeMOM = Convert.ToDecimal(row["CAPE_MOM"]);

            if (row["HILO_DAYS"].GetType() != typeof(DBNull))
                HiLoDays = Convert.ToDecimal(row["HILO_DAYS"]);
            if (row["HILO_PERCENT"].GetType() != typeof(DBNull))
                HiLoPercent = Convert.ToDecimal(row["HILO_PERCENT"]);

            if (row["AVG_SPY_DAYS"].GetType() != typeof(DBNull))
                SPYAvgDays1 = Convert.ToInt32(row["AVG_SPY_DAYS"]);
            if (row["AVG_SPY_2_DAYS"].GetType() != typeof(DBNull))
                SPYAvgDays2 = Convert.ToInt32(row["AVG_SPY_2_DAYS"]);

            if (row["SPXU_PERCENT"].GetType() != typeof(DBNull))
                SPXUPercent = Convert.ToDecimal(row["SPXU_PERCENT"]);


            if (row["ID_SYMBOL_BENCHMARK"].GetType() != typeof(DBNull))
                IdSymbolBenchmark = Convert.ToInt32(row["ID_SYMBOL_BENCHMARK"]);
            if (row["ID_SYMBOL_INVERSE"].GetType() != typeof(DBNull))
                IdSymbolInverse = Convert.ToInt32(row["ID_SYMBOL_INVERSE"]);

            if (row["USE_CUSTOM_FILE_MODEL"].GetType() != typeof(DBNull))
                UseCustomFileModel = Convert.ToInt32(row["USE_CUSTOM_FILE_MODEL"]);

            if (row["CUSTOM_RISK_FILE"].GetType() != typeof(DBNull))
                CustomRiskFile = row["CUSTOM_RISK_FILE"].ToString();

            if (row["ANNUAL_FEE"].GetType() != typeof(DBNull))
                AnnualFee = Convert.ToDecimal(row["ANNUAL_FEE"]);

            if (row["BUY_BENCHMARK_WITH_CASH"].GetType() != typeof(DBNull))
                BuyBenchmarkLeftoverCash = Convert.ToInt32(row["BUY_BENCHMARK_WITH_CASH"]);

            if (row["RE_BUY_AFTER_STOP"].GetType() != typeof(DBNull))
                ReBuyAfterStop = Convert.ToInt32(row["RE_BUY_AFTER_STOP"]);

            if (row["PE_WEIGHT"].GetType() != typeof(DBNull))
                CompositePEWeight = Convert.ToDecimal(row["PE_WEIGHT"]);

            if (row["PS_WEIGHT"].GetType() != typeof(DBNull))
                CompositePSWeight = Convert.ToDecimal(row["PS_WEIGHT"]);

            if (row["PB_WEIGHT"].GetType() != typeof(DBNull))
                CompositePBWeight = Convert.ToDecimal(row["PB_WEIGHT"]);

            if (row["PFCF_WEIGHT"].GetType() != typeof(DBNull))
                CompositePFCFWeight = Convert.ToDecimal(row["PFCF_WEIGHT"]);

            if (row["FILTER_COMISSION_PERC"].GetType() != typeof(DBNull))
                FilterComissionsPerc = Convert.ToDecimal(row["FILTER_COMISSION_PERC"]);
            if (row["FILTER_MIN_VOLUME"].GetType() != typeof(DBNull))
                FilterMinVolume = Convert.ToInt32(row["FILTER_MIN_VOLUME"]);
            if (row["MAX_SHARES_PENNY_STOCK"].GetType() != typeof(DBNull))
                MaxSharesPennyStocks = Convert.ToInt32(row["MAX_SHARES_PENNY_STOCK"]);

            if (row["USE_MACD_MODEL"].GetType() != typeof(DBNull))
                UseMACDModel = Convert.ToInt32(row["USE_MACD_MODEL"]);
            if (row["MACD_LOOPBACK"].GetType() != typeof(DBNull))
                MACDLoopback= Convert.ToDecimal(row["MACD_LOOPBACK"]);
            if (row["MACD_LOOPBACK1"].GetType() != typeof(DBNull))
                MACDLoopback1 = Convert.ToDecimal(row["MACD_LOOPBACK1"]);
            if (row["MACD_LOOPBACK2"].GetType() != typeof(DBNull))
                MACDLoopback2 = Convert.ToDecimal(row["MACD_LOOPBACK2"]);

            if (row["MACD_COMPARE"].GetType() != typeof(DBNull))
                MACDCompare = Convert.ToInt32(row["MACD_COMPARE"]);
            if (row["MACD_THRESHOLD"].GetType() != typeof(DBNull))
                MACDThreshold = Convert.ToDecimal(row["MACD_THRESHOLD"]);
            if (row["USE_RSI_MODEL"].GetType() != typeof(DBNull))
                UseRSIModel = Convert.ToInt32(row["USE_RSI_MODEL"]);
            if (row["RSI_LOOPBACK"].GetType() != typeof(DBNull))
                RSILoopback = Convert.ToDecimal(row["RSI_LOOPBACK"]);
            if (row["RSI_COMPARE"].GetType() != typeof(DBNull))
                RSICompare = Convert.ToInt32(row["RSI_COMPARE"]);
            if (row["RSI_THRESHOLD"].GetType() != typeof(DBNull))
                RSIThreshold = Convert.ToDecimal(row["RSI_THRESHOLD"]);
            if (row["USE_STOCHASTIC_MODEL"].GetType() != typeof(DBNull))
                UseStochasticModel = Convert.ToInt32(row["USE_STOCHASTIC_MODEL"]);
            if (row["STOCHASTIC_LOOPBACK"].GetType() != typeof(DBNull))
                StochasticLoopback = Convert.ToDecimal(row["STOCHASTIC_LOOPBACK"]);
            if (row["STOCHASTIC_COMPARE"].GetType() != typeof(DBNull))
                StochasticCompare = Convert.ToInt32(row["STOCHASTIC_COMPARE"]);
            if (row["STOCHASTIC_THRESHOLD"].GetType() != typeof(DBNull))
                StochasticThreshold = Convert.ToDecimal(row["STOCHASTIC_THRESHOLD"]);
            if (row["LOSS_ONLY_INITIAL"].GetType() != typeof(DBNull))
                LossOnlyInitial = Convert.ToInt32(row["LOSS_ONLY_INITIAL"]);
            if (row["MONTH_DELAY_ENTRY"].GetType() != typeof(DBNull))
                MonthDelayEntry = Convert.ToDecimal(row["MONTH_DELAY_ENTRY"]);

            VRHistoryYears = Convert.ToInt32(row["VR_HISTORY_YEARS"]);

            BABAsset1Id = Convert.ToInt32(row["BAB_ASSET_1"]);
            BABAsset2Id = Convert.ToInt32(row["BAB_ASSET_2"]);
            BABAsset3Id = Convert.ToInt32(row["BAB_ASSET_3"]);
            BABAsset4Id = Convert.ToInt32(row["BAB_ASSET_4"]);
            BABRiskAssetConfigurable1 = Convert.ToInt32(row["BAB_ASSET_RISK_1"]);
            BABRiskAssetConfigurable2 = Convert.ToInt32(row["BAB_ASSET_RISK_2"]);
            BABStocksPercent = Convert.ToDecimal(row["BAB_STOCK_PERC"]);
            BABBondsPercent = Convert.ToDecimal(row["BAB_BOND_PERC"]);
            BABCashPercent = Convert.ToDecimal(row["BAB_CASH_PERC"]);
            BABCustomRiskFile = Convert.ToString(row["BAB_CUSTOM_RISK_FILE"]);

            BondsAsset1Id = Convert.ToInt32(row["BONDS_ASSET_1"]);
            BondsAsset2Id = Convert.ToInt32(row["BONDS_ASSET_2"]);
            BondsAsset3Id = Convert.ToInt32(row["BONDS_ASSET_3"]);
            BondsAsset4Id = Convert.ToInt32(row["BONDS_ASSET_4"]);
            BondsRiskModelAssetId = Convert.ToInt32(row["BONDS_RISK_ASSET_1"]);
            BondsMomentumWindow = Convert.ToInt32(row["BONDS_MOMENTUM_WINDOW"]);
            BondsMovingAvg1 = Convert.ToInt32(row["BONDS_MOVING_AVG1"]);
            BondsMovingAvg2 = Convert.ToInt32(row["BONDS_MOVING_AVG2"]);

            if (IndustriesIncluded.Count > 0)
                if (IndustriesIncluded[0] == "")
                    IndustriesIncluded.Clear();

            if(Id > 0)
            {
                MailingParameters = MailingParameters.Load(Id);
            }
        }

        public static int GetCompositeRollingMedianYearsStatic()
        {
            if (compositeRollingMedianYears != -1)
                return compositeRollingMedianYears;

            DataTable data = DatabaseSingleton.Instance.GetData("SELECT VALUE FROM SYSTEM_PARAMETERS WHERE ID = 3");
            if (data.Rows.Count > 0)
                compositeRollingMedianYears = Convert.ToInt32(data.Rows[0][0]);
            else
            {
                DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO SYSTEM_PARAMETERS VALUES (3, 5)", null);
                compositeRollingMedianYears = 5;
            }

            if (compositeRollingMedianYears == -1)
                return 5;

            return compositeRollingMedianYears;
        }

        public static String[] GetPerformanceGraphBenchmarkSymbols()
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT VALUE FROM SYSTEM_PARAMETERS WHERE ID = " + (int)SystemParameters.BenchmarksToShow);
            if (data.Rows.Count == 0)
                return new String[0];

            return data.Rows[0][0].ToString().Split(',');
        }

        public static void SavePerformanceGraphBenchmarkSymbols(String[] symbols)
        {
            String result = String.Join(",", symbols);

            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM SYSTEM_PARAMETERS WHERE ID = " + (int)SystemParameters.BenchmarksToShow, null);

            DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO SYSTEM_PARAMETERS (ID, VALUE) VALUES (" + (int)SystemParameters.BenchmarksToShow + ", '" + result + "')", null);
        }

        public static void RefreshCompositeYearsValue()
        {
            compositeRollingMedianYears = -1;
        }

        public int GetCompositeRollingMedianYears()
        {
            return PortfolioParameters.GetCompositeRollingMedianYearsStatic();
        }

        public void Save()
        {
            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM PORFOLIO_PARAMETERS WHERE ID = " + this.Id, null);

            List<SQLiteParameter> parameters;
                   
            parameters = new List<SQLiteParameter>();

            parameters.Add(new SQLiteParameter("@p1", this.Id));
            parameters.Add(new SQLiteParameter("@p2", this.AccountSize));
            parameters.Add(new SQLiteParameter("@p3", this.BenchmarkPercent));
            parameters.Add(new SQLiteParameter("@p4", this.CommisionPerShare));
            parameters.Add(new SQLiteParameter("@p5", this.MaxPositionsPerIndustry));
            parameters.Add(new SQLiteParameter("@p6", this.MaxWeightPerPosition));
            parameters.Add(new SQLiteParameter("@p7", this.Positions));
            parameters.Add(new SQLiteParameter("@p8", this.getIndustriesString()));
            parameters.Add(new SQLiteParameter("@p9", this.RebalanceFrequencyMonths));
            parameters.Add(new SQLiteParameter("@p10", this.EntryStopLoss));
            parameters.Add(new SQLiteParameter("@p11", this.TrailingStopLoss));
            parameters.Add(new SQLiteParameter("@p12", this.UseRiskModel));
            parameters.Add(new SQLiteParameter("@p13", this.CapeYOY));
            parameters.Add(new SQLiteParameter("@p14", this.CapeMOM));
            parameters.Add(new SQLiteParameter("@p15", this.HiLoDays));
            parameters.Add(new SQLiteParameter("@p16", this.HiLoPercent));
            parameters.Add(new SQLiteParameter("@p17", this.SPYAvgDays1));
            parameters.Add(new SQLiteParameter("@p18", this.SPYAvgDays2));
            parameters.Add(new SQLiteParameter("@p19", this.SPXUPercent));
            parameters.Add(new SQLiteParameter("@p20", this.IdSymbolBenchmark));
            parameters.Add(new SQLiteParameter("@p21", this.IdSymbolInverse));
            parameters.Add(new SQLiteParameter("@p22", this.UseCapeModel));
            parameters.Add(new SQLiteParameter("@p23", this.UseAVGModel));
            parameters.Add(new SQLiteParameter("@p24", this.UseHiLoModel));
            parameters.Add(new SQLiteParameter("@p25", this.UseCustomFileModel));
            parameters.Add(new SQLiteParameter("@p26", this.CustomRiskFile));
            parameters.Add(new SQLiteParameter("@p27", this.AnnualFee));
            parameters.Add(new SQLiteParameter("@p28", this.BuyBenchmarkLeftoverCash));
            parameters.Add(new SQLiteParameter("@p29", this.CompositePEWeight));
            parameters.Add(new SQLiteParameter("@p30", this.CompositePSWeight));
            parameters.Add(new SQLiteParameter("@p31", this.CompositePBWeight));
            parameters.Add(new SQLiteParameter("@p32", this.CompositePFCFWeight));
            parameters.Add(new SQLiteParameter("@p33", this.FilterComissionsPerc));
            parameters.Add(new SQLiteParameter("@p34", this.FilterMinVolume));
            parameters.Add(new SQLiteParameter("@p35", this.MaxSharesPennyStocks));
            parameters.Add(new SQLiteParameter("@p36", this.VRHistoryYears));

            parameters.Add(new SQLiteParameter("@p60", this.UseMACDModel));
            parameters.Add(new SQLiteParameter("@p61", this.MACDLoopback));
            parameters.Add(new SQLiteParameter("@p62", this.MACDLoopback1));
            parameters.Add(new SQLiteParameter("@p63", this.MACDLoopback2));
            parameters.Add(new SQLiteParameter("@p64", this.MACDCompare));
            parameters.Add(new SQLiteParameter("@p65", this.MACDThreshold));
            parameters.Add(new SQLiteParameter("@p66", this.UseRSIModel));
            parameters.Add(new SQLiteParameter("@p67", this.RSILoopback));
            parameters.Add(new SQLiteParameter("@p68", this.RSICompare));
            parameters.Add(new SQLiteParameter("@p69", this.RSIThreshold));
            parameters.Add(new SQLiteParameter("@p70", this.UseStochasticModel));
            parameters.Add(new SQLiteParameter("@p71", this.StochasticLoopback));
            parameters.Add(new SQLiteParameter("@p72", this.StochasticCompare));
            parameters.Add(new SQLiteParameter("@p73", this.StochasticThreshold));
            parameters.Add(new SQLiteParameter("@p74", this.LossOnlyInitial));
            parameters.Add(new SQLiteParameter("@p75", this.MonthDelayEntry));
            parameters.Add(new SQLiteParameter("@p76", this.ReBuyAfterStop));
            parameters.Add(new SQLiteParameter("@p77", this.LongSelling));
            parameters.Add(new SQLiteParameter("@p78", this.ShortSelling));
            parameters.Add(new SQLiteParameter("@p79", this.ShortPositions));

            DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO PORFOLIO_PARAMETERS (ID, PORTFOLIO_VALUE, BENCHMARK_PERCENT, COMMISION_PER_SHARE, MAX_POSITIONS_PER_IND, MAX_WEIGHT_PER_POS, TOTAL_POSITIONS, INDUSTRIES, REBALANCING_MONTHS, ENTRY_STOP_LOSS, TRAILING_STOP_LOSS, USE_RISK_MODEL, CAPE_YOY , CAPE_MOM , HILO_DAYS, HILO_PERCENT, AVG_SPY_DAYS, AVG_SPY_2_DAYS, SPXU_PERCENT, ID_SYMBOL_BENCHMARK, ID_SYMBOL_INVERSE, USE_CAPE_MODEL, USE_AVG_MODEL, USE_HILO_MODEL, USE_CUSTOM_FILE_MODEL, CUSTOM_RISK_FILE, ANNUAL_FEE, BUY_BENCHMARK_WITH_CASH, PE_WEIGHT, PS_WEIGHT, PB_WEIGHT, PFCF_WEIGHT, FILTER_COMISSION_PERC, FILTER_MIN_VOLUME, MAX_SHARES_PENNY_STOCK, VR_HISTORY_YEARS, " +
                "USE_MACD_MODEL, MACD_LOOPBACK, MACD_LOOPBACK1, MACD_LOOPBACK2, MACD_COMPARE, MACD_THRESHOLD, USE_RSI_MODEL, RSI_LOOPBACK, RSI_COMPARE, RSI_THRESHOLD, " +
                "USE_STOCHASTIC_MODEL, STOCHASTIC_LOOPBACK, STOCHASTIC_COMPARE, STOCHASTIC_THRESHOLD, LOSS_ONLY_INITIAL, MONTH_DELAY_ENTRY, RE_BUY_AFTER_STOP, LONG_SELLING, SHORT_SELLING, TOTAL_SHORT_POSITIONS) " +
                " VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18,@p19,@p20,@p21,@p22,@p23,@p24,@p25,@p26,@p27,@p28,@p29,@p30,@p31,@p32,@p33,@p34,@p35,@p36," +
                "@p60,@p61,@p62,@p63,@p64,@p65,@p66,@p67,@p68,@p69,@p70,@p71,@p72,@p73,@p74,@p75,@p76,@p77,@p78,@p79)", parameters.ToArray());


            if (this.Id == 0)
            {
                //update blended benchmark for all portfolio parameters
                parameters = new List<SQLiteParameter>();

                parameters.Add(new SQLiteParameter("@p37", this.BABStocksPercent));
                parameters.Add(new SQLiteParameter("@p38", this.BABBondsPercent));
                parameters.Add(new SQLiteParameter("@p39", this.BABCashPercent));

                parameters.Add(new SQLiteParameter("@p40", this.BABAsset1Id));
                parameters.Add(new SQLiteParameter("@p41", this.BABAsset2Id));
                parameters.Add(new SQLiteParameter("@p42", this.BABAsset3Id));
                parameters.Add(new SQLiteParameter("@p43", this.BABAsset4Id));

                parameters.Add(new SQLiteParameter("@p44", this.BABCustomRiskFile));

                parameters.Add(new SQLiteParameter("@p45", this.BondsAsset1Id));
                parameters.Add(new SQLiteParameter("@p46", this.BondsAsset2Id));
                parameters.Add(new SQLiteParameter("@p47", this.BondsAsset3Id));
                parameters.Add(new SQLiteParameter("@p48", this.BondsAsset4Id));
                parameters.Add(new SQLiteParameter("@p49", this.BondsMomentumWindow));
                parameters.Add(new SQLiteParameter("@p50", this.BondsMovingAvg1));
                parameters.Add(new SQLiteParameter("@p51", this.BondsMovingAvg2));

                parameters.Add(new SQLiteParameter("@p52", this.BABRiskAssetConfigurable1));
                parameters.Add(new SQLiteParameter("@p53", this.BABRiskAssetConfigurable2));
                parameters.Add(new SQLiteParameter("@p54", this.BondsRiskModelAssetId));

                DatabaseSingleton.Instance.ExecuteNonQuery("UPDATE PORFOLIO_PARAMETERS SET " +
                    " BAB_STOCK_PERC = @p37, BAB_BOND_PERC = @p38, BAB_CASH_PERC = @p39, BAB_ASSET_1 = @p40, BAB_ASSET_2 = @p41, BAB_ASSET_3 = @p42, BAB_ASSET_4 = @p43, BAB_CUSTOM_RISK_FILE = @p44, " +
                    " BONDS_ASSET_1 = @p45, BONDS_ASSET_2 = @p46, BONDS_ASSET_3 = @p47, BONDS_ASSET_4 = @p48, BONDS_MOMENTUM_WINDOW = @p49, BONDS_MOVING_AVG1 = @p50, BONDS_MOVING_AVG2 = @p51, " +
                    " BAB_ASSET_RISK_1 = @p52, BAB_ASSET_RISK_2 = @p53, BONDS_RISK_ASSET_1 = @p54 ", parameters.ToArray());
            }
            else
            {
                //overwrite from default portfolio
                parameters = new List<SQLiteParameter>();

                var defaultPortfolio = PortfolioParameters.Load(0);

                parameters.Add(new SQLiteParameter("@p37", defaultPortfolio.BABStocksPercent));
                parameters.Add(new SQLiteParameter("@p38", defaultPortfolio.BABBondsPercent));
                parameters.Add(new SQLiteParameter("@p39", defaultPortfolio.BABCashPercent));

                parameters.Add(new SQLiteParameter("@p40", defaultPortfolio.BABAsset1Id));
                parameters.Add(new SQLiteParameter("@p41", defaultPortfolio.BABAsset2Id));
                parameters.Add(new SQLiteParameter("@p42", defaultPortfolio.BABAsset3Id));
                parameters.Add(new SQLiteParameter("@p43", defaultPortfolio.BABAsset4Id));

                parameters.Add(new SQLiteParameter("@p44", defaultPortfolio.BABCustomRiskFile));

                parameters.Add(new SQLiteParameter("@p45", defaultPortfolio.BondsAsset1Id));
                parameters.Add(new SQLiteParameter("@p46", defaultPortfolio.BondsAsset2Id));
                parameters.Add(new SQLiteParameter("@p47", defaultPortfolio.BondsAsset3Id));
                parameters.Add(new SQLiteParameter("@p48", defaultPortfolio.BondsAsset4Id));
                parameters.Add(new SQLiteParameter("@p49", defaultPortfolio.BondsMomentumWindow));
                parameters.Add(new SQLiteParameter("@p50", defaultPortfolio.BondsMovingAvg1));
                parameters.Add(new SQLiteParameter("@p51", defaultPortfolio.BondsMovingAvg2));

                parameters.Add(new SQLiteParameter("@p52", defaultPortfolio.BABRiskAssetConfigurable1));
                parameters.Add(new SQLiteParameter("@p53", defaultPortfolio.BABRiskAssetConfigurable1));
                parameters.Add(new SQLiteParameter("@p54", defaultPortfolio.BondsRiskModelAssetId));

                parameters.Add(new SQLiteParameter("@p55", this.Id));

                DatabaseSingleton.Instance.ExecuteNonQuery("UPDATE PORFOLIO_PARAMETERS SET " +
                    " BAB_STOCK_PERC = @p37, BAB_BOND_PERC = @p38, BAB_CASH_PERC = @p39, BAB_ASSET_1 = @p40, BAB_ASSET_2 = @p41, BAB_ASSET_3 = @p42, BAB_ASSET_4 = @p43, BAB_CUSTOM_RISK_FILE = @p44, " +
                    " BONDS_ASSET_1 = @p45, BONDS_ASSET_2 = @p46, BONDS_ASSET_3 = @p47, BONDS_ASSET_4 = @p48, BONDS_MOMENTUM_WINDOW = @p49, BONDS_MOVING_AVG1 = @p50, BONDS_MOVING_AVG2 = @p51, " +
                    " BAB_ASSET_RISK_1 = @p52, BAB_ASSET_RISK_2 = @p53, BONDS_RISK_ASSET_1 = @p54 WHERE ID = @p55", parameters.ToArray());
            }
        }

        public String getIndustriesString()
        {
            String result = "";
            foreach(String ind in IndustriesIncluded)
            {
                if (result.Length > 0)
                    result += "|";
                result += ind;
            }

            return result;
        }


        public static List<Position> GetCurrentPortfolio()
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT * FROM PORTFOLIO_POSITION");
            List<Position> positions = new List<Position>();

            foreach(DataRow row in data.Rows)
            {
                positions.Add(new Position(row));
            }

            return positions;
        }

        public static void SaveCurrentPortfolio(List<Position> positions)
        {
            DatabaseSingleton.Instance.StartTransaction();

            try
            {
                DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM PORTFOLIO_POSITION", null);

                foreach (Position pos in positions)
                {

                    List<SQLiteParameter> parameters;

                    parameters = new List<SQLiteParameter>();

                    parameters.Add(new SQLiteParameter("@p1", pos.IdStock));
                    parameters.Add(new SQLiteParameter("@p2", pos.Shares));
                    parameters.Add(new SQLiteParameter("@p3", pos.EntryPrice));
                    parameters.Add(new SQLiteParameter("@p4", pos.DateEntered));

                    DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO PORTFOLIO_POSITION (ID_STOCK, SHARES, ENTRY_PRICE, ENTRY_DATE) VALUES (@p1,@p2,@p3,@p4)", parameters.ToArray());
                }

                DatabaseSingleton.Instance.EndTransaction();
            }
            catch (Exception e)
            {
                DatabaseSingleton.Instance.RollbackTransaction();
            }
            
        }

        public string GetFrequencyText()
        {
            switch(RebalanceFrequencyMonths)
            {
                case 1:
                    return "Monthly";
                    break;
                case 3:
                    return "Quarter";
                    break;
                case 6:
                    return "Six Months";
                    break;
                case 12:
                    return "Annually";
                    break;
            }

            return "";
        }

        public string GetRiskModelText()
        {
            String riskModel = "";

            if (UseAVGModel == 1)
                riskModel += "AVG ";

            if (UseCapeModel == 1)
                riskModel += "CAPE ";

            if (UseHiLoModel == 1)
                riskModel += "HILO ";

            if (UseCustomFileModel == 1)
                riskModel += "CUSTOM ";

            return riskModel.Trim();
        }
    }
}
