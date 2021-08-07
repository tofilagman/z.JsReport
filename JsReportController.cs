using z.JsReport.Model;
using jsreport.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace z.JsReport
{
    public abstract class JsReportController<TReportParameter> : Controller where TReportParameter : class
    {
        protected IJsReportFeature JsFeature { get; private set; }
         
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            JsFeature = Configure();
            base.OnActionExecuting(context); 
        }

        // the normal jsreport base url injection into the html doesn't work properly with docker and asp.net because of port mapping
        // the project typically starts with some http://localhost:1234 url but inside docker the url is http://localhost
        private IJsReportFeature Configure(string uriBase = "http://localhost")
        {
            return HttpContext.JsReportFeature()
                 .Configure((req) => req.Options.Base = uriBase)
                 .Recipe(Recipe.ChromePdf)
                 .Base64();
        }

        public async Task<ReportDataModel<TData, TReportParameter>> PostProcess<TData>(List<TData> data)
        {
            if (data.Count == 0)
                throw new ReportNoDataException();
             
            return new ReportDataModel<TData, TReportParameter>
            {
                ResultSet = data,
                Parameters = await BuildReportParameter()
            };
        }

        public abstract Task<TReportParameter> BuildReportParameter();

    }
}
