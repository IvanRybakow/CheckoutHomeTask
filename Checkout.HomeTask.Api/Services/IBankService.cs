using Checkout.HomeTask.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.HomeTask.Api.Services
{
    public interface IBankService
    {
        Task<PaymentResult> ProceedPaymentAsync(BankPaymentRequest request);
    }
}
