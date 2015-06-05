using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace PontoRemoto.Web.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class InspiniaExtensions
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null)
        {
            const string cssClass = "active";
            var currentAction = (string)html.ViewContext.RouteData.Values["action"];
            var currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (string.IsNullOrEmpty(controller))
                controller = currentController;

            if (string.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ? cssClass : string.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            var currentAction = (string)html.ViewContext.RouteData.Values["action"];

            return currentAction;
        }
	}
}
