using StockRanking.Entities;
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
    public class FilterConditions
    {
        public FilterTypes FilterType { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }
        public FilterRanges FilterRange { get; set; }
        public int Order { get; set; }
        public int Option { get; set; }
        public String equation1 { get; set; }
        public string equation2 { get; set; }
        public int equCompare { get; set; }
        public int isCustom { get; set; }

        public FilterConditions()
        {

        }

        public FilterConditions(DataRow row)
        {
            FilterType = (FilterTypes)Convert.ToInt32(row["FILTER_TYPE"]);
            FilterRange = (FilterRanges)Convert.ToInt32(row["CONDITION"]);
            Value1 = Convert.ToDouble(row["PARAMETER1"]);
            Value2= Convert.ToDouble(row["PARAMETER2"]);
            Order = Convert.ToInt32(row["APPLYORDER"]);
            Option = Convert.ToInt32(row["APPLYOPTION"]);
            equation1 = Convert.ToString(row["EQUATION1"]);
            equation2 = Convert.ToString(row["EQUATION2"]);
            equCompare = Convert.ToInt32(row["EQUCOMPARE"]);
            isCustom = Convert.ToInt32(row["ISCUSTOM"]);
        }

        public FilterConditions(EquationFilter ef)
        {
            FilterType = 0;
            FilterRange = (FilterRanges)ef.FilterType;
            Value1 = Convert.ToDouble(ef.Value1);
            Value2 = Convert.ToDouble(ef.Value2);
            Order = 0;
            Option = ef.FilterOption;
            equation1 = ef.Equation1;
            equation2 = ef.Equation2;
            equCompare = ef.EquCompare;
            isCustom = ef.Id;
        }

        private int findBracket(String str, int i)
        {
            int r = 0;
            while (i >= 0)
            {
                if (str[i] == ')') r++;
                if (str[i] == '(') r--;
                if (r == 0) return i;
                i--;
            }
            return i;
        }
        public double CalcValue(String str, int st, int en, Stock stock, out bool error)
        {
            //if (stock.IdStock > 1000 && stock.IdStock < 1010)
            //{
            //    Console.WriteLine(stock.Symbol + " " + str.Substring(st, en - st));
            //}
            error = false;

            while (en > st && str[en - 1] == ' ') en--;
            while (en > st && str[st] == ' ') st++;

            if (st >= en)
            {
                error = true;
                return Double.MinValue;
            }

            String op = "+-*/";
            int p = en - 1;
            if (str[p] == ')')
            {
                p = findBracket(str, p);
                if (p < st)
                {
                    error = true;
                    return Double.MinValue;
                }
                if (p == st)
                {
                    return CalcValue(str, st + 1, en - 1, stock, out error);
                }
            }
            p = en - 1;
            while (p >= st)
            {
                if (str[p] == ')') p = findBracket(str, p);
                if (p < st)
                {
                    error = true;
                    return Double.MinValue;
                }

                if (str[p] == '+' || str[p] == '-')
                {
                    Double left = CalcValue(str, st, p, stock, out error);
                    if (error)
                    {
                        return Double.MinValue;
                    }
                    Double right = CalcValue(str, p+1, en, stock, out error);
                    if (error)
                    {
                        return Double.MinValue;
                    }
                    if (str[p] == '+')
                    {
                        return left + right;
                    } else
                    {
                        return left - right;
                    }
                }
                p--;
            }

            p = en - 1;
            while (p >= st)
            {
                if (str[p] == ')') p = findBracket(str, p);
                if (p < st)
                {
                    error = true;
                    return Double.MinValue;
                }

                if (str[p] == '*' || str[p] == '/')
                {
                    Double left = CalcValue(str, st, p, stock, out error);
                    if (error)
                    {
                        return Double.MinValue;
                    }
                    Double right = CalcValue(str, p + 1, en, stock, out error);
                    if (error)
                    {
                        return Double.MinValue;
                    }
                    if (str[p] == '*')
                    {
                        return left * right;
                    }
                    else
                    {
                        if (right != 0)
                            return left / right;
                        else
                        {
                            error = true;
                            return Double.MinValue;
                        }
                    }
                }
                p--;
            }

            if (Char.IsDigit(str[st]))
            {
                try
                {
                    Double value = Convert.ToDouble(str.Substring(st, en - st));
                    return value;
                }
                catch (Exception e)
                {
                    error = true;
                    return Double.MinValue;
                }
            }
            String val = str.Substring(st, en - st);
            //if (stock.IdStock > 1000 && stock.IdStock < 1010)
            //{
            //    Console.WriteLine("Last = " + str.Substring(st, en - st));
            //}

            switch (val)
            {
                case "volume":
                    return stock.CurrentFilters.Volume;
                    break;
                case "closeprice":
                    return Convert.ToDouble(stock.CurrentFilters.ClosePrice);
                    break;
                case "ebitda":
                    return Convert.ToDouble(stock.CurrentRatio.EBITDA);
                    break;
                case "ebitdausd":
                    return Convert.ToDouble(stock.CurrentRatio.ebitdausd);
                    break;
                case "ebitdamargin":
                    return Convert.ToDouble(stock.CurrentRatio.ebitdamargin);
                    break;
                case "sharesbas":
                    return Convert.ToDouble(stock.CurrentRatio.SharesOut);
                    break;
                case "debt":
                    return Convert.ToDouble(stock.CurrentRatio.LtermTotalDebt);
                    break;
                case "fcf":
                    return Convert.ToDouble(stock.CurrentRatio.freeCashFlow);
                    break;
                case "roic":
                    return Convert.ToDouble(stock.CurrentRatio.returnInvestedCap);
                    break;
                case "currentratio":
                    return Convert.ToDouble(stock.CurrentRatio.CurrRatio);
                    break;
                case "divyield":
                    return Convert.ToDouble(stock.CurrentRatio.DividendYield);
                    break;
                case "ev":
                    return Convert.ToDouble(stock.CurrentRatio.EV);
                    break;
                case "bvps":
                    return Convert.ToDouble(stock.CurrentRatio.BVPS);
                    break;
                case "marketcap":
                    return Convert.ToDouble(stock.CurrentRatio.MarketCap);
                    break;
                case "invcap":
                    return Convert.ToDouble(stock.CurrentRatio.InvestedCapital);
                    break;
                case "revenueusd":
                    return Convert.ToDouble(stock.CurrentRatio.SalesUSD);
                    break;
                case "eps":
                    return Convert.ToDouble(stock.CurrentRatio.EPS);
                    break;
                case "pe1":
                    return Convert.ToDouble(stock.CurrentRatio.PE);
                    break;
                case "ps1":
                    return Convert.ToDouble(stock.CurrentRatio.PS);
                    break;
                case "pb":
                    return Convert.ToDouble(stock.CurrentRatio.PB);
                    break;
                case "price":
                    return Convert.ToDouble(stock.CurrentRatio.StockPrice);
                    break;
                case "sps":
                    return Convert.ToDouble(stock.CurrentRatio.SPS);
                    break;
                case "fcfps":
                    return Convert.ToDouble(stock.CurrentRatio.FCFPS);
                    break;
                case "cor":
                    return Convert.ToDouble(stock.CurrentRatio.cor);
                    break;
                case "netinc":
                    return Convert.ToDouble(stock.CurrentRatio.netinc);
                    break;
                case "epsdil":
                    return Convert.ToDouble(stock.CurrentRatio.epsdil);
                    break;
                case "shareswa":
                    return Convert.ToDouble(stock.CurrentRatio.shareswa);
                    break;
                case "capex":
                    return Convert.ToDouble(stock.CurrentRatio.capex);
                    break;
                case "assets":
                    return Convert.ToDouble(stock.CurrentRatio.assets);
                    break;
                case "cashneq":
                    return Convert.ToDouble(stock.CurrentRatio.cashneq);
                    break;
                case "liabilities":
                    return Convert.ToDouble(stock.CurrentRatio.liabilities);
                    break;
                case "assetsc":
                    return Convert.ToDouble(stock.CurrentRatio.assetsc);
                    break;
                case "liabilitiesc":
                    return Convert.ToDouble(stock.CurrentRatio.liabilitiesc);
                    break;
                case "tangibles":
                    return Convert.ToDouble(stock.CurrentRatio.tangibles);
                    break;
                case "roe":
                    return Convert.ToDouble(stock.CurrentRatio.roe);
                    break;
                case "roa":
                    return Convert.ToDouble(stock.CurrentRatio.roa);
                    break;
                case "gp":
                    return Convert.ToDouble(stock.CurrentRatio.gp);
                    break;
                case "grossmargin":
                    return Convert.ToDouble(stock.CurrentRatio.grossmargin);
                    break;
                case "netmargin":
                    return Convert.ToDouble(stock.CurrentRatio.netmargin);
                    break;
                case "ros":
                    return Convert.ToDouble(stock.CurrentRatio.ros);
                    break;
                case "assetturnover":
                    return Convert.ToDouble(stock.CurrentRatio.assetturnover);
                    break;
                case "payoutratio":
                    return Convert.ToDouble(stock.CurrentRatio.payoutratio);
                    break;
                case "workingcapital":
                    return Convert.ToDouble(stock.CurrentRatio.workingcapital);
                    break;
                case "tbvps":
                    return Convert.ToDouble(stock.CurrentRatio.tbvps);
                    break;
            }
            
            error = true;
            return Double.MinValue;
        }

        public bool CheckFilter(Stock stock, PortfolioParameters portfolioParams, out double outValue)
        {
            if (isCustom > 0)
            {
                outValue = 0;

                if (stock.CurrentRatio == null)
                {
                    return false;
                }

                String left = "";
                if ((int)FilterRange < 3)
                {
                    left = equation1;
                } else
                {
                    left = "(" + equation1 + ")-(" + equation2 +")";
                }
                bool error = false;
                outValue = CalcValue(left, 0, left.Length, stock, out error);
                stock.filterResults[isCustom.ToString()] = outValue;
                if (error) return false;

                if (Option == 0)
                {
                    switch (FilterRange)
                    {
                        case FilterRanges.MoreThan:
                            if (outValue > Value1)
                                return true;
                            break;
                        case FilterRanges.LessThan:
                            if (outValue < Value1)
                                return true;
                            break;
                        case FilterRanges.Between:
                            if (outValue >= Value1 && outValue <= Value2)
                                return true;
                            break;
                    }
                    return false;
                }
                else if (FilterRange == FilterRanges.CompareEquation)
                {
                    if (equCompare == 0)
                    {
                        if (outValue >= 0)
                            return true;
                    }
                    else
                    {
                        if (outValue <= 0)
                            return true;
                    }
                    return false;
                }
                else 
                {
                    return true;
                }

                return true;
            }

            outValue = 0;
            if (this.FilterType == FilterTypes.VRUniverse ||
                this.FilterType == FilterTypes.VRSector ||
                this.FilterType == FilterTypes.VRHistory)
            {
                outValue = 0;
                return true;
            }

            if (this.FilterType == FilterTypes.MovingAverage)
            {
                try
                {
                    int daysAvg = Convert.ToInt32(this.Value1);
                    int date = stock.CurrentFilters.Date;
                    
                    double movingavg = Convert.ToDouble(DatabaseSingleton.Instance.GetData("select avg(close_price) from(select close_price from price_history where id_stock = " + stock.Id.ToString() + " and date <= " + date.ToString() + " order by date desc limit " + daysAvg.ToString() + ")")
                        .Rows[0][0]);

                    //Console.WriteLine(daysAvg.ToString() + ", " + date.ToString() + ", " + movingavg.ToString() + ", " + stock.CurrentFilters.ClosePrice.ToString());
                    stock.filterResults[Enum.GetName(typeof(FilterTypes), FilterType)] = (Convert.ToDouble(stock.CurrentFilters.ClosePrice) - movingavg);

                    if (this.FilterRange == FilterRanges.LessThan)
                    {
                        return Convert.ToDouble(stock.CurrentFilters.ClosePrice) <= movingavg;
                    } else if (this.FilterRange == FilterRanges.MoreThan)
                    {
                        return Convert.ToDouble(stock.CurrentFilters.ClosePrice) >= movingavg;
                    } else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    return false;

                }
            }
            else if (this.FilterType == FilterTypes.RoCVSBenchmark)
            {
                try
                {
                    int daysAvg = Convert.ToInt32(this.Value1);
                    int date = stock.CurrentFilters.Date;

                    DataTable data = DatabaseSingleton.Instance.GetData("select close_price from price_history where id_stock = " + stock.Id.ToString() + " and date <= " + date.ToString() + " order by date desc limit " + daysAvg.ToString());
                    int count = data.Rows.Count;
                    double prevPrice = Convert.ToDouble(data.Rows[count - 1][0]);

                    DataTable SPYdata = DatabaseSingleton.Instance.GetData("select close_price from price_history where id_stock = -1 and date <= " + date.ToString() + " order by date desc limit " + daysAvg.ToString());
                    int SPYcount = SPYdata.Rows.Count;
                    double SPYcurPrice = Convert.ToDouble(SPYdata.Rows[0][0]);
                    double SPYprevPrice = Convert.ToDouble(SPYdata.Rows[SPYcount - 1][0]);

                    double RoC = (Convert.ToDouble(stock.CurrentFilters.ClosePrice) - prevPrice) / prevPrice * 100;
                    double SPYRoC = (SPYcurPrice - SPYprevPrice) / SPYprevPrice * 100;

                    stock.filterResults[Enum.GetName(typeof(FilterTypes), FilterType)] = (RoC-SPYRoC);
                    //Console.WriteLine(stock.Id.ToString() + ", " + date.ToString() + ", " + RoC.ToString() + ", " + SPYRoC.ToString());

                    if (this.FilterRange == FilterRanges.MoreThan)
                    {
                        return RoC >= SPYRoC;
                    }                    
                    else if (this.FilterRange == FilterRanges.LessThan)
                    {
                        return RoC <= SPYRoC;
                    } else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    return false;

                }
            }
            else if (this.FilterType == FilterTypes.RoC)
            {
                try
                {
                    int daysAvg = Convert.ToInt32(this.Value2);
                    double X = Convert.ToDouble(this.Value1);

                    int date = stock.CurrentFilters.Date;

                    DataTable data = DatabaseSingleton.Instance.GetData("select close_price from price_history where id_stock = " + stock.Id.ToString() + " and date <= " + date.ToString() + " order by date desc limit " + daysAvg.ToString());
                    int count = data.Rows.Count;
                    double prevPrice = Convert.ToDouble(data.Rows[count - 1][0]);

                    double RoC = (Convert.ToDouble(stock.CurrentFilters.ClosePrice) - prevPrice) / prevPrice * 100;

                    //Console.WriteLine(stock.Id.ToString() + ", " + stock.Symbol+ ", " +  date.ToString() + ", " + RoC.ToString());

                    stock.filterResults[Enum.GetName(typeof(FilterTypes), FilterType)] = RoC;

                    if (this.FilterRange == FilterRanges.MoreThan)
                    {
                        return RoC >= X;
                    }
                    else if (this.FilterRange == FilterRanges.LessThan)
                    {
                        return RoC <= X;
                    } else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    return false;

                }
            }

            return CheckFilter(stock, portfolioParams, 0, out outValue);
        }

        public bool CheckFilter(Stock stock, PortfolioParameters portfolioParams, double vrValue, out double outValue)
        {
            double ValueToFilter = 0;
            double ValueToFilter1 = 0;
            outValue = 0;

            if (isCustom > 0)
            {
                return true;
            }

            if (stock.CurrentFilters == null)
                return false;


            switch (FilterType)
            {
                case FilterTypes.AvgVolume: 
                    ValueToFilter = Convert.ToDouble(stock.CurrentFilters.AvgVolume);
                    break;
                case FilterTypes.ClosingPrice:
                    ValueToFilter = Convert.ToDouble(stock.CurrentFilters.ClosePrice);
                    break;
                case FilterTypes.CurrentRatio:
                    ValueToFilter = Convert.ToDouble(stock.CurrentFilters.CurrRatio);
                    break;
                case FilterTypes.DebtToEBIT:
                    ValueToFilter = Convert.ToDouble(stock.CurrentFilters.DebtToEbit);
                    break;
                case FilterTypes.MktCapitalization:
                    ValueToFilter = Convert.ToDouble(stock.CurrentFilters.MktCap);
                    break;
                case FilterTypes.PositiveEBITYears:
                    ValueToFilter = Convert.ToDouble(stock.CurrentFilters.PositiveEbitYears);
                    break;
                case FilterTypes.Volume:
                    ValueToFilter = Convert.ToDouble(stock.CurrentFilters.Volume);
                    break;
                case FilterTypes.ShareholdersYield:
                    ValueToFilter = Convert.ToDouble(stock.CurrentFilters.ShareholdersYield);
                    break;
                case FilterTypes.BuyBackRate:
                    ValueToFilter = Convert.ToDouble(stock.CurrentFilters.BuyBackRate);
                    break;
                case FilterTypes.DebtReduction:
                    ValueToFilter = Convert.ToDouble(stock.CurrentFilters.DebtReduction);
                    break;
                case FilterTypes.QoQEarnings:
                    ValueToFilter = Convert.ToDouble(stock.CurrentFilters.QoQEarnings);
                    break;
                case FilterTypes.EV_EBITDA:
                    ValueToFilter = Convert.ToDouble(stock.CurrentFilters.EvEBITDA);
                    break;
                case FilterTypes.ROICScore:
                    if (stock.CurrentFilters.RoicScore == decimal.MinValue) 
                        return false;
                    ValueToFilter = Convert.ToDouble(stock.CurrentFilters.RoicScore);
                    break;
                case FilterTypes.CROICScore:
                    if (stock.CurrentFilters.CroicScore == decimal.MinValue)
                        return false;
                    ValueToFilter = Convert.ToDouble(stock.CurrentFilters.CroicScore);
                    break;
                case FilterTypes.FCFScore:
                    if (stock.CurrentFilters.FcfScore == decimal.MinValue)
                        return false;
                    ValueToFilter = Convert.ToDouble(stock.CurrentFilters.FcfScore);
                    break;
                case FilterTypes.EBITDAScore:
                    if (stock.CurrentFilters.EbitdaScore == decimal.MinValue)
                        return false;
                    ValueToFilter = Convert.ToDouble(stock.CurrentFilters.EbitdaScore);
                    break;
                case FilterTypes.PB:
                    ValueToFilter = stock.CurrentFilters.GetPriceOverPercent(stock.CurrentFilters.PB);
                    break;
                case FilterTypes.PE:
                    ValueToFilter = stock.CurrentFilters.GetPriceOverPercent(stock.CurrentFilters.PE);
                    break;
                case FilterTypes.PFCF:
                    ValueToFilter = stock.CurrentFilters.GetPriceOverPercent(stock.CurrentFilters.PFCF);
                    break;
                case FilterTypes.PS:
                    ValueToFilter = stock.CurrentFilters.GetPriceOverPercent(stock.CurrentFilters.PS);
                    break;
                case FilterTypes.Composite:
                    ValueToFilter = stock.CurrentFilters.GetComposite(portfolioParams);
                    break;
                case FilterTypes.VRSector:
                    ValueToFilter = vrValue;
                    break;
                case FilterTypes.VRUniverse:
                    ValueToFilter = vrValue;
                    break;
                case FilterTypes.VRHistory:
                    ValueToFilter = vrValue;
                    break;
            }

            if (ValueToFilter == double.MinValue)
                return false;

            outValue = ValueToFilter;
            if (isCustom == 0)
            {
                stock.filterResults[Enum.GetName(typeof(FilterTypes), FilterType)] = outValue;
            }
            else
            {
                stock.filterResults[isCustom.ToString()] = outValue;
            }

            if (Option == 0)
            {
                switch (FilterRange)
                {
                    case FilterRanges.MoreThan:
                        if (ValueToFilter > Value1)
                            return true;
                        break;
                    case FilterRanges.LessThan:
                        if (ValueToFilter < Value1)
                            return true;
                        break;
                    case FilterRanges.Between:
                        if (ValueToFilter >= Value1 && ValueToFilter <= Value2)
                            return true;
                        break;
                }
            } else
            {
                return true;
            }
            return false;
        }

        public void Save(int idStrategy)
        {

            List<SQLiteParameter> parameters = new List<SQLiteParameter>();

            parameters.Add(new SQLiteParameter("@p1", idStrategy));
            parameters.Add(new SQLiteParameter("@p2", (int)this.FilterType));
            parameters.Add(new SQLiteParameter("@p3", (int)this.FilterRange));
            parameters.Add(new SQLiteParameter("@p4", this.Value1));
            parameters.Add(new SQLiteParameter("@p5", this.Value2));
            parameters.Add(new SQLiteParameter("@p6", this.Order));
            parameters.Add(new SQLiteParameter("@p7", this.Option));
            parameters.Add(new SQLiteParameter("@p8", this.isCustom));
            parameters.Add(new SQLiteParameter("@p9", this.equCompare));
            parameters.Add(new SQLiteParameter("@p10", this.equation1));
            parameters.Add(new SQLiteParameter("@p11", this.equation2));

            DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO STRATEGY_FILTER (ID_STRATEGY, FILTER_TYPE, CONDITION, PARAMETER1, PARAMETER2, APPLYORDER, APPLYOPTION, ISCUSTOM, EQUCOMPARE, EQUATION1, EQUATION2) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11)", parameters.ToArray());
        }


        public static List<FilterConditions> LoadFilters(int idStrategy)
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT * FROM STRATEGY_FILTER WHERE ID_STRATEGY = " + idStrategy.ToString());
            List<FilterConditions> result = new List<FilterConditions>();

            foreach(DataRow row in data.Rows)
            {
                FilterConditions cond = new FilterConditions(row);
                if (cond.isCustom > 0)
                {
                    EquationFilter ef = EquationFilter.GetFilter(cond.isCustom);
                    if (ef.FilterOption < 0) continue;
                    FilterConditions newf = new FilterConditions(ef);
                    newf.Order = cond.Order;
                    result.Add(newf);
                    continue;
                }

                result.Add(cond);
            }
                

            return result;
        }



    }
}
