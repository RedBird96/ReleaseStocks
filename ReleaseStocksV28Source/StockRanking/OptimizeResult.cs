using StockRanking.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockRanking
{
    public partial class OptimizeResult : Form
    {
        public partial class ThreadParameter
        {
            public List<FeatureWeight> fwlist;
            public BacktestCalculator calculator;
            public BacktestResults result;
            public int progressId;
            public int progress = 0;
            public BacktestResult outCash;
            
            public ThreadParameter()
            {

            }
        }

        Strategy strategy;
        List<FeatureWeight> orgFeatureWeights;
        List<FeatureWeight> bestFeatureWeights;
        decimal bestValue = 0;

        List<int> intervalIndex = new List<int>();
        List<float> diffValues = new List<float>();

        List<ProgressBar> progressBars = new List<ProgressBar>();
        List<BackgroundWorker> threadList = new List<BackgroundWorker>();
        List<ThreadParameter> results = new List<ThreadParameter>();
        List<BacktestResult> resultdata = new List<BacktestResult>();

        int TotalCount = 0;
        int bestUpdated = 0;
        int [] progressStatus = new int[10];
        private static Mutex mutex = new Mutex();

        public static bool isClosed = true;

        public int optimzeFunc = 0;

        static int FIRST_RANDOM = 5;
        Random rndGenerator = new Random();

        public OptimizeResult()
        {
            InitializeComponent();
        }

        public OptimizeResult(Strategy _strategy, List<FeatureWeight> _featureWeights, int selectedfunc)
        {
            InitializeComponent();

            strategy = _strategy;
            orgFeatureWeights = _featureWeights;
            optimzeFunc = selectedfunc;

            progressBars.Clear();
            progressBars.Add(progressBar0);
            progressBars.Add(progressBar1);
            progressBars.Add(progressBar2);
            progressBars.Add(progressBar3);
            for (int i = 0; i < 4; i++)
            {
                progressStatus[i] = 0;
                progressBars[i].Minimum = 0;
                progressBars[i].Step = 1;
                progressBars[i].Maximum = ((DateTime.Now.Year - 2004) * (12 / strategy.PortfolioParameters.RebalanceFrequencyMonths));
            }

            gridResults.Rows.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int index = gridResults.SelectedRows[0].Index;
            
            if (index == -1) return;

            DatabaseSingleton.Instance.StartTransaction();
            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM STRATEGY_FEATURE WHERE ID_STRATEGY = " + strategy.Id, null);

            results[index].fwlist.ForEach((item) =>
            {
                if (item.Weight > 0 && item.IsEnabled)
                {
                    Feature fw = new Feature(item);
                    fw.Save(strategy.Id);
                }
            });
            DatabaseSingleton.Instance.EndTransaction();

            StrategyView.refreshFeatures = true;
        }

        void RefreshFeaturesGrid()
        {
            foreach (DataGridViewRow row in grdFeatures.Rows)
            {
                if ((bool)row.Cells[0].Value == true)
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.Cells[2].ReadOnly = false;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.ForeColor = Color.Gray;
                    row.Cells[2].ReadOnly = true;
                }
            }
        }


        private void OptimizeResult_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;

            CachedFeatures.Instance.clearCache();
        }

        private List<FeatureWeight> GenerateRandomWeights()
        {
            List<FeatureWeight> featuresW = Feature.generateFeatureWeightsList();

            if (bestUpdated == 0 && TotalCount >= FIRST_RANDOM)
            {
                diffValues.Clear();
                if (intervalIndex.Count == 0)
                {
                    return null;
                }
                for (int i = 0; i < featuresW.Count; i++) diffValues.Add(0);
                for (int i = 0; i<2; i++)
                {
                    int ri = intervalIndex[rndGenerator.Next(0, intervalIndex.Count)];
                    FeatureWeight featureFound = orgFeatureWeights.Find(x => x.FeatureIndex == ri);
                    diffValues[ri] = rndGenerator.Next(0, (int)((featureFound.To - featureFound.From)/featureFound.Step) / 2 + 1) * featureFound.Step;
                    if (rndGenerator.Next(0, 100) % 2 == 0)
                    {
                        diffValues[ri] = -diffValues[ri];
                    }
                }
            }
            for (int i = 0; i < featuresW.Count; i++)
            {
                FeatureWeight fw = featuresW[i];

                fw.IsEnabled = false;
                fw.Weight = 0;

                try
                {
                    FeatureWeight featureFound = orgFeatureWeights.Find(x => x.FeatureIndex == i);
                    FeatureWeight bestFound;

                    if (TotalCount >= FIRST_RANDOM)
                    {
                        bestFound = bestFeatureWeights.Find(x => x.FeatureIndex == i);
                    } else
                    {
                        bestFound = featureFound;
                    }

                    if (featureFound == null)
                        continue;

                    if (featureFound.IsEnabled == false)
                    {
                        fw.Weight = 0;
                    } else if (featureFound.From == featureFound.To)
                    {
                        fw.Weight = featureFound.Weight;
                    } else
                    {
                        if (TotalCount < FIRST_RANDOM)
                        {
                            fw.Weight = (float)featureFound.From + rndGenerator.Next(0, (int)((featureFound.To - featureFound.From)/featureFound.Step) + 1) * featureFound.Step;
                            intervalIndex.Add(i);
                        } else if (bestUpdated == 0)
                        {
                            fw.Weight = bestFound.Weight + diffValues[i];
                        } else
                        {
                            fw.Weight = bestFound.Weight - diffValues[i];
                        }
                        if (fw.Weight < featureFound.From) fw.Weight = featureFound.From;
                        if (fw.Weight > featureFound.To) fw.Weight = featureFound.To;
                    }

                    if (fw.Weight != 0)
                        fw.IsEnabled = true;
                }
                catch (Exception)
                { }
            }

            bestUpdated++;
            return featuresW;
        }
        
        private void OptimizeResult_Shown(object sender, EventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                List<FeatureWeight> flist = GenerateRandomWeights();
                runBackTestThread(flist);
            }
            isClosed = false;

            gridResults.Width = 260;
            gridResults.Columns[0].Width = 60;
            gridResults.Columns[1].Width = 55;
            gridResults.Columns[2].Width = 60;
            gridResults.Columns[3].Width = 85;

            grdFeatures.Left = 260 + 20;
            grdFeatures.Width = this.Width - 260 - 20 - 25;
        }

        private void runBackTestThread(List<FeatureWeight> flist)
        {
            BackgroundWorker newThread = new BackgroundWorker();

            newThread.DoWork += new DoWorkEventHandler(runNewAlgorithm);
            newThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadCompleted);
            newThread.WorkerSupportsCancellation = true;
            threadList.Add(newThread);

            newThread.RunWorkerAsync(flist);
        }

        public void runNewAlgorithm(object sender, DoWorkEventArgs e)
        {
            int pi;
            BacktestResult outCash = new BacktestResult();

            try
            {
                mutex.WaitOne();
                List<FeatureWeight> flist = (List<FeatureWeight>) e.Argument;
            
                ThreadParameter param = new ThreadParameter();
                param.fwlist = flist;
                param.calculator = new BacktestCalculator(strategy.PortfolioParameters, flist, strategy.Filters, true);
            
                for (pi = 0; pi < 4; pi++) if (progressStatus[pi] == 0) break;

                progressStatus[pi] = 1;
                param.progressId = pi;
                mutex.ReleaseMutex();

                param.calculator.RunBacktest(progressBars[pi], new DateTime(2005, 1, 1), out outCash);
                
                mutex.WaitOne();
                param.outCash = outCash;
                e.Result = param;
                mutex.ReleaseMutex();
            } catch (Exception ex)
            {
                Console.WriteLine("Running Exception: ");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void OptimizeResult_FormClosed(object sender, FormClosedEventArgs e)
        {
            isClosed = true;
            foreach (var thread in threadList)
            {
                if (thread.IsBusy)
                {
                    thread.CancelAsync();
                }
            }
        }
        private void threadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && isClosed == false)
            {
                finishProcess(e.Result as ThreadParameter);
            }
        }

        private decimal getValue(BacktestResult result)
        {
            switch (optimzeFunc)
            {
                case 0:
                    return result.PNL;
                case 1:
                    return result.DD;
                case 2:
                    return result.RATIO;
                case 3:
                    return result.NUMPROFIT;
                default:
                    return 0;
            }
        }

        private void finishProcess(ThreadParameter param)
        {
            try
            {
                mutex.WaitOne();

                //param.result = new BacktestResults(param.calculator, true);

                progressStatus[param.progressId] = 0;
                progressBars[param.progressId].Value = 0;

                resultdata.Add(param.outCash);

                results.Add(param);
                gridResults.Rows.Add(param.outCash.Pnl.ToString("0.000"), (-(param.outCash.DD)).ToString("0.000"), param.outCash.Ratio.ToString("0.000"), param.outCash.NumProfit.ToString("0.000"));
                
                TotalCount++;
                mutex.ReleaseMutex();

                if (TotalCount >= 100)
                {
                    return;
                }
                else
                {
                    if (TotalCount == 1)
                    {
                        bestValue = (decimal)getValue(param.outCash);
                        bestFeatureWeights = param.fwlist;
                        bestUpdated = 0;
                    } else if (bestValue < (decimal)getValue(param.outCash))
                    {
                        bestValue = (decimal)getValue(param.outCash);
                        bestFeatureWeights = param.fwlist;
                        bestUpdated = 0;
                    } else if (bestUpdated > 1)
                    {
                        bestUpdated = 0;
                    }
                    if (TotalCount <= FIRST_RANDOM) bestUpdated = 0;
                    List<FeatureWeight> flist = GenerateRandomWeights();
                    if (flist == null) return;
                    runBackTestThread(flist);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("Finish Exception : " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void OptimizeResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            isClosed = true;
            foreach (var thread in threadList)
            {
                if (thread.IsBusy)
                {
                    thread.CancelAsync();
                }
            }
        }

        private void gridReults_SelectionChanged(object sender, EventArgs e)
        {
            if (gridResults.SelectedRows.Count == 0)
            {
                btnSave.Enabled = false;
            }
            else
            {
                try
                {
                    btnSave.Enabled = true;
                    int index = gridResults.SelectedRows[0].Index;
                    ThreadParameter param = results[index];
                    grdFeatures.AutoGenerateColumns = false;
                    grdFeatures.DataSource = param.fwlist;

                    RefreshFeaturesGrid();
                } catch (Exception ex)
                {
                }
            }
        }

        private void gridReults_DoubleClick(object sender, EventArgs e)
        {
            int id = gridResults.SelectedRows[0].Index;
            if (id == -1) return;
            ThreadParameter param = results[id];
            BacktestResults result = new BacktestResults(param.calculator, true);
            result.Show();
        }
    }
}
