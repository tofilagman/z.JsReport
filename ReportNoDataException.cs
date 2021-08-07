using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.JsReport
{
    public class ReportNoDataException : Exception
    {
        public ReportNoDataException() : base("Report doesn't contain any data") { }
    }
}
