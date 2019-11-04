using System.Threading.Tasks;
using AutoMapper;
using Checkout.HomeTask.Api.Contracts.v1;
using Checkout.HomeTask.Api.Contracts.v1.Requests;
using Checkout.HomeTask.Api.Data;
using Checkout.HomeTask.Api.Domain;
using Checkout.HomeTask.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.HomeTask.Api.Controllers.v1
{
    public class PaymentController : ControllerBase
    {
        private readonly CheckoutDbContext dbContext;
        private readonly IBankService bankService;
        private readonly IMapper mapper;

        public PaymentController(CheckoutDbContext ctx, IBankService bank, IMapper _mapper)
        {
            dbContext = ctx;
            bankService = bank;
            mapper = _mapper;
        }

        [HttpPost(ApiRoutes.Payment.AddPayment)]
        public async Task<IActionResult> ProceedPayment([FromBody] PaymentRequest paymentRequest)
        {
            var requestToBank = mapper.Map<BankPaymentRequest>(paymentRequest);
            requestToBank.MerchantAccount = "";

        } 
    }
}