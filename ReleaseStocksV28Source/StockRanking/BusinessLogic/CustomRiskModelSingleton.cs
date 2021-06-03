using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace StockRanking
{
    public class CustomRiskModelSingleton
    {
        Dictionary<int, bool> signals = null;
        String fileName = "";

        static CustomRiskModelSingleton instance = null;

        public static CustomRiskModelSingleton Instance
        {
            get {
                if (instance == null)
                    instance = new CustomRiskModelSingleton();

                return instance;
            }
        }

        public bool GetSignal(DateTime date)
        {
            try
            {
                if (signals == null)
                    loadFile();

                if (signals != null)
                {
                    //go backwards in time to get first signal
                    DateTime askDate = date;
                    DateTime limitDate = askDate.AddMonths(-4);
                    while (askDate > limitDate)
                    {
                        if (signals.ContainsKey(Utils.ConvertDateTimeToInt(askDate)))
                            return signals[Utils.ConvertDateTimeToInt(askDate)];

                        askDate = askDate.AddDays(-1);
                    }
                }
                
                return false;
            }
            catch (Exception e)
            {
                throw new Exception("Error loading Custom Risk Model File: " + e.Message);
            }

        }

        public void SetFile(String file)
        {
            fileName = file;

            signals = null;
        }

        void loadFile()
        {
            if (fileName.Trim() == "")
                return;

            if (!File.Exists(fileName))
                throw new Exception("File not found. Please check your risk file path");

            signals = new Dictionary<int, bool>();

            try
            {

                TextFieldParser textParser = null;
                textParser = new TextFieldParser(fileName);
                textParser.SetDelimiters(new string[] { "," });
                textParser.HasFieldsEnclosedInQuotes = false;

                while (!textParser.EndOfData)
                {
                    String[] data = textParser.ReadFields();

                    if (!signals.ContainsKey(Convert.ToInt32(data[0])))
                        signals.Add(Convert.ToInt32(data[0]), data[1].Trim() == "1");
                    else
                        data = null;
                }
            }
            catch(Exception e)
            {
                signals = null;
                throw e;
            }
        }

    }
}
