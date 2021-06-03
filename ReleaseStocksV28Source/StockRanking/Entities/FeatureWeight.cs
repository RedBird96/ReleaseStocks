using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public class FeatureWeight
    {
        private String name = "";
        private float weight = 1;
        private int featureIndex = 0;
        private bool isEnabled = true;
        private String shortName = "";
        private int years = 0;

        private float fromWeight, toWeight, step;

        public string Name { get => name; set => name = value; }
        public float Weight { get => weight; set => weight = value; }
        public int FeatureIndex { get => featureIndex; set => featureIndex = value; }
        public bool IsEnabled { get => isEnabled; set => isEnabled = value; }
        public string ShortName { get => this.shortName; set => this.shortName = value; }
        public int Years { get => this.years; set => this.years = value; }
        public float From { get => fromWeight; set => fromWeight = value; }
        public float To { get => toWeight; set => toWeight = value; }
        public float Step { get => step; set => step = value; }

        public FeatureWeight(String name, int index, String shortName, int pyears)
        {
            Name = name;
            FeatureIndex = index;
            ShortName = shortName;
            years = pyears;

            step = (float) 0.1;
        }

        public FeatureWeight(String name, int index, bool enabled, String shortName, int pyears)
        {
            Name = name;
            FeatureIndex = index;
            IsEnabled = enabled;
            ShortName = shortName;
            years = pyears;

            step = (float) 0.1;
        }


    }
}
