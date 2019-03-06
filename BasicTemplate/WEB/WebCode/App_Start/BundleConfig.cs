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

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/layer").Include(
                      "~/Plugins/layer/layer.js",
                      "~/Plugins/layer/mobile/layer.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            bundles.Add(new StyleBundle("~/Plugins/layer/css").Include(
                      "~/Plugins/layer/theme/default/layer.css",
                      "~/Plugins/layer/mobile/need/layer.css"));


        }
    }
}
