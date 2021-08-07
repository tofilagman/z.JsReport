using jsreport.Types;
using Microsoft.AspNetCore.Http;
using System;

namespace z.JsReport
{
    public interface IJsReportFeature
    {
        RenderRequest RenderRequest { get; set; }
        bool Enabled { get; set; }
        IJsReportFeature Configure(Action<RenderRequest> req);
        HttpContext Context { get; set; }
        IJsReportFeature DebugLogsToResponse();
        IJsReportFeature NoBaseTag();
        IJsReportFeature Engine(Engine engine);
        IJsReportFeature Recipe(Recipe recipe);
        Action<Report> AfterRender { get; set; }
        IJsReportFeature OnAfterRender(Action<Report> action);
        IJsReportFeature Base64();
        bool Base64Content { get; }
        IJsReportFeature Landscape();
    }

    public class JsReportFeature : IJsReportFeature
    {
        public JsReportFeature(HttpContext context)
        {
            RenderRequest = new RenderRequest();
            RenderRequest.Template.Engine = jsreport.Types.Engine.None;
            Context = context;
            RenderRequest.Options.Base = $"{Context.Request.Scheme}://{Context.Request.Host}";
            Enabled = true;
            Base64Content = false;
        }

        public RenderRequest RenderRequest { get; set; }
        public bool Enabled { get; set; }
        public HttpContext Context { get; set; }
        public Action<Report> AfterRender { get; set; }
        public bool Base64Content { get; private set; }

        public IJsReportFeature OnAfterRender(Action<Report> action)
        {
            AfterRender = action;
            return this;
        }

        public IJsReportFeature Engine(Engine engine)
        {
            RenderRequest.Template.Engine = engine;
            return this;
        }

        public IJsReportFeature Recipe(Recipe recipe)
        {
            RenderRequest.Template.Recipe = recipe;
            return this;
        }

        public IJsReportFeature NoBaseTag()
        {
            RenderRequest.Options.Base = null;
            return this;
        }

        public IJsReportFeature DebugLogsToResponse()
        {
            RenderRequest.Options.Debug.LogsToResponse = true;
            return this;
        }

        public IJsReportFeature Configure(Action<RenderRequest> req)
        {
            req.Invoke(RenderRequest);
            return this;
        }

        public IJsReportFeature Base64()
        {
            Base64Content = true;
            return this;
        }

        public IJsReportFeature Landscape()
        {
            var chrome = RenderRequest.Template.Chrome ?? new Chrome();
            chrome.Landscape = true;
            RenderRequest.Template.Chrome = chrome;
            return this;
        }
    }
}
