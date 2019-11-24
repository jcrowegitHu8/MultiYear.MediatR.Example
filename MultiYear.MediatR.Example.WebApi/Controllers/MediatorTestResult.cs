using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiYear.MediatR.Example.WebApi.Controllers
{
    public class MediatorTestResult
    {
        public string CoreInstance { get; set; }
        public string Year1Instance { get; set; }
        public string Year1ReflectionInstance { get; set; }
    }
}
