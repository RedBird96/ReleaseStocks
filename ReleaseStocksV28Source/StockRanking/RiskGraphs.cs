using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts.Wpf;
using LiveCharts.WinForms;
using LiveCharts;
using LiveCharts.Defaults;

namespace StockRanking
{
    public partial class RiskGraphs : Form
    {
        PortfolioParameters portfolioParams;
        public RiskGraphs(PortfolioParameters portfolioParameters)
        {
            InitializeComponent();

            portfolioParams = portfolioParameters;

            createCAPEGraph();
        }

        void createCAPEGraph()
        {
            //render graphs
            LiveCharts.SeriesCollection seriesToPlot = new LiveCharts.SeriesCollection(); 

            LiveCharts.Wpf.LineSeries cape1Line = new LineSeries();
            cape1Line.Title = "CAPE1";
            cape1Line.Values = new ChartValues<decimal> { };
            cape1Line.PointGeometry = null;
            cape1Line.Stroke = System.Windows.Media.Brushes.Black;
            cape1Line.Stroke.Freeze();

            LiveCharts.Wpf.LineSeries cape2Line = new LineSeries();
            cape2Line.Title = "CAPE2";
            cape2Line.Values = new ChartValues<decimal> { };
            cape2Line.PointGeometry = null;
            cape2Line.Stroke = System.Windows.Media.Brushes.Yellow;
            cape2Line.Stroke.Freeze();
             
            chartCAPE.Series.Clear();
            chartCAPE.AxisX.Clear();
            chartCAPE.AxisY.Clear();

            int spyIndex = 0;
            if (chkHiLo.Checked || chkCape.Checked)
            {
                spyIndex++;
                chartCAPE.AxisY.Add(new Axis
                {
                    Foreground = System.Windows.Media.Brushes.Black,
                    Title = "Percentage"
                });

                if(filterBar.Value < 9)
                {
                    chartCAPE.AxisY[0].MinValue = -100;
                    chartCAPE.AxisY[0].MaxValue = 100;
                }

                if (chkHiLo.Checked && !chkCape.Checked && filterBar.Value < 9)
                {
                    chartCAPE.AxisY[0].MinValue = -30;
                    chartCAPE.AxisY[0].MaxValue = 30;
                }
            }

            if (chkSPY.Checked)
            {
                chartCAPE.AxisY.Add(new Axis
                {
                    Foreground = System.Windows.Media.Brushes.Black,
                    Title = "SPY AVG",
                    Position = AxisPosition.RightTop//,
                    //MinValue = 0
                });
            }

            cape1Line.LineSmoothness = 0;
            cape2Line.LineSmoothness = 0;


            LiveCharts.Wpf.LineSeries HiLo1Line = new LineSeries();
            HiLo1Line.Title = "HiLo";
            HiLo1Line.Values = new ChartValues<decimal> { };
            HiLo1Line.PointGeometry = null;
            HiLo1Line.Stroke = System.Windows.Media.Brushes.Red;
            HiLo1Line.Stroke.Freeze();
             
            //chartHiLo.Series.Clear();
            //chartHiLo.AxisX.Clear();

            HiLo1Line.LineSmoothness = 0;


            LiveCharts.Wpf.LineSeries avg1Line = new LineSeries();
            avg1Line.Title = "SPY AVG FAST";
            avg1Line.Values = new ChartValues<decimal> { };
            avg1Line.PointGeometry = null;
            avg1Line.Stroke = System.Windows.Media.Brushes.Green;
            avg1Line.Stroke.Freeze();
            avg1Line.ScalesYAt = spyIndex;

            LiveCharts.Wpf.LineSeries avg2Line = new LineSeries();
            avg2Line.Title = "SPY AVG SLOW";
            avg2Line.Values = new ChartValues<decimal> { };
            avg2Line.PointGeometry = null;
            avg2Line.Stroke = System.Windows.Media.Brushes.Blue;
            avg2Line.Stroke.Freeze();
            avg2Line.ScalesYAt = spyIndex;

            avg1Line.LineSmoothness = 0;
            avg2Line.LineSmoothness = 0;


            List<String> labels = new List<string>();

            //cape value divided by interval time
            int months = 12 * (DateTime.Now.Year - 2004);
            int interval = 12;
            bool monthsInterval = true;
            switch (filterBar.Value)
            {
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    months = months / filterBar.Value;
                    interval = (int)Math.Floor((double)interval / (double)filterBar.Value);
                    break;
                case 9:
                    months = 2;
                    interval = 6*30/15;
                    monthsInterval = false;
                    break;
                case 10:
                    months = 1;
                    interval = 2;
                    monthsInterval = false;
                    break;
            }
            DateTime startDate = DateTime.Now.AddMonths(-months);

            BacktestCalculator backtestcalc = new BacktestCalculator(portfolioParams, new List<FeatureWeight>(), new List<FilterConditions>());
            while (Utils.ConvertDateTimeToInt(startDate) < Utils.ConvertDateTimeToInt(DateTime.Now))
            {
                //get cape for this date
                decimal avg50, avg200;
                bool spyRes = backtestcalc.checkSPYAvgSignal(startDate, out avg50, out avg200);

                decimal hiloValue;
                bool hiloRes = backtestcalc.checkHiLoSignal(startDate, out hiloValue);

                decimal cape1, cape2;
                bool capeRes = backtestcalc.checkCapeSignal(startDate, out cape1, out cape2);

                cape1Line.Values.Add(Math.Round(cape1, 2));
                cape2Line.Values.Add(Math.Round(cape2, 2));
                HiLo1Line.Values.Add(Math.Round(hiloValue, 2));
                avg1Line.Values.Add(Math.Round(avg50, 2));
                avg2Line.Values.Add(Math.Round(avg200, 2));

                labels.Add(startDate.ToString("dd/MM/yyyy"));

                if (Utils.ConvertDateTimeToInt(startDate) >= Utils.ConvertDateTimeToInt(DateTime.Now.AddDays(-1)))
                    break;

                if(monthsInterval) 
                    startDate = startDate.AddMonths(1);
                else
                    startDate = startDate.AddDays(1);

                if (Utils.ConvertDateTimeToInt(startDate) >= Utils.ConvertDateTimeToInt(DateTime.Now.AddDays(-1)))
                    startDate = DateTime.Now.AddDays(-1);
            }

            cape1Line.Fill = System.Windows.Media.Brushes.Transparent;
            cape1Line.Fill.Freeze();
            //cape1Line.DataLabels = false;

            cape2Line.Fill = System.Windows.Media.Brushes.Transparent;
            cape2Line.Fill.Freeze();
            //cape2Line.DataLabels = false;

            HiLo1Line.Fill = System.Windows.Media.Brushes.Transparent;
            HiLo1Line.Fill.Freeze();
            //HiLo1Line.DataLabels = false;

            avg1Line.Fill = System.Windows.Media.Brushes.Transparent;
            avg1Line.Fill.Freeze();
            //avg1Line.DataLabels = false;

            avg2Line.Fill = System.Windows.Media.Brushes.Transparent;
            avg2Line.Fill.Freeze();
            //avg2Line.DataLabels = false;


            if (chkCape.Checked)
            {
                seriesToPlot.Add(cape2Line);
                seriesToPlot.Add(cape1Line);
            }

            if (chkHiLo.Checked)
                seriesToPlot.Add(HiLo1Line);

            if (chkSPY.Checked)
            {
                seriesToPlot.Add(avg1Line);
                seriesToPlot.Add(avg2Line);
            }
            
            //add line to chart
            chartCAPE.Series = seriesToPlot;

            chartCAPE.AxisX.Add(new LiveCharts.Wpf.Axis
                {
                    Title = "Risk Model",
                    Labels = labels
                });
            chartCAPE.AxisX[0].LabelsRotation = 45;

            chartCAPE.AxisX[0].Separator.Step = interval;

            chartCAPE.LegendLocation = LegendLocation.Bottom; 
        }

        private void chartCAPE_DataClick(object sender, ChartPoint chartPoint)
        {
             
        }

        private void chkCape_CheckedChanged(object sender, EventArgs e)
        {
            createCAPEGraph();
        }

        private void chkSPY_CheckedChanged(object sender, EventArgs e)
        {
            createCAPEGraph();
        }

        private void chkHiLo_CheckedChanged(object sender, EventArgs e)
        {
            createCAPEGraph();
        }

        private void filterBar_Scroll(object sender, EventArgs e)
        {
            createCAPEGraph();
        }
    }
}
