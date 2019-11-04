using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.HomeTask.Api.Domain
{
    public class PaymentResult
    {
        public string PaymentId { get; set; }
        public PaymentStatusCode StatusCode { get; set; }
    }
}
