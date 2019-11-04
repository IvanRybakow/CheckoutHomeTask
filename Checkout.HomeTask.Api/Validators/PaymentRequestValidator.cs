using Checkout.HomeTask.Api.Contracts.v1.Requests;
using FluentValidation;
using System;

namespace Checkout.HomeTask.Api.Validators
{
    public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
    {
        public PaymentRequestValidator()
        {
            //card number must be 12-digit number
            RuleFor(pr => pr.CardNumber).NotEmpty().Matches(@"^\d{12}$");

            //cvv must be 3-digit number
            RuleFor(pr => pr.Cvv).NotEmpty().Matches(@"^\d{3}$");

            //year must be greater or equal to current yaer
            RuleFor(pr => pr.ExpireYear).NotEmpty().Must(y => 
            {
                var isNumber = int.TryParse(y, out int n);
                return isNumber && n >= DateTime.UtcNow.Year;
            });

            //when expire year is a current year we check if month is greater than current one
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
                });
            });

            //month must be a number between 1 and 12
            RuleFor(pr => pr.ExpireMonth).NotEmpty().Must(m =>
            {
                var isNumber = int.TryParse(m, out int n);
                return isNumber && n > 0 && n < 13;
            });

            //currency must either EUR or USD
            RuleFor(pr => pr.Currency).NotEmpty().Matches(@"^(EUR|USD|eur|usd)$");

            //amount must be greater than 0
            RuleFor(pr => pr.Amount).GreaterThan(0);
        }
    }
}
