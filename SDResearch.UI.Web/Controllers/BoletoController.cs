using SDResearch.Core.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SDResearch.Core.Entity;

namespace SDResearch.UI.Web.Controllers
{
    public class BoletoController : Controller
    {
        public ActionResult Barcode()
        {

            BoletoWorker bWorker = new BoletoWorker();
            string banks = bWorker.GetBanksJson(Server.MapPath("~/"));
            ViewBag.banks = banks;

            return View();
        }

        [HttpPost]
        public JsonResult GenerateBoleto(string boleto)
        {
            if (!String.IsNullOrEmpty(boleto))
            {
                BoletoInfo boletoJson = JsonConvert.DeserializeObject<BoletoInfo>(boleto);

                BoletoWorker bWorker = new BoletoWorker();

                if (boletoJson.autoGenerate)
                {
                    boletoJson.bankCode = bWorker.RandomBank().ToString();

                    if (boletoJson.valueCheck)
                    {
                        boletoJson.value = bWorker.RandomValue(boletoJson.valueStart, boletoJson.valueEnd);
                    }

                    if (boletoJson.expirateCheck)
                    {
                        boletoJson.expirate = bWorker.RandomDate(boletoJson.expirateStart, boletoJson.expirateEnd).ToString("dd/MM/yyyy");
                    }

                    CodebarResult boletoResult = bWorker.GenerateBoleto(boletoJson.bankCode, boletoJson.expirate, boletoJson.value.ToString());

                    boletoJson.line = boletoResult.Line;
                    boletoJson.lineFormatted = boletoResult.LineFormatted;
                    boletoJson.barcodeBase64 = boletoResult.BarcodeBase64;

                    return Json(JsonConvert.SerializeObject(boletoJson));
                }
                else
                {
                    boletoJson.value = boletoJson.value.Replace(".", ",");
                    Double v;
                    Double.TryParse(boletoJson.value, out v);
                    boletoJson.value = v.ToString("N");

                    CodebarResult boletoResult = bWorker.GenerateBoleto(boletoJson.bankCode, boletoJson.expirate, boletoJson.value.ToString());

                    boletoJson.line = boletoResult.Line;
                    boletoJson.lineFormatted = boletoResult.LineFormatted;
                    boletoJson.barcodeBase64 = boletoResult.BarcodeBase64;

                    return Json(JsonConvert.SerializeObject(boletoJson));

                }

            }

            return Json(new { boleto = boleto });
        }
    }
}