using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.HomeTask.Api.Contracts.v1.Responses
{
    public class ErrorModel
    {
        public string ErrorName { get; set; }
        public string Message { get; set; }
    }
}
