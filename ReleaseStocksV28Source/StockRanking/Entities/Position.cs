using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace StockRanking
{
    public class Position
    {
        private int idStock = -1;
        private int shares = 0;
        private int dateEntered = 0;
        private decimal entryPrice = 0;
        private bool multipleEntries = false;
        private decimal stopLossPrice = 0;
        private bool buyAfterStopLoss = false;
        private SellingType sellingstyle = SellingType.Long;
        
        private String auxSymbol = "";
        private decimal auxCurrentPrice = 0;
        private String auxName = "";
        private String auxSector = "";
        private decimal auxDailyChange = 0;
        private decimal auxYTD = 0;
        private decimal auxStop = 0;

        public int IdStock { get => this.idStock; set => this.idStock = value; }
        public int Shares { get => this.shares; set => this.shares = value; }
        public int DateEntered { get => this.dateEntered; set => this.dateEntered = value; }
        public decimal EntryPrice { get => this.entryPrice; set => this.entryPrice = value; }
        public bool MultipleEntries { get => this.multipleEntries; set => this.multipleEntries = value; }
        public decimal StopLossPrice { get => this.stopLossPrice; set => this.stopLossPrice = value; }
        public bool BuyAfterStopLoss { get => this.buyAfterStopLoss; set => this.buyAfterStopLoss = value; }
        public string AuxSymbol { get => this.auxSymbol; set => this.auxSymbol = value; }
        public decimal AuxCurrentPrice { get => this.auxCurrentPrice; set => this.auxCurrentPrice = value; }
        public string AuxName { get => this.auxName; set => this.auxName = value; }
        public decimal AuxDailyChange { get => this.auxDailyChange; set => this.auxDailyChange = value; }
        public decimal AuxYTD { get => this.auxYTD; set => this.auxYTD = value; }
        public decimal AuxStop { get => this.auxStop; set => this.auxStop = value; }
        public string AuxSector { get => this.auxSector; set => this.auxSector = value; }
        public SellingType SellingStyle { get => this.sellingstyle; set => this.sellingstyle = value; }
        public decimal AuxPortGain => this.Shares == 0 ? 0 : (this.sellingstyle == SellingType.Long ? (1) : (-1)) * (this.Shares * this.AuxCurrentPrice - this.Shares * this.EntryPrice) / (this.Shares * this.EntryPrice) * 100;

        public decimal AuxPtsAboveStop => this.IdStock < 0 ? 0 : (this.sellingstyle == SellingType.Long ? (1) : (-1)) * (this.AuxCurrentPrice - this.StopLossPrice);

        public decimal AuxPercAboveStop => this.StopLossPrice == 0 ? 0 : (this.sellingstyle == SellingType.Long ? (1) : (-1)) * (this.AuxCurrentPrice - this.StopLossPrice) / this.StopLossPrice * 100;

        public decimal AuxDaysInTrade => (decimal) Math.Floor((DateTime.Now - Utils.ConvertIntToDateTime(this.DateEntered)).TotalDays);

        public DateTime AuxDateEntered => Utils.ConvertIntToDateTime(this.DateEntered);

        public Position(int stock, int shares, int date, decimal price, decimal stopLossPrice, String symbol, String name, String sector, SellingType selltype = SellingType.Long)
        {
            IdStock = stock;
            Shares = shares;
            DateEntered = date;
            EntryPrice = price;
            StopLossPrice = stopLossPrice;
            AuxSymbol = symbol;
            auxName = name;
            auxSector = sector;
            sellingstyle = selltype;
        }

        public Position(Position oldPos)
        {
            IdStock = oldPos.IdStock;
            Shares = oldPos.Shares;
            DateEntered = oldPos.DateEntered;
            EntryPrice = oldPos.EntryPrice;
            StopLossPrice = oldPos.StopLossPrice;
            AuxSymbol = oldPos.AuxSymbol;
            buyAfterStopLoss = oldPos.buyAfterStopLoss;
            auxName = oldPos.AuxName;
            multipleEntries = oldPos.multipleEntries;
            auxSector = oldPos.auxSector;
            sellingstyle = oldPos.sellingstyle;
        }

        public Position(DataRow row)
        {
            IdStock = Convert.ToInt32(row["ID_STOCK"]);
            Shares = Convert.ToInt32(row["SHARES"]);
            DateEntered = Convert.ToInt32(row["ENTRY_DATE"]);
            EntryPrice = Convert.ToDecimal(row["ENTRY_PRICE"]);
        }

        public void UpdateStopLossPrice(decimal currentPrice, PortfolioParameters portfolio)
        {
            if (portfolio.LossOnlyInitial != 0)
            {
                return;
            }
            decimal newStopLoss = currentPrice - ((decimal)1-(decimal)portfolio.TrailingStopLoss/100) * EntryPrice;

            if (newStopLoss > StopLossPrice)
                StopLossPrice = newStopLoss;
        }

        public void UpdateStopLossShortPrice(decimal currentPrice, PortfolioParameters portfolio)
        {
            if (portfolio.LossOnlyInitial != 0)
            {
                return;
            }
            decimal newStopLoss = currentPrice + ((decimal)1 - (decimal)portfolio.TrailingStopLoss / 100) * EntryPrice;

            if (newStopLoss < StopLossPrice)
                StopLossPrice = newStopLoss;
        }
    }
}
