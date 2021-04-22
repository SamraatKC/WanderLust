using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WanderLust.MiddleWare
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CORMiddleware
    {
        private readonly RequestDelegate _next;

        public CORMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
            httpContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type, Accept, Athorization, ActualUserOrImpersonatedUserSamAccount, IsImpersonatedUser" });
            httpContext.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
            if (httpContext.Request.Method == HttpMethod.Options.Method)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                return httpContext.Response.WriteAsync("OK");
            }
            return _next.Invoke(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CORMiddlewareExtensions
    {
        public static IApplicationBuilder UseCORMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CORMiddleware>();
        }
    }
}
