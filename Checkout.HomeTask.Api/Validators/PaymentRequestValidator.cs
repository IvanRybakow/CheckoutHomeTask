using Checkout.HomeTask.Api.Contracts.v1.Requests;
using FluentValidation;
using System;

namespace Checkout.HomeTask.Api.Validators
{
    public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
    {
        public PaymentRequestValidator()
        {
            RuleFor(pr => pr.CardNumber).NotEmpty().Matches(@"^\d{12}$")
                .WithMessage("Card number must be 12-digit number");

            RuleFor(pr => pr.Cvv).NotEmpty().Matches(@"^\d{3}$")
                .WithMessage("cvv must be a 3-digit number");

            RuleFor(pr => pr.ExpireYear).NotEmpty().Must(y => 
            {
                var isNumber = int.TryParse(y, out int n);
                return isNumber && n >= DateTime.UtcNow.Year;
            }).WithMessage("Year must be greater or equal to current yaer");

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
                }).WithMessage("When expire year is a current year month must be greater than current one"); 
            });

            RuleFor(pr => pr.ExpireMonth).NotEmpty().Must(m =>
            {
                var isNumber = int.TryParse(m, out int n);
                return isNumber && n > 0 && n < 13;
            }).WithMessage("Month must be a number between 1 and 12");

            RuleFor(pr => pr.Currency).NotEmpty().Matches(@"^(EUR|USD|eur|usd)$").WithMessage("Currency must be either EUR or USD");

            RuleFor(pr => pr.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0");
        }
    }
}
