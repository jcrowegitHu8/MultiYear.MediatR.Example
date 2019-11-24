using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.Year1.Application
{
    public class DoSomething1Request : IRequest<string>
    {
    }
    //MediatR.IRequestHandler`2[BusinessLogic.Year1.Application.DoSomething1Request,System.String]
    public class DoSomething1RequestHandler : IRequestHandler<DoSomething1Request, string>
    {
        public Task<string> Handle(DoSomething1Request request, CancellationToken cancellationToken)
        {
            return Task.FromResult("Year1 Handler Executed");
        }
    }
}
