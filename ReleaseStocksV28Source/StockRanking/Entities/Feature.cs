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
    public class Feature
    {
        int years = 0;
        int ranking = 0;
        float rankedValue = 0;
        float featureValue = 0;
        FeatureTypes featureType = FeatureTypes.FCFGrowth;
        int featureId = 0;
        decimal featureWeight = 0;

        public int Years { get => years; set => years = value; }
        public int Ranking { get => ranking; set => ranking = value; }
        public float RankedValue { get => rankedValue; set => rankedValue = value; }
        public float FeatureValue { get => featureValue; set => featureValue = value; }
        public FeatureTypes FeatureType { get => featureType; set => featureType = value; }
        public int FeatureId { get => this.featureId; set => this.featureId = value; }
        public decimal FeatureWeight { get => this.featureWeight; set => this.featureWeight = value; }

        public Feature()
        {

        }

        public Feature(FeatureWeight weight)
        {
            this.featureWeight = (decimal)weight.Weight;
            this.featureId = weight.FeatureIndex;
        }

        public Feature(DataRow row)
        {
            featureId = Convert.ToInt32(row["ID_FEATURE"]);
            featureWeight = Convert.ToDecimal(row["WEIGHT"]);
        }

        public Feature(FeatureTypes featureType, float value)
        {
            this.featureType = featureType;
            this.featureValue = value;
        }

        public Feature(FeatureTypes featureType, float value, int years)
        {
            this.featureType = featureType;
            this.featureValue = value;
            this.years = years;
        }

        public static List<FeatureWeight> generateFeatureIntervalList()
        {
            List<FeatureWeight> featureWeights = generateFeatureWeightsList();
            for (int i = 0; i < featureWeights.Count; i++)
            {
                featureWeights[i].From = featureWeights[i].Weight;
                featureWeights[i].To = featureWeights[i].Weight;
            }
            return featureWeights;
        }

        public static List<FeatureWeight> generateFeatureWeightsList()
        {
            List<FeatureWeight> featureWeights = new List<FeatureWeight>();
            int index = 0;

            featureWeights.Add(new FeatureWeight("FCFPS 1 Year Growth", index++, "FCF1G", 1));
            featureWeights.Add(new FeatureWeight("FCFPS 3 Year Growth", index++, "FCF3G", 3));
            featureWeights.Add(new FeatureWeight("FCFPS 5 Year Growth", index++, "FCF5G", 5));
            featureWeights.Add(new FeatureWeight("FCFPS 1 Year Median", index++, "FCF1M", 1));
            featureWeights.Add(new FeatureWeight("FCFPS 3 Year Median", index++, "FCF3M", 3));
            featureWeights.Add(new FeatureWeight("FCFPS 5 Year Median", index++, "FCF5M", 5));

            featureWeights.Add(new FeatureWeight("ROIC 1 Year Growth", index++, "ROIC1G", 1));
            featureWeights.Add(new FeatureWeight("ROIC 3 Year Growth", index++, "ROIC3G", 3));
            featureWeights.Add(new FeatureWeight("ROIC 5 Year Growth", index++, "ROIC5G", 5));
            featureWeights.Add(new FeatureWeight("ROIC 1 Year Median", index++, "ROIC1M", 1));
            featureWeights.Add(new FeatureWeight("ROIC 3 Year Median", index++, "ROIC3M", 3));
            featureWeights.Add(new FeatureWeight("ROIC 5 Year Median", index++, "ROIC5M", 5));

            featureWeights.Add(new FeatureWeight("EBITDA 1 Year Growth", index++, "EBITDA1G", 1));
            featureWeights.Add(new FeatureWeight("EBITDA 3 Year Growth", index++, "EBITDA3G", 3));
            featureWeights.Add(new FeatureWeight("EBITDA 5 Year Growth", index++, "EBITDA5G", 5));
            featureWeights.Add(new FeatureWeight("EBITDA 1 Year Median", index++, "EBITDA1M", 1));
            featureWeights.Add(new FeatureWeight("EBITDA 3 Year Median", index++, "EBITDA3M", 3));
            featureWeights.Add(new FeatureWeight("EBITDA 5 Year Median", index++, "EBITDA5M", 5));

            featureWeights.Add(new FeatureWeight("FIP Score", index++, "FIP", 0));

            featureWeights.Add(new FeatureWeight("EV 1 Year Growth", index++, "EV1G", 1));
            featureWeights.Add(new FeatureWeight("EV 3 Year Growth", index++, "EV3G", 3));
            featureWeights.Add(new FeatureWeight("EV 5 Year Growth", index++, "EV5G", 5));
            featureWeights.Add(new FeatureWeight("EV 1 Year Median", index++, "EV1M", 1));
            featureWeights.Add(new FeatureWeight("EV 3 Year Median", index++, "EV3M", 3));
            featureWeights.Add(new FeatureWeight("EV 5 Year Median", index++, "EV5M", 5));

            featureWeights.Add(new FeatureWeight("BVPS 1 Year Growth", index++, "BVPS1G", 1));
            featureWeights.Add(new FeatureWeight("BVPS 3 Year Growth", index++, "BVPS3G", 3));
            featureWeights.Add(new FeatureWeight("BVPS 5 Year Growth", index++, "BVPS5G", 5));
            featureWeights.Add(new FeatureWeight("BVPS 1 Year Median", index++, "BVPS1M", 1));
            featureWeights.Add(new FeatureWeight("BVPS 3 Year Median", index++, "BVPS3M", 3));
            featureWeights.Add(new FeatureWeight("BVPS 5 Year Median", index++, "BVPS5M", 5));

            featureWeights.Add(new FeatureWeight("CROIC 1 Year Growth", index++, "CROIC1G", 1));
            featureWeights.Add(new FeatureWeight("CROIC 3 Year Growth", index++, "CROIC3G", 3));
            featureWeights.Add(new FeatureWeight("CROIC 5 Year Growth", index++, "CROIC5G", 5));
            featureWeights.Add(new FeatureWeight("CROIC 1 Year Median", index++, "CROIC1M", 1));
            featureWeights.Add(new FeatureWeight("CROIC 3 Year Median", index++, "CROIC3M", 3));
            featureWeights.Add(new FeatureWeight("CROIC 5 Year Median", index++, "CROIC5M", 5));
             
            featureWeights.Add(new FeatureWeight("MTUM Score", index++, "MTUM", 0));

            featureWeights.Add(new FeatureWeight("Sortino FIP Score", index++, "SORTINO", 0));

            featureWeights.Add(new FeatureWeight("SALES 1 Year Growth", index++, "SALES1G", 1));
            featureWeights.Add(new FeatureWeight("SALES 3 Year Growth", index++, "SALES3G", 3));
            featureWeights.Add(new FeatureWeight("SALES 5 Year Growth", index++, "SALES5G", 5));
            featureWeights.Add(new FeatureWeight("SALES 1 Year Median", index++, "SALES1M", 1));
            featureWeights.Add(new FeatureWeight("SALES 3 Year Median", index++, "SALES3M", 3));
            featureWeights.Add(new FeatureWeight("SALES 5 Year Median", index++, "SALES5M", 5));

            featureWeights.Add(new FeatureWeight("PEG RATIO", index++, "PEG_RATIO", 5));
            featureWeights.Add(new FeatureWeight("EBITDA / Liabilities", index++, "EBITDA_Liabilities", 5));
            featureWeights.Add(new FeatureWeight("GP / Assets", index++, "GP_Assets", 5));
            featureWeights.Add(new FeatureWeight("FCF / Sales", index++, "FCF_Sales", 5));
            featureWeights.Add(new FeatureWeight("EV / EBITDA", index++, "EV_EBITDA", 5));
            featureWeights.Add(new FeatureWeight("Close / FCFPS", index++, "Close_FCF", 5));
            featureWeights.Add(new FeatureWeight("EV / Revenue", index++, "EV_Revenue", 5));
            featureWeights.Add(new FeatureWeight("Dividend + BuyBackRate", index++, "Dividend_BuyBackRate", 5));
            featureWeights.Add(new FeatureWeight("Operating Margin", index++, "Operating_Margin", 5));
            featureWeights.Add(new FeatureWeight("ROIC / (EV / EBITDA)", index++, "ROIC_EV_EBITDA", 5));
            
            featureWeights.Add(new FeatureWeight("ROIC Score", index++, "ROICSCORE", 0));
            featureWeights.Add(new FeatureWeight("CROIC Score", index++, "CROICSCORE", 0));
            featureWeights.Add(new FeatureWeight("FCFPS Score", index++, "FCFSCORE", 0));
            featureWeights.Add(new FeatureWeight("EBITDA Score", index++, "EBITDASCORE", 0));
            featureWeights.Add(new FeatureWeight("SALES Score", index++, "SALESSCORE", 0));

            featureWeights.Add(new FeatureWeight("VR Universe", index++, "VRUNIVERSE", 0));
            featureWeights.Add(new FeatureWeight("VR Sector", index++, "VRSECTOR", 0));
            featureWeights.Add(new FeatureWeight("VR History", index++, "VRHISTORY", 0));

            featureWeights.Add(new FeatureWeight("Sharpe FIP Score", index++, "SHARPE", 0));

            return featureWeights;
        }


        public static List<FeatureWeight> generateFeatureWeightsList(Strategy strategy)
        {
            List<FeatureWeight> featuresW = generateFeatureWeightsList();

            for (int i = 0; i < featuresW.Count; i++)
            {
                FeatureWeight fw = featuresW[i];

                fw.IsEnabled = false;
                fw.Weight = 0;

                try
                {
                    Feature featureFound = strategy.Features.Find(x => x.FeatureId == i);

                    if (featureFound == null)
                        continue;

                    fw.Weight = (float)featureFound.FeatureWeight;

                    if (fw.Weight != 0)
                        fw.IsEnabled = true;
                }
                catch (Exception)
                { }
            }

            return featuresW;
        }


        public static float calcMedian(List<decimal> magnitudes)
        {
            List<decimal> sortedList = magnitudes.ToList();
            sortedList.Sort();

            if (sortedList.Count % 2 == 0)
            {
                // count is even, need to get the middle two elements, add them together, then divide by 2
                decimal middleElement1 = sortedList[(sortedList.Count / 2) - 1];
                decimal middleElement2 = sortedList[(sortedList.Count / 2)];
                return (float)(middleElement1 + middleElement2) / 2;
            }
            else
            {
                // count is odd, simply get the middle element.
                return (float)sortedList[(sortedList.Count / 2)];
            }

        }

        public static decimal CalcZScore(decimal currentValue, List<decimal> allValues)
        {
            //(value - average) / Stdev
            decimal newVal = currentValue - allValues.Average();
            decimal stDev = DataUpdater.CalcStdDev(allValues);
            if (Math.Abs(stDev) < 0.001m)
                return 0;
            return newVal / stDev;
        }

        public static void rankFeatures(List<Feature> features)
        {
            features.Sort(delegate (Feature x, Feature y)
            {
                return x.featureValue.CompareTo(y.featureValue);
            });

            int totalFeatures = features.Count;
            float lastFeatureValue = float.MinValue;
            int lastCurrentFeature = 0;
            for (int currentFeature = 0; currentFeature < totalFeatures; currentFeature++)
            {
                if(lastFeatureValue != features[currentFeature].featureValue)
                {
                    lastFeatureValue = features[currentFeature].featureValue;
                    lastCurrentFeature = currentFeature;
                }

                features[currentFeature].ranking = lastCurrentFeature;
                features[currentFeature].rankedValue = (float)lastCurrentFeature / (float)totalFeatures;
            }

        }

        static decimal CalcFeature(FeatureTypes featureType, int years, List<StockRatios> ratios, int ratioIndex)
        {
            decimal result = 0;
            
            switch (featureType)
            {
                case FeatureTypes.FCFGrowth:
                    result = calcGrowthFeature(years, 1, ratios, ratioIndex);
                    break;
                case FeatureTypes.ROICGrowth:
                    result = calcGrowthFeature(years, 2, ratios, ratioIndex);
                    break;
                case FeatureTypes.EBITDAGrowth:
                    result = calcGrowthFeature(years, 3, ratios, ratioIndex);
                    break;
                case FeatureTypes.EVGrowth:
                    result = calcGrowthFeature(years, 4, ratios, ratioIndex);
                    break;
                case FeatureTypes.BVPSGrowth:
                    result = calcGrowthFeature(years, 5, ratios, ratioIndex);
                    break;
                case FeatureTypes.FCFMedian:
                    result = calcMedianFeature(years, 1, ratios, ratioIndex);
                    break;
                case FeatureTypes.ROICMedian:
                    result = calcMedianFeature(years, 2, ratios, ratioIndex);
                    break;
                case FeatureTypes.EBITDAMedian:
                    result = calcMedianFeature(years, 3, ratios, ratioIndex);
                    break;
                case FeatureTypes.EVMedian:
                    result = calcMedianFeature(years, 4, ratios, ratioIndex);
                    break;
                case FeatureTypes.BVPSMedian:
                    result = calcMedianFeature(years, 5, ratios, ratioIndex);
                    break;

                case FeatureTypes.CROICGrowth:
                    result = calcGrowthFeature(years, 6, ratios, ratioIndex);
                    break;
                case FeatureTypes.CROICMedian:
                    result = calcMedianFeature(years, 6, ratios, ratioIndex);
                    break;

                case FeatureTypes.SALESGrowth:
                    result = calcGrowthFeature(years, 7, ratios, ratioIndex);
                    break;
                case FeatureTypes.SALESMedian:
                    result = calcMedianFeature(years, 7, ratios, ratioIndex);
                    break;
                case FeatureTypes.PEG_RATIO:
                    result = calcPEGRATIOFeature(ratios, ratioIndex);
                    break;
                case FeatureTypes.EBITDA_Liabilities:
                    if (ratios[ratioIndex].liabilities != 0)
                    {
                        result = ratios[ratioIndex].EBITDA / ratios[ratioIndex].liabilities;
                    } else
                    {
                        result = 0;
                    }
                    break;
                case FeatureTypes.GP_Assets:
                    if (ratios[ratioIndex].assets != 0)
                    {
                        result = ratios[ratioIndex].gp / ratios[ratioIndex].assets;
                    }
                    else
                    {
                        result = 0;
                    }
                    break;
                case FeatureTypes.FCF_Sales:
                    if (ratios[ratioIndex].SalesUSD != 0)
                    {
                        result = ratios[ratioIndex].freeCashFlow / ratios[ratioIndex].SalesUSD;
                    }
                    else
                    {
                        result = 0;
                    }
                    break;
                case FeatureTypes.EV_EBITDA:
                    if (ratios[ratioIndex].EBITDA != 0)
                    {
                        result = - ratios[ratioIndex].EV / ratios[ratioIndex].EBITDA;
                    }
                    else
                    {
                        result = 0;
                    }
                    break;
                case FeatureTypes.Close_FCF:
                    if (ratios[ratioIndex].FCFPS != 0)
                    {
                        result = - ratios[ratioIndex].StockPrice / ratios[ratioIndex].FCFPS;
                    }
                    else
                    {
                        result = 0;
                    }
                    break;
                case FeatureTypes.EV_Revenue:
                    if (ratios[ratioIndex].SalesUSD != 0)
                    {
                        result = - ratios[ratioIndex].EV / ratios[ratioIndex].SalesUSD;
                    }
                    else
                    {
                        result = 0;
                    }
                    break;
                case FeatureTypes.Dividend_BuyBackRate:
                    result = CalcDividendBuyBackRateFeature(ratios, ratioIndex);
                    break;
                case FeatureTypes.Operating_Margin:
                    result = - ratios[ratioIndex].ebitdamargin;
                    break;
                case FeatureTypes.ROIC_EV_EBITDA:
                    if (ratios[ratioIndex].EV == 0 || ratios[ratioIndex].EBITDA == 0)
                    {
                        result = 0;
                    } else
                    {
                        result = ratios[ratioIndex].returnInvestedCap / (ratios[ratioIndex].EV / ratios[ratioIndex].EBITDA);
                    }
                    break;
            }
            return result;
        }


        static decimal CalcDividendBuyBackRateFeature(List<StockRatios> ratios, int ratioIndex)
        {
            int currYear = ratios[ratioIndex].Year;
            int currQuarter = ratios[ratioIndex].Quarter;
            decimal epsGrowth = 0;
            StockRatios oldRatio = null;

            int singleCount = 0;
            foreach (var x in ratios)
            {
                if (x.Quarter == currQuarter && x.Year == currYear - 1)
                {
                    oldRatio = x;
                    singleCount++;
                }
            }
            if (oldRatio == null || singleCount != 1)
            {
                return 0;
            }
            if (oldRatio.SharesOut == 0 || oldRatio.SharesOut < ratios[ratioIndex].SharesOut)
            {
                return ratios[ratioIndex].DividendYield * 100;
            } else
            {
                return ratios[ratioIndex].DividendYield * 100 - (ratios[ratioIndex].SharesOut - oldRatio.SharesOut) / oldRatio.SharesOut * 100;
            }
        }

        static decimal calcPEGRATIOFeature(List<StockRatios> ratios, int ratioIndex)
        {
            int currYear = ratios[ratioIndex].Year;
            int currQuarter = ratios[ratioIndex].Quarter;
            decimal epsGrowth = 0;
            StockRatios oldRatio = null;

            int singleCount = 0;
            foreach (var x in ratios)
            {
                if (x.Quarter == currQuarter && x.Year == currYear - 1)
                {
                    oldRatio = x;
                    singleCount++;
                }
            }
            if (oldRatio == null || singleCount != 1)
            {
                return 0;
            }
            if (oldRatio.EPS != 0)
            {
                epsGrowth = (ratios[ratioIndex].EPS - oldRatio.EPS) / oldRatio.EPS * 100;
            }
            if (ratios[ratioIndex].EPS != 0 && epsGrowth != 0)
            {
                return -ratios[ratioIndex].StockPrice / ratios[ratioIndex].EPS / epsGrowth;
            }
            
            return 0;
        }
        static decimal calcGrowthFeature(int years, int metric, List<StockRatios> ratios, int ratioIndex)
        {
            int currYear = ratios[ratioIndex].Year;
            int currQuarter = ratios[ratioIndex].Quarter;

            StockRatios oldRatio = null;

            int singleCount = 0;
            foreach (var x in ratios)
            {
                if (x.Quarter == currQuarter && x.Year == currYear - years)
                {
                    oldRatio = x;
                    singleCount++;
                }
            }
            if (oldRatio == null || singleCount != 1)
            {
                return 0;
            }
            switch (metric)
            {
                case 1:
                    if (oldRatio.FCFPS == 0)
                        return 0;
                    return (ratios[ratioIndex].FCFPS - oldRatio.FCFPS) / Math.Abs(oldRatio.FCFPS);
                case 2:
                    if (oldRatio.returnInvestedCap == 0)
                        return 0;
                    return (ratios[ratioIndex].returnInvestedCap - oldRatio.returnInvestedCap) / Math.Abs(oldRatio.returnInvestedCap);
                case 3:
                    if (oldRatio.EBITDA == 0)
                        return 0;
                    return (ratios[ratioIndex].EBITDA - oldRatio.EBITDA) / Math.Abs(oldRatio.EBITDA);
                case 4:
                    if (oldRatio.EV == 0)
                        return 0;
                    return (ratios[ratioIndex].EV - oldRatio.EV) / Math.Abs(oldRatio.EV);
                case 5:
                    if (oldRatio.BVPS == 0)
                        return 0;
                    return (ratios[ratioIndex].BVPS - oldRatio.BVPS) / Math.Abs(oldRatio.BVPS);
                case 6:
                    if (oldRatio.InvestedCapital == 0 || oldRatio.freeCashFlow == 0 || ratios[ratioIndex].InvestedCapital == 0)
                        return 0;
                    return ((ratios[ratioIndex].freeCashFlow / ratios[ratioIndex].InvestedCapital)
                        - (oldRatio.freeCashFlow / oldRatio.InvestedCapital)) / Math.Abs((oldRatio.freeCashFlow / oldRatio.InvestedCapital));
                case 7:
                    if (oldRatio.SalesUSD == 0)
                        return 0;
                    return (ratios[ratioIndex].SalesUSD - oldRatio.SalesUSD) / Math.Abs(oldRatio.SalesUSD);
            }

            return 0;
        }



        static decimal calcMedianFeature(int years, int metric, List<StockRatios> ratios, int ratioIndex)
        {
            int currYear = ratios[ratioIndex].Year;
            int currQuarter = ratios[ratioIndex].Quarter;

            StockRatios oldRatio = null;

            int singleCount = 0;
            foreach (var x in ratios)
            {
                if (x.Quarter == currQuarter && x.Year == currYear - years)
                {
                    oldRatio = x;
                    singleCount++;
                }
            }
            if (oldRatio == null || singleCount != 1)
            {
                return 0;
            }


            //add all the values into a collection
            List<decimal> metrics = new List<decimal>();
            foreach (StockRatios ratio in ratios.FindAll(a => a.Date >= oldRatio.Date && a.Date <= ratios[ratioIndex].Date))
            {
                switch (metric)
                {
                    case 1:
                        metrics.Add(ratio.FCFPS);
                        break;
                    case 2:
                        metrics.Add(ratio.returnInvestedCap);
                        break;
                    case 3:
                        metrics.Add(ratio.EBITDA);
                        break;
                    case 4:
                        metrics.Add(ratio.EV);
                        break;
                    case 5:
                        metrics.Add(ratio.BVPS);
                        break;
                    case 6:
                        if (ratio.InvestedCapital != 0)
                            metrics.Add(ratio.freeCashFlow / ratio.InvestedCapital);
                        break;
                    case 7:
                        metrics.Add(ratio.SalesUSD);
                        break;
                }

            }

            return calcMedianDecimal(metrics);
        }



        static decimal calcMedianDecimal(List<decimal> magnitudes)
        {

            if (magnitudes == null ||  magnitudes.Count == 0)
            {
                return 0;
            }

            List<decimal> sortedList = magnitudes.ToList();
            sortedList.Sort();

            if (sortedList.Count % 2 == 0)
            {
                // count is even, need to get the middle two elements, add them together, then divide by 2
                decimal middleElement1 = sortedList[(sortedList.Count / 2) - 1];
                decimal middleElement2 = sortedList[(sortedList.Count / 2)];
                return (decimal)(middleElement1 + middleElement2) / 2;
            }
            else
            {
                // count is odd, simply get the middle element.
                return (decimal)sortedList[(sortedList.Count / 2)];
            }

        }

        public static FeatureTable CalcFeaturesTable(List<StockRatios> ratios, int ratioIndex)
        {
            FeatureTable result = new FeatureTable();
            result.IdStock = ratios[ratioIndex].IdStock;
             
            result.FCF1G = CalcFeature(FeatureTypes.FCFGrowth, 1, ratios, ratioIndex);
            result.FCF3G = CalcFeature(FeatureTypes.FCFGrowth, 3, ratios, ratioIndex);
            result.FCF5G = CalcFeature(FeatureTypes.FCFGrowth, 5, ratios, ratioIndex);
            result.FCF1M= CalcFeature(FeatureTypes.FCFMedian, 1, ratios, ratioIndex);
            result.FCF3M = CalcFeature(FeatureTypes.FCFMedian, 3, ratios, ratioIndex);
            result.FCF5M = CalcFeature(FeatureTypes.FCFMedian, 5, ratios, ratioIndex);
             
            result.ROIC1G = CalcFeature(FeatureTypes.ROICGrowth, 1, ratios, ratioIndex);
            result.ROIC3G = CalcFeature(FeatureTypes.ROICGrowth, 3, ratios, ratioIndex);
            result.ROIC5G = CalcFeature(FeatureTypes.ROICGrowth, 5, ratios, ratioIndex);
            result.ROIC1M = CalcFeature(FeatureTypes.ROICMedian, 1, ratios, ratioIndex);
            result.ROIC3M = CalcFeature(FeatureTypes.ROICMedian, 3, ratios, ratioIndex);
            result.ROIC5M = CalcFeature(FeatureTypes.ROICMedian, 5, ratios, ratioIndex);
             
            result.EBITA1G = CalcFeature(FeatureTypes.EBITDAGrowth, 1, ratios, ratioIndex);
            result.EBITA3G = CalcFeature(FeatureTypes.EBITDAGrowth, 3, ratios, ratioIndex);
            result.EBITA5G = CalcFeature(FeatureTypes.EBITDAGrowth, 5, ratios, ratioIndex);
            result.EBITA1M = CalcFeature(FeatureTypes.EBITDAMedian, 1, ratios, ratioIndex);
            result.EBITA3M = CalcFeature(FeatureTypes.EBITDAMedian, 3, ratios, ratioIndex);
            result.EBITA5M = CalcFeature(FeatureTypes.EBITDAMedian, 5, ratios, ratioIndex);

            result.EV1G = CalcFeature(FeatureTypes.EVGrowth, 1, ratios, ratioIndex);
            result.EV3G = CalcFeature(FeatureTypes.EVGrowth, 3, ratios, ratioIndex);
            result.EV5G = CalcFeature(FeatureTypes.EVGrowth, 5, ratios, ratioIndex);
            result.EV1M = CalcFeature(FeatureTypes.EVMedian, 1, ratios, ratioIndex);
            result.EV3M = CalcFeature(FeatureTypes.EVMedian, 3, ratios, ratioIndex);
            result.EV5M = CalcFeature(FeatureTypes.EVMedian, 5, ratios, ratioIndex);

            result.BVPS1G = CalcFeature(FeatureTypes.BVPSGrowth, 1, ratios, ratioIndex);
            result.BVPS3G = CalcFeature(FeatureTypes.BVPSGrowth, 3, ratios, ratioIndex);
            result.BVPS5G = CalcFeature(FeatureTypes.BVPSGrowth, 5, ratios, ratioIndex);
            result.BVPS1M = CalcFeature(FeatureTypes.BVPSMedian, 1, ratios, ratioIndex);
            result.BVPS3M = CalcFeature(FeatureTypes.BVPSMedian, 3, ratios, ratioIndex);
            result.BVPS5M = CalcFeature(FeatureTypes.BVPSMedian, 5, ratios, ratioIndex);
             
            result.CROIC1G = CalcFeature(FeatureTypes.CROICGrowth, 1, ratios, ratioIndex);
            result.CROIC3G = CalcFeature(FeatureTypes.CROICGrowth, 3, ratios, ratioIndex);
            result.CROIC5G = CalcFeature(FeatureTypes.CROICGrowth, 5, ratios, ratioIndex);
            result.CROIC1M = CalcFeature(FeatureTypes.CROICMedian, 1, ratios, ratioIndex);
            result.CROIC3M = CalcFeature(FeatureTypes.CROICMedian, 3, ratios, ratioIndex);
            result.CROIC5M = CalcFeature(FeatureTypes.CROICMedian, 5, ratios, ratioIndex);

            result.SALES1G = CalcFeature(FeatureTypes.SALESGrowth, 1, ratios, ratioIndex);
            result.SALES3G = CalcFeature(FeatureTypes.SALESGrowth, 3, ratios, ratioIndex);
            result.SALES5G = CalcFeature(FeatureTypes.SALESGrowth, 5, ratios, ratioIndex);
            result.SALES1M = CalcFeature(FeatureTypes.SALESMedian, 1, ratios, ratioIndex);
            result.SALES3M = CalcFeature(FeatureTypes.SALESMedian, 3, ratios, ratioIndex);
            result.SALES5M = CalcFeature(FeatureTypes.SALESMedian, 5, ratios, ratioIndex);

            result.PEG_RATIO = CalcFeature(FeatureTypes.PEG_RATIO, 1, ratios, ratioIndex);
            result.EBITDA_Liabilities = CalcFeature(FeatureTypes.EBITDA_Liabilities, 1, ratios, ratioIndex);
            result.GP_Assets = CalcFeature(FeatureTypes.GP_Assets, 1, ratios, ratioIndex);
            result.FCF_Sales = CalcFeature(FeatureTypes.FCF_Sales, 1, ratios, ratioIndex);
            result.EV_EBITDA = CalcFeature(FeatureTypes.EV_EBITDA, 1, ratios, ratioIndex);
            result.Close_FCF = CalcFeature(FeatureTypes.Close_FCF, 1, ratios, ratioIndex);
            result.EV_Revenue = CalcFeature(FeatureTypes.EV_Revenue, 1, ratios, ratioIndex);
            result.Dividend_BuyBackRate = CalcFeature(FeatureTypes.Dividend_BuyBackRate, 1, ratios, ratioIndex);
            result.Operating_Margin = CalcFeature(FeatureTypes.Operating_Margin, 1, ratios, ratioIndex);
            result.ROIC_EV_EBITDA = CalcFeature(FeatureTypes.ROIC_EV_EBITDA, 1, ratios, ratioIndex);

            result.Has1YearBackData = checkHasCorrectData(1, ratios, ratioIndex);
            result.Has3YearBackData = checkHasCorrectData(3, ratios, ratioIndex);
            result.Has5YearBackData = checkHasCorrectData(5, ratios, ratioIndex);
            
            return result;

        }

        static bool checkHasCorrectData(int years, List<StockRatios> ratios, int ratioIndex)
        {
            int currYear = ratios[ratioIndex].Year;
            int currQuarter = ratios[ratioIndex].Quarter;

            StockRatios oldRatio = null;

            int singleCount = 0;
            foreach (var x in ratios)
            {
                if (x.Quarter == currQuarter && x.Year == currYear - years)
                {
                    oldRatio = x;
                    singleCount++;
                }
            }
            if (oldRatio == null || singleCount != 1)
            {
                return false;
            }

            return true;
        }

        public static void SaveFIPFeature(long idStock, int date, decimal fipScore, decimal sortino, decimal sharpeFip, int correspondingQuarter)
        {
            //check if the value exists, then update or create it
            if(!DatabaseSingleton.Instance.Exists("SELECT COUNT(*) FROM FEATURE WHERE id_stock = " + idStock.ToString() + " and DATE = " + date.ToString()))
            {
                List<SQLiteParameter> parameters = new List<SQLiteParameter>();
                parameters.Add(new SQLiteParameter("@p1", date));
                parameters.Add(new SQLiteParameter("@p2", fipScore));
                parameters.Add(new SQLiteParameter("@p3", sortino));
                parameters.Add(new SQLiteParameter("@p4", sharpeFip));
                parameters.Add(new SQLiteParameter("@p5", idStock));
                parameters.Add(new SQLiteParameter("@p6", correspondingQuarter));

                DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO FEATURE (DATE, FIP, SORTINO_FIP, SHARPE_FIP, id_stock, CORRESPONDING_QUARTER) VALUES (@p1,@p2, @p3, @p4, @p5, @p6)", parameters.ToArray(), false);
            }
            else
            {
                List<SQLiteParameter> parameters = new List<SQLiteParameter>();
                parameters.Add(new SQLiteParameter("@p1", fipScore));
                parameters.Add(new SQLiteParameter("@p4", sortino));
                parameters.Add(new SQLiteParameter("@p2", idStock));
                parameters.Add(new SQLiteParameter("@p3", date));
                parameters.Add(new SQLiteParameter("@p5", sharpeFip));

                DatabaseSingleton.Instance.ExecuteNonQuery("UPDATE FEATURE SET FIP = @p1, SORTINO_FIP = @p4, SHARPE_FIP = @p5 WHERE id_stock = @p2 AND DATE = @p3", parameters.ToArray(), false);
            }

        }


        public void Save(int idStrategy)
        { 
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();

            parameters.Add(new SQLiteParameter("@p1", idStrategy));
            parameters.Add(new SQLiteParameter("@p2", featureId));
            parameters.Add(new SQLiteParameter("@p3", featureWeight)); 

            DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO STRATEGY_FEATURE (ID_STRATEGY, ID_FEATURE, WEIGHT) VALUES (@p1,@p2,@p3)", parameters.ToArray());

        }
         
        public static List<Feature> LoadFeatures(int idStrategy)
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT * FROM STRATEGY_FEATURE WHERE ID_STRATEGY = " + idStrategy.ToString());
            List<Feature> result = new List<Feature>();

            foreach (DataRow row in data.Rows)
                result.Add(new Feature(row));

            return result;
        }

        public static List<Feature> LoadOptimalFeatures(int idStrategy)
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT * FROM STRATEGY_OPTIMAL_FEATURE WHERE ID_STRATEGY = " + idStrategy.ToString());
            List<Feature> result = new List<Feature>();

            foreach (DataRow row in data.Rows)
                result.Add(new Feature(row));

            return result;
        }

        public void OptimalSave(int idStrategy)
        {
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();

            parameters.Add(new SQLiteParameter("@p1", idStrategy));
            parameters.Add(new SQLiteParameter("@p2", featureId));
            parameters.Add(new SQLiteParameter("@p3", featureWeight));

            DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO STRATEGY_OPTIMAL_FEATURE (ID_STRATEGY, ID_FEATURE, WEIGHT) VALUES (@p1,@p2,@p3)", parameters.ToArray());
        }

    }
}
