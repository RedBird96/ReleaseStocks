using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking.Entities
{
    public class TradeHistory
    {
        private decimal price = 0;
        private int shares = 0;
        private int idStock = 0;
        private int date = 0;
        private decimal profit = 0;
        private decimal commission = 0;
        private SellingType selltype = SellingType.Long;
        private TradeType tradeType = TradeType.Buy;
        private String symbol = "";
        private bool isExit = false;

        /*
        private bool stopLossSell = false;
        private bool buyAfterStopLoss = false;
        private RiskModelResults riskModel = RiskModelResults.Normal;
        private String auxSector = "";
        private bool currentDateValorizationSell = false;
        */
        
        public DateTime DateDT { get { return Utils.ConvertIntToDateTime(date); } }
        public String Action { get { return tradeType.GetDisplayName(); } }
        public decimal Price { get => this.price; set => this.price = value; }
        public bool IsExit { get => this.isExit; set => this.isExit = value; }
        public int Shares { get => this.shares; set => this.shares = value; }
        public int IdStock { get => this.idStock; set => this.idStock = value; }
        public decimal Profit { get => this.profit; set => this.profit = value; }
        public int Date { get => this.date; set => this.date = value; }
        public decimal Commission { get => this.commission; set => this.commission = value; }
        public string Symbol { get => this.symbol; set => this.symbol = value; }
        
        public decimal NetProfit { get => this.profit - this.commission; }
        public SellingType SellType { get => this.selltype; set => this.selltype = value; }
        public TradeType TradeType { get => this.tradeType; set => this.tradeType= value; }
        /*
        public bool StopLossSell { get => this.stopLossSell; set => this.stopLossSell = value; }
        public bool BuyAfterStopLoss { get => this.buyAfterStopLoss; set => this.buyAfterStopLoss = value; }
        public RiskModelResults RiskModel { get => this.riskModel; set => this.riskModel = value; }
        public String RiskModelName { get => Enum.GetName(typeof(RiskModelResults), this.riskModel); }
        public string AuxSector { get => this.auxSector; set => this.auxSector = value; }
        public bool CurrentDateValorizationSell { get => this.currentDateValorizationSell; set => this.currentDateValorizationSell = value; }
        */
        public TradeHistory()
        {

        }
    }
}
