using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Extensions.Logging;

namespace Checkout.HomeTask.Api.Controllers.v1
{
    public class PaymentController : ControllerBase
    {
        private readonly CheckoutDbContext dbContext;
        private readonly IBankService bankService;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILogger logger;

        public PaymentController(CheckoutDbContext ctx, IBankService bank, IMapper _mapper
            ,UserManager<IdentityUser> manager, ILoggerFactory loggerFactory)
        {
            dbContext = ctx;
            bankService = bank;
            mapper = _mapper;
            userManager = manager;
            logger = loggerFactory.CreateLogger<PaymentController>();
        }


        /// <summary>
        /// Allows merchants to proceed payments
        /// </summary>
        /// <response code = "200">Returns details of successfull payment</response>
        /// <response code = "400">Return errors if somethins went wrong</response>
        [Authorize]
        [HttpPost(ApiRoutes.Payment.AddPayment)]
        public async Task<IActionResult> ProceedPayment([FromBody] PaymentRequest paymentRequest)
        {
            var requestToBank = mapper.Map<BankPaymentRequest>(paymentRequest);
            requestToBank.MerchantId = HttpContext.GetMerchantId();
            var paymentResult = await bankService.ProceedPaymentAsync(requestToBank);
            if (paymentResult.StatusCode != PaymentStatusCode.Success)
            {
                logger.LogInformation($"Payment for merchant {HttpContext.GetMerchantId()} was declined");
                return BadRequest(
                    new ErrorResponse
                    {
                        Errors = new List<ErrorModel>
                        {
                            new ErrorModel { ErrorName = "Payment Failed", Message = "Some custom message" }
                        }
                    });
            }
            var payment = mapper.Map<PaymentDTO>(requestToBank);
            payment.BankPaymentId = paymentResult.PaymentId;
            await dbContext.Payments.AddAsync(payment);
            await dbContext.SaveChangesAsync();
            var response = mapper.Map<PaymentResponse>(payment);
            logger.LogInformation($"Successfully proceeded payment for merchant {payment.MerchantId}");
            return Ok(response);
        }

        /// <summary>
        /// Allows merchants to get history of its payments
        /// </summary>
        /// <response code = "200">Returns list of payment of requesting merchant</response>
        [Authorize]
        [HttpGet(ApiRoutes.Payment.GetAllPayments)]
        public async Task<IActionResult> GetPayments()
        {
            var merchantId = HttpContext.GetMerchantId();
            var payments = (await dbContext.Payments.Where(p => p.MerchantId == merchantId)
                .ToListAsync())
                .Select(p => mapper.Map<PaymentResponse>(p));
            logger.LogInformation($"Merchant {merchantId} requested history of its operations");
            return Ok(payments);
        }

    }
}