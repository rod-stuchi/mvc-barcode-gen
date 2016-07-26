using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDResearch.Core.Entity
{
    public class ByLineInfo
    {
        public string value { get; set; }
        public string dealership { get; set; }
        public string bank { get; set; }
        public string expirate { get; set; }

        public string fromLine { get; set; }
        public string line { get; set; }
        public string lineFormatted { get; set; }
        public string barcodeBase64 { get; set; }
    }
}
