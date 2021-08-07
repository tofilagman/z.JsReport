using Microsoft.AspNetCore.Builder;

namespace z.JsReport
{
    public class JsReportPipeline
    {
        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseJsReport();
        }
    }
}
