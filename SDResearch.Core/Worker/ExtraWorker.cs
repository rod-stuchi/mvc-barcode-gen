using SDResearch.Core.Barcode;
using SDResearch.Core.Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDResearch.Core.Worker
{
    public class ExtraWorker
    {
        public CodebarResult GenerateBarcodeByLine(string line)
        {
            CodebarResult barcode = new CodebarResult();

            barcode.BarcodeLine = Utils.ConvertToBarcode(Utils.FormatLine(line));
            barcode.Line = Utils.GetNumbersFromLine(line);
            barcode.LineFormatted = Utils.FormatLine(line);

            Image img = Image.FromStream(new MemoryStream(new C2of5i(barcode.BarcodeLine, 1, 50, barcode.BarcodeLine.Length).ToByte()));
            using (MemoryStream imgJpg = new MemoryStream())
            {
                img.Save(imgJpg, ImageFormat.Png);
                barcode.BarcodeBase64 = Convert.ToBase64String(imgJpg.ToArray());
            }
            return barcode;
        }
    }
}
