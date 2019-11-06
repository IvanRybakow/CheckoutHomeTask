using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.HomeTask.Api.Domain;

namespace Checkout.HomeTask.Api.Services
{
    public class BankService : IBankService
    {
        private readonly List<CreditCard> Cards = new List<CreditCard>
        {
            new CreditCard { CardNumber = "1234567891234567", Currency = "EUR", Balance = 300, CardHolderName = "Max Muster", Cvv = 111 },
            new CreditCard { CardNumber = "2345678912345678", Currency = "USD", Balance = 3000, CardHolderName = "John Doe", Cvv = 222 },
            new CreditCard { CardNumber = "1234567891234568", Currency = "EUR", Balance = 100, CardHolderName = "Boris Petrov", Cvv = 333 },
            new CreditCard { CardNumber = "1234567891234560", Currency = "USD", Balance = 200, CardHolderName = "Huan Alvarez", Cvv = 444 },
            new CreditCard { CardNumber = "1234567891234569", Currency = "EUR", Balance = 1000000, CardHolderName = "Lionel Messi", Cvv = 555 },
        };

        public async Task<PaymentResult> ProceedPaymentAsync(BankPaymentRequest request)
        {
            return await Task.Factory.StartNew<PaymentResult>(() =>
                {
                    var card = Cards.FirstOrDefault(c =>
                        {
                            return
                            c.CardNumber == request.CardNumber &&
                            c.Currency == request.Currency &&
                            c.Cvv.ToString() == request.Cvv;
                        });
                    if (card != null)
                    {
                        if (card.Balance > request.Amount)
                        {
                            card.Balance -= request.Amount;
                            return new PaymentResult { PaymentId = Guid.NewGuid().ToString(), StatusCode = PaymentStatusCode.Success };
                        }
                    }
                    return new PaymentResult { PaymentId = Guid.Empty.ToString(), StatusCode = PaymentStatusCode.Failed };
                }); 
        }

        private class CreditCard
        {
            public string CardNumber { get; set; }
            public string CardHolderName { get; set; }
            public int Cvv { get; set; }
            public int Balance { get; set; }
            public string Currency { get; set; }
        }
    }
}
