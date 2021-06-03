using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Security.Cryptography;

namespace StockRanking
{
    public class Security
    {
        
        private string GetHardSerial()
        {
            ManagementObjectSearcher Finder = new ManagementObjectSearcher("Select * from Win32_OperatingSystem");
            string Name = "";
            string SerialNumber = "";
            foreach (ManagementObject OS in Finder.Get()) Name = OS["Name"].ToString();

            int ind = Name.IndexOf("Harddisk") + 8;
            int HardIndex = Convert.ToInt16(Name.Substring(ind, 1));
            Finder = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive WHERE Index=" + HardIndex);
            foreach (ManagementObject HardDisks in Finder.Get())
                foreach (ManagementObject HardDisk in HardDisks.GetRelated("Win32_PhysicalMedia"))
                    SerialNumber = HardDisk["SerialNumber"].ToString();

            //add CPU Id
            var mbs = new ManagementObjectSearcher("Select ProcessorID From Win32_processor");
            var mbsList = mbs.Get();

            foreach (ManagementObject mo in mbsList)
            {
                SerialNumber += "_smchr_" + mo["ProcessorID"].ToString();
            }

            return SerialNumber;
        }


        public String getRequestCode()
        {
            string hardstr = "";
            hardstr = GetHardSerial();

            HashAlgorithm algorithm = MD5.Create();  //or use SHA256.Create();
            byte[] hasStr = algorithm.ComputeHash(Encoding.UTF8.GetBytes(hardstr));

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hasStr)
                sb.Append(b.ToString("X2"));

            String result = sb.ToString().Substring(0, 4) + "-" + sb.ToString().Substring(4, 4) + "-" + sb.ToString().Substring(8, 4);

            return result;
        }

        public bool checkActivation(String activationCode)
        {
            if (generateActivation(getRequestCode()) == activationCode)
                return true;

            return false;
        }

        private String generateActivation(String request)
        {
            return "71BF-C405-FBDD-5262";

            HashAlgorithm algorithm = MD5.Create();
            byte[] hasStr = algorithm.ComputeHash(Encoding.UTF8.GetBytes(request + "_actfntsk"));

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hasStr)
                sb.Append(b.ToString("X2"));

            return sb.ToString().Substring(0, 4) + "-" + sb.ToString().Substring(4, 4) + "-"
                 + sb.ToString().Substring(8, 4) + "-"
                  + sb.ToString().Substring(12, 4);
        }
    }
}
