using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DorsaFinUI.Models
{
    public class Option
    {
        public string Symbol { get; set; }
        public string Description { get; set; }
        public string Exchange { get; set; }
        public string Type { get; set; }
        public decimal Last { get; set; }
        public decimal Change { get; set; }
        public int Volume { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public string Underlying { get; set; }
        public decimal Strike { get; set; }
        public Greeks Greeks { get; set; }
        public decimal ChangePercentage { get; set; }
        public int AverageVolume { get; set; }
        public int LastVolume { get; set; }
        public long TradeDate { get; set; }
        public decimal PrevClose { get; set; }
        public decimal FiftyTwoWeekHigh {get;set;}
        public decimal FiftyTwoWeekLow { get; set; }
        public int BidSize { get; set; }
        public char BidExchange { get; set; }
        public int BidDate { get; set; }
        public int AskSize { get; set; }
        public char AskExchange { get; set; }
        public int OpenInterest { get; set; }
        public int ContractSize { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string ExpirationType { get; set; }
        public string OptionType { get; set; }
        public string RootSymbol { get; set; }


  
    }
}