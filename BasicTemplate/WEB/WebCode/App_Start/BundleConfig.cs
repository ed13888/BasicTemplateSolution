using System.Web;
using System.Web.Optimization;

namespace WebCode
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;
            //cdn
            bundles.UseCdn = true;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.signalR-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/layer").Include(
                      "~/Plugins/layer/layer.js",
                      "~/Plugins/layer/mobile/layer.js"));


            bundles.Add(new ScriptBundle("~/bundles/fileinput").Include(
                      "~/Plugins/bootstrap-fileinput/js/fileinput.js",
                      "~/Plugins/bootstrap-fileinput/js/locales/zh.js"));


            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                      "~/Scripts/site.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            bundles.Add(new StyleBundle("~/Plugins/layer/css").Include(
                      "~/Plugins/layer/theme/default/layer.css",
                      "~/Plugins/layer/mobile/need/layer.css"));


            bundles.Add(new StyleBundle("~/Plugins/fileinput/css").Include(
                      "~/Plugins/bootstrap-fileinput/css/fileinput.css"));



        }
    }
}
