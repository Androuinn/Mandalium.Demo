using Mandalium.Core.Abstractions.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace App_Code.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RequestAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUnitOfWork _unitOfWork;

        public RequestAuthenticationMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
            _unitOfWork = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>();

        }

        public Task Invoke(HttpContext httpContext)
        {



#if DEBUG
            // do nothing
#else
            string apikey = httpContext.Request.Headers["ApiKey"];

            if (string.IsNullOrEmpty(apikey) || _unitOfWork.GetRepository<SystemAuthenticationKey>().Get("ApiKey").Result.Value != Utility.ReplaceCharactersWithWhiteSpace(new string[] { "{", "}" }, apikey))
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                httpContext.Response.WriteAsync(LanguageResource.Api_Authorization_Failed);
                return Task.Run(() => { return httpContext; });
            }
#endif
            return _next(httpContext);

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RequestAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestAuthenticationMiddleware>();
        }
    }
}
