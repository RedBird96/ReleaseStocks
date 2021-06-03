using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public class FilterTable
    {
        public int IdStock = 0;

        public static bool CommandSaved = false;

        public static List<decimal> [] ratioValues = new List<decimal>[5];
        public static int[] validSum = { 0, 0, 0, 0, 0 };
        public static double[] sumX = { 0, 0, 0, 0, 0 };
        public static double[] sumXSquare = { 0, 0, 0, 0, 0 };
        public static double[] sumXY = { 0, 0, 0, 0, 0 };
        public static double[] firstValue = { 0, 0, 0, 0, 0 };
        public static double[] lastValue = { 0, 0, 0, 0, 0 };

        public decimal ClosePrice;
        public decimal CloseJan1;
        public long Volume;
        public long AvgVolume;
        public decimal CurrRatio;
        public decimal DebtToEbit;
        public decimal MktCap;
        public int PositiveEbitYears;
        public decimal ShareholdersYield;
        public decimal BuyBackRate;
        public decimal DebtReduction;
        public decimal QoQEarnings;
        public decimal EvEBITDA;
        public decimal RoicScore;
        public decimal CroicScore;
        public decimal FcfScore;
        public decimal EbitdaScore;
        public decimal SalesScore;

        //these are the rolling medians used for the composite
        public decimal EPS = 0;
        public decimal SPS = 0;
        public decimal BVPS = 0;
        public decimal FCFPS = 0;
        public decimal PE = 0;
        public decimal PS = 0;
        public decimal PB = 0;
        public decimal PFCF = 0;

        public decimal PE_VAL = 0;
        public decimal PS_VAL = 0;
        public decimal PB_VAL = 0;
        public decimal PFCF_VAL = 0;

        public int Date { get; set; }
        

        public FilterTable()
        {

        }


        public FilterTable(DataRow row)
        {

            ClosePrice = Convert.ToDecimal(row["CLOSE_PRICE"]);
            CloseJan1 = Convert.ToDecimal(row["CLOSE_PRICE_1JAN"]);
            Volume = Convert.ToInt64(row["VOLUME"]);
            AvgVolume = Convert.ToInt64(row["AVG_VOLUME"]);
            CurrRatio = Convert.ToDecimal(row["CURRENCY_RATIO"]);
            DebtToEbit = Convert.ToDecimal(row["DEBT_TO_EBIT"]);
            MktCap = Convert.ToDecimal(row["MKT_CAP"]);
            PositiveEbitYears = Convert.ToInt32(row["POSITIVE_EBIT_YEARS"]);
            ShareholdersYield = Convert.ToDecimal(row["SHAREHOLDERS_YIELD"]);
            BuyBackRate = Convert.ToDecimal(row["BUY_BACK_RATE"]);
            DebtReduction = Convert.ToDecimal(row["DEBT_REDUCTION"]);
            QoQEarnings = Convert.ToDecimal(row["QOQ_EARNINGS"]);
            EvEBITDA= Convert.ToDecimal(row["EV_EBITDA"]);
            EPS = Convert.ToDecimal(row["EPS"]);
            SPS = Convert.ToDecimal(row["SPS"]);
            BVPS = Convert.ToDecimal(row["BVPS"]);
            FCFPS = Convert.ToDecimal(row["FCFPS"]);
            PE = Convert.ToDecimal(row["PE"]);
            PS = Convert.ToDecimal(row["PS"]);
            PB = Convert.ToDecimal(row["PB"]);
            PFCF = Convert.ToDecimal(row["PFCF"]);

            PE_VAL = Convert.ToDecimal(row["PE_VAL"]);
            PS_VAL = Convert.ToDecimal(row["PS_VAL"]);
            PB_VAL = Convert.ToDecimal(row["PB_VAL"]);
            PFCF_VAL = Convert.ToDecimal(row["FCFPS_VAL"]);

            RoicScore = decimal.MinValue;
            CroicScore = decimal.MinValue;
            FcfScore = decimal.MinValue;
            EbitdaScore = decimal.MinValue;
            SalesScore = decimal.MinValue;

            RoicScore = Convert.ToDecimal(row["ROIC_SCORE"]);
            CroicScore = Convert.ToDecimal(row["CROIC_SCORE"]);
            FcfScore = Convert.ToDecimal(row["FCF_SCORE"]);
            EbitdaScore = Convert.ToDecimal(row["EBITDA_SCORE"]);
            SalesScore = Convert.ToDecimal(row["SALES_SCORE"]);

            Date = Convert.ToInt32(row["DATE"]);

            IdStock = Convert.ToInt32(row["ID_STOCK"]);
        }
        
        public FilterTable(FilterTable filterToClone)
        {
            this.ClosePrice = filterToClone.ClosePrice;
            this.CloseJan1 = filterToClone.CloseJan1;
            this.Volume = filterToClone.Volume;
            this.AvgVolume = filterToClone.AvgVolume;
            this.CurrRatio = filterToClone.CurrRatio;
            this.DebtToEbit = filterToClone.DebtToEbit;
            this.MktCap = filterToClone.MktCap;
            this.PositiveEbitYears = filterToClone.PositiveEbitYears;
            this.ShareholdersYield = filterToClone.ShareholdersYield;
            this.BuyBackRate = filterToClone.BuyBackRate;
            this.DebtReduction = filterToClone.DebtReduction;
            this.QoQEarnings = filterToClone.QoQEarnings;
            this.EvEBITDA = filterToClone.EvEBITDA;
            this.EPS = filterToClone.EPS;
            this.SPS = filterToClone.SPS;
            this.BVPS = filterToClone.BVPS;
            this.FCFPS = filterToClone.FCFPS;
            this.PE = filterToClone.PE;
            this.PS = filterToClone.PS;
            this.PB = filterToClone.PB;
            this.PFCF = filterToClone.PFCF;
            this.PE_VAL = filterToClone.PE_VAL;
            this.PS_VAL = filterToClone.PS_VAL;
            this.PB_VAL = filterToClone.PB_VAL;
            this.PFCF_VAL = filterToClone.PFCF_VAL;
            this.RoicScore = filterToClone.RoicScore;
            this.CroicScore = filterToClone.CroicScore;
            this.FcfScore = filterToClone.FcfScore;
            this.EbitdaScore = filterToClone.EbitdaScore;
            this.SalesScore = filterToClone.SalesScore;
            this.RoicScore = filterToClone.RoicScore;
            this.CroicScore = filterToClone.CroicScore;
            this.FcfScore = filterToClone.FcfScore;
            this.EbitdaScore = filterToClone.EbitdaScore;
            this.SalesScore = filterToClone.SalesScore;
            this.Date = filterToClone.Date;
            this.IdStock = filterToClone.IdStock;
        }

        public static List<FilterTable> LoadFilters(int id)
        {
            return LoadFilters(id, DatabaseSingleton.Instance);
        }

        public static List<FilterTable> LoadFilters(int id, DatabaseSingleton dbConn)
        {
            List<FilterTable> result = new List<FilterTable>();

            DataTable data = dbConn.GetData("SELECT IFNULL(DATE, 0) DATE ,  " +
                Utils.AddCastFromDB("CLOSE_PRICE") + ", " +
                Utils.AddCastFromDB("CLOSE_PRICE_1JAN") + ", " +
                Utils.AddCastFromDB("VOLUME") + ", " +
                Utils.AddCastFromDB("AVG_VOLUME") + ", " +
                Utils.AddCastFromDB("CURRENCY_RATIO") + ", " +
                Utils.AddCastFromDB("DEBT_TO_EBIT") + ", " +
                Utils.AddCastFromDB("MKT_CAP") + ", " +
                Utils.AddCastFromDB("POSITIVE_EBIT_YEARS") + ", " +
                Utils.AddCastFromDB("SHAREHOLDERS_YIELD") + ", " +
                Utils.AddCastFromDB("BUY_BACK_RATE") + ", " +
                Utils.AddCastFromDB("DEBT_REDUCTION") + ", " +
                Utils.AddCastFromDB("QOQ_EARNINGS") + ", " +
                Utils.AddCastFromDB("EV_EBITDA") + ", " +
                Utils.AddCastFromDB("ROIC_SCORE") + ", " +
                Utils.AddCastFromDB("CROIC_SCORE") + ", " +
                Utils.AddCastFromDB("FCF_SCORE") + ", " +
                Utils.AddCastFromDB("EBITDA_SCORE") + ", " +
                Utils.AddCastFromDB("SALES_SCORE") + ", " +
                Utils.AddCastFromDB("EPS") + ", " +
                Utils.AddCastFromDB("PE") + ", " +
                Utils.AddCastFromDB("PS") + ", " +
                Utils.AddCastFromDB("PB") + ", " +
                Utils.AddCastFromDB("PFCF") + ", " +
                Utils.AddCastFromDB("PE_VAL") + ", " +
                Utils.AddCastFromDB("PS_VAL") + ", " +
                Utils.AddCastFromDB("PB_VAL") + ", " +
                Utils.AddCastFromDB("FCFPS_VAL") + ", " +
                Utils.AddCastFromDB("SPS") + ", " +
                Utils.AddCastFromDB("BVPS") + ", " +
                Utils.AddCastFromDB("FCFPS") + ", " +
                " IFNULL(id_stock, 0) id_stock FROM FILTER WHERE ID_STOCK = " + id.ToString() + " ORDER BY DATE ASC");

            foreach (DataRow row in data.Rows)
            {
                result.Add(new FilterTable(row));
            }

            return result;
        }

        public static void LoadAllFilters(Dictionary<int, Stock> dict, DatabaseSingleton dbConn)
        {
            DataTable data = dbConn.GetData("SELECT IFNULL(DATE, 0) DATE ,  " +
                Utils.AddCastFromDB("CLOSE_PRICE") + ", " +
                Utils.AddCastFromDB("CLOSE_PRICE_1JAN") + ", " +
                Utils.AddCastFromDB("VOLUME") + ", " +
                Utils.AddCastFromDB("AVG_VOLUME") + ", " +
                Utils.AddCastFromDB("CURRENCY_RATIO") + ", " +
                Utils.AddCastFromDB("DEBT_TO_EBIT") + ", " +
                Utils.AddCastFromDB("MKT_CAP") + ", " +
                Utils.AddCastFromDB("POSITIVE_EBIT_YEARS") + ", " +
                Utils.AddCastFromDB("SHAREHOLDERS_YIELD") + ", " +
                Utils.AddCastFromDB("BUY_BACK_RATE") + ", " +
                Utils.AddCastFromDB("DEBT_REDUCTION") + ", " +
                Utils.AddCastFromDB("QOQ_EARNINGS") + ", " +
                Utils.AddCastFromDB("EV_EBITDA") + ", " +
                Utils.AddCastFromDB("ROIC_SCORE") + ", " +
                Utils.AddCastFromDB("CROIC_SCORE") + ", " +
                Utils.AddCastFromDB("FCF_SCORE") + ", " +
                Utils.AddCastFromDB("EBITDA_SCORE") + ", " +
                Utils.AddCastFromDB("SALES_SCORE") + ", " +
                Utils.AddCastFromDB("EPS") + ", " +
                Utils.AddCastFromDB("PE") + ", " +
                Utils.AddCastFromDB("PS") + ", " +
                Utils.AddCastFromDB("PB") + ", " +
                Utils.AddCastFromDB("PFCF") + ", " +
                Utils.AddCastFromDB("PE_VAL") + ", " +
                Utils.AddCastFromDB("PS_VAL") + ", " +
                Utils.AddCastFromDB("PB_VAL") + ", " +
                Utils.AddCastFromDB("FCFPS_VAL") + ", " +
                Utils.AddCastFromDB("SPS") + ", " +
                Utils.AddCastFromDB("BVPS") + ", " +
                Utils.AddCastFromDB("FCFPS") + ", " +
                " IFNULL(id_stock, 0) id_stock FROM FILTER ORDER BY DATE ASC");

            foreach (DataRow row in data.Rows)
            {
                FilterTable filter = new FilterTable(row);
                Stock stock = dict[filter.IdStock];
                stock.FiltersTables.Add(filter);
            }
        }

        public double GetComposite(PortfolioParameters portfolioParams)
        {
            double compositePrice = Convert.ToDouble(
                portfolioParams.CompositePBWeight * this.PB +
                portfolioParams.CompositePEWeight * this.PE +
                portfolioParams.CompositePSWeight * this.PS +
                portfolioParams.CompositePFCFWeight * this.PFCF) / 4;

            if (compositePrice <= 0)
                return 0;

            if (this.ClosePrice == 0)
                return 0;

            return ((double)1 -(compositePrice / (double)this.ClosePrice)) * (double)100;
        }

        public double GetPriceOverPercent(decimal compareValue)
        {
            if (compareValue <= 0)
                return 0;

            if (this.ClosePrice == 0)
                return 0;

            return ((double)1 - ((double)compareValue / (double)this.ClosePrice)) * (double)100;
        }

        public double GetCompositeForPrice(PortfolioParameters portfolioParams, double closePrice)
        {
            double compositePrice = Convert.ToDouble(
                portfolioParams.CompositePBWeight * this.PB +
                portfolioParams.CompositePEWeight * this.PE +
                portfolioParams.CompositePSWeight * this.PS +
                portfolioParams.CompositePFCFWeight * this.PFCF) / 4;

            if (compositePrice <= 0)
                return 0;

            if (closePrice == 0)
                return 0;

            return ((double)1 - (compositePrice / closePrice)) * (double)100;
        }

        public double GetCompositePrice(PortfolioParameters portfolioParams)
        {
            double compositePrice = Convert.ToDouble(
                portfolioParams.CompositePBWeight * this.PB +
                portfolioParams.CompositePEWeight * this.PE +
                portfolioParams.CompositePSWeight * this.PS +
                portfolioParams.CompositePFCFWeight * this.PFCF) / 4;

            return compositePrice;
        }

        public double GetYTD(decimal currentPrice = 0)
        {
            if (currentPrice == 0)
                currentPrice = ClosePrice;

            if (CloseJan1 == 0)
                return 0;

            decimal  ytd = ((ClosePrice - CloseJan1) / CloseJan1) * 100;
            if (Math.Abs(ytd) < (decimal)0.0001)
                return 0;

            return (double)ytd;
        }

        public static void ResetScoreValues()
        {
            for (int i = 0; i < 5; i++)
            {
                if (ratioValues[i] == null)
                {
                    ratioValues[i] = new List<decimal>();
                } else 
                {
                    ratioValues[i].Clear();
                }
                validSum[i] = 0;
                sumX[i] = 0;
                sumXSquare[i] = 0;
                sumXY[i] = 0;
                firstValue[i] = 0;
                lastValue[i] = 0;
            }
        }

        public static decimal GenerateScoreNew(FilterTypes scoreType, int ratioIndex, List<StockRatios> ratios)
        {
            decimal value = 0, invalidVal = 1000000000000000000000000000M;
            int valid, fromID, toID;
            int id = -1;
            int count = 0;
            
            switch (scoreType)
            {
                case FilterTypes.ROICScore:
                    id = 0;
                    break;
                case FilterTypes.CROICScore:
                    id = 1;
                    break;
                case FilterTypes.FCFScore:
                    id = 2;
                    break;
                case FilterTypes.EBITDAScore:
                    id = 3;
                    break;
                case FilterTypes.SALESScore:
                    id = 4;
                    break;
            }
            if (id == -1)
            {
                return -1000000;
            }

            count = ratioValues[id].Count;
            
            if (count == 0)
            {
                fromID = ratioIndex - 7;
                if (fromID < 0) fromID = 0;
                toID = ratioIndex;
            } else
            {
                fromID = ratioIndex;
                toID = ratioIndex;
            }

            for (int i = fromID; i <= toID; i++)
            {
                valid = 1;
                switch (scoreType)
                {
                    case FilterTypes.ROICScore:
                        value = ratios[i].returnInvestedCap;
                        break;
                    case FilterTypes.CROICScore:
                        if (ratios[i].InvestedCapital == 0)
                        {
                            value = invalidVal;
                            valid = 0;
                        }
                        else
                            value = ratios[i].freeCashFlow / ratios[i].InvestedCapital;
                        break;
                    case FilterTypes.FCFScore:
                        value = ratios[i].FCFPS;
                        break;
                    case FilterTypes.EBITDAScore:
                        value = ratios[i].EBITDA;
                        break;
                    case FilterTypes.SALESScore:
                        value = ratios[i].SalesUSD;
                        break;
                }
                ratioValues[id].Add(value);
                count++;
                sumX[id] += (double)value;
                sumXSquare[id] += (double)value * (double)value;
                sumXY[id] += (double)value * count;
                validSum[id] += valid;
            }
            
            if (count < 8)
            {
                return -1000000;
            } else if (count == 8)
            {
                firstValue[id] = (double)(ratioValues[id][0] + ratioValues[id][1] + ratioValues[id][2] + ratioValues[id][3] + ratioValues[id][4]);
                lastValue[id] = (double)(ratioValues[id][3] + ratioValues[id][4] + ratioValues[id][5] + ratioValues[id][6] + ratioValues[id][7]);
            } else
            {
                sumXY[id] -= sumX[id];
                validSum[id] -= ratioValues[id][0] == invalidVal ? 0 : 1;
                sumX[id] -= (double)ratioValues[id][0];
                sumXSquare[id] -= (double)ratioValues[id][0] * (double)ratioValues[id][0];
                firstValue[id] = firstValue[id] - (double)ratioValues[id][0] + (double)ratioValues[id][5];
                lastValue[id] = lastValue[id] - (double)ratioValues[id][3] + (double)ratioValues[id][8];
                ratioValues[id].RemoveAt(0);
                count--;
            }
            
            if (validSum[id] != 8)
            {
                return -1000000;
            }

            double cagr = lastValue[id] / firstValue[id];
            if (Math.Abs(firstValue[id]) < 0.00000001)
            {
                return 0;
            }

            
            try
            {
                double SumX = sumX[id],
                       SumXSquare = sumXSquare[id],
                       SumXY = sumXY[id], r = 0.00;
                double SumY = 36.00; // 1 + 2 + ... + 8
                double SumYSquare = 204.00; // 1 + 4 + ... + 64
                
                double NTimesSumXY = count * SumXY;
                double SumXTimesSumY = SumX * SumY;
                double SquareRoot1 = count * SumXSquare - Math.Pow(SumX, 2);
                double SquareRoot2 = count * SumYSquare - Math.Pow(SumY, 2);

                r = (NTimesSumXY - SumXTimesSumY) / (Math.Sqrt(SquareRoot1) *
                                     Math.Sqrt(SquareRoot2));
                if (Double.IsNaN(r))
                {
                    return -1000000;
                }
                double score = cagr * Math.Pow(r, 6);
                if (Double.IsNaN(score) || score > (double)invalidVal || score < (double)-invalidVal)
                {
                    return -1000000;
                }
                else
                {
                    return Convert.ToDecimal(score);
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static decimal GenerateScore(FilterTypes scoreType, int ratioIndex, List<StockRatios> ratios)
        {
            //get values from previous ratios and calc scores
            List<System.Drawing.PointF> values = new List<System.Drawing.PointF>();
            float firstValue = 0;
            float lastValue = 0;

            //if there are not enough historic values
            if (ratioIndex < 7)
                return -1000000;

            for (int i = 0; i < 8; i++)
            {
                int index = ratioIndex - 7 + i;
                decimal value = 0;

                switch (scoreType)
                {
                    case FilterTypes.ROICScore:
                        value = ratios[index].returnInvestedCap;
                        break;
                    case FilterTypes.CROICScore:
                        if (ratios[index].InvestedCapital == 0)
                            return -1000000;
                        value = ratios[index].freeCashFlow / ratios[index].InvestedCapital;
                        break;
                    case FilterTypes.FCFScore:
                        value = ratios[index].FCFPS;
                        break;
                    case FilterTypes.EBITDAScore:
                        value = ratios[index].EBITDA;
                        break;
                    case FilterTypes.SALESScore:
                        value = ratios[index].SalesUSD;
                        break;
                }

                values.Add(new System.Drawing.PointF((float)value, i+1));
            }

            firstValue = values[0].X + values[1].X + values[2].X + values[3].X + values[4].X;
            lastValue = values[3].X + values[4].X + values[5].X + values[6].X + values[7].X;

            double cagr = lastValue / firstValue;

            if (Math.Abs(firstValue) < 0.00000001)
            {
                return 0;
            }
            double r2 = StatisticsMath.GenerateR2(values);
            double score = cagr * Math.Pow(r2, 3);

            if (Double.IsNaN(score) || score > 1000000000000D|| score < -1000000000000D)
            {
                return -1000000;
            } else
            {
                return Convert.ToDecimal(score);
            }
        }

        internal static void UpdateFilterScores(decimal rOICScore, decimal cROICScore, decimal fCFScore, decimal eBITDAScore, decimal sALESScore, Stock stock, DateTime quarterStart, DateTime quarterEnd)
        {
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();

            if(rOICScore == decimal.MinValue)
                parameters.Add(new SQLiteParameter("@p1", null));
            else
                parameters.Add(new SQLiteParameter("@p1", rOICScore));

            if (cROICScore == decimal.MinValue)
                parameters.Add(new SQLiteParameter("@p2", null));
            else
                parameters.Add(new SQLiteParameter("@p2", cROICScore));

            if (fCFScore == decimal.MinValue)
                parameters.Add(new SQLiteParameter("@p3", null));
            else
                parameters.Add(new SQLiteParameter("@p3", fCFScore));
            
            if (eBITDAScore == decimal.MinValue)
                parameters.Add(new SQLiteParameter("@p4", null));
            else
                parameters.Add(new SQLiteParameter("@p4", eBITDAScore));

            if (sALESScore == decimal.MinValue)
                parameters.Add(new SQLiteParameter("@p8", null));
            else
                parameters.Add(new SQLiteParameter("@p8", sALESScore));

            

            parameters.Add(new SQLiteParameter("@p5", stock.Id));
            parameters.Add(new SQLiteParameter("@p6", Utils.ConvertDateTimeToInt(quarterStart)));
            parameters.Add(new SQLiteParameter("@p7", Utils.ConvertDateTimeToInt(quarterEnd)));

            DatabaseSingleton.Instance.ExecuteNonQuery("UPDATE FILTER SET ROIC_SCORE_AUX = @p1, CROIC_SCORE_AUX = @p2, FCF_SCORE_AUX = @p3, EBITDA_SCORE_AUX = @p4, SALES_SCORE_AUX = @p8 WHERE id_stock = @p5 AND DATE BETWEEN @p6 AND @p7", parameters.ToArray(), CommandSaved);
            CommandSaved = true;
        }

        public static Dictionary<int, int> GetLastFilterDates(int maxDate)
        {
            int lastDate = 0;
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT MAX(DATE), ID_STOCK FROM FILTER WHERE DATE <= " + maxDate.ToString() + " GROUP BY ID_STOCK ");
            Dictionary<int, int> results = new Dictionary<int, int>();
            foreach (DataRow row in data.Rows)
            {
                results.Add(Convert.ToInt32(row[1]), Convert.ToInt32(row[0]));
            }

            return results;
        }


    }
}
