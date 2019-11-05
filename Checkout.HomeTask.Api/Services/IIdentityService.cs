using Checkout.HomeTask.Api.Domain;
using System.Threading.Tasks;

namespace Checkout.HomeTask.Api.Services
{
    public interface IIdentityService
    {
        Task<MerchantAuthResult> RegisterMerchant(string email, string password);
        Task<MerchantAuthResult> LoginMerchant(string email, string password);
    }
}
