using Newtonsoft.Json;
using SDResearch.Core;
using SDResearch.Core.Entity;
using SDResearch.Core.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SDResearch.UI.Web.Controllers
{
    public class ExtraController : Controller
    {
        // GET: Extra
        public ActionResult ByLine()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ByLine(string line)
        {
            if (!String.IsNullOrEmpty(line))
            {
                ByLineInfo lineJson = JsonConvert.DeserializeObject<ByLineInfo>(line);

                ExtraWorker extra = new ExtraWorker();
                CodebarResult extraResult = extra.GenerateBarcodeByLine(lineJson.fromLine);

                //+ 47 boleto
                if (extraResult.Line.Length == 47)
                {
                    int bank = Int32.Parse(extraResult.Line.Substring(0, 3));
                    string date = extraResult.Line.Substring(33, 4);
                    string value = extraResult.Line.Substring(37, 10);

                    var _banks = BoletoWorker.GetBanks(Server.MapPath("~/")).Where(x => x.value == bank).FirstOrDefault();
                    lineJson.bank = _banks != null ? _banks.label : "";

                    lineJson.expirate = Utils.GetDateFromFactor(date).ToString("dd/MM/yyyy");
                    lineJson.value = Utils.GetValueFormattedFromFactor(value);
                }
                //+ 48 conta de consumo
                else
                {
                    var l = extraResult.Line;
                    int segment = Int32.Parse(l.Substring(1, 1));
                    string code = l.Substring(16, 4);
                    string value = string.Concat(l.Substring(4, 7), l.Substring(12, 4));

                    ConsumoWorker consumo = new ConsumoWorker(Server.MapPath("~/"));
                    var _dealer = consumo.GetDealerships().Where(x => x.segment == segment && x.code == code).FirstOrDefault();
                    lineJson.dealership = _dealer != null ? _dealer.dealership : "";
                    lineJson.value = Utils.GetValueFormattedFromFactor(value);
                }

                lineJson.line = extraResult.Line;
                lineJson.lineFormatted = extraResult.LineFormatted;
                lineJson.barcodeBase64 = extraResult.BarcodeBase64;

                return Json(JsonConvert.SerializeObject(lineJson));
            }

            return Json(null);
        }

        public ActionResult InvalidBoleto()
        {
            return View();
        }

        public ActionResult InvalidConsumo()
        {
            return View();
        }

        public ActionResult BatchGenBoleto()
        {
            return View();
        }

        public ActionResult BatchGenConsumo()
        {
            return View();
        }
    }
}