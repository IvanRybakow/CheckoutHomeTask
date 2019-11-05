using Checkout.HomeTask.Api.Contracts.v1;
using Checkout.HomeTask.Api.Contracts.v1.Requests;
using Checkout.HomeTask.Api.Contracts.v1.Responses;
using Checkout.HomeTask.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.HomeTask.Api.Controllers.v1
{

    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register(MerchantAuthRequest request)
        {
            var result = await _identityService.RegisterMerchant(request.Email, request.Password);
            if (!result.IsSuccessfull)
            {
                return BadRequest(error: new ErrorResponse { Errors = result.Errors.ToList() });
            }
            return Ok(new MerchantAuthSuccessResponse
            {
                Token = result.Token
            });
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login(MerchantAuthRequest request)
        {
            var result = await _identityService.LoginMerchant(request.Email, request.Password);
            if (!result.IsSuccessfull)
            {
                return BadRequest(new ErrorResponse { Errors = result.Errors.ToList() });
            }
            return Ok(new MerchantAuthSuccessResponse
            {
                Token = result.Token
            });
        }
    }
}