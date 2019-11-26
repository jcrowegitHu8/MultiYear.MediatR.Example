using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace MultiYear.MediatR.Example.WebApi.Servies
{
    public interface IReflectionRequestLocatorService
    {
        dynamic FindYearSpecificRequest(object request, string yearNumber);
    }

    public class ReflectionRequestLocatorService: IReflectionRequestLocatorService
    {
        public dynamic FindYearSpecificRequest(object request, string yearNumber)
        {
            try
            {
                var requestName = request.GetType().Name;

                var library = $"BusinessLogic.Year{yearNumber}.Application." + requestName;
                var dll = $"BusinessLogic.Year{yearNumber}.dll";

                var dir = AppDomain.CurrentDomain.BaseDirectory;
                var path = Path.Combine(dir, dll);
                //Net Core uses AssemblyLoadContext because full app domains are not supported.
                //https://docs.microsoft.com/en-us/dotnet/api/system.runtime.loader.assemblyloadcontext?view=netcore-2.2
                var asmContext = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly());
                var asm = asmContext.LoadFromAssemblyPath(path);
                var type = asm.GetType(library);

                var result = Activator.CreateInstance(type);

                return result;

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
