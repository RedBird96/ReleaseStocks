using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StockRanking
{
    public class GraphMetric
    {
        public GraphMetrics Metric = GraphMetrics.Price;
        public bool AllowsMin = true;
        public bool AllowsMax = true;
        public bool AllowsMedian = true;
        public bool AllowsRollingMedian = true;
        
        private bool min = false;
        private bool max = false;
        private bool median = false;
        private bool rollingMedian = false;
        private bool value = true;
        private bool all = false;
        

        public System.Windows.Media.Color LineColor = System.Windows.Media.Colors.Black;
        
        public String Name
        {
            get
            {
                return ((DisplayAttribute)(Metric.GetType()
                        .GetMember(Metric.ToString())
                        .First())
                        .GetCustomAttributes(typeof(DisplayAttribute), false)[0]).Name;
            }
        }

        public bool Min {
            get => 
                this.AllowsMin?this.min:false;
            set => this.min = value; }
        public bool Max { get => this.AllowsMax ? this.max : false; set => this.max = value; }
        public bool Median { get => this.AllowsMedian ? this.median : false; set => this.median = value; }
        public bool RollingMedian { get => this.AllowsRollingMedian ? this.rollingMedian : false; set => this.rollingMedian = value; }
        public bool Value { get => this.value; set => this.value = value; }
        public bool All { get => this.all; set => this.all = value; }

        public GraphMetric()
        {

        }

        public bool GetShowInOutputTable()
        {
            if (this.Metric == GraphMetrics.PriceToMaxPE)
                return false;

            return true;
        }

        public int GetPositionInCompositeValues()
        {
            int startPosition = 7;
            switch(this.Metric)
            {
                case GraphMetrics.PB:
                    return startPosition+1;
                case GraphMetrics.PE:
                    return startPosition + 0;
                case GraphMetrics.PFCF:
                    return startPosition + 2;
                case GraphMetrics.PS:
                    return startPosition + 3;
            }

            return 0;
        }

        public bool HasAny()
        {
            return Min || Max || Median || RollingMedian || Value;
        }

        public static List<GraphMetric> GenerateCompositeMetrics()
        {
            List<GraphMetric> result = new List<GraphMetric>();
            result.Add(new GraphMetric()
            {
                Metric = GraphMetrics.Price,
                Min = true,
                Max = true,
                Median = true,
                RollingMedian = true,
                AllowsMedian = true,
                AllowsRollingMedian = true
            });
            result.Add(new GraphMetric()
            {
                Metric = GraphMetrics.Composite,
                LineColor = System.Windows.Media.Colors.Red
            });
            result.Add(new GraphMetric()
            {
                Metric = GraphMetrics.PB,
                LineColor = System.Windows.Media.Colors.Green
            });
            result.Add(new GraphMetric()
            {
                Metric = GraphMetrics.PE,
                LineColor = System.Windows.Media.Colors.Yellow
            });
            result.Add(new GraphMetric()
            {
                Metric = GraphMetrics.PFCF,
                LineColor = System.Windows.Media.Colors.Blue
            });
            result.Add(new GraphMetric()
            {
                Metric = GraphMetrics.PS,
                LineColor = System.Windows.Media.Colors.Violet
            });/*
            result.Add(new GraphMetric()
            {
                Metric = GraphMetrics.PriceToMaxPE,
                AllowsMax = false,
                AllowsMin = false,
                AllowsMedian = false,
                AllowsRollingMedian = false,
                LineColor = System.Windows.Media.Colors.Pink,
            });
            */
            return result;
        }

        public static void SaveValues(List<GraphMetric> metrics)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<GraphMetric>));
            
            using (StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, metrics);
                Properties.Settings.Default.CompositeGraphMetrics = textWriter.ToString();
            }
            
            Properties.Settings.Default.Save();
        }

        public static List<GraphMetric> LoadValues()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<GraphMetric>));

            if (Properties.Settings.Default.CompositeGraphMetrics == "")
                return null;

            using (TextReader reader = new StringReader(Properties.Settings.Default.CompositeGraphMetrics))
            {
                List<GraphMetric> metrics = (List<GraphMetric>)serializer.Deserialize(reader);
                GraphMetric metricToRemove = null;
                foreach (var met in metrics)
                    if (met.Metric == GraphMetrics.PriceToMaxPE)
                        metricToRemove = met;

                if (metricToRemove != null)
                    metrics.Remove(metricToRemove);

                return metrics;
            }
            
        }
    }
}
