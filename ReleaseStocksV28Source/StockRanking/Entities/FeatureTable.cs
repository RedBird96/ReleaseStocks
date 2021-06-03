using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public class FeatureTable
    {
        public int IdStock = 0;

        public static bool CommandSaved = false;

        public decimal FCF1G = 0;
        public decimal FCF3G = 0;
        public decimal FCF5G = 0;
        public decimal FCF1M = 0;
        public decimal FCF3M = 0;
        public decimal FCF5M = 0;

        public decimal ROIC1G = 0;
        public decimal ROIC3G = 0;
        public decimal ROIC5G = 0;
        public decimal ROIC1M = 0;
        public decimal ROIC3M = 0;
        public decimal ROIC5M = 0;

        public decimal EBITA1G = 0;
        public decimal EBITA3G = 0;
        public decimal EBITA5G = 0;
        public decimal EBITA1M = 0;
        public decimal EBITA3M = 0;
        public decimal EBITA5M = 0;
        
        public decimal EV1G = 0;
        public decimal EV3G = 0;
        public decimal EV5G = 0;
        public decimal EV1M = 0;
        public decimal EV3M = 0;
        public decimal EV5M = 0;
        
        public decimal BVPS1G = 0;
        public decimal BVPS3G = 0;
        public decimal BVPS5G = 0;
        public decimal BVPS1M = 0;
        public decimal BVPS3M = 0;
        public decimal BVPS5M = 0;

        public decimal CROIC1G = 0;
        public decimal CROIC3G = 0;
        public decimal CROIC5G = 0;
        public decimal CROIC1M = 0;
        public decimal CROIC3M = 0;
        public decimal CROIC5M = 0;

        public decimal SALES1G = 0;
        public decimal SALES3G = 0;
        public decimal SALES5G = 0;
        public decimal SALES1M = 0;
        public decimal SALES3M = 0;
        public decimal SALES5M = 0;

        public decimal PEG_RATIO = 0;
        public decimal EBITDA_Liabilities = 0;
        public decimal GP_Assets = 0;
        public decimal FCF_Sales = 0;
        public decimal EV_EBITDA = 0;
        public decimal Close_FCF = 0;
        public decimal EV_Revenue = 0;
        public decimal Dividend_BuyBackRate = 0;
        public decimal Operating_Margin = 0;
        public decimal ROIC_EV_EBITDA = 0;

        public decimal ROICSCORE = 0;
        public decimal CROICSCORE = 0;
        public decimal FCFSCORE = 0;
        public decimal EBITDASCORE = 0;
        public decimal SALESSCORE = 0;

        public decimal FIP = 0;
        public decimal SortinoFIP = 0;
        public decimal MTUM = 0;
        public decimal SharpeFIP = 0;
        
        public int CorrespondingQuarter { get; set; }
        public int Date { get; set; }

        public bool Has1YearBackData = false;
        public bool Has3YearBackData = false;
        public bool Has5YearBackData = false;

        public FeatureTable()
        {

        }


        public FeatureTable(DataRow row)
        {
            FCF1G = Convert.ToDecimal(row["FCF1G"]);
            FCF3G = Convert.ToDecimal(row["FCF3G"]);
            FCF5G = Convert.ToDecimal(row["FCF5G"]);
            FCF1M = Convert.ToDecimal(row["FCF1M"]);
            FCF3M = Convert.ToDecimal(row["FCF3M"]);
            FCF5M = Convert.ToDecimal(row["FCF5M"]);

            ROIC1G = Convert.ToDecimal(row["ROIC1G"]);
            ROIC3G = Convert.ToDecimal(row["ROIC3G"]);
            ROIC5G = Convert.ToDecimal(row["ROIC5G"]);
            ROIC1M = Convert.ToDecimal(row["ROIC1M"]);
            ROIC3M = Convert.ToDecimal(row["ROIC3M"]);
            ROIC5M = Convert.ToDecimal(row["ROIC5M"]);

            EBITA1G = Convert.ToDecimal(row["EBITA1G"]);
            EBITA3G = Convert.ToDecimal(row["EBITA3G"]);
            EBITA5G = Convert.ToDecimal(row["EBITA5G"]);
            EBITA1M = Convert.ToDecimal(row["EBITA1M"]);
            EBITA3M = Convert.ToDecimal(row["EBITA3M"]);
            EBITA5M = Convert.ToDecimal(row["EBITA5M"]);

            EV1G = Convert.ToDecimal(row["EV1G"]);
            EV3G = Convert.ToDecimal(row["EV3G"]);
            EV5G = Convert.ToDecimal(row["EV5G"]);
            EV1M = Convert.ToDecimal(row["EV1M"]);
            EV3M = Convert.ToDecimal(row["EV3M"]);
            EV5M = Convert.ToDecimal(row["EV5M"]);

            BVPS1G = Convert.ToDecimal(row["BVPS1G"]);
            BVPS3G = Convert.ToDecimal(row["BVPS3G"]);
            BVPS5G = Convert.ToDecimal(row["BVPS5G"]);
            BVPS1M = Convert.ToDecimal(row["BVPS1M"]);
            BVPS3M = Convert.ToDecimal(row["BVPS3M"]);
            BVPS5M = Convert.ToDecimal(row["BVPS5M"]);

            CROIC1G = Convert.ToDecimal(row["CROIC1G"]);
            CROIC3G = Convert.ToDecimal(row["CROIC3G"]);
            CROIC5G = Convert.ToDecimal(row["CROIC5G"]);
            CROIC1M = Convert.ToDecimal(row["CROIC1M"]);
            CROIC3M = Convert.ToDecimal(row["CROIC3M"]);
            CROIC5M = Convert.ToDecimal(row["CROIC5M"]);

            SALES1G = Convert.ToDecimal(row["SALES1G"]);
            SALES3G = Convert.ToDecimal(row["SALES3G"]);
            SALES5G = Convert.ToDecimal(row["SALES5G"]);
            SALES1M = Convert.ToDecimal(row["SALES1M"]);
            SALES3M = Convert.ToDecimal(row["SALES3M"]);
            SALES5M = Convert.ToDecimal(row["SALES5M"]);

            PEG_RATIO = Convert.ToDecimal(row["PEG_RATIO"]);
            EBITDA_Liabilities = Convert.ToDecimal(row["EBITDA_Liabilities"]);
            GP_Assets = Convert.ToDecimal(row["GP_Assets"]);
            FCF_Sales = Convert.ToDecimal(row["FCF_Sales"]);
            EV_EBITDA = Convert.ToDecimal(row["EV_EBITDA"]);
            Close_FCF = Convert.ToDecimal(row["Close_FCF"]);
            EV_Revenue = Convert.ToDecimal(row["EV_Revenue"]);
            Dividend_BuyBackRate = Convert.ToDecimal(row["Dividend_BuyBackRate"]);
            Operating_Margin = Convert.ToDecimal(row["Operating_Margin"]);
            ROIC_EV_EBITDA = Convert.ToDecimal(row["ROIC_EV_EBITDA"]);

            ROICSCORE = Convert.ToDecimal(row["ROIC_SCORE"]);
            CROICSCORE = Convert.ToDecimal(row["CROIC_SCORE"]);
            FCFSCORE = Convert.ToDecimal(row["FCF_SCORE"]);
            EBITDASCORE = Convert.ToDecimal(row["EBITDA_SCORE"]);
            SALESSCORE = Convert.ToDecimal(row["SALES_SCORE"]);

            Has1YearBackData = Convert.ToInt32(row["HAS_1_YEAR_BACK"]) == 1;
            Has3YearBackData = Convert.ToInt32(row["HAS_3_YEAR_BACK"]) == 1;
            Has5YearBackData = Convert.ToInt32(row["HAS_5_YEAR_BACK"]) == 1;

            FIP = Convert.ToDecimal(row["FIP"]);

            SortinoFIP = Convert.ToDecimal(row["SORTINO_FIP"]);
            SharpeFIP = Convert.ToDecimal(row["SHARPE_FIP"]);

            MTUM = Convert.ToDecimal(row["MTUM"]);

            Date = Convert.ToInt32(row["DATE"]);
            CorrespondingQuarter = Convert.ToInt32(row["CORRESPONDING_QUARTER"]);
            

            IdStock = Convert.ToInt32(row["ID_STOCK"]);
        }

        public bool IsAll0()
        {
            if (FCF1G == 0 && FCF3G == 0 && FCF5G == 0 && FCF1M == 0 &&
                    FCF3M == 0 && FCF5M == 0 && ROIC1G == 0 && ROIC3G == 0 &&
                    ROIC5G == 0 && ROIC1M == 0 && ROIC3M == 0 && ROIC5M == 0 &&
                    EBITA1G == 0 && EBITA3G == 0 && EBITA5G == 0 && EBITA1M == 0 &&
                    EBITA3M == 0 && EBITA5M == 0 && EV1G == 0 && EV3G == 0 &&
                    EV5G == 0 && EV1M == 0 && EV3M == 0 && EV5M == 0 &&
                    BVPS1G == 0 && BVPS3G == 0 && BVPS5G == 0 && BVPS1M == 0 &&
                    BVPS3M == 0 && BVPS5M == 0 && CROIC1G == 0 && CROIC3G == 0 &&
                    CROIC5G == 0 && CROIC1M == 0 && CROIC3M == 0 && CROIC5M == 0 &&
                    SALES1G == 0 && SALES3G == 0 &&
                    SALES5G == 0 && SALES1M == 0 && SALES3M == 0 && SALES5M == 0 &&
                    PEG_RATIO == 0 && EBITDA_Liabilities == 0 && GP_Assets == 0 && 
                    FCF_Sales == 0 && EV_EBITDA == 0 && Close_FCF == 0 && EV_Revenue ==  0 &&
                    Dividend_BuyBackRate == 0 && Operating_Margin == 0 && ROIC_EV_EBITDA == 0 &&
                    CROICSCORE == 0 && ROICSCORE == 0 && FCFSCORE == 0 && EBITDASCORE == 0 && SALESSCORE == 0)
                return true;

            return false;
        }

        public void CopyAllExcepFIP(FeatureTable sourceFeature)
        {
            this.FCF1G = sourceFeature.FCF1G;
            this.FCF3G = sourceFeature.FCF3G;
            this.FCF5G = sourceFeature.FCF5G;
            this.FCF1M = sourceFeature.FCF1M;
            this.FCF3M = sourceFeature.FCF3M;
            this.FCF5M = sourceFeature.FCF5M;

            this.ROIC1G = sourceFeature.ROIC1G;
            this.ROIC3G = sourceFeature.ROIC3G;
            this.ROIC5G = sourceFeature.ROIC5G;
            this.ROIC1M = sourceFeature.ROIC1M;
            this.ROIC3M = sourceFeature.ROIC3M;
            this.ROIC5M = sourceFeature.ROIC5M;

            this.EBITA1G = sourceFeature.EBITA1G;
            this.EBITA3G = sourceFeature.EBITA3G;
            this.EBITA5G = sourceFeature.EBITA5G;
            this.EBITA1M = sourceFeature.EBITA1M;
            this.EBITA3M = sourceFeature.EBITA3M;
            this.EBITA5M = sourceFeature.EBITA5M;

            this.EV1G = sourceFeature.EV1G;
            this.EV3G = sourceFeature.EV3G;
            this.EV5G = sourceFeature.EV5G;
            this.EV1M = sourceFeature.EV1M;
            this.EV3M = sourceFeature.EV3M;
            this.EV5M = sourceFeature.EV5M;

            this.BVPS1G = sourceFeature.BVPS1G;
            this.BVPS3G = sourceFeature.BVPS3G;
            this.BVPS5G = sourceFeature.BVPS5G;
            this.BVPS1M = sourceFeature.BVPS1M;
            this.BVPS3M = sourceFeature.BVPS3M;
            this.BVPS5M = sourceFeature.BVPS5M;

            this.CROIC1G = sourceFeature.CROIC1G;
            this.CROIC3G = sourceFeature.CROIC3G;
            this.CROIC5G = sourceFeature.CROIC5G;
            this.CROIC1M = sourceFeature.CROIC1M;
            this.CROIC3M = sourceFeature.CROIC3M;
            this.CROIC5M = sourceFeature.CROIC5M;

            this.SALES1G = sourceFeature.SALES1G;
            this.SALES3G = sourceFeature.SALES3G;
            this.SALES5G = sourceFeature.SALES5G;
            this.SALES1M = sourceFeature.SALES1M;
            this.SALES3M = sourceFeature.SALES3M;
            this.SALES5M = sourceFeature.SALES5M;

            this.PEG_RATIO = sourceFeature.PEG_RATIO;
            this.EBITDA_Liabilities = sourceFeature.EBITDA_Liabilities;
            this.GP_Assets = sourceFeature.GP_Assets;
            this.FCF_Sales = sourceFeature.FCF_Sales;
            this.EV_EBITDA = sourceFeature.EV_EBITDA;
            this.Close_FCF = sourceFeature.Close_FCF;
            this.EV_Revenue = sourceFeature.EV_Revenue;
            this.Dividend_BuyBackRate = sourceFeature.Dividend_BuyBackRate;
            this.Operating_Margin = sourceFeature.Operating_Margin;
            this.ROIC_EV_EBITDA = sourceFeature.ROIC_EV_EBITDA;

            this.ROICSCORE = sourceFeature.ROICSCORE;
            this.CROICSCORE = sourceFeature.CROICSCORE;
            this.FCFSCORE = sourceFeature.FCFSCORE;
            this.EBITDASCORE = sourceFeature.EBITDASCORE;
            this.SALESSCORE = sourceFeature.SALESSCORE;
             
            this.Has1YearBackData = sourceFeature.Has1YearBackData;
            this.Has3YearBackData = sourceFeature.Has3YearBackData;
            this.Has5YearBackData = sourceFeature.Has5YearBackData;
        }

        public void SaveAllExceptFIP()
        {
            //check if the value exists, then update or create it
            if (!DatabaseSingleton.Instance.Exists("SELECT COUNT(*) FROM FEATURE WHERE id_stock = " + IdStock.ToString() + " and DATE = " + Date.ToString()))
            {
                List<SQLiteParameter> parameters = new List<SQLiteParameter>();
                parameters.Add(new SQLiteParameter("@p1", Date));

                parameters.Add(new SQLiteParameter("@p2", FCF1G));
                parameters.Add(new SQLiteParameter("@p3", FCF3G));
                parameters.Add(new SQLiteParameter("@p4", FCF5G));
                parameters.Add(new SQLiteParameter("@p5", FCF1M));
                parameters.Add(new SQLiteParameter("@p6", FCF3M));
                parameters.Add(new SQLiteParameter("@p7", FCF5M));

                parameters.Add(new SQLiteParameter("@p8", ROIC1G));
                parameters.Add(new SQLiteParameter("@p9", ROIC3G));
                parameters.Add(new SQLiteParameter("@p10", ROIC5G));
                parameters.Add(new SQLiteParameter("@p11", ROIC1M));
                parameters.Add(new SQLiteParameter("@p12", ROIC3M));
                parameters.Add(new SQLiteParameter("@p13", ROIC5M));

                parameters.Add(new SQLiteParameter("@p14", EBITA1G));
                parameters.Add(new SQLiteParameter("@p15", EBITA3G));
                parameters.Add(new SQLiteParameter("@p16", EBITA5G));
                parameters.Add(new SQLiteParameter("@p17", EBITA1M));
                parameters.Add(new SQLiteParameter("@p18", EBITA3M));
                parameters.Add(new SQLiteParameter("@p19", EBITA5M));
                 
                parameters.Add(new SQLiteParameter("@p20", EV1G));
                parameters.Add(new SQLiteParameter("@p21", EV3G));
                parameters.Add(new SQLiteParameter("@p22", EV5G));
                parameters.Add(new SQLiteParameter("@p23", EV1M));
                parameters.Add(new SQLiteParameter("@p24", EV3M));
                parameters.Add(new SQLiteParameter("@p25", EV5M));

                parameters.Add(new SQLiteParameter("@p26", BVPS1G));
                parameters.Add(new SQLiteParameter("@p27", BVPS3G));
                parameters.Add(new SQLiteParameter("@p28", BVPS5G));
                parameters.Add(new SQLiteParameter("@p29", BVPS1M));
                parameters.Add(new SQLiteParameter("@p30", BVPS3M));
                parameters.Add(new SQLiteParameter("@p31", BVPS5M));

                parameters.Add(new SQLiteParameter("@p37", CROIC1G));
                parameters.Add(new SQLiteParameter("@p38", CROIC3G));
                parameters.Add(new SQLiteParameter("@p39", CROIC5G));
                parameters.Add(new SQLiteParameter("@p40", CROIC1M));
                parameters.Add(new SQLiteParameter("@p41", CROIC3M));
                parameters.Add(new SQLiteParameter("@p42", CROIC5M));

                parameters.Add(new SQLiteParameter("@p43", SALES1G));
                parameters.Add(new SQLiteParameter("@p44", SALES3G));
                parameters.Add(new SQLiteParameter("@p45", SALES5G));
                parameters.Add(new SQLiteParameter("@p46", SALES1M));
                parameters.Add(new SQLiteParameter("@p47", SALES3M));
                parameters.Add(new SQLiteParameter("@p48", SALES5M));

                parameters.Add(new SQLiteParameter("@p54", PEG_RATIO));
                parameters.Add(new SQLiteParameter("@p55", EBITDA_Liabilities));
                parameters.Add(new SQLiteParameter("@p56", GP_Assets));
                parameters.Add(new SQLiteParameter("@p57", FCF_Sales));
                parameters.Add(new SQLiteParameter("@p58", EV_EBITDA));
                parameters.Add(new SQLiteParameter("@p59", Close_FCF));
                parameters.Add(new SQLiteParameter("@p60", EV_Revenue));
                parameters.Add(new SQLiteParameter("@p61", Dividend_BuyBackRate));
                parameters.Add(new SQLiteParameter("@p62", Operating_Margin));
                parameters.Add(new SQLiteParameter("@p63", ROIC_EV_EBITDA));

                parameters.Add(new SQLiteParameter("@p49", ROICSCORE));
                parameters.Add(new SQLiteParameter("@p50", CROICSCORE));
                parameters.Add(new SQLiteParameter("@p51", FCFSCORE));
                parameters.Add(new SQLiteParameter("@p52", EBITDASCORE));
                parameters.Add(new SQLiteParameter("@p53", SALESSCORE));

                parameters.Add(new SQLiteParameter("@p32", IdStock));
                parameters.Add(new SQLiteParameter("@p33", CorrespondingQuarter));

                parameters.Add(new SQLiteParameter("@p34", Has1YearBackData ? 1 : 0));
                parameters.Add(new SQLiteParameter("@p35", Has1YearBackData ? 1 : 0));
                parameters.Add(new SQLiteParameter("@p36", Has1YearBackData ? 1 : 0));
                DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO FEATURE (DATE,  FCF1G , FCF3G , FCF5G , FCF1M , FCF3M , FCF5M , ROIC1G , ROIC3G , ROIC5G , ROIC1M , ROIC3M , ROIC5M , EBITA1G , EBITA3G , EBITA5G , EBITA1M , EBITA3M , EBITA5M, EV1G, EV3G, EV5G, EV1M, EV3M, EV5M, BVPS1G, BVPS3G, BVPS5G, BVPS1M, BVPS3M, BVPS5M, CROIC1G, CROIC3G, CROIC5G, CROIC1M, CROIC3M, CROIC5M, SALES1G, SALES3G, SALES5G, SALES1M, SALES3M, SALES5M, PEG_RATIO, EBITDA_Liabilities, GP_Assets, FCF_Sales, EV_EBITDA, Close_FCF, EV_Revenue, Dividend_BuyBackRate, Operating_Margin, ROIC_EV_EBITDA, ROIC_SCORE, CROIC_SCORE, FCF_SCORE, EBITDA_SCORE, SALES_SCORE, id_stock, CORRESPONDING_QUARTER, HAS_1_YEAR_BACK, HAS_3_YEAR_BACK, HAS_5_YEAR_BACK) VALUES (@p1,@p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21, @p22, @p23, @p24, @p25, @p26, @p27, @p28, @p29, @p30, @p31, @p37, @p38, @p39, @p40, @p41, @p42, @p43, @p44, @p45, @p46, @p47, @p48, @p49, @p50, @p51, @p52, @p53, @p54, @p55, @p56, @p57, @p58, @p59, @p60, @p61, @p62, @p63, @p32, @p33, @p34, @p35, @p36)", parameters.ToArray(), false);
            }
            else
            {
                List<SQLiteParameter> parameters = new List<SQLiteParameter>();
                parameters.Add(new SQLiteParameter("@p1", Date));

                parameters.Add(new SQLiteParameter("@p2", FCF1G));
                parameters.Add(new SQLiteParameter("@p3", FCF3G));
                parameters.Add(new SQLiteParameter("@p4", FCF5G));
                parameters.Add(new SQLiteParameter("@p5", FCF1M));
                parameters.Add(new SQLiteParameter("@p6", FCF3M));
                parameters.Add(new SQLiteParameter("@p7", FCF5M));

                parameters.Add(new SQLiteParameter("@p8", ROIC1G));
                parameters.Add(new SQLiteParameter("@p9", ROIC3G));
                parameters.Add(new SQLiteParameter("@p10", ROIC5G));
                parameters.Add(new SQLiteParameter("@p11", ROIC1M));
                parameters.Add(new SQLiteParameter("@p12", ROIC3M));
                parameters.Add(new SQLiteParameter("@p13", ROIC5M));

                parameters.Add(new SQLiteParameter("@p14", EBITA1G));
                parameters.Add(new SQLiteParameter("@p15", EBITA3G));
                parameters.Add(new SQLiteParameter("@p16", EBITA5G));
                parameters.Add(new SQLiteParameter("@p17", EBITA1M));
                parameters.Add(new SQLiteParameter("@p18", EBITA3M));
                parameters.Add(new SQLiteParameter("@p19", EBITA5M));

                parameters.Add(new SQLiteParameter("@p20", EV1G));
                parameters.Add(new SQLiteParameter("@p21", EV3G));
                parameters.Add(new SQLiteParameter("@p22", EV5G));
                parameters.Add(new SQLiteParameter("@p23", EV1M));
                parameters.Add(new SQLiteParameter("@p24", EV3M));
                parameters.Add(new SQLiteParameter("@p25", EV5M));

                parameters.Add(new SQLiteParameter("@p26", BVPS1G));
                parameters.Add(new SQLiteParameter("@p27", BVPS3G));
                parameters.Add(new SQLiteParameter("@p28", BVPS5G));
                parameters.Add(new SQLiteParameter("@p29", BVPS1M));
                parameters.Add(new SQLiteParameter("@p30", BVPS3M));
                parameters.Add(new SQLiteParameter("@p31", BVPS5M));
                
                parameters.Add(new SQLiteParameter("@p37", CROIC1G));
                parameters.Add(new SQLiteParameter("@p38", CROIC3G));
                parameters.Add(new SQLiteParameter("@p39", CROIC5G));
                parameters.Add(new SQLiteParameter("@p40", CROIC1M));
                parameters.Add(new SQLiteParameter("@p41", CROIC3M));
                parameters.Add(new SQLiteParameter("@p42", CROIC5M));

                parameters.Add(new SQLiteParameter("@p43", SALES1G));
                parameters.Add(new SQLiteParameter("@p44", SALES3G));
                parameters.Add(new SQLiteParameter("@p45", SALES5G));
                parameters.Add(new SQLiteParameter("@p46", SALES1M));
                parameters.Add(new SQLiteParameter("@p47", SALES3M));
                parameters.Add(new SQLiteParameter("@p48", SALES5M));

                parameters.Add(new SQLiteParameter("@p54", PEG_RATIO));
                parameters.Add(new SQLiteParameter("@p55", EBITDA_Liabilities));
                parameters.Add(new SQLiteParameter("@p56", GP_Assets));
                parameters.Add(new SQLiteParameter("@p57", FCF_Sales));
                parameters.Add(new SQLiteParameter("@p58", EV_EBITDA));
                parameters.Add(new SQLiteParameter("@p59", Close_FCF));
                parameters.Add(new SQLiteParameter("@p60", EV_Revenue));
                parameters.Add(new SQLiteParameter("@p61", Dividend_BuyBackRate));
                parameters.Add(new SQLiteParameter("@p62", Operating_Margin));
                parameters.Add(new SQLiteParameter("@p63", ROIC_EV_EBITDA));

                parameters.Add(new SQLiteParameter("@p49", ROICSCORE));
                parameters.Add(new SQLiteParameter("@p50", CROICSCORE));
                parameters.Add(new SQLiteParameter("@p51", FCFSCORE));
                parameters.Add(new SQLiteParameter("@p52", EBITDASCORE));
                parameters.Add(new SQLiteParameter("@p53", SALESSCORE));

                parameters.Add(new SQLiteParameter("@p32", IdStock));

                parameters.Add(new SQLiteParameter("@p34", Has1YearBackData ? 1 : 0));
                parameters.Add(new SQLiteParameter("@p35", Has1YearBackData ? 1 : 0));
                parameters.Add(new SQLiteParameter("@p36", Has1YearBackData ? 1 : 0));

                parameters.Add(new SQLiteParameter("@p43", CorrespondingQuarter));
                DatabaseSingleton.Instance.ExecuteNonQuery("UPDATE FEATURE SET FCF1G = @p2 , FCF3G = @p3, FCF5G = @p4, FCF1M = @p5, FCF3M = @p6, FCF5M = @p7, ROIC1G = @p8 , " +
                    " ROIC3G = @p9, ROIC5G = @p10, ROIC1M = @p11, ROIC3M = @p12, ROIC5M = @p13, EBITA1G = @p14, EBITA3G = @p15, EBITA5G = @p16, EBITA1M = @p17, EBITA3M = @p18, EBITA5M = @p19, " +
                    " EV1G = @p20 , EV3G = @p21, EV5G = @p22, EV1M = @p23, EV3M = @p24, EV5M = @p25, " +
                    " BVPS1G = @p26, BVPS3G = @p27, BVPS5G = @p28, BVPS1M = @p29, BVPS3M = @p30, BVPS5M = @p31, " +
                    " CROIC1G = @p37, CROIC3G = @p38, CROIC5G = @p39, CROIC1M = @p40, CROIC3M = @p41, CROIC5M = @p42, " +
                    " SALES1G = @p43, SALES3G = @p44, SALES5G = @p45, SALES1M = @p46, SALES3M = @p47, SALES5M = @p48, " +
                    " PEG_RATIO = @p54, EBITDA_Liabilities = @p55, GP_Assets = @p56, FCF_Sales = @p57,  EV_EBITDA = @p58, " +
                    " Close_FCF = @p59, EV_Revenue = @p60, Dividend_BuyBackRate = @p61, Operating_Margin = @p62, ROIC_EV_EBITDA = @p63, " + 
                    " ROIC_SCORE = @p49, CROIC_SCORE = @p50, FCF_SCORE = @p51, EBITDA_SCORE = @p52, SALES_SCORE = @p53, " +
                    " CORRESPONDING_QUARTER = @p43, " +
                    " HAS_1_YEAR_BACK = @p34, HAS_3_YEAR_BACK = @p35, HAS_5_YEAR_BACK = @p36 " +
                    " WHERE id_stock = @p32 AND DATE = @p1", parameters.ToArray(), false);
            }

        }

        public static List<FeatureTable> LoadFeatures(int id)
        {
            return LoadFeatures(id, DatabaseSingleton.Instance);
        }

        public static List<FeatureTable> LoadFeatures(int id, DatabaseSingleton dbConn)
        {
            List<FeatureTable> result = new List<FeatureTable>();

            DataTable data = dbConn.GetData("SELECT IFNULL(DATE, 0) DATE , " +
                 Utils.AddCastFromDB("FCF1G") + ", " +
                 Utils.AddCastFromDB("FCF3G") + ", " +
                 Utils.AddCastFromDB("FCF5G") + ", " +
                 Utils.AddCastFromDB("FCF1M") + ", " +
                 Utils.AddCastFromDB("FCF3M") + ", " +
                 Utils.AddCastFromDB("FCF5M") + ", " +
                 Utils.AddCastFromDB("ROIC1G") + ", " +
                 Utils.AddCastFromDB("ROIC3G") + ", " +
                 Utils.AddCastFromDB("ROIC5G") + ", " +
                 Utils.AddCastFromDB("ROIC1M") + ", " +
                 Utils.AddCastFromDB("ROIC3M") + ", " +
                 Utils.AddCastFromDB("ROIC5M") + ", " +
                 Utils.AddCastFromDB("EBITA1G") + ", " +
                 Utils.AddCastFromDB("EBITA3G") + ", " +
                 Utils.AddCastFromDB("EBITA5G") + ", " +
                 Utils.AddCastFromDB("EBITA1M") + ", " +
                 Utils.AddCastFromDB("EBITA3M") + ", " +
                 Utils.AddCastFromDB("EBITA5M") + ", " +
                 Utils.AddCastFromDB("EV1G") + ", " +
                 Utils.AddCastFromDB("EV3G") + ", " +
                 Utils.AddCastFromDB("EV5G") + ", " +
                 Utils.AddCastFromDB("EV1M") + ", " +
                 Utils.AddCastFromDB("EV3M") + ", " +
                 Utils.AddCastFromDB("EV5M") + ", " +
                 Utils.AddCastFromDB("BVPS1G") + ", " +
                 Utils.AddCastFromDB("BVPS3G") + ", " +
                 Utils.AddCastFromDB("BVPS5G") + ", " +
                 Utils.AddCastFromDB("BVPS1M") + ", " +
                 Utils.AddCastFromDB("BVPS3M") + ", " +
                 Utils.AddCastFromDB("BVPS5M") + ", " +
                 Utils.AddCastFromDB("FIP") + ", " +
                 Utils.AddCastFromDB("CROIC1G") + ", " +
                 Utils.AddCastFromDB("CROIC3G") + ", " +
                 Utils.AddCastFromDB("CROIC5G") + ", " +
                 Utils.AddCastFromDB("CROIC1M") + ", " +
                 Utils.AddCastFromDB("CROIC3M") + ", " +
                 Utils.AddCastFromDB("CROIC5M") + ", " +
                 Utils.AddCastFromDB("SALES1G") + ", " +
                 Utils.AddCastFromDB("SALES3G") + ", " +
                 Utils.AddCastFromDB("SALES5G") + ", " +
                 Utils.AddCastFromDB("SALES1M") + ", " +
                 Utils.AddCastFromDB("SALES3M") + ", " +
                 Utils.AddCastFromDB("SALES5M") + ", " +
                 Utils.AddCastFromDB("PEG_RATIO") + ", " +
                 Utils.AddCastFromDB("EBITDA_Liabilities") + ", " +
                 Utils.AddCastFromDB("GP_Assets") + ", " +
                 Utils.AddCastFromDB("FCF_Sales") + ", " +
                 Utils.AddCastFromDB("EV_EBITDA") + ", " +
                 Utils.AddCastFromDB("Close_FCF") + ", " +
                 Utils.AddCastFromDB("EV_Revenue") + ", " +
                 Utils.AddCastFromDB("Dividend_BuyBackRate") + ", " +
                 Utils.AddCastFromDB("Operating_Margin") + ", " +
                 Utils.AddCastFromDB("ROIC_EV_EBITDA") + ", " +
                 Utils.AddCastFromDB("ROIC_SCORE") + ", " +
                 Utils.AddCastFromDB("CROIC_SCORE") + ", " +
                 Utils.AddCastFromDB("FCF_SCORE") + ", " +
                 Utils.AddCastFromDB("EBITDA_SCORE") + ", " +
                 Utils.AddCastFromDB("SALES_SCORE") + ", " +
                 Utils.AddCastFromDB("MTUM") + ", " +
                 Utils.AddCastFromDB("SORTINO_FIP") + ", " +
                 Utils.AddCastFromDB("SHARPE_FIP") + ", " +
                    " IFNULL(id_stock, 0) id_stock, CORRESPONDING_QUARTER, IFNULL(HAS_1_YEAR_BACK, 0) HAS_1_YEAR_BACK, IFNULL(HAS_3_YEAR_BACK, 0) HAS_3_YEAR_BACK, IFNULL(HAS_5_YEAR_BACK, 0) HAS_5_YEAR_BACK   FROM FEATURE WHERE ID_STOCK = " + id.ToString() + " ORDER BY DATE ASC");

            foreach (DataRow row in data.Rows)
            {
                result.Add(new FeatureTable(row));
            }

            return result;

        }

        public static void LoadAllFeatures(Dictionary<int, Stock> stockList, DatabaseSingleton dbConn)
        {
            DataTable data = dbConn.GetData("SELECT IFNULL(DATE, 0) DATE , " +
                 Utils.AddCastFromDB("FCF1G") + ", " +
                 Utils.AddCastFromDB("FCF3G") + ", " +
                 Utils.AddCastFromDB("FCF5G") + ", " +
                 Utils.AddCastFromDB("FCF1M") + ", " +
                 Utils.AddCastFromDB("FCF3M") + ", " +
                 Utils.AddCastFromDB("FCF5M") + ", " +
                 Utils.AddCastFromDB("ROIC1G") + ", " +
                 Utils.AddCastFromDB("ROIC3G") + ", " +
                 Utils.AddCastFromDB("ROIC5G") + ", " +
                 Utils.AddCastFromDB("ROIC1M") + ", " +
                 Utils.AddCastFromDB("ROIC3M") + ", " +
                 Utils.AddCastFromDB("ROIC5M") + ", " +
                 Utils.AddCastFromDB("EBITA1G") + ", " +
                 Utils.AddCastFromDB("EBITA3G") + ", " +
                 Utils.AddCastFromDB("EBITA5G") + ", " +
                 Utils.AddCastFromDB("EBITA1M") + ", " +
                 Utils.AddCastFromDB("EBITA3M") + ", " +
                 Utils.AddCastFromDB("EBITA5M") + ", " +
                 Utils.AddCastFromDB("EV1G") + ", " +
                 Utils.AddCastFromDB("EV3G") + ", " +
                 Utils.AddCastFromDB("EV5G") + ", " +
                 Utils.AddCastFromDB("EV1M") + ", " +
                 Utils.AddCastFromDB("EV3M") + ", " +
                 Utils.AddCastFromDB("EV5M") + ", " +
                 Utils.AddCastFromDB("BVPS1G") + ", " +
                 Utils.AddCastFromDB("BVPS3G") + ", " +
                 Utils.AddCastFromDB("BVPS5G") + ", " +
                 Utils.AddCastFromDB("BVPS1M") + ", " +
                 Utils.AddCastFromDB("BVPS3M") + ", " +
                 Utils.AddCastFromDB("BVPS5M") + ", " +
                 Utils.AddCastFromDB("FIP") + ", " +
                 Utils.AddCastFromDB("CROIC1G") + ", " +
                 Utils.AddCastFromDB("CROIC3G") + ", " +
                 Utils.AddCastFromDB("CROIC5G") + ", " +
                 Utils.AddCastFromDB("CROIC1M") + ", " +
                 Utils.AddCastFromDB("CROIC3M") + ", " +
                 Utils.AddCastFromDB("CROIC5M") + ", " +
                 Utils.AddCastFromDB("SALES1G") + ", " +
                 Utils.AddCastFromDB("SALES3G") + ", " +
                 Utils.AddCastFromDB("SALES5G") + ", " +
                 Utils.AddCastFromDB("SALES1M") + ", " +
                 Utils.AddCastFromDB("SALES3M") + ", " +
                 Utils.AddCastFromDB("SALES5M") + ", " +
                 Utils.AddCastFromDB("PEG_RATIO") + ", " +
                 Utils.AddCastFromDB("EBITDA_Liabilities") + ", " +
                 Utils.AddCastFromDB("GP_Assets") + ", " +
                 Utils.AddCastFromDB("FCF_Sales") + ", " +
                 Utils.AddCastFromDB("EV_EBITDA") + ", " +
                 Utils.AddCastFromDB("Close_FCF") + ", " +
                 Utils.AddCastFromDB("EV_Revenue") + ", " +
                 Utils.AddCastFromDB("Dividend_BuyBackRate") + ", " +
                 Utils.AddCastFromDB("Operating_Margin") + ", " +
                 Utils.AddCastFromDB("ROIC_EV_EBITDA") + ", " +
                 Utils.AddCastFromDB("ROIC_SCORE") + ", " +
                 Utils.AddCastFromDB("CROIC_SCORE") + ", " +
                 Utils.AddCastFromDB("FCF_SCORE") + ", " +
                 Utils.AddCastFromDB("EBITDA_SCORE") + ", " +
                 Utils.AddCastFromDB("SALES_SCORE") + ", " +
                 Utils.AddCastFromDB("MTUM") + ", " +
                 Utils.AddCastFromDB("SORTINO_FIP") + ", " +
                 Utils.AddCastFromDB("SHARPE_FIP") + ", " +
                    " IFNULL(id_stock, 0) id_stock, CORRESPONDING_QUARTER, IFNULL(HAS_1_YEAR_BACK, 0) HAS_1_YEAR_BACK, IFNULL(HAS_3_YEAR_BACK, 0) HAS_3_YEAR_BACK, IFNULL(HAS_5_YEAR_BACK, 0) HAS_5_YEAR_BACK   FROM FEATURE ORDER BY DATE ASC");

            foreach (DataRow row in data.Rows)
            {
                FeatureTable feature = new FeatureTable(row);
                Stock stock = stockList[feature.IdStock];
                stock.FeatureTables.Add(feature);
            }

        }
    }
}
