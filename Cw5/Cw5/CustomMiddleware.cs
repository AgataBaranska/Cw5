using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Cw5
{


    public class CustomMiddleware
    {

        private RequestDelegate _next;

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {

            context.Response.Headers.Add("Cos", "123");


            await _next.Invoke(context);//odpalam kolejny middleware


        }

    }
}
