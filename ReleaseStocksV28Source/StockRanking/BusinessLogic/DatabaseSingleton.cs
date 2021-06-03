using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Windows.Forms;

namespace StockRanking
{
    public class DatabaseSingleton
    {

        static DatabaseSingleton instance = null;
        //Realm realm = null;
        //RealmConfiguration config = null;

        SQLiteConnection dbConnection = null;
        SQLiteCommand storedCommand = null;
        public SQLiteTransaction currentTransaction = null;

        public static DatabaseSingleton Instance
        {
            get {
                if (instance == null)
                    instance = new DatabaseSingleton();

                return instance;
            }
        }

        public DatabaseSingleton(bool secondaryConnection)
        {
            if (secondaryConnection == false)
                throw new Exception("This is just to generate read only connections");
#if TESTDEV || DEBUG
            dbConnection = new SQLiteConnection("Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\stockstestdb.dat" + ";Version=3;");
#else
            dbConnection = new SQLiteConnection("Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\stocksdb5.dat" + ";Version=3;");
#endif
            dbConnection.Open();
        }

        public DatabaseSingleton()
        {
            //check if database file exists, if not: create
#if !(TESTDEV || DEBUG)
            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\stocksdb5.dat"))
            {
                SQLiteConnection.CreateFile(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\stocksdb5.dat");
            }

            //connect database
            dbConnection = new SQLiteConnection("Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\stocksdb5.dat" + ";Version=3;");
#else
            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\stockstestdb.dat"))
            {
                SQLiteConnection.CreateFile(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\stockstestdb.dat");
            }

            //connect database
            dbConnection = new SQLiteConnection("Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\stockstestdb.dat" + ";Version=3;");
#endif
            dbConnection.Open();

            var attributes = (new Median()).GetType().GetCustomAttributes(typeof(SQLiteFunctionAttribute), true).Cast<SQLiteFunctionAttribute>().ToArray();
            dbConnection.BindFunction(attributes[0], new Median());

            //check structure and create tables if needed
            CreateDataTables();

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS PORFOLIO_PARAMETERS (ID INTEGER, PORTFOLIO_VALUE REAL, BENCHMARK_PERCENT REAL, " +
                " COMMISION_PER_SHARE REAL, MAX_POSITIONS_PER_IND INTEGER, MAX_WEIGHT_PER_POS REAL, LONG_SELLING INTEGER DEFAULT 1, TOTAL_POSITIONS INTEGER, SHORT_SELLING INTEGER DEFAULT 0, TOTAL_SHORT_POSITIONS INTEGER, INDUSTRIES TEXT, " +
                " REBALANCING_MONTHS NUMBER, ENTRY_STOP_LOSS DECIMAL, TRAILING_STOP_LOSS DECIMAL, USE_RISK_MODEL NUMBER, CAPE_YOY DECIMAL, " +
                " CAPE_MOM DECIMAL, HILO_DAYS DECIMAL, HILO_PERCENT DECIMAL, AVG_SPY_DAYS INTEGER, AVG_SPY_2_DAYS INTEGER, SPXU_PERCENT DECIMAL, " +
                " ID_SYMBOL_BENCHMARK INTEGER, ID_SYMBOL_INVERSE INTEGER, USE_CAPE_MODEL NUMBER, USE_AVG_MODEL NUMBER, USE_HILO_MODEL NUMBER, " +
                " USE_CUSTOM_FILE_MODEL NUMBER, CUSTOM_RISK_FILE TEXT, ANNUAL_FEE DECIMAL, BUY_BENCHMARK_WITH_CASH NUMBER, PE_WEIGHT DECIMAL, " + 
                " PS_WEIGHT DECIMAL, PB_WEIGHT DECIMAL, PFCF_WEIGHT DECIMAL, FILTER_COMISSION_PERC DECIMAL, FILTER_MIN_VOLUME NUMBER, MAX_SHARES_PENNY_STOCK NUMBER, VR_HISTORY_YEARS INTEGER, " +
                " BAB_STOCK_PERC DECIMAL DEFAULT 75, BAB_BOND_PERC DECIMAL DEFAULT 22, BAB_CASH_PERC DECIMAL DEFAULT 3, BAB_ASSET_1 INTEGER DEFAULT 0, BAB_ASSET_2 INTEGER DEFAULT 0, BAB_ASSET_3 INTEGER DEFAULT 0, BAB_ASSET_4 INTEGER DEFAULT 0, BAB_CUSTOM_RISK_FILE TEXT, " +
                " BONDS_ASSET_1 INTEGER DEFAULT 0, BONDS_ASSET_2 INTEGER DEFAULT 0, BONDS_ASSET_3 INTEGER DEFAULT 0, BONDS_ASSET_4 INTEGER DEFAULT 0, BONDS_MOMENTUM_WINDOW INTEGER DEFAULT 3, BONDS_MOVING_AVG1 INTEGER DEFAULT 10, BONDS_MOVING_AVG2 INTEGER DEFAULT 20, BAB_ASSET_RISK_1 INTEGER DEFAULT -1, BAB_ASSET_RISK_2 INTEGER DEFAULT -11, BONDS_RISK_ASSET_1 INTEGER DEFAULT 0, " +
                " USE_MACD_MODEL NUMBER, MACD_LOOPBACK DECIMAL, MACD_LOOPBACK1 DECIMAL, MACD_LOOPBACK2 DECIMAL, MACD_COMPARE NUMBER, MACD_THRESHOLD DECIMAL, " +
                " USE_RSI_MODEL NUMBER, RSI_LOOPBACK DECIMAL, RSI_COMPARE NUMBER, RSI_THRESHOLD DECIMAL, " +
                " USE_STOCHASTIC_MODEL NUMBER, STOCHASTIC_LOOPBACK DECIMAL, STOCHASTIC_COMPARE NUMBER, STOCHASTIC_THRESHOLD DECIMAL, " +
                " LOSS_ONLY_INITIAL NUMBER, MONTH_DELAY_ENTRY DECIMAL, RE_BUY_AFTER_STOP NUMBER DEFAULT 0)", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS STRATEGY_DESCRIPTION (ID INTEGER, DESCRIPTION TEXT)", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS STRATEGY_MAILING (ID_STRATEGY INTEGER, EMAILS TEXT, SEND_REBALANCE NUMBER DEFAULT 0, SEND_WEEKDAYS TEXT DEFAULT '', SEND_SELLS NUMBER DEFAULT 0, INCLUDE_PORTFOLIO NUMBER DEFAULT 0, INCLUDE_TOP_STOCKS NUMBER DEFAULT 50, INCLUDE_RISK_MODEL NUMBER DEFAULT 0, INCLUDE_BONDS_MODEL NUMBER DEFAULT 0, INCLUDE_PERFORMANCE_TABLE NUMBER DEFAULT 0, LAST_MAIL_SENT NUMBER DEFAULT 0)", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS STRATEGY (ID INTEGER, NAME TEXT, BENCHMARK_RESULT REAL, SELECTED NUMBER DEFAULT 0)", null);
            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS STRATEGY_FEATURE (ID_STRATEGY INTEGER, ID_FEATURE INTEGER, WEIGHT REAL)", null);
            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS STRATEGY_OPTIMAL_FEATURE (ID_STRATEGY INTEGER, ID_FEATURE INTEGER, WEIGHT REAL)", null);
            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS STRATEGY_FILTER (ID_STRATEGY INTEGER, FILTER_TYPE INTEGER, CONDITION INTEGER, PARAMETER1 REAL, PARAMETER2 REAL, APPLYORDER INTEGER, APPLYOPTION INTEGER)", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS PORTFOLIO_POSITION (ID_STOCK INTEGER, SHARES INTEGER, ENTRY_PRICE DECIMAL, ENTRY_DATE INTEGER)", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS SECTOR (ID INTEGER, NAME TEXT)", null);
            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS SECTOR_INDUSTRY (ID_SECTOR INTEGER, INDUSTRY TEXT)", null);

            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'STRATEGY_FEATURE_IX' ON 'STRATEGY_FEATURE'(ID_STRATEGY)", null);
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'STRATEGY_OPTIMAL_FEATURE_IX' ON 'STRATEGY_OPTIMAL_FEATURE'(ID_STRATEGY)", null);
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'STRATEGY_FILTER_IX' ON 'STRATEGY_FILTER'(ID_STRATEGY)", null);
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'STRATEGY_MAILING_IX' ON 'STRATEGY_MAILING'(ID_STRATEGY)", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS STOCK_FILE (ID INTEGER, FILE_PATH TEXT, KEEP_UPDATED INTEGER)", null);
            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS STOCK_FILE_PRICE (ID_STOCK INTEGER, DATE INTEGER, VALUE DECIMAL)", null);
            
            ExecuteNonQuery("PRAGMA journal_mode = TRUNCATE", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS EQUATIONFILTER (ID INTEGER, FILTERNAME TEXT, FILTERTYPE NUMBER DEFAULT 0, FILTEROPTION NUMBER DEFAULT 0, VALUE1 DECIMAL DEFAULT 0, VALUE2 DECIMAL DEFAULT 0, EQUATION1 TEXT, EQUATION2 TEXT, EQUCOMPARE NUMBER DEFAULT 0)", null);

            fillSectorTables();

            ExecuteNonQuery("PRAGMA cache_size = -10000", null);

            UpdateTable_V1();
            UpdateTable_V2();
            UpdateTable();

        }

        private void UpdateTable_V1()
        {
            try
            {
                GetData("SELECT COR FROM RATIO LIMIT 1");
            }
            catch (Exception e)
            {
                try
                {
                    ExecuteNonQuery("DROP INDEX  IF EXISTS RATIO_DATE_X", null);
                    ExecuteNonQuery("DROP INDEX  IF EXISTS RATIO_DATE_Y", null);
                    ExecuteNonQuery("DROP TABLE  IF EXISTS RATIO", null);

                    ExecuteNonQuery("CREATE TABLE IF NOT EXISTS RATIO (ID_STOCK INTEGER, EBIT DECIMAL, EBITUSD DECIMAL, EBITMARGIN DECIMAL, SHARES_OUT NUMBER, TOT_LTERM_DEBT DECIMAL, FCF REAL, ROIC REAL, CURR_RATIO REAL, DIVIDEND_YIELD REAL, EV DECIMAL, BVPS DECIMAL, INVESTED_CAPITAL DECIMAL, MARKET_CAP DECIMAL, SALES_USD DECIMAL, EPS DECIMAL, PE DECIMAL, PS DECIMAL, PB DECIMAL, PFCF DECIMAL, STOCK_PRICE DECIMAL, SPS DECIMAL, FCFPS DECIMAL, DATE INTEGER, SNAP_DATE INTEGER, QUARTER INTEGER, YEAR INTEGER, DATA_RANGE TEXT, DATE_FIRST_MONTH INTEGER, DATE_KEY INTEGER," +
                        "COR DECIMAL, NETINC DECIMAL, EPSDIL DECIMAL, SHARESWA DECIMAL, CAPEX DECIMAL, ASSETS DECIMAL, CASHNEQ DECIMAL, LIABILITIES DECIMAL, ASSETSC DECIMAL, LIABILITIESC DECIMAL, TANGIBLES DECIMAL, ROE DECIMAL, ROA DECIMAL, GP DECIMAL, GROSSMARGIN  DECIMAL, NETMARGIN DECIMAL, ROS DECIMAL, ASSETTURNOVER DECIMAL, PAYOUTRATIO DECIMAL, WORKINGCAPITAL DECIMAL, TBVPS DECIMAL)", null);

                    ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'RATIO_DATE_X' ON 'RATIO'(ID_STOCK, DATE)", null);
                    ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'RATIO_DATE_Y' ON 'RATIO'(ID_STOCK, YEAR)", null);

                    ExecuteNonQuery("DELETE FROM SYSTEM_PARAMETERS WHERE ID = 10", null);
                    ExecuteNonQuery("INSERT INTO SYSTEM_PARAMETERS (ID, VALUE) VALUES (10, 'True')", null);
                }
                catch (Exception e2)
                {
                }
            }

            if (GetData("SELECT VALUE FROM SYSTEM_PARAMETERS WHERE ID = 10").Rows.Count > 0)
            {
                ExecuteNonQuery("DROP INDEX  IF EXISTS RATIO_DATE_X", null);
                ExecuteNonQuery("DROP INDEX  IF EXISTS RATIO_DATE_Y", null);
                ExecuteNonQuery("DROP TABLE  IF EXISTS RATIO", null);

                ExecuteNonQuery("CREATE TABLE IF NOT EXISTS RATIO (ID_STOCK INTEGER, EBIT DECIMAL, EBITUSD DECIMAL, EBITMARGIN DECIMAL, SHARES_OUT NUMBER, TOT_LTERM_DEBT DECIMAL, FCF REAL, ROIC REAL, CURR_RATIO REAL, DIVIDEND_YIELD REAL, EV DECIMAL, BVPS DECIMAL, INVESTED_CAPITAL DECIMAL, MARKET_CAP DECIMAL, SALES_USD DECIMAL, EPS DECIMAL, PE DECIMAL, PS DECIMAL, PB DECIMAL, PFCF DECIMAL, STOCK_PRICE DECIMAL, SPS DECIMAL, FCFPS DECIMAL, DATE INTEGER, SNAP_DATE INTEGER, QUARTER INTEGER, YEAR INTEGER, DATA_RANGE TEXT, DATE_FIRST_MONTH INTEGER, DATE_KEY INTEGER," +
                    "COR DECIMAL, NETINC DECIMAL, EPSDIL DECIMAL, SHARESWA DECIMAL, CAPEX DECIMAL, ASSETS DECIMAL, CASHNEQ DECIMAL, LIABILITIES DECIMAL, ASSETSC DECIMAL, LIABILITIESC DECIMAL, TANGIBLES DECIMAL, ROE DECIMAL, ROA DECIMAL, GP DECIMAL, GROSSMARGIN  DECIMAL, NETMARGIN DECIMAL, ROS DECIMAL, ASSETTURNOVER DECIMAL, PAYOUTRATIO DECIMAL, WORKINGCAPITAL DECIMAL, TBVPS DECIMAL)", null);

                ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'RATIO_DATE_X' ON 'RATIO'(ID_STOCK, DATE)", null);
                ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'RATIO_DATE_Y' ON 'RATIO'(ID_STOCK, YEAR)", null);

                ExecuteNonQuery("DELETE FROM SYSTEM_PARAMETERS WHERE ID = 10", null);
                ExecuteNonQuery("INSERT INTO SYSTEM_PARAMETERS (ID, VALUE) VALUES (10, 'True')", null);
            }
        }

        private void UpdateTable_V2()
        {
            try
            {
                GetData("SELECT MONTH_DELAY_ENTRY FROM PORFOLIO_PARAMETERS LIMIT 1");
            }
            catch (Exception e)
            {
                try
                {
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN USE_MACD_MODEL NUMBER DEFAULT 0 ", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN MACD_LOOPBACK DECIMAL DEFAULT 12 ", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN MACD_LOOPBACK1 DECIMAL DEFAULT 26 ", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN MACD_LOOPBACK2 DECIMAL DEFAULT 9", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN MACD_COMPARE NUMBER DEFAULT 0", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN MACD_THRESHOLD DECIMAL DEFAULT 0", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN USE_RSI_MODEL NUMBER DEFAULT 0", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN RSI_LOOPBACK DECIMAL DEFAULT 14", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN RSI_COMPARE NUMBER DEFAULT 0 ", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN RSI_THRESHOLD DECIMAL DEFAULT 0 ", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN USE_STOCHASTIC_MODEL NUMBER DEFAULT 0 ", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN STOCHASTIC_LOOPBACK DECIMAL DEFAULT 5", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN STOCHASTIC_COMPARE NUMBER DEFAULT 0", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN STOCHASTIC_THRESHOLD DECIMAL DEFAULT 0", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN LOSS_ONLY_INITIAL NUMBER DEFAULT 0", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN MONTH_DELAY_ENTRY DECIMAL DEFAULT 0", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN RE_BUY_AFTER_STOP NUMBER DEFAULT 0", null);
                }
                catch (Exception e2)
                {
                }
            }

            try
            {
                GetData("SELECT APPLYORDER FROM STRATEGY_FILTER LIMIT 1");
            }
            catch (Exception e)
            {
                try
                {
                    ExecuteNonQuery("ALTER TABLE STRATEGY_FILTER ADD COLUMN APPLYORDER INTEGER DEFAULT 0 ", null);
                    ExecuteNonQuery("ALTER TABLE STRATEGY_FILTER ADD COLUMN APPLYOPTION INTEGER DEFAULT 0 ", null);
                }
                catch (Exception e2)
                {
                }
            }

            try
            {
                GetData("SELECT ISCUSTOM FROM STRATEGY_FILTER LIMIT 1");
            }
            catch (Exception e)
            {
                try
                {
                    ExecuteNonQuery("ALTER TABLE STRATEGY_FILTER ADD COLUMN EQUATION1 TEXT", null);
                    ExecuteNonQuery("ALTER TABLE STRATEGY_FILTER ADD COLUMN EQUATION2 TEXT", null);
                    ExecuteNonQuery("ALTER TABLE STRATEGY_FILTER ADD COLUMN ISCUSTOM INTEGER DEFAULT 0", null);
                    ExecuteNonQuery("ALTER TABLE STRATEGY_FILTER ADD COLUMN EQUCOMPARE INTEGER DEFAULT 0", null);
                }
                catch (Exception e2)
                {
                }
            }
        }

        private void UpdateTable()
        {
            try
            {
                GetData("SELECT LONG_SELLING FROM PORFOLIO_PARAMETERS LIMIT 1");
            }
            catch (Exception e)
            {
                try
                {
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN LONG_SELLING INTEGER DEFAULT 1 ", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN SHORT_SELLING INTEGER DEFAULT 0 ", null);
                    ExecuteNonQuery("ALTER TABLE PORFOLIO_PARAMETERS ADD COLUMN TOTAL_SHORT_POSITIONS INTEGER DEFAULT 5 ", null);
                }
                catch (Exception e2)
                {
                }
            }
        }
        public void CreateDataTables()
        {
            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS STOCK (ID INTEGER, SYMBOL TEXT, COMPANY_NAME TEXT, INDUSTRY TEXT, INDUSTRY2 TEXT, CURRENCY TEXT, SECTOR TEXT, EXCHAGE TEXT, ISFOREIGN NUMBER, RELATED_TICKERS TEXT, TYPE INTEGER DEFAULT 0)", null);
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'STOCK_ID_X' ON 'STOCK'('ID')", null);
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'STOCK_ID_SYMBOL' ON 'STOCK'('SYMBOL')", null);
            ExecuteNonQuery("insert into stock SELECT * FROM(SELECT - 1, 'SPY', 'SPY Index', 'None', 'None', 'USD', 'None', 'NASDAQ', '0', '',1) WHERE NOT EXISTS(SELECT ID FROM STOCK WHERE ID = -1) LIMIT 1", null);
            ExecuteNonQuery("insert into stock SELECT * FROM(SELECT - 2, 'SPXU', 'SPXU Index', 'None', 'None', 'USD', 'None', 'NASDAQ', '0', '',1) WHERE NOT EXISTS(SELECT ID FROM STOCK WHERE ID = -2) LIMIT 1", null);
            ExecuteNonQuery("insert into stock SELECT * FROM(SELECT - 3, 'QQQ', 'PowerShares QQQ Trust, Series 1', 'None', 'None', 'USD', 'None', 'NASDAQ', '0', '',1) WHERE NOT EXISTS(SELECT ID FROM STOCK WHERE ID = -3) LIMIT 1", null);
            ExecuteNonQuery("insert into stock SELECT * FROM(SELECT - 4, 'DIA', 'Dow Jones Industrial Average', 'None', 'None', 'USD', 'None', 'NASDAQ', '0', '',1) WHERE NOT EXISTS(SELECT ID FROM STOCK WHERE ID = -4) LIMIT 1", null);
            ExecuteNonQuery("insert into stock SELECT * FROM(SELECT - 5, 'XLK', 'Technology Select Sector', 'None', 'None', 'USD', 'None', 'NASDAQ', '0', '',1) WHERE NOT EXISTS(SELECT ID FROM STOCK WHERE ID = -5) LIMIT 1", null);
            ExecuteNonQuery("insert into stock SELECT * FROM(SELECT - 6, 'XLV', 'Health Care Select Sector', 'None', 'None', 'USD', 'None', 'NASDAQ', '0', '',1) WHERE NOT EXISTS(SELECT ID FROM STOCK WHERE ID = -6) LIMIT 1", null);
            ExecuteNonQuery("insert into stock SELECT * FROM(SELECT - 7, 'IEF', 'iShares 7-10 Year Treasury Bond', 'None', 'None', 'USD', 'None', 'NASDAQ', '0', '',1) WHERE NOT EXISTS(SELECT ID FROM STOCK WHERE ID = -7) LIMIT 1", null);
            ExecuteNonQuery("insert into stock SELECT * FROM(SELECT - 8, 'TLT', 'iShares 20+ Year Treasury Bond', 'None', 'None', 'USD', 'None', 'NASDAQ', '0', '',1) WHERE NOT EXISTS(SELECT ID FROM STOCK WHERE ID = -8) LIMIT 1", null);
            ExecuteNonQuery("insert into stock SELECT * FROM(SELECT - 9, 'MTUM', 'Momentum Fctr ETF', 'None', 'None', 'USD', 'None', 'NASDAQ', '0', '',1) WHERE NOT EXISTS(SELECT ID FROM STOCK WHERE ID = -9) LIMIT 1", null);
            ExecuteNonQuery("insert into stock SELECT * FROM(SELECT - 10, 'CMTUM', 'Custom Momentum', 'None', 'None', 'USD', 'None', 'NASDAQ', '0', '',1) WHERE NOT EXISTS(SELECT ID FROM STOCK WHERE ID = -10) LIMIT 1", null);
            ExecuteNonQuery("insert into stock SELECT * FROM(SELECT - 11, 'AGG', 'iShares Barclays Aggregate Bond Fund', 'None', 'None', 'USD', 'None', 'NASDAQ', '0', '',1) WHERE NOT EXISTS(SELECT ID FROM STOCK WHERE ID = -11) LIMIT 1", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS ETF_TMP_LIST (SYMBOL TEXT, COMPANY_NAME TEXT, EXCHANGE TEXT, CATEGORY TEXT, FIRST_DATE INTEGER, LAST_DATE INTEGER)", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS FEATURE (ID_STOCK INTEGER, FCF1G DECIMAL, FCF3G DECIMAL, FCF5G DECIMAL, FCF1M DECIMAL, FCF3M DECIMAL, FCF5M DECIMAL, ROIC1G DECIMAL, ROIC3G DECIMAL, ROIC5G DECIMAL, ROIC1M DECIMAL, ROIC3M DECIMAL, ROIC5M DECIMAL, EBITA1G DECIMAL, EBITA3G DECIMAL, EBITA5G DECIMAL, EBITA1M DECIMAL, EBITA3M DECIMAL, EBITA5M DECIMAL , EV1G DECIMAL, EV3G DECIMAL, EV5G DECIMAL, EV1M DECIMAL, EV3M DECIMAL, EV5M DECIMAL, BVPS1G DECIMAL, BVPS3G DECIMAL, BVPS5G DECIMAL, BVPS1M DECIMAL, BVPS3M DECIMAL, BVPS5M DECIMAL, FIP DECIMAL, " +
                " CROIC1G DECIMAL, CROIC3G DECIMAL, CROIC5G DECIMAL, CROIC1M DECIMAL, CROIC3M DECIMAL, CROIC5M DECIMAL, " +
                " SALES1G DECIMAL, SALES3G DECIMAL, SALES5G DECIMAL, SALES1M DECIMAL, SALES3M DECIMAL, SALES5M DECIMAL, " +
                " PEG_RATIO DECIMAL DEFAULT 0, EBITDA_Liabilities DECIMAL DEFAULT 0, GP_Assets DECIMAL DEFAULT 0, FCF_Sales DECIMAL DEFAULT 0, EV_EBITDA DECIMAL DEFAULT 0, Close_FCF DECIMAL DEFAULT 0, EV_Revenue DECIMAL DEFAULT 0, Dividend_BuyBackRate DECIMAL DEFAULT 0, Operating_Margin DECIMAL DEFAULT 0, ROIC_EV_EBITDA DECIMAL DEFAULT 0, " +
                " MTUM DECIMAL, SORTINO_FIP DECIMAL, " +
                " ROIC_score DECIMAL, CROIC_score DECIMAL, FCF_score DECIMAL, EBITDA_score DECIMAL, SALES_SCORE DECIMAL, SHARPE_FIP DECIMAL, " + 
                " DATE INTEGER, CORRESPONDING_QUARTER INTEGER, HAS_1_YEAR_BACK INTEGER, HAS_3_YEAR_BACK INTEGER, HAS_5_YEAR_BACK INTEGER)", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS FILTER (ID_STOCK INTEGER, CLOSE_PRICE DECIMAL, CLOSE_PRICE_1JAN DECIMAL, VOLUME INTEGER, AVG_VOLUME INTEGER, CURRENCY_RATIO DECIMAL, DEBT_TO_EBIT DECIMAL, MKT_CAP DECIMAL, POSITIVE_EBIT_YEARS INTEGER, SHAREHOLDERS_YIELD DECIMAL, BUY_BACK_RATE DECIMAL, DEBT_REDUCTION DECIMAL, QOQ_EARNINGS DECIMAL, EV_EBITDA DECIMAL, ROIC_SCORE DECIMAL, CROIC_SCORE DECIMAL, FCF_SCORE DECIMAL,  EBITDA_SCORE DECIMAL,  SALES_SCORE DECIMAL, ROIC_SCORE_AUX DECIMAL, CROIC_SCORE_AUX DECIMAL, FCF_SCORE_AUX DECIMAL,  EBITDA_SCORE_AUX DECIMAL, SALES_SCORE_AUX DECIMAL, EPS DECIMAL, SPS DECIMAL, BVPS DECIMAL, FCFPS DECIMAL, PE DECIMAL, PS DECIMAL, PB DECIMAL, PFCF DECIMAL, PE_VAL DECIMAL, PS_VAL DECIMAL, PB_VAL DECIMAL, FCFPS_VAL DECIMAL, DATE INTEGER)", null);

            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'FEATURE_DATE_X' ON 'FEATURE'(ID_STOCK, DATE)", null);
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'FILTER_DATE_X' ON 'FILTER'(ID_STOCK, DATE)", null);
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'FILTER_DATE_ONLY' ON 'FILTER'(DATE)", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS RATIO (ID_STOCK INTEGER, EBIT DECIMAL, EBITUSD DECIMAL, EBITMARGIN DECIMAL, SHARES_OUT NUMBER, TOT_LTERM_DEBT DECIMAL, FCF REAL, ROIC REAL, CURR_RATIO REAL, DIVIDEND_YIELD REAL, EV DECIMAL, BVPS DECIMAL, INVESTED_CAPITAL DECIMAL, MARKET_CAP DECIMAL, SALES_USD DECIMAL, EPS DECIMAL, PE DECIMAL, PS DECIMAL, PB DECIMAL, PFCF DECIMAL, STOCK_PRICE DECIMAL, SPS DECIMAL, FCFPS DECIMAL, DATE INTEGER, SNAP_DATE INTEGER, QUARTER INTEGER, YEAR INTEGER, DATA_RANGE TEXT, DATE_FIRST_MONTH INTEGER, DATE_KEY INTEGER," +
                "COR DECIMAL, NETINC DECIMAL, EPSDIL DECIMAL, SHARESWA DECIMAL, CAPEX DECIMAL, ASSETS DECIMAL, CASHNEQ DECIMAL, LIABILITIES DECIMAL, ASSETSC DECIMAL, LIABILITIESC DECIMAL, TANGIBLES DECIMAL, ROE DECIMAL, ROA DECIMAL, GP DECIMAL, GROSSMARGIN  DECIMAL, NETMARGIN DECIMAL, ROS DECIMAL, ASSETTURNOVER DECIMAL, PAYOUTRATIO DECIMAL, WORKINGCAPITAL DECIMAL, TBVPS DECIMAL)", null);

            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'RATIO_DATE_X' ON 'RATIO'(ID_STOCK, DATE)", null);
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'RATIO_DATE_Y' ON 'RATIO'(ID_STOCK, YEAR)", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS PRICE (ID_STOCK INTEGER, VOLUME DECIMAL, CLOSE_PRICE DECIMAL, AVG_20DAYS_VOLUME REAL, DATE INTEGER )", null);
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'PRICE_DATE_X' ON 'PRICE'(ID_STOCK, DATE)", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS PRICE_HISTORY (ID_STOCK INTEGER, VOLUME DECIMAL, CLOSE_PRICE DECIMAL, CHANGE_PERCENT REAL, DATE INTEGER, THREE_MONTHS_HIGH INTEGER, THREE_MONTHS_LOW INTEGER)", null);
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'PRICE_HISTORY_DATE_X' ON 'PRICE_HISTORY'(ID_STOCK, DATE)", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS DIVIDEND_HISTORY (ID_STOCK INTEGER, DIVIDEND DECIMAL, DATE INTEGER)", null);
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'DIVIDEND_HISTORY_DATE_X' ON 'DIVIDEND_HISTORY'(ID_STOCK)", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS CAPE_VALUE (YEAR INTEGER, MONTH INTEGER, CAPE DECIMAL)", null);
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'CAPE_VALUE_DATE_X' ON 'CAPE_VALUE'(YEAR, MONTH)", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS SPY_STOCKS (ID_STOCK INTEGER)", null);
            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS SPY_HILOW (DATE INTEGER, VALUE DECIMAL)", null);
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'SPY_HILOW_DATE_X' ON 'SPY_HILOW'(DATE)", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS SYSTEM_PARAMETERS (ID INTEGER, VALUE TEXT)", null);

            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS VR_HISTORY_RANKING (VR_HISTORY_ID INTEGER, ID_STOCK INTEGER, DATE INTEGER, RANK_VALUE DECIMAL)", null);
            ExecuteNonQuery("CREATE TABLE IF NOT EXISTS VR_HISTORY (ID INTEGER, YEARS INTEGER, PE_WEIGHT DECIMAL, PS_WEIGHT DECIMAL, PB_WEIGHT DECIMAL, PFCF_WEIGHT DECIMAL, CREATION_DATE INTEGER)", null);
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'VR_HISTORY_RANKING_DATE_X' ON 'VR_HISTORY_RANKING'(ID_STOCK, VR_HISTORY_ID, DATE)", null);
        }

        public void DropDataTables(ProcessingPanelControl pnlProcessing)
        {
            ExecuteNonQuery("DROP INDEX  IF EXISTS STOCK_ID_X", null);
            ExecuteNonQuery("DROP INDEX  IF EXISTS FEATURE_DATE_X", null);
            ExecuteNonQuery("DROP INDEX  IF EXISTS FILTER_DATE_ONLY", null);
            ExecuteNonQuery("DROP INDEX  IF EXISTS FILTER_DATE_X", null);
            ExecuteNonQuery("DROP INDEX  IF EXISTS RATIO_DATE_X", null);
            ExecuteNonQuery("DROP INDEX  IF EXISTS PRICE_DATE_X", null);
            ExecuteNonQuery("DROP INDEX  IF EXISTS PRICE_HISTORY_DATE_X", null);
            ExecuteNonQuery("DROP INDEX  IF EXISTS CAPE_VALUE_DATE_X", null);
            ExecuteNonQuery("DROP INDEX  IF EXISTS SPY_HILOW_DATE_X", null);
            ExecuteNonQuery("DROP INDEX  IF EXISTS DIVIDEND_HISTORY_DATE_X", null);

            ExecuteNonQuery("DROP INDEX IF EXISTS 'FILTER_SCORE_RANK1'", null);
            ExecuteNonQuery("DROP INDEX IF EXISTS 'FILTER_SCORE_RANK2'", null);
            ExecuteNonQuery("DROP INDEX IF EXISTS 'FILTER_SCORE_RANK3'", null);
            ExecuteNonQuery("DROP INDEX IF EXISTS 'FILTER_SCORE_RANK4'", null);
            ExecuteNonQuery("DROP INDEX IF EXISTS 'FILTER_SCORE_RANK5'", null);

            try
            {
                //delete all except file based and ETF stocks
                ExecuteNonQuery("DELETE FROM STOCK WHERE ID > -100", null);
            }
            catch (Exception)
            { }

            ExecuteNonQuery("DROP TABLE  IF EXISTS FEATURE", null);

            ExecuteNonQuery("DROP TABLE  IF EXISTS FILTER", null);

            ExecuteNonQuery("DROP TABLE  IF EXISTS RATIO", null);

            ExecuteNonQuery("DROP TABLE  IF EXISTS PRICE", null);

            ExecuteNonQuery("DROP TABLE  IF EXISTS PRICE_HISTORY", null);
            ExecuteNonQuery("DELETE FROM SYSTEM_PARAMETERS WHERE ID < 3", null);
            ExecuteNonQuery("DROP TABLE  IF EXISTS CAPE_VALUE", null);
            ExecuteNonQuery("DROP TABLE  IF EXISTS SPY_HILOW", null);
            ExecuteNonQuery("DROP TABLE  IF EXISTS DIVIDEND_HISTORY", null);

            ExecuteNonQuery("DROP INDEX IF EXISTS 'VR_HISTORY_RANKING_DATE_X'", null);
            ExecuteNonQuery("DROP TABLE IF EXISTS 'VR_HISTORY_RANKING'", null);
            ExecuteNonQuery("DROP TABLE IF EXISTS 'VR_HISTORY'", null);
            
            try
            {
                ExecuteNonQuery("DELETE FROM PORTFOLIO_POSITION", null);
            }
            catch (Exception e2)
            {
            }

            ExecuteNonQuery("VACUUM", null);
        }

        void fillSectorTables()
        {
            //fill sectors tables
            if (!Exists("SELECT COUNT(*) FROM SECTOR"))
            {
                ExecuteNonQuery("INSERT INTO SECTOR VALUES (1, 'Energy')", null);
                ExecuteNonQuery("INSERT INTO SECTOR VALUES (2, 'Materials')", null);
                ExecuteNonQuery("INSERT INTO SECTOR VALUES (3, 'Industrials')", null);
                ExecuteNonQuery("INSERT INTO SECTOR VALUES (4, 'Consumer Discretionary')", null);
                ExecuteNonQuery("INSERT INTO SECTOR VALUES (5, 'Consumer Staples')", null);
                ExecuteNonQuery("INSERT INTO SECTOR VALUES (6, 'Health Care')", null);
                ExecuteNonQuery("INSERT INTO SECTOR VALUES (7, 'Financials')", null);
                ExecuteNonQuery("INSERT INTO SECTOR VALUES (8, 'Information Technology')", null);
                ExecuteNonQuery("INSERT INTO SECTOR VALUES (9, 'Telecommunication Services')", null);
                ExecuteNonQuery("INSERT INTO SECTOR VALUES (10, 'Utilities')", null);
                ExecuteNonQuery("INSERT INTO SECTOR VALUES (11, 'Real Estate')", null);
                ExecuteNonQuery("INSERT INTO SECTOR VALUES (12, 'Others')", null);
            }

            if (!Exists("SELECT COUNT(*) FROM SECTOR_INDUSTRY"))
            {
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (3, 'Agriculture')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (3, 'Aircraft')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (12, 'Almost Nothing')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (4, 'Automobiles and Trucks')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (7, 'Banking')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (5, 'Beer and Liquor')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (3, 'Business Services')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (3, 'Business Supplies')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (5, 'Candy and Soda')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (2, 'Chemicals')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (1, 'Coal')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (9, 'Communication')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (2, 'Computers')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (3, 'Construction')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (3, 'Construction Materials')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (4, 'Consumer Goods')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (3, 'Defense')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (3, 'Electrical Equipment')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (8, 'Electronic Equipment')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (4, 'Entertainment')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (5, 'Food Products')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (6, 'Healthcare')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (7, 'Insurance')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (3, 'Machinery')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (3, 'Measuring and Control Equipment')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (6, 'Medical Equipment')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (12, 'None')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (2, 'Non-Metallic and Industrial Metal Mining')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (3, 'Personal Services')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (1, 'Petroleum')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (6, 'Pharmaceutical Products')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (2, 'Precious Metals')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (4, 'Printing and Publishing')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (11, 'Real Estate')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (4, 'Recreation')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (4, 'Restaraunts, Hotels, Motels')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (4, 'Retail')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (2, 'Rubber and Plastic Products')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (2, 'Shipbuilding, Railroad Equipment')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (2, 'Shipping Containers')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (2, 'Steel Works')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (4, 'Textiles')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (5, 'Tobacco Products')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (10, 'Utilities')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (7, 'Trading')", null);
                ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY VALUES (5, 'Wholesale')", null);
            }

        }

        public void ExecuteNonQuery(String sql, SQLiteParameter[] parameters)
        {
            ExecuteNonQuery(sql, parameters, false);
        }

        public void ExecuteNonQuery(String sql, SQLiteParameter[] parameters, bool reuseLastCommand)
        {
            //System.Diagnostics.Debug.WriteLine(sql);

            SQLiteCommand command = storedCommand;
            if (!reuseLastCommand)
            {
                command = new SQLiteCommand(sql, dbConnection);
                command.CommandType = CommandType.Text;
            }

            if (parameters != null)
            {
                command.Parameters.Clear();
                command.Parameters.AddRange(parameters);
            }

            command.ExecuteNonQuery();

            storedCommand = command;
        }


        public DataTable GetData(String sql, SQLiteParameter[] parameters)
        {
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            command.CommandType = CommandType.Text;

            if (parameters != null)
            {
                command.Parameters.Clear();
                command.Parameters.AddRange(parameters);
            }
            
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet);

            command.Dispose();
            dataAdapter.Dispose();

            return dataSet.Tables[0];
        }

        public DataTable GetData(String sql)
        {
            DataSet dataSet;
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            command.CommandType = CommandType.Text;

            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

            dataSet = new DataSet();

            dataAdapter.Fill(dataSet);

            command.Dispose();
            dataAdapter.Dispose();
            

            return dataSet.Tables[0];
        }


        public bool Exists(String sql)
        {
            bool result = false;
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            command.CommandType = CommandType.Text;

            if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                result = true;

            command.Dispose();
            
            return result;
        }



        ~DatabaseSingleton()
        {
            try
            {
                dbConnection.Close();
            }
            catch (Exception)
            { }
        }

        public void CloseDatabase()
        {
            try
            {
                dbConnection.Close();
            }
            catch (Exception)
            { }
        }

        internal void StartTransaction()
        {
            if (currentTransaction != null)
                throw new Exception("There is a current transaction opened.");

            currentTransaction = dbConnection.BeginTransaction();
        }

        internal void EndTransaction()
        {
            if (currentTransaction != null)
            {
                currentTransaction.Commit();
                currentTransaction = null;
            }
        }
        
        internal void RollbackTransaction()
        {
            if (currentTransaction != null)
            {
                currentTransaction.Rollback();
                currentTransaction = null;
            }
        }
    }
}
