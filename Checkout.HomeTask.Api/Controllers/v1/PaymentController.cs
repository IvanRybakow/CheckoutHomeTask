using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.HomeTask.Api.Contracts.v1;
using Checkout.HomeTask.Api.Contracts.v1.Requests;
using Checkout.HomeTask.Api.Contracts.v1.Responses;
using Checkout.HomeTask.Api.Data;
using Checkout.HomeTask.Api.Data.Entities;
using Checkout.HomeTask.Api.Domain;
using Checkout.HomeTask.Api.Extensions;
using Checkout.HomeTask.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Checkout.HomeTask.Api.Controllers.v1
{
    public class PaymentController : ControllerBase
    {
        private readonly CheckoutDbContext dbContext;
        private readonly IBankService bankService;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public PaymentController(CheckoutDbContext ctx, IBankService bank, IMapper _mapper, UserManager<IdentityUser> manager)
        {
            dbContext = ctx;
            bankService = bank;
            mapper = _mapper;
            userManager = manager;
        }

        [Authorize]
        [HttpPost(ApiRoutes.Payment.AddPayment)]
        public async Task<IActionResult> ProceedPayment([FromBody] PaymentRequest paymentRequest)
        {
            var requestToBank = mapper.Map<BankPaymentRequest>(paymentRequest);
            requestToBank.MerchantId = HttpContext.GetMerchantId();
            var paymentResult = await bankService.ProceedPaymentAsync(requestToBank);
            if (paymentResult.StatusCode != PaymentStatusCode.Success)
            {
                return BadRequest(
                    new ErrorResponse
                    {
                        Errors = new List<ErrorModel>
                        {
                            new ErrorModel { ErrorName = "Payment Failed", Message = "Some custom message" }
                        }
                    });
            }
            var payment = mapper.Map<Payment>(requestToBank);
            payment.BankPaymentId = paymentResult.PaymentId;
            await dbContext.Payments.AddAsync(payment);
            await dbContext.SaveChangesAsync();
            return Ok();
        } 

        [Authorize]
        [HttpGet(ApiRoutes.Payment.GetAllPayments)]
        public async Task<IActionResult> GetPayments()
        {
            return Ok(await dbContext.Payments.ToListAsync());
        }

    }
}