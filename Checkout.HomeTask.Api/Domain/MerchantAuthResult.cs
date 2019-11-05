using Checkout.HomeTask.Api.Contracts.v1.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.HomeTask.Api.Domain
{
    public class MerchantAuthResult
    {
        public bool IsSuccessfull { get; set; }
        public IEnumerable<ErrorModel> Errors { get; set; }
        public string Token { get; set; }
    }
}
