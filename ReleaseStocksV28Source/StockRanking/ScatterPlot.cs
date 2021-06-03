using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.WinForms;
using LiveCharts.Wpf;

namespace StockRanking
{
    public partial class ScatterPlot : Form
    {
        Dictionary<int, Dictionary<int, RankingResult>> rankingResults = null;
        bool updating = true;
        bool reduceResults = false;
        int decimalsQtty = 2;
        public ScatterPlot(Dictionary<int, Dictionary<int, RankingResult>> rankingResultsp, bool preduceResults)
        {
            InitializeComponent();

            reduceResults = preduceResults;
            rankingResults = rankingResultsp;

            if (WindowState == FormWindowState.Normal)
            {
                if (Properties.Settings.Default.FormSizeScatterplot.Width > 0)
                    Size = Properties.Settings.Default.FormSizeScatterplot;
            } 

            dateFrom.MaxDate = DateTime.Now.AddYears(-1);
            dateTo.MaxDate = DateTime.Now;
            dateTo.MinDate = new DateTime(2015,01,01);
            dateTo.Value = DateTime.Now;

            charScatterPlot.ChartAreas[0].AxisX.Interval = 10;
            charScatterPlot.ChartAreas[0].AxisX.IsMarginVisible = false;
            charScatterPlot.ChartAreas[0].AxisY.IsMarginVisible = false;
             
            createChart();


            updating = false;
             
        }

        void createChart()
        {
            charScatterPlot.Series[0].Points.SuspendUpdates();
            while (charScatterPlot.Series[0].Points.Count > 0)
                charScatterPlot.Series[0].Points.RemoveAt(charScatterPlot.Series[0].Points.Count - 1);
            charScatterPlot.Series[0].Points.ResumeUpdates();

            List<DataPoint> allValues = new List<DataPoint>();
            List<DataPoint> allValuesForLinear = new List<DataPoint>();
            Dictionary<string, DataPoint> diffValues = new Dictionary<string, DataPoint>();
            Random rnd = new Random();
            int minValue = 1;
            double maxValue = 0;
            decimal maxValueFilter = (decimal)filterBar.Value/(decimal)100;
            decimal maxRealValue = 0;

            charScatterPlot.ChartAreas[0].AxisX.LabelStyle.IsEndLabelVisible = true;

            charScatterPlot.ChartAreas[0].AxisY.LabelStyle.Format = "P0";
            charScatterPlot.ChartAreas[0].AxisY.Interval = 0.5;
            if(maxValueFilter > 7)
                charScatterPlot.ChartAreas[0].AxisY.Interval = 1;

            charScatterPlot.ChartAreas[0].AxisY.Minimum = -1;
            charScatterPlot.ChartAreas[0].AxisY.Maximum = (double)maxValueFilter;

            decimalsQtty = 2;
            if (reduceResults)
                decimalsQtty = 1;

            foreach (int rankResultGroupKey in rankingResults.Keys)
            {
                if (rankResultGroupKey < Utils.ConvertDateTimeToInt(dateFrom.Value)
                    || rankResultGroupKey > Utils.ConvertDateTimeToInt(dateTo.Value))
                    continue;

                Dictionary<int, RankingResult> rankResultGroup = rankingResults[rankResultGroupKey];

                foreach (RankingResult rank in rankResultGroup.Values)
                {
                    if (rank.ForwardGain > maxRealValue)
                        maxRealValue = rank.ForwardGain;

                    double val1 = Math.Round((double)rank.RankPosition * 100d, decimalsQtty);
                    double val2 = Math.Round((double)rank.ForwardGain, 2);

                    if (Math.Abs(rank.ForwardGain) < 40)
                        allValuesForLinear.Add(new DataPoint(val1, val2));
                    
                    if (Math.Abs(rank.ForwardGain) < maxValueFilter)
                    {
                        
                        if (!diffValues.ContainsKey(val1.ToString() + val2.ToString()))
                        {
                            DataPoint newPoint = new DataPoint(val1, val2);
                            allValues.Add(newPoint);
                            diffValues.Add(val1.ToString() + val2.ToString(), newPoint);
                        }
                        else
                        {
                            //DataPoint newPoint = diffValues[val1.ToString() + val2.ToString()];
                            //if(newPoint.Weight < 20)
                            //    newPoint.Weight++;
                            //maxValue = Math.Max(newPoint.Weight, maxValue);
                        }
                    }
                }
            }

            if ((int)maxRealValue * 100 > 200)
            {
                filterBar.Maximum = (int)maxRealValue * 100;
                if (filterBar.Maximum > 2000)
                    filterBar.Maximum = 2000;
            }
            else
                filterBar.Maximum = 200;

            charScatterPlot.Series[0].Points.SuspendUpdates();
            foreach (DataPoint dp in diffValues.Values)
                charScatterPlot.Series[0].Points.Add(dp);
            charScatterPlot.Series[0].Points.ResumeUpdates();

            charScatterPlot.Legends[0].Enabled = false;


            double a = 0;
            double b = 0;
            if (allValuesForLinear.Count > 0)
            {
                StatisticsMath.GenerateLinearBestFit(allValuesForLinear, out a, out b);

                charScatterPlot.Series[1].BorderWidth = 5;
                charScatterPlot.Series[1].Color = Color.Blue;
                charScatterPlot.Series[1].Points.Clear();
                charScatterPlot.Series[1].Points.Add(new DataPoint(0, b));
                charScatterPlot.Series[1].Points.Add(new DataPoint(100, (a * 100 + b)));

                txtSlope.Text = "Y = " + (a * 100).ToString("n2") + "X " + (b >= 0 ? "+ ": "- ") + (Math.Abs(b) * 100).ToString("n2");
            }

        }

