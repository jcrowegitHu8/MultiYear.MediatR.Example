using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.Core.Application
{
    public class DoSomething1Request : IRequest<string>
    {
    }

    public class DoSomething1RequestHandler : IRequestHandler<DoSomething1Request, string>
    {
        public Task<string> Handle(DoSomething1Request request, CancellationToken cancellationToken)
        {
            return Task.FromResult("Core Handler Executed");
        }
    }
}
