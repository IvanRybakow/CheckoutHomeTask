namespace Checkout.HomeTask.Api.Contracts.v1.Responses
{
    public class PaymentResponse
    {
        public string MaskedCardNumber { get; set; }

        public string CardHolderName { get; set; }

        public int Amount { get; set; }

        public string Currency { get; set; }
    }
}
