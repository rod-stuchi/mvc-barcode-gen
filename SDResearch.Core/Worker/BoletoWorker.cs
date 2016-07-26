
using Newtonsoft.Json;
using SDResearch.Core.Barcode;
using SDResearch.Core.Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SDResearch.Core.Worker
{
    public class BoletoWorker
    {
        private readonly Random randomValues = new Random((int)DateTime.Now.Ticks);
        private readonly Random randomDates = new Random((int)DateTime.Now.Ticks);

        public BoletoWorker()
        {

        }

        public string GetBanksJson(string virtualPath)
        {
            string banks = File.ReadAllText(Path.Combine(virtualPath, @"bin\Resources\Banks.json"));

            return banks;
        }

        public static List<BanksInfo> GetBanks(string virtualPath)
        {
            string banksFile = File.ReadAllText(Path.Combine(virtualPath, @"bin\Resources\Banks.json"));
            List<BanksInfo> banks = JsonConvert.DeserializeObject<List<BanksInfo>>(banksFile);

            return banks;
        }

        public string RandomValue(int min, int max)
        {
            if (min <= 0 | max <= 1)
                return "1,00";

            return Math.Round(((double)randomValues.Next(min, max) + (double)randomValues.NextDouble()), 2).ToString("N");
        }

        public DateTime RandomDate(string min, string max)
        {
            DateTime dmin, dmax;
            if (DateTime.TryParse(min, out dmin) && DateTime.TryParse(max, out dmax))
            {
                int rDate = randomDates.Next(dmin.ToInt32(), dmax.ToInt32() + 1);

                return Utils.FromInt32ToDateTime(rDate);
            }
            else
            {
                return DateTime.Now;
            }
        }

        public int RandomBank()
        {
            int[] codeBanks = new int[] { 1, 33, 104, 237, 341, 399 };
            int bank = codeBanks[randomValues.Next(0, codeBanks.Length)];

            return bank;
        }

        public CodebarResult GenerateBoleto(string bank, string date, string value)
        {
            CodebarResult boleto = new CodebarResult();

            string block01 = bank.PadLeft(3, '0') + randomValues.Next(50, 999999).ToString().PadLeft(6, '0');

            string block02 = randomValues.Next(1000, 99999).ToString().PadLeft(5, '0') + randomValues.Next(1000, 99999).ToString().PadLeft(5, '0');

            string block03 = randomValues.Next(1000, 99999).ToString().PadLeft(5, '0') + randomValues.Next(1000, 99999).ToString().PadLeft(5, '0');

            string block05 = String.Concat(Utils.GetFactorFromDate(date), Utils.GetFactorFromValueFormatted(value));

            string block04 = Barcode.Mod11.GetMod11Digit(String.Concat(block01.Insert(4, block05), block02, block03), 1);

            block01 = Barcode.Mod10.ConcactMod10Digit(block01).Insert(5, ".");
            block02 = Barcode.Mod10.ConcactMod10Digit(block02).Insert(5, ".");
            block03 = Barcode.Mod10.ConcactMod10Digit(block03).Insert(5, ".");

            boleto.LineFormatted = String.Format("{0} {1} {2} {3} {4}", block01, block02, block03, block04, block05);
            boleto.Line = Utils.GetNumbersFromLine(boleto.LineFormatted);
            boleto.BarcodeLine = Utils.ConvertToBarcode(boleto.LineFormatted);
            Image img = Image.FromStream(new MemoryStream(new C2of5i(boleto.BarcodeLine, 1, 50, boleto.BarcodeLine.Length).ToByte()));
            using (MemoryStream imgJpg = new MemoryStream())
            {
                img.Save(imgJpg, ImageFormat.Png);
                boleto.BarcodeBase64 = Convert.ToBase64String(imgJpg.ToArray());
            }
            return boleto;
        }
    }
}
