using System.Web;
using System.Web.Optimization;

namespace AdminCode
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Script/layui").Include(
                        "~/layui/layui.js",
                        "~/js/BusinessExtenssion.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/layui").Include(
                      "~/layui/css/layui.css",
                      "~/layui/font/iconfont.css"
                      ));
        }
    }
}
