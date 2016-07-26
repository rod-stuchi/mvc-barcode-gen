using Newtonsoft.Json;
using SDResearch.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SDResearch.UI.Web.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMail(FormCollection frm)
        {

            // Response.Redirect("/About/Index");
            //! this.RedirectToAction("/About/Index");
            //return redi
            return RedirectToRoute(new { controller = "About", action = "Index" });
        }

        [HttpPost]
        [ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult ValidateCaptcha(string dataToken)
        {

            try
            {
                //AntiForgery.Validate(Request.Cookies[AntiForgeryConfig.CookieName].Value, Request.Headers["__RequestVerificationToken"]);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6LdrXwkTAAAAALd-B-IipgpHQNjSbri6ioduA2Zt&response=" + dataToken);
                using (WebResponse wResponse = req.GetResponse())
                {

                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        GoogleCaptcha captchaResponse = JsonConvert.DeserializeObject<GoogleCaptcha>(jsonResponse);

                        //if(captchaResponse.success)
                        //{
                        //    Response.StatusCode = 200;
                        //}

                        return Json(captchaResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                //logar
                return Json(new GoogleCaptcha { success = false });
            }
        }
    }

    


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateAntiForgeryTokenOnAllPosts : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            //  Only validate POSTs
            if (request.HttpMethod == WebRequestMethods.Http.Post)
            {
                //  Ajax POSTs and normal form posts have to be treated differently when it comes
                //  to validating the AntiForgeryToken
                if (request.IsAjaxRequest())
                {
                    var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];

                    var cookieValue = antiForgeryCookie != null ? antiForgeryCookie.Value : null;

                    try
                    {
                        AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"]);
                    }
                    catch (Exception)
                    {
                        base.HandleUnauthorizedRequest(filterContext);
                    }
                }
                else
                {
                    new ValidateAntiForgeryTokenAttribute().OnAuthorization(filterContext);
                }
            }
        }

    }

}


