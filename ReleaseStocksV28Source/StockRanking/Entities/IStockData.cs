using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    interface IStockData
    {
        int Date { get; set; }
        int Quarter { get; set; }
        int Year { get; set; }

    }
}
