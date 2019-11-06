using Checkout.HomeTask.Api.Contracts.v1;
using Checkout.HomeTask.Api.Contracts.v1.Requests;
using Checkout.HomeTask.Api.Contracts.v1.Responses;
using Checkout.HomeTask.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.HomeTask.Api.Controllers.v1
{

    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger logger;

        public IdentityController(IIdentityService identityService, ILoggerFactory loggerFactory)
        {
            _identityService = identityService;
            logger = loggerFactory.CreateLogger<IdentityController>();
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] MerchantAuthRequest request)
        {
            var result = await _identityService.RegisterMerchant(request.Email, request.Password);
            if (!result.IsSuccessfull)
            {
                logger.LogInformation("Failed to register new merchant");
                return BadRequest(error: new ErrorResponse { Errors = result.Errors.ToList() });
            }
            logger.LogInformation("Successfully created new account for merchant");
            return Ok(new MerchantAuthSuccessResponse
            {
                Token = result.Token
            });
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] MerchantAuthRequest request)
        {
            var result = await _identityService.LoginMerchant(request.Email, request.Password);
            if (!result.IsSuccessfull)
            {
                logger.LogInformation("Failed to login merchant");
                return BadRequest(new ErrorResponse { Errors = result.Errors.ToList() });
            }
            logger.LogInformation($"Merchant {request.Email} successfully logged in");
            return Ok(new MerchantAuthSuccessResponse
            {
                Token = result.Token
            });
        }
    }
}