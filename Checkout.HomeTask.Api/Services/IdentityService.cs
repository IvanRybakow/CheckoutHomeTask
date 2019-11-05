using Checkout.HomeTask.Api.Contracts.v1.Responses;
using Checkout.HomeTask.Api.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.HomeTask.Api.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;

        public IdentityService(UserManager<IdentityUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<MerchantAuthResult> LoginMerchant(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || await _userManager.CheckPasswordAsync(user, password))
            {
                return new MerchantAuthResult
                {
                    Errors = new ErrorModel[] { new ErrorModel { ErrorName = "Wrong credentials", Message = "Login/Password pair is incorrect" } },
                    IsSuccessfull = false
                };
            }
            var token = _tokenService.getToken(user);
            return new MerchantAuthResult { IsSuccessfull = true, Token = token };
        }

        public async Task<MerchantAuthResult> RegisterMerchant(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return new MerchantAuthResult
                {
                    Errors = new ErrorModel[] { new ErrorModel { ErrorName = "Email exists", Message = "This email is already taken" } },
                    IsSuccessfull = false
                };
            }
            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email
            };
            var createdUser = await _userManager.CreateAsync(newUser, password);
            if (!createdUser.Succeeded)
            {
                return new MerchantAuthResult
                {
                    Errors = createdUser.Errors.Select(e => new ErrorModel { ErrorName = e.Code, Message = e.Description }),
                    IsSuccessfull = false
                };
            }
            var token = _tokenService.getToken(newUser);
            return new MerchantAuthResult { IsSuccessfull = true, Token = token };
        }
    }
}
