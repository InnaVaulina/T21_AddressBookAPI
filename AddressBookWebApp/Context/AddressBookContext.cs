
using AddressBookWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AddressBookWebApp.Context
{
    public class AddressBookContext : IdentityDbContext<AppUser>
    {
        public AddressBookContext(DbContextOptions<AddressBookContext> options)
            : base(options)
        {
        }

        public DbSet<Note> Note { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }

    
    
}
