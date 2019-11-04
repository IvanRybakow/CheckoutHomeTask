using System;
using System.Threading.Tasks;
using Checkout.HomeTask.Api.Domain;

namespace Checkout.HomeTask.Api.Services
{
    public class MockBankService : IBankService
    {

        public async Task<PaymentResult> ProceedPaymentAsync(BankPaymentRequest request)
        {

            return await Task.Factory.StartNew<PaymentResult>(() => new PaymentResult
                                                                {
                                                                    PaymentId = Guid.NewGuid().ToString(),
                                                                    StatusCode = PaymentStatusCode.Success
                                                                }); 
        }
    }
}
