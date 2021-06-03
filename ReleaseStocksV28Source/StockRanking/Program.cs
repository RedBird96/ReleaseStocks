using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockRanking
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool result;
            var mutex = new System.Threading.Mutex(true, "StockRankingSolution", out result);
            
            if (!result)
            {
                MessageBox.Show("Only one instance of the application can run at the same time.");
                return;
            } 
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new MainPanel());
             
            GC.KeepAlive(mutex);
            
        }
    }
}