        private void dateFrom_ValueChanged(object sender, EventArgs e)
        {
            if (!updating)
            { 
                createChart();
            }
        }

        private void dateTo_ValueChanged(object sender, EventArgs e)
        {
            if (!updating)
            { 
                createChart();
            }
        }

        private void ScatterPlot_Resize(object sender, EventArgs e)
        { 
            if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.FormSizeScatterplot = Size;
                Properties.Settings.Default.Save();
            }
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdExport_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(RankingCalculator.DebugExport);

            MessageBox.Show("Data exported to the clipboard.");
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            lblFilterValues.Text = filterBar.Value.ToString() + " %";
            createChart();
        }

        private void ScatterPlot_Load(object sender, EventArgs e)
        {

        }

        private void charScatterPlot_AxisViewChanged(object sender, ViewEventArgs e)
        {
            double min = charScatterPlot.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
            double max = charScatterPlot.ChartAreas[0].AxisX.ScaleView.ViewMaximum;
            List<DataPoint> allValuesForLinear = new List<DataPoint>();

            foreach (int rankResultGroupKey in rankingResults.Keys)
            {
                if (rankResultGroupKey < Utils.ConvertDateTimeToInt(dateFrom.Value)
                    || rankResultGroupKey > Utils.ConvertDateTimeToInt(dateTo.Value))
                    continue;

                Dictionary<int, RankingResult> rankResultGroup = rankingResults[rankResultGroupKey];
                
                foreach (RankingResult rank in rankResultGroup.Values)
                { 
                    double val1 = Math.Round((double)rank.RankPosition * 100d, decimalsQtty);
                    double val2 = Math.Round((double)rank.ForwardGain, 2);

                    if (val1 < min || val1 > max)
                        continue;

                    if (Math.Abs(rank.ForwardGain) < 40)
                        allValuesForLinear.Add(new DataPoint(val1, val2));
                    
                }
            }

            double a = 0;
            double b = 0;
            if (allValuesForLinear.Count > 0)
            {
                StatisticsMath.GenerateLinearBestFit(allValuesForLinear, out a, out b);

                charScatterPlot.Series[1].BorderWidth = 5;
                charScatterPlot.Series[1].Color = Color.Blue;
                charScatterPlot.Series[1].Points.Clear();
                charScatterPlot.Series[1].Points.Add(new DataPoint(0, b));
                charScatterPlot.Series[1].Points.Add(new DataPoint(100, (a * 100 + b)));

                txtSlope.Text = "Y = " + (a * 100).ToString("n2") + "X " + (b >= 0 ? "+ " : "- ") + (Math.Abs(b) * 100).ToString("n2");
            }


        }
    }
}
