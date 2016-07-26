using Newtonsoft.Json;
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
	public class ConsumoWorker
	{
		#region Variables
		private readonly Random randomValues = new Random((int)DateTime.Now.Ticks);
		private readonly Random randomCode = new Random((int)DateTime.Now.Millisecond);
        private readonly List<DealershipInfo> dealerShips;
		
		private readonly string VirtualPath;

		private static string[] commonCodes = new string[] 
		{ "40291", "40293", "40290", "40292", "40294", "40295", "40047", "40049", "40041", "40042", "40055", "40074", "40073", "40072", "40081", "40076", "40075", "40058", "40053", "40060", "40064", "40079", "40069", "40048", "40080", "40392", "40110", "40111", "21335", "40284", "40109", "40056", "40046", "40062", "40052", "40043", "40040", "40034", "40037", "40059", "20282", "20097", "40033", "40221", "40160", "40297", "40158", "40165", "40161", "40163", "20026", "40162", "40159", "40113", "40166", "30178", "30182", "30057", "30116", "30123", "30040", "30055", "30039", "30026", "30052", "30027", "30053", "30045", "30046", "30048", "40296", "40305", "30007", "30149", "30054", "30146", "30024", "30145", "30049", "30148", "30068", "30089", "30019", "30147", "30047", "30119", "30073", "30028", "30155", "40082", "41029", "40084", "40006" 
		}; 
		#endregion

		public ConsumoWorker(string virtualPath)
		{
			VirtualPath = virtualPath;

            string _dships = File.ReadAllText(Path.Combine(VirtualPath, @"bin\Resources\Dealerships.json"));

            dealerShips = JsonConvert.DeserializeObject<List<DealershipInfo>>(_dships);

		}

		public List<DealershipInfo> GetDealerships()
		{
            return dealerShips;
		}

		public string RandomValue(int min, int max)
		{
			if (min <= 0 | max <= 1)
				return "1,00";

			return Math.Round(((double)randomValues.Next(min, max) + (double)randomValues.NextDouble()), 2).ToString("N");
		}

		public DealershipInfo RandomDealership()
		{
            int ramdonCode = randomCode.Next(0, commonCodes.Length);
            string dealershipCode = commonCodes[ramdonCode];
            int segment = Int32.Parse(dealershipCode.Substring(0, 1));
            string code = dealershipCode.Substring(1, 4);

            return dealerShips.Where(x => 
                x.segment == segment && 
                x.code.Equals(code)).First();
		}


        public CodebarResult GenerateConsumo(int segment, string code, string value)
        {
            int identifierType = randomValues.Next(2) == 0 ? 6 : 8;
            string valueF = Utils.GetFactorFromValueFormatted(value, 11);

            CodebarResult consumo = new CodebarResult();

            string block01 = String.Concat("8", segment, identifierType, valueF.Substring(0, 7));

            string block02 = String.Concat(valueF.Substring(7, 4), code, randomCode.Next(1000).ToString().PadLeft(3, '0'));

            string block03 = String.Concat(
                    randomCode.Next(100000).ToString().PadLeft(5, '0'), 
                    randomCode.Next(1000).ToString().PadLeft(3, '0'),
                    randomCode.Next(1000).ToString().PadRight(3, '0'));

            string block04 = String.Concat(
                    randomCode.Next(100000).ToString().PadLeft(5, '0'),
                    randomCode.Next(1000).ToString().PadLeft(3, '0'),
                    randomCode.Next(1000).ToString().PadRight(3, '0'));

            string allBlocks = String.Concat(block01, block02, block03, block04);

            string digVMaster = identifierType == 6 ? Mod10.GetMod10Digit(allBlocks) : Mod11.GetMod11Digit(allBlocks);

            block01 = block01.Insert(3, digVMaster);


            if (identifierType == 6)
            {
                block01 = Mod10.ConcactMod10Digit(block01);
                block02 = Mod10.ConcactMod10Digit(block02);
                block03 = Mod10.ConcactMod10Digit(block03);
                block04 = Mod10.ConcactMod10Digit(block04);
            }
            else
            {
                block01 = Mod11.ConcactMod11Digit(block01);
                block02 = Mod11.ConcactMod11Digit(block02);
                block03 = Mod11.ConcactMod11Digit(block03);
                block04 = Mod11.ConcactMod11Digit(block04);
            }

            consumo.LineFormatted = String.Format("{0} {1} {2} {3}", block01, block02, block03, block04);
            consumo.Line = Utils.GetNumbersFromLine(consumo.LineFormatted);
            consumo.BarcodeLine = Utils.ConvertToBarcode(consumo.LineFormatted);
            Image img = Image.FromStream(new MemoryStream(new C2of5i(consumo.BarcodeLine, 1, 50, consumo.BarcodeLine.Length).ToByte()));
            using (MemoryStream imgJpg = new MemoryStream())
            {
                img.Save(imgJpg, ImageFormat.Png);
                consumo.BarcodeBase64 = Convert.ToBase64String(imgJpg.ToArray());
            }
            return consumo;
        }
	}
}
