using Checkout.HomeTask.Api.Contracts.v1.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.HomeTask.Api.Validators
{
    public class MerchantAuthRequestValidator: AbstractValidator<MerchantAuthRequest>
    {
        public MerchantAuthRequestValidator()
        {
            RuleFor(r => r.Email).EmailAddress();
        }
    }
}
