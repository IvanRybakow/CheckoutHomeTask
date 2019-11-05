using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Checkout.HomeTask.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetMerchantId(this HttpContext context)
        {
            if (context.User == null)
            {
                return String.Empty;
            }
            return context.User.Claims.Single(c => c.Type == "Id").Value;
        }
    }
}
