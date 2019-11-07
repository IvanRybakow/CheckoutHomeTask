using Checkout.HomeTask.Api.Contracts.v1.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace Checkout.HomeTask.Api.SwaggerExamples
{
    public class MerchantAuthRequestExample : IExamplesProvider<MerchantAuthRequest>
    {
        public MerchantAuthRequest GetExamples()
        {
            return new MerchantAuthRequest
            {
                Email = "example@mail.com",
                Password = "Qwerty1!"
            };
        }
    }
}
