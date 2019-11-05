using Checkout.HomeTask.Api.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Checkout.HomeTask.Api.Data
{
    public class CheckoutDbContext : IdentityDbContext
    {
        public CheckoutDbContext(DbContextOptions<CheckoutDbContext> options)
            : base(options)
        {
        }
        public DbSet<Payment> Payments { get; set; }
    }
}
