using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;

namespace Mandalium.API.App_Code.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            try
            {
                string lang = httpContext.Request.Headers["lang"];

                CultureInfo.CurrentUICulture = new CultureInfo(lang);
            }
            catch
            {
                CultureInfo.CurrentUICulture = new CultureInfo("tr-TR");
            }
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RequestCultureExtensions
    {
        public static IApplicationBuilder UseRequestCultureMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCultureMiddleware>();
        }
    }
}
