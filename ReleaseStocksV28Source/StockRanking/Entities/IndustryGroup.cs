using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{

    public class IndustryGroup
    {
        public IndustryGroup(String name) { industry = name; }

        public String industry = "";
        public List<double> featureSum = new List<double>();
        public int count = 0;
        public double rankingSum = 0;
        public double ytdSum = 0;
        public decimal volumeSum = 0;
        public decimal closingPriceSum = 0;
        public decimal marketCapSum = 0;
        public decimal forwardGain = 0;

        public static Dictionary<int, String> Sectors = new Dictionary<int, string>();
        public static Dictionary<String, int> IndustrySector = new Dictionary<String, int>();

        public static String GetSector(String industry)
        {
            //load cache lists if needed
            if(Sectors.Count == 0)
            {
                DataTable results = DatabaseSingleton.Instance.GetData("SELECT ID, NAME FROM SECTOR");
                Sectors = new Dictionary<int, string>();

                foreach(DataRow row in results.Rows)
                    Sectors.Add(Convert.ToInt32(row[0]), row[1].ToString());

                results = DatabaseSingleton.Instance.GetData("SELECT INDUSTRY, ID_SECTOR FROM SECTOR_INDUSTRY");
                IndustrySector = new Dictionary<String, int>();

                foreach (DataRow row in results.Rows)
                    IndustrySector.Add(row[0].ToString(), Convert.ToInt32(row[1]));
            }

            if (!IndustrySector.ContainsKey(industry))
                return "NONE";
            int sectorId = IndustrySector[industry];

            if (!Sectors.ContainsKey(sectorId))
                return "NONE";

            return Sectors[sectorId];
        }

    }

}
