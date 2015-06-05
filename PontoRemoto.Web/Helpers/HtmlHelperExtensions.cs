using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Web.Mvc;

namespace PontoRemoto.Web.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString StatusMessage(this HtmlHelper helper, int defaultSeconds)
        {
            var milliseconds = ConfigurationManager.AppSettings["MillisecondsToShowStatusMessage"] ?? defaultSeconds.ToString(CultureInfo.InvariantCulture);
            var message = helper.ViewContext.TempData["StatusMessageText"];
            var type = helper.ViewContext.TempData["StatusMessageType"];

            var html = string.Format(@"
            <script type='text/javascript'>
                $(document).ready(function() {{
                    toastr.options = {{
                        'positionClass': 'toast-top-center',
                        'progressBar': true,
                        'timeOut': '{0}',
                        'extendedTimeOut': '1000',
                    }};

                    showStatusMessage('{1}', '{2}');
                }});

                function showStatusMessage(message, type) {{
                    if (message && message != '') {{
                        if (type = 'success') {{
                            toastr.success(message);
                        }} else if (type = 'info') {{
                            toastr.info(message);
                        }} else if (type = 'warning') {{
                            toastr.warning(message);
                        }} else if (type = 'error') {{
                            toastr.error(message);
                        }}
                    }}
                }}
            </script>
            ", milliseconds, message, type);

            return new MvcHtmlString(html);
        }
    }
}