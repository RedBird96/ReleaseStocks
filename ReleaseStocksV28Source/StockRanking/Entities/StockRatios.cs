using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public class StockRatios : IStockData
    {
        public String Stock = "";
        public int IdStock = 0;
        public String CompanyName = "";
        public decimal freeCashFlow = 0;
        public decimal returnInvestedCap = 0;
        public decimal EBITDA = 0;
        public decimal ebitdausd = 0;
        public decimal ebitdamargin = 0;
        public decimal CurrRatio = 0;
        public long SharesOut = 0;
        public double LtermTotalDebt = 0;
        public decimal DividendYield = 0;
        public decimal EV = 0;
        public decimal BVPS = 0;
        public decimal InvestedCapital = 0;
        public decimal MarketCap = 0;
        public decimal SalesUSD = 0;
        public decimal EPS = 0;
        public decimal PE = 0;
        public decimal PS = 0;
        public decimal PB = 0;
        public decimal PFCF = 0;
        public decimal StockPrice = 0;
        public decimal FCFPS = 0;
        public decimal SPS = 0;

        public decimal cor = 0;
        public decimal netinc = 0;
        public decimal epsdil = 0;
        public decimal shareswa = 0;
        public decimal capex = 0;
        public decimal assets = 0;
        public decimal cashneq = 0;
        public decimal liabilities = 0;
        public decimal assetsc = 0;
        public decimal liabilitiesc = 0;
        public decimal tangibles = 0;
        public decimal roe = 0;
        public decimal roa = 0;
        public decimal gp = 0;
        public decimal grossmargin = 0;
        public decimal netmargin = 0;
        public decimal ros = 0;
        public decimal assetturnover = 0;
        public decimal payoutratio = 0;
        public decimal workingcapital = 0;
        public decimal tbvps = 0;

        public String DataRange = "Q";

        public int DateFirstMonth { get; set; }
        public int Date { get; set; }
        public int DateKey { get; set; }
        public int SnapDate { get; set; }
        public int Quarter { get; set; }
        public int Year { get; set; } 
        public static bool CommandSaved = false;



        public StockRatios()
        {

        }

        public StockRatios(DataRow row)
        {
            freeCashFlow = Convert.ToDecimal(row["FCF"]);
            returnInvestedCap = Convert.ToDecimal(row["ROIC"]);
            EBITDA = Convert.ToDecimal(row["EBIT"]);
            ebitdausd = Convert.ToDecimal(row["EBITUSD"]);
            ebitdamargin = Convert.ToDecimal(row["EBITMARGIN"]);
            CurrRatio = Convert.ToDecimal(row["CURR_RATIO"]);
            DividendYield = Convert.ToDecimal(row["DIVIDEND_YIELD"]);
            SharesOut = Convert.ToInt64(row["SHARES_OUT"]);
            LtermTotalDebt = Convert.ToDouble(row["TOT_LTERM_DEBT"]);
            IdStock = Convert.ToInt32(row["ID_STOCK"]);
            BVPS = Convert.ToDecimal(row["BVPS"]);
            EV = Convert.ToDecimal(row["EV"]);
            InvestedCapital = Convert.ToDecimal(row["INVESTED_CAPITAL"]);
            MarketCap = Convert.ToDecimal(row["MARKET_CAP"]);
            SalesUSD = Convert.ToDecimal(row["SALES_USD"]);
            EPS = Convert.ToDecimal(row["EPS"]);
            PE = Convert.ToDecimal(row["PE"]);
            PS = Convert.ToDecimal(row["PS"]);
            PB = Convert.ToDecimal(row["PB"]);
            PFCF = Convert.ToDecimal(row["PFCF"]);
            StockPrice = Convert.ToDecimal(row["STOCK_PRICE"]);
            FCFPS = Convert.ToDecimal(row["FCFPS"]);
            SPS = Convert.ToDecimal(row["SPS"]);
            DateKey = Convert.ToInt32(row["DATE_KEY"]);

            cor = Convert.ToDecimal(row["COR"]);
            netinc = Convert.ToDecimal(row["NETINC"]);
            epsdil = Convert.ToDecimal(row["EPSDIL"]);
            shareswa = Convert.ToDecimal(row["SHARESWA"]);
            capex = Convert.ToDecimal(row["CAPEX"]);
            assets = Convert.ToDecimal(row["ASSETS"]);
            cashneq = Convert.ToDecimal(row["CASHNEQ"]);
            liabilities = Convert.ToDecimal(row["LIABILITIES"]);
            assetsc = Convert.ToDecimal(row["ASSETSC"]);
            liabilitiesc = Convert.ToDecimal(row["LIABILITIESC"]);
            tangibles = Convert.ToDecimal(row["TANGIBLES"]);
            roe = Convert.ToDecimal(row["ROE"]);
            roa = Convert.ToDecimal(row["ROA"]);
            gp = Convert.ToDecimal(row["GP"]);
            grossmargin = Convert.ToDecimal(row["GROSSMARGIN"]);
            netmargin = Convert.ToDecimal(row["NETMARGIN"]);
            ros = Convert.ToDecimal(row["ROS"]);
            assetturnover = Convert.ToDecimal(row["ASSETTURNOVER"]);
            payoutratio = Convert.ToDecimal(row["PAYOUTRATIO"]);
            workingcapital = Convert.ToDecimal(row["WORKINGCAPITAL"]);
            tbvps = Convert.ToDecimal(row["TBVPS"]);

            DateFirstMonth = Convert.ToInt32(row["DATE_FIRST_MONTH"]);
            DataRange = Convert.ToString(row["DATA_RANGE"]);
            Date = Convert.ToInt32(row["DATE"]);
            SnapDate = Convert.ToInt32(row["SNAP_DATE"]);
            Quarter = Convert.ToInt32(row["QUARTER"]);
            Year = Convert.ToInt32(row["YEAR"]);
        }

        public void Save(int stockId, int dateToSave, int quarter, int year, int dateFirstMonth, decimal roic)
        {
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();

            parameters.Add(new SQLiteParameter("@p1", stockId));
            parameters.Add(new SQLiteParameter("@p2", EBITDA));
            parameters.Add(new SQLiteParameter("@p3", SharesOut));
            parameters.Add(new SQLiteParameter("@p4", LtermTotalDebt));
            parameters.Add(new SQLiteParameter("@p5", freeCashFlow));
            parameters.Add(new SQLiteParameter("@p6", roic));
            parameters.Add(new SQLiteParameter("@p7", CurrRatio));
            parameters.Add(new SQLiteParameter("@p8", DividendYield));
            parameters.Add(new SQLiteParameter("@p9", EV));
            parameters.Add(new SQLiteParameter("@p10", BVPS));
            parameters.Add(new SQLiteParameter("@p11", InvestedCapital));
            parameters.Add(new SQLiteParameter("@p12", dateToSave));
            parameters.Add(new SQLiteParameter("@p13", SnapDate));
            parameters.Add(new SQLiteParameter("@p14", quarter));
            parameters.Add(new SQLiteParameter("@p15", year));
            parameters.Add(new SQLiteParameter("@p16", DataRange));
            parameters.Add(new SQLiteParameter("@p17", dateFirstMonth));
            parameters.Add(new SQLiteParameter("@p18", MarketCap));
            parameters.Add(new SQLiteParameter("@p19", SalesUSD));
            parameters.Add(new SQLiteParameter("@p20", EPS));
            parameters.Add(new SQLiteParameter("@p21", PE));
            parameters.Add(new SQLiteParameter("@p22", PS));
            parameters.Add(new SQLiteParameter("@p23", PB));
            parameters.Add(new SQLiteParameter("@p24", PFCF));
            parameters.Add(new SQLiteParameter("@p25", StockPrice));
            parameters.Add(new SQLiteParameter("@p26", FCFPS));
            parameters.Add(new SQLiteParameter("@p27", SPS));
            parameters.Add(new SQLiteParameter("@p28", DateKey));
            parameters.Add(new SQLiteParameter("@p29", cor));
            parameters.Add(new SQLiteParameter("@p30", netinc));
            parameters.Add(new SQLiteParameter("@p31", epsdil));
            parameters.Add(new SQLiteParameter("@p32", shareswa));
            parameters.Add(new SQLiteParameter("@p33", capex));
            parameters.Add(new SQLiteParameter("@p34", assets));
            parameters.Add(new SQLiteParameter("@p35", cashneq));
            parameters.Add(new SQLiteParameter("@p36", liabilities));
            parameters.Add(new SQLiteParameter("@p37", assetsc));
            parameters.Add(new SQLiteParameter("@p38", liabilitiesc));
            parameters.Add(new SQLiteParameter("@p39", tangibles));
            parameters.Add(new SQLiteParameter("@p40", roe));
            parameters.Add(new SQLiteParameter("@p41", roa));
            parameters.Add(new SQLiteParameter("@p42", gp));
            parameters.Add(new SQLiteParameter("@p43", grossmargin));
            parameters.Add(new SQLiteParameter("@p44", netmargin));
            parameters.Add(new SQLiteParameter("@p45", ros));
            parameters.Add(new SQLiteParameter("@p46", assetturnover));
            parameters.Add(new SQLiteParameter("@p47", payoutratio));
            parameters.Add(new SQLiteParameter("@p48", workingcapital));
            parameters.Add(new SQLiteParameter("@p49", tbvps));
            parameters.Add(new SQLiteParameter("@p50", ebitdausd));
            parameters.Add(new SQLiteParameter("@p51", ebitdamargin));

            DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO RATIO (ID_STOCK, EBIT, SHARES_OUT, TOT_LTERM_DEBT, FCF, ROIC, CURR_RATIO, DIVIDEND_YIELD, EV, BVPS, INVESTED_CAPITAL, DATE, SNAP_DATE, QUARTER, YEAR, DATA_RANGE, DATE_FIRST_MONTH, MARKET_CAP, SALES_USD, EPS, PE, PS, PB, PFCF, STOCK_PRICE, FCFPS, SPS, DATE_KEY" +
                ", COR, NETINC, EPSDIL, SHARESWA, CAPEX, ASSETS, CASHNEQ, LIABILITIES, ASSETSC, LIABILITIESC, TANGIBLES, ROE, ROA, GP, GROSSMARGIN, NETMARGIN, ROS, ASSETTURNOVER, PAYOUTRATIO, WORKINGCAPITAL, TBVPS, EBITUSD, EBITMARGIN ) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18,@p19,@p20,@p21,@p22,@p23,@p24,@p25,@p26,@p27,@p28" +
                ",@p29,@p30,@p31,@p32,@p33,@p34,@p35,@p36,@p37,@p38,@p39,@p40,@p41,@p42,@p43,@p44,@p45,@p46,@p47,@p48,@p49,@p50,@p51 )", parameters.ToArray(), CommandSaved);
            CommandSaved = true;
        }

        public static void LoadRatios(Stock stock)
        {
            LoadRatios(stock, 0);
        }

        public static void LoadAllRatios(Dictionary<int, Stock> dict)
        {
            foreach(var id in dict)
            {
                id.Value.Ratios.Clear();
            }
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT ID_STOCK, cast(EBIT as double) EBIT , cast(EBITUSD as double) EBITUSD , cast(EBITMARGIN as double) EBITMARGIN , cast(SHARES_OUT as double) SHARES_OUT , cast(TOT_LTERM_DEBT as double) TOT_LTERM_DEBT , cast(FCF as double) FCF ,  cast(CURR_RATIO as double) CURR_RATIO , cast(DIVIDEND_YIELD as double) DIVIDEND_YIELD , cast(EV as double) EV , cast(BVPS as double) BVPS , cast(INVESTED_CAPITAL as double) INVESTED_CAPITAL, cast(MARKET_CAP as double) MARKET_CAP, cast(SALES_USD as double) SALES_USD " +
                ", cast(EPS as double) EPS, cast(PE as double) PE, cast(PS as double) PS, cast(PB as double) PB, cast(PFCF as double) PFCF, cast(STOCK_PRICE as double) STOCK_PRICE, cast(FCFPS as double) FCFPS, cast(SPS as double) SPS " +
                ", DATE , QUARTER , YEAR , DATA_RANGE , SNAP_DATE, DATE_FIRST_MONTH, DATE_KEY " +
                ", CAST(ROIC AS DOUBLE) ROIC " +
                ", CAST(COR AS DOUBLE) COR " +
                ", CAST(NETINC AS DOUBLE) NETINC " +
                ", CAST(EPSDIL AS DOUBLE) EPSDIL " +
                ", CAST(SHARESWA AS DOUBLE) SHARESWA " +
                ", CAST(CAPEX AS DOUBLE) CAPEX " +
                ", CAST(ASSETS AS DOUBLE) ASSETS " +
                ", CAST(CASHNEQ AS DOUBLE) CASHNEQ " +
                ", CAST(LIABILITIES AS DOUBLE) LIABILITIES " +
                ", CAST(ASSETSC AS DOUBLE) ASSETSC " +
                ", CAST(LIABILITIESC AS DOUBLE) LIABILITIESC " +
                ", CAST(TANGIBLES AS DOUBLE) TANGIBLES " +
                ", CAST(ROE AS DOUBLE) ROE " +
                ", CAST(ROA AS DOUBLE) ROA " +
                ", CAST(GP AS DOUBLE) GP " +
                ", CAST(GROSSMARGIN AS DOUBLE) GROSSMARGIN " +
                ", CAST(NETMARGIN AS DOUBLE) NETMARGIN " +
                ", CAST(ROS AS DOUBLE) ROS " +
                ", CAST(ASSETTURNOVER AS DOUBLE) ASSETTURNOVER " +
                ", CAST(PAYOUTRATIO AS DOUBLE) PAYOUTRATIO " +
                ", CAST(WORKINGCAPITAL AS DOUBLE) WORKINGCAPITAL " +
                ", CAST(TBVPS AS DOUBLE) TBVPS "
                + " FROM RATIO R WHERE DATA_RANGE = 'ARQ' ORDER BY DATE ASC");

            foreach (DataRow row in data.Rows)
            {
                StockRatios ratio = new StockRatios(row);
                Stock stock = dict[ratio.IdStock];
                stock.Ratios.Add(ratio);
            }
        }

        public static List<StockRatios> LoadRatiosByDate(int dateInt)
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT ID_STOCK, cast(EBIT as double) EBIT , cast(EBITUSD as double) EBITUSD ,cast(EBITMARGIN as double) EBITMARGIN ,cast(SHARES_OUT as double) SHARES_OUT , cast(TOT_LTERM_DEBT as double) TOT_LTERM_DEBT , cast(FCF as double) FCF ,  cast(CURR_RATIO as double) CURR_RATIO , cast(DIVIDEND_YIELD as double) DIVIDEND_YIELD , cast(EV as double) EV , cast(BVPS as double) BVPS , cast(INVESTED_CAPITAL as double) INVESTED_CAPITAL, cast(MARKET_CAP as double) MARKET_CAP, cast(SALES_USD as double) SALES_USD " +
                ", cast(EPS as double) EPS, cast(PE as double) PE, cast(PS as double) PS, cast(PB as double) PB, cast(PFCF as double) PFCF, cast(STOCK_PRICE as double) STOCK_PRICE, cast(FCFPS as double) FCFPS, cast(SPS as double) SPS " +
                ", DATE , QUARTER , YEAR , DATA_RANGE , SNAP_DATE, DATE_FIRST_MONTH, DATE_KEY " +
                ", CAST(ROIC AS DOUBLE) ROIC " +
                ", CAST(COR AS DOUBLE) COR " +
                ", CAST(NETINC AS DOUBLE) NETINC " +
                ", CAST(EPSDIL AS DOUBLE) EPSDIL " +
                ", CAST(SHARESWA AS DOUBLE) SHARESWA " +
                ", CAST(CAPEX AS DOUBLE) CAPEX " +
                ", CAST(ASSETS AS DOUBLE) ASSETS " +
                ", CAST(CASHNEQ AS DOUBLE) CASHNEQ " +
                ", CAST(LIABILITIES AS DOUBLE) LIABILITIES " +
                ", CAST(ASSETSC AS DOUBLE) ASSETSC " +
                ", CAST(LIABILITIESC AS DOUBLE) LIABILITIESC " +
                ", CAST(TANGIBLES AS DOUBLE) TANGIBLES " +
                ", CAST(ROE AS DOUBLE) ROE " +
                ", CAST(ROA AS DOUBLE) ROA " +
                ", CAST(GP AS DOUBLE) GP " +
                ", CAST(GROSSMARGIN AS DOUBLE) GROSSMARGIN " +
                ", CAST(NETMARGIN AS DOUBLE) NETMARGIN " +
                ", CAST(ROS AS DOUBLE) ROS " +
                ", CAST(ASSETTURNOVER AS DOUBLE) ASSETTURNOVER " +
                ", CAST(PAYOUTRATIO AS DOUBLE) PAYOUTRATIO " +
                ", CAST(WORKINGCAPITAL AS DOUBLE) WORKINGCAPITAL " +
                ", CAST(TBVPS AS DOUBLE) TBVPS "
                + " FROM RATIO R WHERE DATE_FIRST_MONTH BETWEEN " + (dateInt - 10000).ToString() + " AND " + dateInt.ToString() + " AND DATA_RANGE = 'ARQ' " +
                " ORDER BY ID_STOCK ASC, DATE_FIRST_MONTH DESC");

            List<StockRatios> result = new List<StockRatios>();

            int lastID = -100000000;
            
            foreach (DataRow row in data.Rows)
            {
                if (Convert.ToInt32(row["ID_STOCK"]) == lastID)
                {
                    continue;
                } else
                {
                    result.Add(new StockRatios(row));
                    lastID = Convert.ToInt32(row["ID_STOCK"]);
                }
                
            }
            return result;
        }

        public static void LoadRatios(Stock stock, int lastDateForUpdates)
        {
            if (stock.Ratios.Count > 0)
                return;

            DataTable data = DatabaseSingleton.Instance.GetData("SELECT ID_STOCK, cast(EBIT as double) EBIT , cast(EBITUSD as double) EBITUSD , cast(EBITMARGIN as double) EBITMARGIN , cast(SHARES_OUT as double) SHARES_OUT , cast(TOT_LTERM_DEBT as double) TOT_LTERM_DEBT , cast(FCF as double) FCF ,  cast(CURR_RATIO as double) CURR_RATIO , cast(DIVIDEND_YIELD as double) DIVIDEND_YIELD , cast(EV as double) EV , cast(BVPS as double) BVPS , cast(INVESTED_CAPITAL as double) INVESTED_CAPITAL, cast(MARKET_CAP as double) MARKET_CAP, cast(SALES_USD as double) SALES_USD " +
                ", cast(EPS as double) EPS, cast(PE as double) PE, cast(PS as double) PS, cast(PB as double) PB, cast(PFCF as double) PFCF, cast(STOCK_PRICE as double) STOCK_PRICE, cast(FCFPS as double) FCFPS, cast(SPS as double) SPS " +
                ", DATE , QUARTER , YEAR , DATA_RANGE , SNAP_DATE, DATE_FIRST_MONTH, DATE_KEY " + 
                ", CAST(ROIC AS DOUBLE) ROIC " +
                ", CAST(COR AS DOUBLE) COR " +
                ", CAST(NETINC AS DOUBLE) NETINC " +
                ", CAST(EPSDIL AS DOUBLE) EPSDIL " +
                ", CAST(SHARESWA AS DOUBLE) SHARESWA " +
                ", CAST(CAPEX AS DOUBLE) CAPEX " +
                ", CAST(ASSETS AS DOUBLE) ASSETS " +
                ", CAST(CASHNEQ AS DOUBLE) CASHNEQ " +
                ", CAST(LIABILITIES AS DOUBLE) LIABILITIES " +
                ", CAST(ASSETSC AS DOUBLE) ASSETSC " +
                ", CAST(LIABILITIESC AS DOUBLE) LIABILITIESC " +
                ", CAST(TANGIBLES AS DOUBLE) TANGIBLES " +
                ", CAST(ROE AS DOUBLE) ROE " +
                ", CAST(ROA AS DOUBLE) ROA " +
                ", CAST(GP AS DOUBLE) GP " +
                ", CAST(GROSSMARGIN AS DOUBLE) GROSSMARGIN " +
                ", CAST(NETMARGIN AS DOUBLE) NETMARGIN " +
                ", CAST(ROS AS DOUBLE) ROS " +
                ", CAST(ASSETTURNOVER AS DOUBLE) ASSETTURNOVER " +
                ", CAST(PAYOUTRATIO AS DOUBLE) PAYOUTRATIO " +
                ", CAST(WORKINGCAPITAL AS DOUBLE) WORKINGCAPITAL " +
                ", CAST(TBVPS AS DOUBLE) TBVPS "
                + " FROM RATIO R WHERE ID_STOCK = " + stock.Id.ToString() + " AND DATA_RANGE = 'ARQ' " + 
                " AND DATE > " + lastDateForUpdates + 
                " ORDER BY DATE ASC");
            
            foreach (DataRow row in data.Rows)
            {
                stock.Ratios.Add(new StockRatios(row));
            }
        }
    }
}
