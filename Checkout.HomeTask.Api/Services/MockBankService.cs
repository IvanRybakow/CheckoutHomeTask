using System;
using System.Threading.Tasks;
using Checkout.HomeTask.Api.Domain;

namespace Checkout.HomeTask.Api.Services
{
    public class MockBankService : IBankService
    {

        public Task<PaymentResult> ProceedPaymentAsync(string CardNumber, string CardHolderName, string ExpireMonth, string ExpireYear, int Amount, string cvv, string MerchantAccountNumber)
        {
            throw new NotImplementedException();
        }
    }
}
