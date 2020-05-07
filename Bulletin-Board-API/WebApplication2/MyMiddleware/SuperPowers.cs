using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.MyMiddleware
{
    public class SuperPowers
    {
        private readonly RequestDelegate _next;

        public SuperPowers(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add("X-Power", "Shoot with lazer beams");
                //context.Response.Headers.Add("Access-Control-Expose-Headers", "X-Power");
                //context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                return Task.CompletedTask;
            });
            await _next(context);
        }


    }
}
