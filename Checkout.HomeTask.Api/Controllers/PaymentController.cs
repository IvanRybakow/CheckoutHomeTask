using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.HomeTask.Api.Data;
using Checkout.HomeTask.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.HomeTask.Api.Controllers
{
    public class PaymentController : ControllerBase
    {
        private readonly CheckoutDbContext dbContext;
        private readonly IBankService bankService;

        public PaymentController()
        {

        }
    }
}