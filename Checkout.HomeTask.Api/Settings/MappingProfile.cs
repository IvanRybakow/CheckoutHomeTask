using AutoMapper;
using Checkout.HomeTask.Api.Contracts.v1.Requests;
using Checkout.HomeTask.Api.Domain;

namespace Checkout.HomeTask.Api.Settings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<PaymentRequest, BankPaymentRequest>();
        }
    }
}
