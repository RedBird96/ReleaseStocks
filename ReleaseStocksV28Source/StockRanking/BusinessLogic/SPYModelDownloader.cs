using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace StockRanking
{
    public class SPYModelDownloader
    {
        public static async Task<List<String>> DownloadSPY500Symbols()
        {
            ChromeOptions chrome_options = new ChromeOptions();
            chrome_options.AddArgument("--headless");
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            List<String> tickers = new List<string>();

            try
            {

                using (IWebDriver driver = new ChromeDriver(driverService, chrome_options))
                {

                    driver.Navigate().GoToUrl("https://en.wikipedia.org/wiki/List_of_S%26P_500_companies");

                    WebDriverWait tempWait = new WebDriverWait(driver, new TimeSpan(0, 0, 3));
                    try
                    {
                        tempWait.Until(d => false);
                    }
                    catch (Exception e)
                    {
                    }
                    
                    var tables = driver.FindElements(By.TagName("table"));

                    var rows = tables[0].FindElements(By.TagName("tr"));
                    bool firstIndex = true;
                    int symbolColIndex = 0;
                    foreach (IWebElement row in rows)
                    {
                        var tds = row.FindElements(By.TagName("td"));

                        if(tds.Count == 0 && firstIndex)
                        {
                            firstIndex = false;
                            var theads = row.FindElements(By.TagName("th"));
                            foreach (IWebElement td in theads)
                            {
                                if(td.Text.Trim().ToLower() == "symbol")
                                {
                                    break;
                                }

                                symbolColIndex++;
                            }

                            if (symbolColIndex > 4)
                                symbolColIndex = 0;

                            continue;
                        }

                        if (tds.Count == 0)
                            continue;

                        String tickerText = tds[0].Text;

                        if (tickerText.Contains("Symbol"))
                            continue;

                        tickers.Add(tickerText);
                    }
                }
            }
            catch(Exception e)
            {
                return tickers;
            }

            return tickers;

        }


    }
}
