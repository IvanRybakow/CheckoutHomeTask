namespace Checkout.HomeTask.Api.Domain
{
    public class BankPaymentRequest
    {
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpireMonth { get; set; }
        public string ExpireYear { get; set; }
        public string Cvv { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string MerchantId { get; set; }
    }
}
