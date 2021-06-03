using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking.Entities
{
    class CachedFeatures
    {
        public class StockFeature
        {
            public List<float> featurevalues;
            public int IdStock;

            public StockFeature(int id)
            {
                IdStock = id;
                featurevalues = new List<float>();
            }
        }

        public class StockFeatureList
        {
            public List<StockFeature> [] featureList = new List<StockFeature> [8];
        }

        static CachedFeatures instance = null;
        public static CachedFeatures Instance
        {
            get
            {
                if (instance == null)
                    instance = new CachedFeatures();

                return instance;
            }
        }

        public Dictionary<int, int> dateIndex;
        public List<StockFeatureList> features;

        int FeaturesCount;

        public CachedFeatures()
        {
            features = new List<StockFeatureList>();
            dateIndex = new Dictionary<int, int>();

            FeaturesCount = Feature.generateFeatureWeightsList().Count;
        }

        public void clearCache()
        {
            foreach (var cf in features)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (cf.featureList[i] != null)
                    {
                        cf.featureList[i].Clear();
                        cf.featureList[i] = null;
                    }
                }
            }
            features.Clear();
            dateIndex.Clear();

            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        public void AddNewDate(int date, int yearId, List<Stock> stocklist)
        {
            int id = -1;
            if (dateIndex.ContainsKey(date))
            {
                id = dateIndex[date];
            } else
            {
                id = features.Count;
                dateIndex[date] = id;
                StockFeatureList cf = new StockFeatureList();
                features.Add(cf);
            }

            if (features[id].featureList[yearId] != null)
            {
                features[id].featureList[yearId].Clear();
            } else
            {
                features[id].featureList[yearId] = new List<StockFeature>();
            }
            foreach (var stock in stocklist)
            {
                if (stock.Features.Count == 0) continue;

                StockFeature sf = new StockFeature(stock.IdStock);
                for (int i = 0; i<FeaturesCount; i++)
                {
                    sf.featurevalues.Add(stock.Features[i].RankedValue);
                }
                features[id].featureList[yearId].Add(sf);
            }
        }
    }
}
