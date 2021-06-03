using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking.BusinessLogic
{
    class Utility
    {
        public static FilterTable BinarySearchFilterTable(List<FilterTable> list, int dateInt)
        {
            int count = list.Count;

            if (count == 0) return null;

            int low = 0, high = count, mid;

            while (high - low > 1)
            {
                mid = (high + low) / 2;
                if (list[mid].Date == dateInt)
                {
                    return list[mid];
                }
                else if (list[mid].Date > dateInt)
                {
                    high = mid;
                }
                else
                {
                    low = mid;
                }
            }
            if (list[low].Date == dateInt)
            {
                return list[low];
            }
            return null;
        }

        public static StockRatios BinarySearchLastRatio(List<StockRatios> list, int dateInt)
        {
            int count = list.Count;

            if (count == 0) return null;

            if (list[0].DateFirstMonth > dateInt) return null;

            int low = 0, high = count, mid;

            while (high - low > 1)
            {
                mid = (high + low) / 2;
                if (list[mid].DateFirstMonth> dateInt)
                {
                    high = mid;
                }
                else
                {
                    low = mid;
                }
            }

            return list[low];
        }
    }
}
