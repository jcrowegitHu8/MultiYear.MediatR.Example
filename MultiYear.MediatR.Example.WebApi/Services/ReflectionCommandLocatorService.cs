using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MultiYear.MediatR.Example.WebApi.Servies
{
    public interface IReflectionRequestLocatorService
    {
        dynamic FindYearSpecificRequest(object request, string yearNumber);
    }

    public class ReflectionCommandLocatorService: IReflectionRequestLocatorService
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
                var asm = Assembly.LoadFile(path);
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
