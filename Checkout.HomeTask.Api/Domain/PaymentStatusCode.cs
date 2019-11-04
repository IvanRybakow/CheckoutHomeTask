using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.HomeTask.Api.Domain
{
    public enum PaymentStatusCode
    {
        Success,
        CardIsInvalid,
        NotEnoughMoney,
        CardDoesNotExist
    }
}
