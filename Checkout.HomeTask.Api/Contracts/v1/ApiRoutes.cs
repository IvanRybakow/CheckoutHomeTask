using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.HomeTask.Api.Contracts.v1
{
    public static class ApiRoutes
    {
        private const string root = "api/";
        private const string version = "v1";
        private const string baseUrl = root + version;

        public static class Payment
        {
            public const string AddPayment = baseUrl + "/payment";
        }
    }
}
