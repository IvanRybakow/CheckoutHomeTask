using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checkout.HomeTask.Api.Data.Entities
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public string MerchantId { get; set; }

        [ForeignKey(nameof(MerchantId))]
        public IdentityUser Merchant { get; set; }

        public string MaskedCardNumber { get; set; }

        public string CardHolderName { get; set; }

        public int Amount { get; set; }

        public string Currency { get; set; }
    }
}
