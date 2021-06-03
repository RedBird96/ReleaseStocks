using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking.Entities
{
    public class BacktestResult
    {
        public decimal Pnl;
        public decimal Dd;
        public decimal Ratio;
        public decimal NumProfit;

        public decimal PNL { get => Pnl; set => Pnl = value; }
        public decimal DD { get => Dd; set => Dd = value; }
        public decimal RATIO { get => Ratio; set => Ratio = value; }
        public decimal NUMPROFIT { get => NumProfit; set => NumProfit= value; }

        public BacktestResult()
        {
            Pnl = 0;
            Dd = 0;
            Ratio = 0;
            NumProfit = 0;
        }
    }
}
