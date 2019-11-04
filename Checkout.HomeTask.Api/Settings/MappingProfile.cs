using AutoMapper;
using Checkout.HomeTask.Api.Contracts.v1.Requests;
using Checkout.HomeTask.Api.Data.Entities;
using Checkout.HomeTask.Api.Domain;
using System.Linq;

namespace Checkout.HomeTask.Api.Settings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<PaymentRequest, BankPaymentRequest>();
            CreateMap<BankPaymentRequest, Payment>()
                .ForMember(dest => dest.MaskedCardNumber, opt => opt.MapFrom(src => MaskCardNumber(src.CardNumber)));
        }

        private string MaskCardNumber(string cardNumber)
        {
            return "XXXXXXXXXXXX" + cardNumber.Substring(cardNumber.Length - 4);
        }
    }
}
