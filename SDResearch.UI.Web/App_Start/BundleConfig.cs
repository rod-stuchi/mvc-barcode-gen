using System.Collections.Generic;
using System.Web.Optimization;

namespace SDResearch.UI.Web.App_Start
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            AsIsBundleOrderer orderFiles = new AsIsBundleOrderer();
            Bundle vendorCss = new Bundle("~/bundles/vendor-css");
            vendorCss.Orderer = orderFiles;
            vendorCss
                .Include("~/Static/jquery/jquery-ui/jquery-ui.css")
                .Include("~/Static/jquery/jquery-ui/jquery-ui.structure.css")
                .Include("~/Static/jquery/jquery-ui/jquery-ui.theme.css")
                .Include("~/Static/bootstrap/css/bootstrap.css")
                .Include("~/Static/bootstrap/css/bootstrap.theme.css");


            Bundle AppBaseCss = new Bundle("~/bundles/app-base-css");
            AppBaseCss.Orderer = orderFiles;
            AppBaseCss
                .Include("~/Static/app/base.css");


            Bundle vendorJs = new Bundle("~/bundles/vendor-js");
            vendorJs.Orderer = orderFiles;
            vendorJs
                .Include("~/Static/jquery/jquery-{version}.js")
                .Include("~/Static/jquery/jquery-ui/jquery-ui.js")
                .Include("~/Static/bootstrap/js/bootstrap.js");


            Bundle AppBaseJs = new Bundle("~/bundles/app-base-js");
            AppBaseJs.Orderer = orderFiles;
            AppBaseJs
                .Include("~/Static/app/base.js");


            bundles.Add(vendorCss);
            bundles.Add(AppBaseCss);
            bundles.Add(vendorJs);
            bundles.Add(AppBaseJs);

            bundles.Add(RegBarcodeCommon());
            bundles.Add(RegBoletoBundle());
            bundles.Add(RegConsumoBundle());
            bundles.Add(RegExtraByLine());
        }

        private static Bundle RegBarcodeCommon()
        {
            Bundle barcodeCommonJs = new Bundle("~/bundles/barcode-common-js");
            barcodeCommonJs.Orderer = new AsIsBundleOrderer();
            barcodeCommonJs
                .Include("~/Static/plugins/ZeroClipboard.min.js")
                .Include("~/Static/plugins/jquery.hotkeys.js")
                .Include("~/Static/app/barcode-common.min.js")
                ;

            return barcodeCommonJs;
        }

        private static Bundle RegBoletoBundle()
        {
            Bundle boletoJs = new Bundle("~/bundles/boleto-js");
            boletoJs.Orderer = new AsIsBundleOrderer();
            boletoJs
                .Include("~/Static/plugins/jquery.maskMoney.min.js")
                .Include("~/Static/plugins/moment.min.js")
                .Include("~/Static/plugins/jquery.maskedinput.min.js")
                .Include("~/Static/plugins/datepicker-pt-BR.js")
                .Include("~/Static/plugins/bootstrap-toggle.min.js")
                .Include("~/Static/app/boleto.min.js");

            return boletoJs;
        }

        private static Bundle RegConsumoBundle()
        {
            Bundle consumoJs = new Bundle("~/bundles/consumo-js");
            consumoJs.Orderer = new AsIsBundleOrderer();
            consumoJs
                .Include("~/Static/plugins/jquery.maskMoney.min.js")
                .Include("~/Static/plugins/bootstrap-toggle.min.js")
                .Include("~/Static/app/consumo.min.js");

            return consumoJs;
        }

        private static Bundle RegExtraByLine()
        {
            Bundle extraByLineJs = new Bundle("~/bundles/extra-byline-js");
            extraByLineJs.Orderer = new AsIsBundleOrderer();
            extraByLineJs
                .Include("~/Static/app/extra-byline.min.js")
                ;

            return extraByLineJs;
        }
    }

    public class AsIsBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}