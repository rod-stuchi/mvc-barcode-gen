
using Newtonsoft.Json;
using SDResearch.Core.Entity;
using SDResearch.Core.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SDResearch.UI.Web.Controllers
{
    public class ContaConsumoController : Controller
    {
        public ActionResult Barcode()
        {
            ConsumoWorker cWorker = new ConsumoWorker(Server.MapPath("~/"));

            var dealerships = cWorker.GetDealerships();
            var dealershipJson = dealerships.Select(x => new { value = String.Concat(x.segment, x.code), label = x.dealership }).ToArray();
            ViewBag.dealerships = JsonConvert.SerializeObject(dealershipJson);

            return View();
        }


        [HttpPost]
        public JsonResult GenerateConsumo(string consumo)
        {
            if (!String.IsNullOrEmpty(consumo))
            {
                ConsumoInfo consumoJson = JsonConvert.DeserializeObject<ConsumoInfo>(consumo);

                ConsumoWorker cWorker = new ConsumoWorker(Server.MapPath("~/"));

                if (consumoJson.autoGenerate)
                {
                    DealershipInfo dealership = cWorker.RandomDealership();
                    consumoJson.segment = dealership.segment;
                    consumoJson.code = dealership.code;

                    consumoJson.value = cWorker.RandomValue(consumoJson.valueStart, consumoJson.valueEnd);

                    CodebarResult consumoResult = cWorker.GenerateConsumo(consumoJson.segment, consumoJson.code, consumoJson.value.ToString());

                    consumoJson.line = consumoResult.Line;
                    consumoJson.lineFormatted = consumoResult.LineFormatted;
                    consumoJson.barcodeBase64 = consumoResult.BarcodeBase64;

                    return Json(JsonConvert.SerializeObject(consumoJson));
                }
                else
                {
                    consumoJson.value = consumoJson.value.Replace(".", ",");
                    Double v;
                    Double.TryParse(consumoJson.value, out v);
                    consumoJson.value = v.ToString("N");

                    CodebarResult consumoResult = cWorker.GenerateConsumo(consumoJson.segment, consumoJson.code, consumoJson.value.ToString());

                    consumoJson.line = consumoResult.Line;
                    consumoJson.lineFormatted = consumoResult.LineFormatted;
                    consumoJson.barcodeBase64 = consumoResult.BarcodeBase64;

                    return Json(JsonConvert.SerializeObject(consumoJson));
                }

            }

            return Json(new { consumo = consumo });
        }
    }
}