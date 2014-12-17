using System.Web;
using System.Web.Optimization;

namespace TRMAudiostem
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/cloudplayer/css").Include(
                        "~/Content/themes/cloudPlayer/css/cloudplayer.css",
                        "~/Content/themes/smartticker/css/jquery.smarticker.min.css"));

            bundles.Add(new StyleBundle("~/content/trm/css").Include(
                        "~/Content/themes/trm/trm.css",
                        "~/Content/themes/trm/jquery.ui.core.css",
                        "~/Content/themes/trm/jquery.ui.resizable.css",
                        "~/Content/themes/trm/jquery.ui.selectable.css",
                        "~/Content/themes/trm/jquery.ui.accordion.css",
                        "~/Content/themes/trm/jquery.ui.autocomplete.css",
                        "~/Content/themes/trm/jquery.ui.button.css",
                        "~/Content/themes/trm/jquery.ui.dialog.css",
                        "~/Content/themes/trm/jquery.ui.slider.css",
                        "~/Content/themes/trm/jquery.ui.tabs.css",
                        "~/Content/themes/trm/jquery.ui.datepicker.css",
                        "~/Content/themes/trm/jquery.ui.progressbar.css",
                        "~/Content/themes/trm/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/content/mobile/css").Include(
                        "~/Content/themes/mobile/mobile.css",
                        "~/Content/themes/mobile/jquery.percentageloader-0.1.css"));

            bundles.Add(new StyleBundle("~/content/trm/new/css").Include(
                        "~/Content/themes/trm/reg_website/base_style.css",
                        "~/Content/themes/trm/reg_website/css/bootstrap-theme.css",
                        "~/Content/themes/trm/reg_website/css/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundles/trm/scripts").Include(
                        "~/Content/themes/trm/scripts/account.js",
                        "~/Content/themes/trm/scripts/album.js",
                        "~/Content/themes/trm/scripts/song.js",
                        "~/Content/themes/trm/scripts/personaldetails.js",
                        "~/Content/themes/trm/scripts/password.js",
                        "~/Content/themes/trm/scripts/admin.js"));

            bundles.Add(new ScriptBundle("~/bundles/trm/new/scripts").Include(
                        "~/Content/themes/trm/reg_website/js/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/content/cloudplayerv2/css").Include(
                        "~/Content/themes/cloudPlayer/css/normalize.css",
                        "~/Content/themes/cloudPlayer/css/font-awesome.min.css",
                        "~/Content/themes/cloudPlayer/css/bootstrap.min.css",
                        "~/Content/themes/cloudPlayer/css/bootstrap-theme.min.css",
                        "~/Content/themes/cloudPlayer/css/demo.css",
                        "~/Content/themes/cloudPlayer/css/component.css",
                        "~/Content/themes/cloudPlayer/css/context.css",
                        "~/Content/themes/cloudPlayer/css/jquery.jscrollpane.css",
                        "~/Content/themes/cloudPlayer/js/jquery.contextMenu.css",
                        "~/Content/themes/cloudPlayer/css/cloudplayer.css"));

            bundles.Add(new ScriptBundle("~/bundles/cloudplayer/scripts").Include(
                        "~/Content/themes/cloudPlayer/js/cloudplayer.js"));

            bundles.Add(new ScriptBundle("~/bundles/cloudplayerv2/scripts").Include(
                        "~/Content/themes/cloudPlayer/js/modernizr.custom.js",
                        "~/Content/themes/cloudPlayer/js/classie.js",
                        "~/Content/themes/cloudPlayer/js/mlpushmenu.js",
                        "~/Content/themes/cloudPlayer/js/jquery-1.10.2.min.js",
                        "~/Content/themes/cloudPlayer/js/jquery-ui-1.9.2-min.js",
                        "~/Content/themes/cloudPlayer/js/jquery.parallax.js",
                        "~/Content/themes/cloudPlayer/js/jquery.drag.js",
                        "~/Content/themes/cloudPlayer/js/jquery.event.destroyed.js",
                        "~/Content/themes/cloudPlayer/js/jquery.contextMenu.js",
                        "~/Content/themes/cloudPlayer/js/jquery.jscrollpane.min.js",
                        "~/Content/themes/cloudPlayer/js/jquery.mousewheel.js",
                        "~/Content/themes/cloudPlayer/js/bootstrap.min.js",
                        "~/Content/themes/cloudPlayer/js/plugins/affix.js",
                        "~/Content/themes/cloudPlayer/js/plugins/alert.js",
                        "~/Content/themes/cloudPlayer/js/plugins/button.js",
                        "~/Content/themes/cloudPlayer/js/plugins/carousel.js",
                        "~/Content/themes/cloudPlayer/js/plugins/collapse.js",
                        "~/Content/themes/cloudPlayer/js/plugins/dropdown.js",
                        "~/Content/themes/cloudPlayer/js/plugins/modal.js",
                        "~/Content/themes/cloudPlayer/js/plugins/popover.js",
                        "~/Content/themes/cloudPlayer/js/plugins/scrollspy.js",
                        "~/Content/themes/cloudPlayer/js/plugins/tab.js",
                        "~/Content/themes/cloudPlayer/js/plugins/tooltip.js",
                        "~/Content/themes/cloudPlayer/js/plugins/transition.js",
                        "~/Content/themes/cloudPlayer/js/core.js",
                        "~/Content/themes/cloudPlayer/js/audio-core/jquery.jplayer.min.js",
                        "~/Content/themes/cloudPlayer/js/audio-core/jplayer.playlist.min.js"));
        }
    }
}