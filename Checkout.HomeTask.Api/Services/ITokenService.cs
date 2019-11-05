using Microsoft.AspNetCore.Identity;

namespace Checkout.HomeTask.Api.Services
{
    public interface ITokenService
    {
        string getToken(IdentityUser user);
    }
}
