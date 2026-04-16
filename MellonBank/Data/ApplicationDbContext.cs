using MellonBank.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MellonBank.Data
{
    public class ApplicationDbContext : IdentityDbContext <ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options) {}
        public virtual DbSet<BankAccount> BankAccounts {get;set;}

        public DbSet<Transaction> Transactions {get;set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BankAccount>().Property(b=>b.Balance).HasPrecision(18,2);

            builder.Entity<BankAccount>().HasKey(b=>b.IBAN);

            builder.Entity<BankAccount>()
                .HasOne(a => a.User)
                .WithMany(u=> u.BankAccounts) 
                .HasForeignKey(a => a.UserId);
        }

    }
}