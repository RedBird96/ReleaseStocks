using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace StockRanking
{
    public class CAPEModelDownloader
    {
        public static async Task<List<KeyValuePair<int, decimal>>> DownloadCAPE()
        {
            ChromeOptions chrome_options = new ChromeOptions();
            chrome_options.AddArgument("--headless");
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            String resultLines = "";

            using (IWebDriver driver = new ChromeDriver(driverService, chrome_options))
            {
                 
                driver.Navigate().GoToUrl("https://dqydj.com/scripts/spcalc/cape_calculator.html");
                 
                WebDriverWait tempWait = new WebDriverWait(driver, new TimeSpan(0,0,10)); 
                try
                {
                    tempWait.Until(d => false); 
                }
                catch (Exception e)
                {
                }

                resultLines = (String)((IJavaScriptExecutor)driver).ExecuteScript("return ConvertToCSV(capeByDate, document.getElementById('CAPESPAN').innerHTML)");
            }

            String[] lines = resultLines.Split('\n');
            List<KeyValuePair<int, decimal>> capeValues = new List<KeyValuePair<int, decimal>>();
            bool firstline = true;
            foreach (String line in lines)
            {
                if (firstline)
                {
                    firstline = false;
                    continue;
                }

                try
                {
                    String[] values = line.Split(',');
                    if (values.Length == 1)
                        continue;

                    String date = values[0].Replace(".", ""); 

                    decimal decValue = Convert.ToDecimal(values[1], CultureInfo.InvariantCulture);

                    capeValues.Add(new KeyValuePair<int, decimal>(Convert.ToInt32(date), decValue));
                }
                catch(Exception e)
                {
                    
                }
            }

            return capeValues;
        }
    }
}
