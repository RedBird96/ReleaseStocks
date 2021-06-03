using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public class SnapshotVariation
    {
        public int Months = 1;
        public int Date = 0;
        public decimal Value = 0;

        public SnapshotVariation(int date, decimal value)
        {
            this.Date = date;
            this.Value = value;
        }
    }
}
