using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using core = BusinessLogic.Core;
using y1 = BusinessLogic.Year1;
using MultiYear.MediatR.Example.WebApi.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace MultiYear.MediatR.Example.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IMediator _mediatR;
        private readonly IReflectionRequestLocatorService _requestLocator;

        public TestController(IMediator mediatR,
            IReflectionRequestLocatorService requestLocator)
        {
            _mediatR = mediatR;
            _requestLocator = requestLocator;
        }

        /// <summary>
        /// Showing issue with MediatR failing to resolve requested created by reflection.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("mediatR")]
        public async Task<ActionResult<MediatorTestResult>> TestReflection()
        {
            var result = new MediatorTestResult();
            var coreRequest = new core.Application.DoSomething1Request();
            try
            {
                //Show that core request hits core handler.
                var coreExample1 = await _mediatR.Send(coreRequest);
                result.CoreInstance = coreExample1;
            }
            catch (Exception e1)
            {
                result.CoreInstance = e1.Message;
            }

            try
            {
                //Show that year1 request hits year1 handler
                var y1Example = await _mediatR.Send(new y1.Application.DoSomething1Request());
                result.Year1Instance = y1Example;
            }
            catch (Exception e2)
            {
                result.Year1Instance = e2.Message;
            }

            try
            {
                //show that year1 request created by reflection works!
                var yearSpecificRequest = _requestLocator.FindYearSpecificRequest(coreRequest, "1");
                var reflectionResult = await _mediatR.Send(yearSpecificRequest);
                result.Year1ReflectionInstance = reflectionResult;
            }
            catch (Exception e3)
            {
                result.Year1ReflectionInstance = e3.Message;
            }


            return Ok(result);
        }

        /// <summary>
        /// See that the api is up and processing
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("health")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> Get()
        {
            return Ok("Ping Success. The MultiYear.MediatR.Example API is working.");
        }
    }
}
