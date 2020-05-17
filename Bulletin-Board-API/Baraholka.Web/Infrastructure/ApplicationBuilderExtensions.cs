using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace Baraholka.Web.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseExceptionHandling(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(config => config.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var erMessage = context.Features.Get<IExceptionHandlerFeature>().Error.Message;
                    await context.Response.WriteAsync(erMessage);
                }));
            }
        }
    }
}