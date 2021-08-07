using Microsoft.AspNetCore.Builder;

namespace z.JsReport
{
    public static class JsReportBuilderExtensions
    {
        public static IApplicationBuilder UseJsReport(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JsReportMiddleware>();
        }
    }
}
