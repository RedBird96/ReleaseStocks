using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public class SimpleIdNameStock : IStockInfo
    {
        public int IdStock { get; }
        public String StockSymbol { get; }

        public SimpleIdNameStock(int id, String name)
        {
            this.IdStock = id;
            this.StockSymbol = name;
        }
    }
}
