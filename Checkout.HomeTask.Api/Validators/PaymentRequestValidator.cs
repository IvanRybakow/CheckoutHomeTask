using Checkout.HomeTask.Api.Contracts.v1.Requests;
using Checkout.HomeTask.Api.Settings;
using FluentValidation;
using System;

namespace Checkout.HomeTask.Api.Validators
{
    public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
    {
        public PaymentRequestValidator()
        {
            RuleFor(pr => pr.CardNumber).NotEmpty().Matches(@"^\d{16}$")
                .WithMessage(Constants.ValidationErrors.CardNumber);

            RuleFor(pr => pr.Cvv).NotEmpty().Matches(@"^\d{3}$")
                .WithMessage(Constants.ValidationErrors.Cvv);

            RuleFor(pr => pr.ExpireYear).NotEmpty().Must(y => 
            {
                var isNumber = int.TryParse(y, out int n);
                return isNumber && n >= DateTime.UtcNow.Year;
            }).WithMessage(Constants.ValidationErrors.Year);

            When(pr => 
            {
                var isNumber = int.TryParse(pr.ExpireYear, out int n);
                return isNumber && n == DateTime.UtcNow.Year;
            }, () => 
            {
                RuleFor(pr => pr.ExpireMonth).Must(m =>
                {
                    var isNumber = int.TryParse(m, out int n);
                    return isNumber && n > DateTime.UtcNow.Month && n < 13;
                }).WithMessage(Constants.ValidationErrors.MonthInCurrentYear); 
            });

            RuleFor(pr => pr.ExpireMonth).NotEmpty().Must(m =>
            {
                var isNumber = int.TryParse(m, out int n);
                return isNumber && n > 0 && n < 13;
            }).WithMessage(Constants.ValidationErrors.Month);

            RuleFor(pr => pr.Currency).NotEmpty().Matches(@"^(EUR|USD|eur|usd)$").WithMessage(Constants.ValidationErrors.Currency);

            RuleFor(pr => pr.Amount).GreaterThan(0).WithMessage(Constants.ValidationErrors.Amount);
        }
    }
}
