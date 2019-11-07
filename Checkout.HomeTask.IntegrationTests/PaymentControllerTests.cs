using Checkout.HomeTask.Api.Contracts.v1;
using Checkout.HomeTask.Api.Contracts.v1.Requests;
using Checkout.HomeTask.Api.Contracts.v1.Responses;
using Checkout.HomeTask.Api.Settings;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Checkout.HomeTask.IntegrationTests
{
    public class PaymentControllerTests : IntegrationTest
    {
        [Fact]
        public async Task ProceedPayment_CantProceedWithoutAuth()
        {
            //Arrange
            var request = new PaymentRequest
                {
                    CardHolderName = "John Doe",
                    CardNumber = "1234567891234567",
                    Currency = "EUR",
                    Cvv = "111",
                    ExpireMonth = "11",
                    ExpireYear = "2020",
                    Amount = 200
                };

            //Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Payment.AddPayment, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetPayments_CantProceedWithoutAuth()
        {
            //Arrange

            //Act
            var response = await TestClient.GetAsync(ApiRoutes.Payment.GetAllPayments);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ProceedPayment_CantProceedWithIncorrectCardData()
        {
            //Arrange
            var request = new PaymentRequest
            {
                CardHolderName = "John Doe",
                CardNumber = "123456789123456",
                Currency = "EU",
                Cvv = "11",
                ExpireMonth = "20",
                ExpireYear = "2011",
                Amount = 0
            };
            await AuthenticateAsync("test@integration.com", "Test111!");

            //Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Payment.AddPayment, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errors = await response.Content.ReadAsAsync<ErrorResponse>();
            errors.Errors.Should().ContainEquivalentOf(new ErrorModel { ErrorName = "CardNumber", Message = Constants.ValidationErrors.CardNumber })
                .And.ContainEquivalentOf(new ErrorModel { ErrorName = "Currency", Message = Constants.ValidationErrors.Currency })
                .And.ContainEquivalentOf(new ErrorModel { ErrorName = "Cvv", Message = Constants.ValidationErrors.Cvv })
                .And.ContainEquivalentOf(new ErrorModel { ErrorName = "ExpireMonth", Message = Constants.ValidationErrors.Month })
                .And.ContainEquivalentOf(new ErrorModel { ErrorName = "ExpireYear", Message = Constants.ValidationErrors.Year })
                .And.ContainEquivalentOf(new ErrorModel { ErrorName = "Amount", Message = Constants.ValidationErrors.Amount });
        }

        [Fact]
        public async Task ProceedPayment_CanCreatePayment()
        {
            //Arrange
            var request = new PaymentRequest
            {
                CardHolderName = "John Doe",
                CardNumber = "1234567891234567",
                Currency = "EUR",
                Cvv = "111",
                ExpireMonth = "12",
                ExpireYear = "2021",
                Amount = 100
            };
            var responseShouldBe = new PaymentResponse
            {
                Amount = 100,
                CardHolderName = "John Doe",
                Currency = "EUR",
                MaskedCardNumber = "XXXXXXXXXXXX4567"
            };
            await AuthenticateAsync("test@integration.com", "Test111!");

            //Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Payment.AddPayment, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var payment = await response.Content.ReadAsAsync<PaymentResponse>();
            payment.Should().BeEquivalentTo(responseShouldBe);
        }

        [Fact]
        public async Task GetPayments_ReturnsCorrectNumberOfPayments()
        {
            //Arrange
            var request = new PaymentRequest
            {
                CardHolderName = "John Doe",
                CardNumber = "1234567891234567",
                Currency = "EUR",
                Cvv = "111",
                ExpireMonth = "12",
                ExpireYear = "2021",
                Amount = 100
            };

            //get token as first user
            await AuthenticateAsync("test@integration.com", "Test111!");

            //create payments as first user
            for (int i = 0; i < 3; i++)
            {
                await TestClient.PostAsJsonAsync(ApiRoutes.Payment.AddPayment, request);
            }

            //get token as second user
            await AuthenticateAsync("test2@integration.com", "Test111!");

            //create payments as second user
            for (int i = 0; i < 3; i++)
            {
                await TestClient.PostAsJsonAsync(ApiRoutes.Payment.AddPayment, request);
            }
            //Act
            var response = await TestClient.GetAsync(ApiRoutes.Payment.GetAllPayments);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var payments = await response.Content.ReadAsAsync<List<PaymentResponse>>();
            payments.Should().HaveCount(3);
        }

    }
}
