namespace Checkout.HomeTask.Api.Contracts.v1.Requests
{
    public class PaymentRequest
    {
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpireMonth { get; set; }
        public string ExpireYear { get; set; }
        public string Cvv { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
    }
}
