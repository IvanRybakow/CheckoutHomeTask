using Checkout.HomeTask.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Checkout.HomeTask.Api.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Checkout.HomeTask.Api.Contracts.v1.Responses;
using Checkout.HomeTask.Api.Contracts.v1.Requests;
using Checkout.HomeTask.Api.Contracts.v1;

namespace Checkout.HomeTask.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;

        public IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder => 
                builder.ConfigureServices(services =>
                {
                    //replace database with in-memory database
                    services.RemoveAll(typeof(CheckoutDbContext));
                    services.AddDbContext<CheckoutDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });
                }));
            TestClient = appFactory.CreateClient();
        }
        protected async Task AuthenticateAsync(string email, string pass)
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync(email, pass));
        }

        private async Task<string> GetJwtAsync(string email, string pass)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Register, new MerchantAuthRequest
            {
                Email = email,
                Password = pass
            });

            var registrationResponse = await response.Content.ReadAsAsync<MerchantAuthSuccessResponse>();
            return registrationResponse.Token;
        }
    }
}
