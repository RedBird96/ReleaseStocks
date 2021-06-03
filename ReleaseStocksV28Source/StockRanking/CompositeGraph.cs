using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Threading;
using System.Runtime.InteropServices;

namespace StockRanking
{
    public partial class CompositeGraph : Form
    {

        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        [return: MarshalAs(
            UnmanagedType.Bool)]
        public static extern bool BitBlt(
            [In()] System.IntPtr hdc, int x, int y, int cx, int cy,
            [In()] System.IntPtr hdcSrc, int x1, int y1, uint rop);

        private const int SRC_COPY = 0xCC0020;

        List<Stock> stocks = new List<Stock>();
        List<GraphMetric> metrics = null;
        Stock stockToPlot = null;
        int interval = 50;
        bool userEnabled = false;
        double lastRange = 0;
        long rangeStart = 0;
        DateTime endTime = DateTime.Now;
        DateTime beginTime = DateTime.Now;
        bool limitMin = false, limitMax = false;
        decimal highestPE = 0;
        PortfolioParameters portfolioParameters = null;

        public CompositeGraph(int stock, PortfolioParameters portfolioParams)
        {
            InitializeComponent();
            portfolioParameters = portfolioParams;

            userEnabled = false;
            grdMetrics.AutoGenerateColumns = false;
            chartComposite.DisableAnimations = true;
            stocks = Stock.GetCurrentStockNames();

            metrics = GraphMetric.LoadValues();
            if(metrics == null)
                metrics = GraphMetric.GenerateCompositeMetrics();

            grdMetrics.DataSource = metrics;

            cmdConfiguration_Click(this, null);

            setStockToPlot(stock);

            userEnabled = true;
        }

        private void CompositeGraph_RangeChanged(LiveCharts.Events.RangeChangedEventArgs eventArgs)
        {
            try
            {
                if (eventArgs.Axis != chartComposite.AxisX[0])
                    return;

                Axis ax = (Axis)eventArgs.Axis;
                if (limitMax) ax.MaxValue = endTime.Ticks;
                if (limitMin) ax.MinValue = beginTime.Ticks;

                int lastStartAux = -1;
                try
                {
                    lastStartAux = Utils.ConvertDateTimeToInt(new DateTime((long)chartComposite.AxisX[0].ActualMinValue));
                }
                catch (Exception) { }

                if (lastRange == eventArgs.Range && rangeStart == lastStartAux)
                    return;

                rangeStart = lastStartAux;
                lastRange = eventArgs.Range;
                
                TimeSpan intervalDate = new TimeSpan((long)eventArgs.Range);

                interval = (int)((float)intervalDate.TotalDays * 0.8f) / 100;

                if (interval <= 5)
                    interval = 1;

                Console.WriteLine("INTERVAL " + interval);

                refreshGraph();
            }
            catch (Exception e)
            {
                interval = 50;
            }
        }

        void setStockToPlot(int stock)
        {
            this.Cursor = Cursors.WaitCursor;
            System.Windows.Forms.Application.DoEvents();

            chartComposite.AxisX.Clear();
            chartComposite.AxisY.Clear();

            chartComposite.AxisY.Add(new Axis
            {
                Foreground = System.Windows.Media.Brushes.Black,
                Title = "Values"
            });

            chartComposite.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Date",
                LabelFormatter = val => new System.DateTime((long)val).ToString("MM/yyyy"),
                Foreground = System.Windows.Media.Brushes.Black
            });

            chartComposite.AxisX[0].RangeChanged += this.CompositeGraph_RangeChanged;
            chartComposite.AxisX[0].PreviewRangeChanged += this.chartComposite_PreviewRangeChanged;

            stockToPlot = Stock.GetStock(stock);
            txtStockSymbol.Text = stockToPlot.Symbol;
            lstSymbols.Visible = false;

            stockToPlot.stockValues = Stock.GetAllPrices(stock);

