using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DorsaFinUI.Models
{
    public class Greeks
    {
        public decimal Delta { get; set; }
        public decimal Gamma { get; set; }
        public decimal Theta { get; set; }
        public decimal Vega { get; set; }
        public decimal Rho { get; set; }
        public decimal Phi { get; set; }
        public decimal BidIV { get; set; }
        public decimal MidIV { get; set; }
        public decimal AskIV { get; set; }
        public decimal SmvVol { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}