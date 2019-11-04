using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checkout.HomeTask.Api.Data.Entities
{
    public class Payment
    {
        [Key]
        public Guid Id { get; set; }

        public string BankPaymentId { get; set; }

        public string MerchantId { get; set; }

        public string MaskedCardNumber { get; set; }

        public string CardHolderName { get; set; }

        public int Amount { get; set; }

        public string Currency { get; set; }
    }
}
