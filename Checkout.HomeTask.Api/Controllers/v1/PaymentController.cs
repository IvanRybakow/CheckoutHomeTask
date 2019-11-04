using System;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.HomeTask.Api.Contracts.v1;
using Checkout.HomeTask.Api.Contracts.v1.Requests;
using Checkout.HomeTask.Api.Data;
using Checkout.HomeTask.Api.Data.Entities;
using Checkout.HomeTask.Api.Domain;
using Checkout.HomeTask.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var paymentResult = await bankService.ProceedPaymentAsync(requestToBank);
            if (paymentResult.StatusCode != PaymentStatusCode.Success)
            {
                return BadRequest("Payment failed");
            }
            var payment = mapper.Map<Payment>(requestToBank);
            payment.MerchantId = Guid.Empty.ToString();
            payment.BankPaymentId = paymentResult.PaymentId;
            await dbContext.Payments.AddAsync(payment);
            await dbContext.SaveChangesAsync();
            return Ok();
        } 

        [HttpGet(ApiRoutes.Payment.GetAllPayments)]
        public async Task<IActionResult> GetPayments()
        {
            return Ok(await dbContext.Payments.ToListAsync());
        }

    }
}