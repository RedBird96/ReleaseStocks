using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public class Trade
    {
        private decimal entryPrice = 0;
        private decimal exitPrice = 0;
        private int shares = 0;
        private int idStock = 0;
        private decimal profit = 0;
        private int entryDate = 0;
        private int exitDate = 0;
        private bool multipleEntries = false;
        private decimal commission = 0;
        private String symbol = "";
        private bool stopLossSell = false;
        private bool buyAfterStopLoss = false;
        private RiskModelResults riskModel = RiskModelResults.Normal;
        private String auxSector = "";
        private bool currentDateValorizationSell = false;
        private int realEntryDate = 0;
        private int realExitDate = 0;
        private SellingType selltype = SellingType.Long;

        public DateTime EntryDateDT { get { return Utils.ConvertIntToDateTime(EntryDate); } }
        public DateTime ExitDateDT { get { return Utils.ConvertIntToDateTime(ExitDate); } }
        public decimal EntryPrice { get => this.entryPrice; set => this.entryPrice = value; }
        public int Shares { get => this.shares; set => this.shares = value; }
        public decimal ExitPrice { get => this.exitPrice; set => this.exitPrice = value; }
        public int IdStock { get => this.idStock; set => this.idStock = value; }
        public decimal Profit { get => this.profit; set => this.profit = value; }
        public int EntryDate { get => this.entryDate; set => this.entryDate = value; }
        public int ExitDate { get => this.exitDate; set => this.exitDate = value; }
        public bool MultipleEntries { get => this.multipleEntries; set => this.multipleEntries = value; }
        public decimal Commission { get => this.commission; set => this.commission = value; }
        public string Symbol { get => this.symbol; set => this.symbol = value; }
        public bool StopLossSell { get => this.stopLossSell; set => this.stopLossSell = value; }
        public bool BuyAfterStopLoss { get => this.buyAfterStopLoss; set => this.buyAfterStopLoss = value; }
        public RiskModelResults RiskModel { get => this.riskModel; set => this.riskModel = value; }
        public decimal NetProfit { get => this.profit - this.commission; }
        public SellingType SellType { get => this.selltype; set => this.selltype = value; }
        public String RiskModelName { get => Enum.GetName(typeof(RiskModelResults), this.riskModel); }
        public string AuxSector { get => this.auxSector; set => this.auxSector = value; }
        public bool CurrentDateValorizationSell { get => this.currentDateValorizationSell; set => this.currentDateValorizationSell = value; }
        public int RealEntryDate { get => this.realEntryDate; set => this.realEntryDate = value; }
        public int RealExitDate { get => this.realExitDate; set => this.realExitDate = value; }

        public Trade()
        {

        }
    }
}
