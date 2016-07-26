using Newtonsoft.Json;
using SDResearch.Core.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDResearch.Core.Entity
{
    public class ConsumoInfo
    {
        [JsonConverter(typeof(ConverterInt32))]
        public int segment { get; set; }
        public string code { get; set; }

        public string value { get; set; }
        public bool valueCheck { get; set; }
        public int valueStart { get; set; }
        public int valueEnd { get; set; }
       
        public bool autoGenerate { get; set; }

        public string line { get; set; }
        public string lineFormatted { get; set; }
        public string barcodeBase64 { get; set; }
    }
}
