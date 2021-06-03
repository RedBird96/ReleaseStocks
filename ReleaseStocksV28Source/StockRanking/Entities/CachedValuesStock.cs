using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public abstract class CachedValuesStock : IStockInfo
    {
        protected List<KeyValuePair<int, decimal>> prices = new List<KeyValuePair<int, decimal>>();
        protected List<KeyValuePair<int, decimal>> dividends = new List<KeyValuePair<int, decimal>>();

        public int PricesCount => prices.Count;

        public abstract String StockSymbol { get; }

        public abstract int IdStock { get; }

        protected abstract void loadPricesInternal();

        public void LoadPrices()
        {
            if (this.prices.Count > 0)
                return;

            this.loadPricesInternal();
        }

        public decimal getPrice(int date, int offset = 0, bool useLastClose = false)
        {
            var itemIndex = -1;

            if (prices.Count == 0)
                return 0;

            if (date >= prices.Last().Key)
                return prices.Last().Value;

            var result = prices.BinarySearch(new KeyValuePair<int, decimal>(date, 0),
                Comparer<KeyValuePair<int, decimal>>.Create((x, y) => x.Key.CompareTo(y.Key)));

            bool indexNotFound = false;
            if (result < 0)
            {
                indexNotFound = true;
                result = ~result;
            }

            result += offset;
            if (result < 0)
                result = 0;
            if (result >= prices.Count)
                result = prices.Count - 1;

            //useLastClose == true really means use next day instead of last day only if current date does not exists
            if (indexNotFound && offset == 0 && !useLastClose && result > 0)
            {
                //result--;
            }

            return prices[result].Value;
        }

        //returns the real price date that corresponds to an asked price for a date (for example if asked date is sunday it will return date for monday if that price exists)
        public int getPriceDate(int date)
        {
            var itemIndex = -1;

            if (prices.Count == 0)
                return 0;

            if (date >= prices.Last().Key)
                return prices.Last().Key;

            var result = prices.BinarySearch(new KeyValuePair<int, decimal>(date, 0),
                Comparer<KeyValuePair<int, decimal>>.Create((x, y) => x.Key.CompareTo(y.Key)));

            bool indexNotFound = false;
            if (result < 0)
            {
                indexNotFound = true;
                result = ~result;
            }

            if (result < 0)
                result = 0;
            if (result >= prices.Count)
                result = prices.Count - 1;

            return prices[result].Key;
        }

        public decimal getDividendSum(int dateFrom, int dateTo)
        {
            decimal dividendResult = 0;

            if (dividends.Count == 0)
                return 0;

            foreach(var dividend in dividends)
            {
                if (dividend.Key > dateTo)
                    return dividendResult;

                if (dividend.Key > dateFrom)
                    dividendResult += dividend.Value;
            }

            return dividendResult;
        }

    }
}
