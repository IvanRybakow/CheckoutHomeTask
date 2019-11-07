using Checkout.HomeTask.Api.Contracts.v1.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace Checkout.HomeTask.Api.SwaggerExamples
{
    public class PaymentRequestExample : IExamplesProvider<PaymentRequest>
    {
        public PaymentRequest GetExamples()
        {
            return new PaymentRequest
            {
                CardHolderName = "John Doe",
                CardNumber = "1234567891234567",
                Currency = "EUR",
                Cvv = "111",
                ExpireMonth = "12",
                ExpireYear = "2030",
                Amount = 100
            };
        }
    }
}
