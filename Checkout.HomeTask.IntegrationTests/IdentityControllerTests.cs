using Checkout.HomeTask.Api.Contracts.v1;
using Checkout.HomeTask.Api.Contracts.v1.Requests;
using Checkout.HomeTask.Api.Contracts.v1.Responses;
using FluentAssertions;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Checkout.HomeTask.IntegrationTests
{
    public class IdentityControllerTests : IntegrationTest
    {
        [Fact]
        public async Task Register_CantRegisterWithWrongEmail()
        {
            //Arrange
            var request = new MerchantAuthRequest { Email = "Wrong Email", Password = "Qwerty1!" };

            //Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Register, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errors = await response.Content.ReadAsAsync<ErrorResponse>();
            errors.Errors.Select(e => e.ErrorName == "Email").Should().NotBeEmpty();
        }

        [Fact]
        public async Task Register_CantRegisterWithWeakPassword()
        {
            //Arrange
            var request = new MerchantAuthRequest { Email = "email@example.com", Password = "toosimple" };

            //Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Register, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_CanRegisterWithCorrectCredentials()
        {
            //Arrange
            var request = new MerchantAuthRequest { Email = "email@example.com", Password = "Qwerty1!" };

            //Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Register, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadAsAsync<MerchantAuthSuccessResponse>();
            result.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Register_CantRegisterWithRepeatedEmail()
        {
            //Arrange
            var request = new MerchantAuthRequest { Email = "email2@example.com", Password = "Qwerty1!" };

            //Act
            var firstResponse = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Register, request);
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Register, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errors = await response.Content.ReadAsAsync<ErrorResponse>();
            errors.Errors.Select(e => e.ErrorName == "Email exists").Should().NotBeEmpty();
        }

        [Fact]
        public async Task Login_CantLoginWithWrongCredentials()
        {
            //Arrange
            var request = new MerchantAuthRequest { Email = "new@example.com", Password = "Qwerty1!" };

            //Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Login, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errors = await response.Content.ReadAsAsync<ErrorResponse>();
            errors.Errors.Select(e => e.ErrorName == "Wrong credentials").Should().NotBeEmpty();
        }

        [Fact]
        public async Task Login_CanLoginWithCorrectCredentials()
        {
            //Arrange
            var request = new MerchantAuthRequest { Email = "correctemail@example.com", Password = "Qwerty1!" };
            var registerResponse = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Register, request);

            //Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Login, request);

            //Assert
            registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadAsAsync<MerchantAuthSuccessResponse>();
            result.Token.Should().NotBeNullOrEmpty();
        }
    }
}
