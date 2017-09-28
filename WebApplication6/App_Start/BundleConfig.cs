using System.Web;
using System.Web.Optimization;

namespace WebApplication6
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/Content/Login/css").Include(
                "~/Content/Login.css"));
            bundles.Add(new Bundle("~/Content/Admin/css").Include(
                "~/Content/Admin.css"));
            bundles.Add(new Bundle("~/Content/Reader/css").Include(
                "~/Content/Reader.css"));
            bundles.Add(new Bundle("~/Content/Tables/css").Include(
                "~/Content/Tables.css"));

        }
    }
}
