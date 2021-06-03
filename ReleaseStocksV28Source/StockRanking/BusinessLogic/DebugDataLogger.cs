using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public class DebugDataLogger
    {
        public static DebugDataLogger instance = null;

        Dictionary<String, StreamWriter> logFiles = new Dictionary<string, StreamWriter>();

        public static DebugDataLogger Instance
        {
            get
            {
                if (instance == null)
                    instance = new DebugDataLogger();

                return instance;
            }
        }

        public DebugDataLogger()
        {
        }

        public void CloseAll()
        {
            foreach(var stream in logFiles.Values)
            {
                stream.Close();
                stream.Dispose();
            }

            logFiles.Clear();
        }

        public void WriteLine(String fileName, String line)
        {
#if DEBUG
            StreamWriter writer;
            if (!logFiles.ContainsKey(fileName))
            {
                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StocksLogs"))
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StocksLogs");

                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StocksLogs\\" + fileName + ".csv"))
                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StocksLogs\\" + fileName + ".csv");
                logFiles.Add(fileName, new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StocksLogs\\" + fileName + ".csv"));
            }
            writer = logFiles[fileName];

            writer.WriteLine(line);

            writer.Flush();
#endif
        }

        public void WriteLogLine(String fileName, String line)
        {
            StreamWriter writer;
            if (!logFiles.ContainsKey(fileName))
            {
                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StocksLogs"))
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StocksLogs");

                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StocksLogs\\" + fileName + ".csv"))
                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StocksLogs\\" + fileName + ".csv");
                logFiles.Add(fileName, new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StocksLogs\\" + fileName + ".csv"));
            }
            writer = logFiles[fileName];

            writer.WriteLine(line);

            writer.Flush();
        }

    }
}
