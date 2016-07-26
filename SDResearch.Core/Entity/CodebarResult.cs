using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDResearch.Core.Entity
{
    public class CodebarResult
    {
        public string Line { get; set; }
        public string LineFormatted { get; set; }
        public string BarcodeBase64 { get; set; }
        public string BarcodeLine { get; set; }
    }
}
