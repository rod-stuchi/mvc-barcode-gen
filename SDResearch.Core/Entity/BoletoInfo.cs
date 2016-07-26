using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDResearch.Core.Entity
{
    public class BoletoInfo
    {
        public string bankCode { get; set; }

        public string value { get; set; }
        public bool valueCheck { get; set; }
        public int valueStart { get; set; }
        public int valueEnd { get; set; }

        public string expirate { get; set; }
        public bool expirateCheck { get; set; }
        public string expirateStart { get; set; }
        public string expirateEnd { get; set; }

        public bool autoGenerate { get; set; }

        public string line { get; set; }
        public string lineFormatted { get; set; }
        public string barcodeBase64 { get; set; }
    }
}