            beginTime = Utils.ConvertIntToDateTime(stockToPlot.stockValues[0].Date);
            endTime = Utils.ConvertIntToDateTime(stockToPlot.stockValues[stockToPlot.stockValues.Count - 1].Date);

            interval = (int)(stockToPlot.stockValues.Count / 100);

            fillCompositeValues();

            refreshGraph();

            calcDataGrid();

            this.Cursor = Cursors.Arrow;
        }

        void refreshGraph()
        {
            chartComposite.Series.Clear();
            LiveCharts.SeriesCollection seriesToPlot = new LiveCharts.SeriesCollection();
            
            List<int> dates = new List<int>();
            foreach (var metric in metrics)
            {
                if (metric.HasAny())
                {
                    dates = new List<int>();
                    plotMetric(metric, seriesToPlot, dates, metric.LineColor);
                }
            }

            chartComposite.Zoom = ZoomingOptions.X;
            chartComposite.Pan = PanningOptions.X;
        }

        void calcDataGrid()
        {
            grdData.Rows.Clear();

            foreach (var metric in metrics)
            {
                if (metric.GetShowInOutputTable())
                {
                    grdData.Rows.Add();
                    var row = grdData.Rows[grdData.Rows.Count - 1];
                    row.Cells[0].Value = metric.Name;

                    row.Cells[1].Value = stockToPlot.stockValues[stockToPlot.stockValues.Count - 1].CompositeValues[(int)metric.Metric];

                    row.Cells[2].Value = stockToPlot.stockValues.Min(x => x.CompositeValues[(int)metric.Metric]);
                    row.Cells[3].Value = stockToPlot.stockValues.Max(x => x.CompositeValues[(int)metric.Metric]);
                    row.Cells[4].Value = stockToPlot.stockValues.Average(x => x.CompositeValues[(int)metric.Metric]);
                    row.Cells[5].Value = Feature.calcMedian(stockToPlot.stockValues.Select(x => x.CompositeValues[(int)metric.Metric]).ToList());

                    row.Cells[6].Value = getPercAbove(stockToPlot, metric);

                    row.Cells[7].Value = Feature.CalcZScore(stockToPlot.stockValues[stockToPlot.stockValues.Count - 1].CompositeValues[(int)metric.Metric], stockToPlot.stockValues.Select(x => x.CompositeValues[(int)metric.Metric]).ToList());
                    
                }
            }
        }

        decimal getPercAbove(Stock stock, GraphMetric metric)
        {
            decimal result = 0;
            if (stock.FiltersTables.Count == 0)
                return 0;
            if (stock.stockValues.Count == 0)
                return 0;
            if (stock.stockValues[stock.stockValues.Count-1].Close == 0)
                return 0;
            FilterTable currentFilter = stock.FiltersTables[stock.FiltersTables.Count - 1];

            switch (metric.Metric)
            {
                case GraphMetrics.Composite:
                    result = -100* (1- stock.stockValues[stock.stockValues.Count - 1].CompositeValues[(int)metric.Metric] / stock.stockValues[stock.stockValues.Count - 1].Close);
                    break;
                case GraphMetrics.PB:
                    result = -100* (1 - currentFilter.PB / stock.stockValues[stock.stockValues.Count - 1].Close);
                    break;
                case GraphMetrics.PS:
                    result = -100 * (1 - currentFilter.PS / stock.stockValues[stock.stockValues.Count - 1].Close);
                    break;
                case GraphMetrics.PFCF:
                    result = -100 * (1 - currentFilter.PFCF / stock.stockValues[stock.stockValues.Count - 1].Close);
                    break;
                case GraphMetrics.PE:
                    result = -100 * (1 - currentFilter.PE / stock.stockValues[stock.stockValues.Count - 1].Close);
                    break;
            }

            return result;
        }

        void plotMetric(GraphMetric metric, LiveCharts.SeriesCollection seriesToPlot, List<int> dates, System.Windows.Media.Color lineColor)
        {
            decimal max = decimal.MinValue;
            decimal min = decimal.MaxValue;
            List<decimal> medVal = new List<decimal>();
            List<decimal> values = new List<decimal>();
            List<decimal> allValues = new List<decimal>();
            List<decimal> rollMedVal = new List<decimal>();

            chartComposite.AxisX[0].LabelFormatter = val => new System.DateTime((long)val).ToString("MM/yyyy");
            if(interval < 10)
                chartComposite.AxisX[0].LabelFormatter = val => new System.DateTime((long)val).ToString("MM/dd/yyyy");

            int skipDate = 0;
            int skip = interval;
            foreach (var value in stockToPlot.stockValues)
            {
                allValues.Add(value.Close);
                max = Math.Max(max, value.Close);
                min = Math.Min(min, value.Close);
                
                if (skipDate > 0)
                {
                    if (value.Date < skipDate)
                    {
                        skip = 0;
                        continue;
                    }
                }
                else
                    if (skip-- > 0)
                        continue;

                skipDate = 0;
                int dateMin = 0;
                int dateMax = 0;

                try
                {
                    dateMin = Utils.ConvertDateTimeToInt(new DateTime((long)chartComposite.AxisX[0].ActualMinValue).AddDays(-20));
                    dateMax = Utils.ConvertDateTimeToInt(new DateTime((long)chartComposite.AxisX[0].ActualMaxValue).AddDays(20));
                }
                catch (Exception) { }

                if (value.Date < dateMin
                    || value.Date > dateMax && dateMax != 0)
                {
                    if (value.Date < dateMin)
                    {
                        skipDate = dateMin;
                        skip = 0;
                    }
                    else
                    {
                        skip = 100;
                    }
                }
                else
                    skip = interval;

                dates.Add(value.Date);

                values.Add(value.CompositeValues[(int)metric.Metric]);

                if(metric.Median)
                    medVal.Add((decimal) StockRanking.Feature.calcMedian(allValues));
                if (metric.RollingMedian)
                    rollMedVal.Add((decimal)StockRanking.Feature.calcMedian((from val in stockToPlot.stockValues
                                        where val.Date >= value.Date - 10000 * PortfolioParameters.GetCompositeRollingMedianYearsStatic() 
                                        && val.Date <= value.Date select val.CompositeValues[(int)metric.Metric]).ToList()));
            }
            if(skip < interval)
            {
                //same as in the for but to always have a last element, it could be done in a method
                var value = stockToPlot.stockValues[stockToPlot.stockValues.Count - 1];
                dates.Add(value.Date);

                values.Add(value.CompositeValues[(int)metric.Metric]);

                if (metric.Median)
                    medVal.Add((decimal)StockRanking.Feature.calcMedian(allValues));
                if (metric.RollingMedian)
                    rollMedVal.Add((decimal)StockRanking.Feature.calcMedian((from val in stockToPlot.stockValues
                                                                             where val.Date >= value.Date - 10000 * PortfolioParameters.GetCompositeRollingMedianYearsStatic()
                                                                             && val.Date <= value.Date
                                                                             select val.CompositeValues[(int)metric.Metric]).ToList()));
            }
            
            if (metric.Value)
                seriesToPlot.Add(addLine(metric.Name, values, lineColor, 0, dates));
            
            //max
            List<decimal> maxVal = Enumerable.Repeat(max, values.Count).ToList();
            if (metric.Max)
                seriesToPlot.Add(addLine("Max " + metric.Name, maxVal, lineColor, 1, dates));

            //min
            List<decimal> minVal = Enumerable.Repeat(min, values.Count).ToList();
            if (metric.Min)
                seriesToPlot.Add(addLine("Min " + metric.Name, minVal, lineColor, 2, dates));

            //median
            if (metric.Median)
                seriesToPlot.Add(addLine("Median " + metric.Name, medVal, lineColor, 3, dates));

            //rolling median
            if (metric.RollingMedian)
                seriesToPlot.Add(addLine("Rolling Median " + metric.Name, rollMedVal, lineColor, 4, dates));
            
            chartComposite.Series = seriesToPlot;
        }

        void fillCompositeValues()
        {
            //fill composite values for this stock
            int currentFilter = 0;
            foreach (var value in stockToPlot.stockValues)
            {
                if (currentFilter + 1 < stockToPlot.FiltersTables.Count)
                {
                    while (value.Date >= stockToPlot.FiltersTables[currentFilter + 1].Date)
                    {
                        currentFilter++;
                        if (currentFilter+1 >= stockToPlot.FiltersTables.Count)
                        {
                            currentFilter = stockToPlot.FiltersTables.Count - 1;
                            break;
                        }
                    }
                }

                var filter = stockToPlot.FiltersTables[currentFilter];
                value.CompositeValues = new List<decimal>();
                
                value.CompositeValues.Add(value.Close);
                //var composite = filter.GetCompositeForPrice(portfolioParameters, (double)value.Close);
                var composite = filter.GetCompositePrice(portfolioParameters);
                if (composite == double.MinValue)
                    composite = 0;
                value.CompositeValues.Add((decimal)composite);
                value.CompositeValues.Add((decimal)filter.PE);
                value.CompositeValues.Add((decimal)filter.PS);
                value.CompositeValues.Add((decimal)filter.PFCF);
                value.CompositeValues.Add((decimal)filter.PB);

                value.CompositeValues.Add((decimal)filter.PE);

                value.CompositeValues.Add(filter.EPS);

                //if (filter.EPS != 0)
                //    highestPE = Math.Max(highestPE, filter.ClosePrice / filter.EPS);

                //placeholder for price to max PE
            }
            /*
            //process price to reach max PE
            foreach (var value in stockToPlot.stockValues)
            {
                value.CompositeValues[6] = value.CompositeValues[6] * highestPE;
            }*/
        }

        LineSeries addLine(String label, List<decimal> values, System.Windows.Media.Color lineColor, int dashType, List<int> dates)
        {
            LineSeries newLine = new LineSeries();
            newLine.Title = label;
            newLine.Values = new ChartValues<DateTimePoint> { };
            newLine.PointGeometry = null;
            newLine.Stroke = new System.Windows.Media.SolidColorBrush(lineColor);
            newLine.StrokeThickness = 1;
            switch (dashType)
            {
                case 1:
                    newLine.StrokeDashArray = new System.Windows.Media.DoubleCollection { 1,1 };
                    break;
                case 2:
                    newLine.StrokeDashArray = new System.Windows.Media.DoubleCollection { 2,2,1,1 };
                    break;
                case 3:
                    newLine.StrokeDashArray = new System.Windows.Media.DoubleCollection { 3,1 };
                    break;
                case 4:
                    newLine.StrokeDashArray = new System.Windows.Media.DoubleCollection { 1,3 };
                    break;
                default:
                    break;
            }
            newLine.Stroke.Freeze();
            newLine.LineSmoothness = 0;
            newLine.Fill = System.Windows.Media.Brushes.Transparent;
            newLine.Fill.Freeze();
            int i = 0;
            foreach(var value in values)
                newLine.Values.Add(new DateTimePoint(Utils.ConvertIntToDateTime(dates[i++]), (double)Math.Round(value, 2)));

            return newLine;
        }

        private void grdMetrics_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (userEnabled)
            {
                GraphMetric.SaveValues(metrics);
                refreshGraph();
            }
        }

        private void grdMetrics_CellContentClick(object sender,DataGridViewCellEventArgs e)
        {
            grdMetrics.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void cmdConfiguration_Click(object sender, EventArgs e)
        {
            if (grdMetrics.Enabled)
            {
                grdMetrics.Visible = false;
                grdMetrics.Enabled = false;
                chartComposite.Height += grdMetrics.Height + 5;
            }
            else
            {
                grdMetrics.Visible = true;
                grdMetrics.Enabled = true;
                chartComposite.Height -= grdMetrics.Height + 5;
            }
        }

        private void cmdChangeStock_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SelectStockPopUp popup = new SelectStockPopUp();
            popup.ShowDialog();

            if (popup.selectedStock == null)
                return;

            setStockToPlot(popup.selectedStock.Id);
        }

        private void filterBar_Scroll(object sender, EventArgs e)
        {
        }

        Stock newSelectedStock = null;
        private void txtStockSymbol_TextChanged(object sender, EventArgs e)
        {
            //show stocks
            lstSymbols.Visible = true;
            lstSymbols.Items.Clear();
            int count = 0;
            String textSearch = txtStockSymbol.Text.Trim().ToUpper();
            newSelectedStock = null;

            foreach (Stock st in stocks)
            {
                if (st.Symbol == textSearch)
                {
                    newSelectedStock = st;
                }

                if (count < 50 && st.Symbol.StartsWith(textSearch))
                {
                    lstSymbols.Items.Add(st.Symbol.PadRight(6, ' ') + st.CompanyName);
                    count++;
                }
            }
        }

        private void txtStockSymbol_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {

                if (this.newSelectedStock == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a correct Stock Symbol");
                    lstSymbols.Visible = false;
                    txtStockSymbol.Text = stockToPlot.Symbol;
                    return;
                }

                lstSymbols.Visible = false;

                setStockToPlot(newSelectedStock.Id);
            }
        }

        private void txtStockSymbol_Leave(object sender, EventArgs e)
        {
            lstSymbols.Visible = false;
        }

        private void cmdPrint_Click(object sender, EventArgs e)
        {
            var fileDialog = new SaveFileDialog();
            fileDialog.DefaultExt = "pdf";
            fileDialog.Filter = "PDF Document|*.pdf";
            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            int oldw = chartComposite.Width;
            int oldh = chartComposite.Height;
            double oldFontX = chartComposite.AxisX[0].FontSize;
            double oldFontY = chartComposite.AxisY[0].FontSize;
            double oldLegend = chartComposite.DefaultLegend.FontSize;
            double oldBulletSize = ((DefaultLegend)chartComposite.DefaultLegend).BulletSize;
            
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font fontBold = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD);

            bool screenScaled = false;

            float dpiX, dpiY;
            Graphics graphics = this.CreateGraphics();
            dpiX = graphics.DpiX;
            dpiY = graphics.DpiY;

            chartComposite.LegendLocation = LegendLocation.Bottom;

            if (dpiX > 96 || dpiY > 96)
            {
                screenScaled = true;
            }
            else
            {
                chartComposite.DefaultLegend.FontSize = oldLegend * 3;
                chartComposite.Width = 1024 * 2;
                chartComposite.Height = 512 * 2;
                ((DefaultLegend)chartComposite.DefaultLegend).BulletSize = 3 * oldBulletSize;
                chartComposite.AxisX[0].FontSize = chartComposite.AxisX[0].FontSize * 3;
                chartComposite.AxisY[0].FontSize = chartComposite.AxisY[0].FontSize * 3;

                foreach (var serie in chartComposite.Series)
                {
                    ((LineSeries)serie).StrokeThickness = 2;
                }
            }

            cmdConfiguration.Visible = false;
            cmdPrint.Visible = false;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                Document document = new Document(PageSize.A4, 72, 72, 72, 72);
                using (FileStream fs = new FileStream(fileDialog.FileName, FileMode.Create, FileAccess.Write))
                {
                    Thread.Sleep(200);
                    System.Windows.Forms.Application.DoEvents();
                    Thread.Sleep(200);

                    PdfWriter.GetInstance(document, fs);
                    document.Open();

                    document.Add(new Paragraph("Symbol:  " + stockToPlot.Symbol, font));
                    Paragraph pa = new Paragraph("Company: " + stockToPlot.CompanyName, font);
                    pa.SpacingAfter = 15;
                    document.Add(pa);

                    Bitmap image = null;
                    
                    if (screenScaled)
                    {
                        image = new Bitmap(chartComposite.Width, chartComposite.Height);
                        
                        using (Graphics gb = Graphics.FromImage(image))
                        using (Graphics gc = Graphics.FromHwnd(chartComposite.Handle))
                        {
                            IntPtr hdcDest = IntPtr.Zero;
                            IntPtr hdcSrc = IntPtr.Zero;

                            try
                            {
                                hdcDest = gb.GetHdc();
                                hdcSrc = gc.GetHdc();

                                BitBlt(hdcDest, 0, 0, this.Width, this.Height, hdcSrc, 0, 0, SRC_COPY);
                            }
                            finally
                            {
                                if (hdcDest != IntPtr.Zero) gb.ReleaseHdc(hdcDest);
                                if (hdcSrc != IntPtr.Zero) gc.ReleaseHdc(hdcSrc);
                            }
                        }
                    }
                    else
                    {
                        image = new System.Drawing.Bitmap(chartComposite.Width, chartComposite.Height);
                        chartComposite.DrawToBitmap(image, chartComposite.ClientRectangle);
                    }

                    iTextSharp.text.Image image2 = iTextSharp.text.Image.GetInstance(image, ImageFormat.Bmp);
                    //PdfImage stream = new PdfImage(image2, "Chart", null);
                    image2.Border = iTextSharp.text.Rectangle.BOX;
                    image2.BorderColor = iTextSharp.text.BaseColor.BLACK;
                    image2.BorderWidth = 1f;

                    image2.ScaleToFit(PageSize.A4.Width - 144, PageSize.A4.Height - 180);

                    //image2.SetAbsolutePosition(72, 180+200);

                    document.Add(image2);


                    float[] columnWidths = { 2, 1, 1, 1, 1, 1, 1, 1 };
                    PdfPTable table = new PdfPTable(columnWidths);
                    table.SpacingBefore = 15;
                    table.HeaderRows = 1;
                    //table.SetWidthPercentage(columnWidths, PageSize.A4);
                    table.TotalWidth = image2.ScaledWidth;
                    table.WidthPercentage = 100;

                    foreach (DataGridViewColumn col in grdData.Columns)
                    {
                        Paragraph p = new Paragraph(col.HeaderText, fontBold);
                        p.Alignment = Element.ALIGN_MIDDLE;
                        table.AddCell(p);
                    }

                    foreach (DataGridViewRow row in grdData.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.Value is string)
                            {
                                Paragraph p = new Paragraph(cell.Value.ToString(), font);
                                p.Alignment = Element.ALIGN_LEFT;
                                table.AddCell(p);
                            }
                            else
                            {
                                Paragraph p = new Paragraph((Convert.ToDecimal(cell.Value)).ToString("n2"), font);
                                PdfPCell pdfCell = new PdfPCell(p);
                                pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                table.AddCell(pdfCell);
                            }
                        }
                    }

                    document.Add(table);


                    document.Close();

                    image.Dispose();

                }

                System.Diagnostics.Process.Start(fileDialog.FileName);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("There was an error generating the PDF, please check that the file is not already open and try again.");
            }

            chartComposite.LegendLocation = LegendLocation.None;
            if (!screenScaled)
            {
                chartComposite.DefaultLegend.FontSize = oldLegend;
                ((DefaultLegend)chartComposite.DefaultLegend).BulletSize = oldBulletSize;
                chartComposite.Width = oldw;
                chartComposite.Height = oldh;
                chartComposite.AxisX[0].FontSize = oldFontX;
                chartComposite.AxisY[0].FontSize = oldFontY;
                foreach (var serie in chartComposite.Series)
                {
                    ((LineSeries)serie).StrokeThickness = 1;
                }
            }

            cmdConfiguration.Visible = true;
            cmdPrint.Visible = true;

            this.Cursor = Cursors.Arrow;
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void CompositeGraph_Resize(object sender, EventArgs e)
        {

        }

        private void chartComposite_PreviewRangeChanged(LiveCharts.Events.PreviewRangeChangedEventArgs e)
        {
            // I use begintime and endtime and limited min and max
            limitMax = e.PreviewMaxValue > endTime.Ticks;
            limitMin = e.PreviewMinValue < beginTime.Ticks;
        }

        

    }
}
