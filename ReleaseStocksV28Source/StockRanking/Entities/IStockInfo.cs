using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public interface IStockInfo
    {
        String StockSymbol { get; }
        int IdStock { get; }
    }
}
