using System.Collections.Generic;

namespace Checkout.HomeTask.Api.Contracts.v1.Responses
{
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; }
    }
}
