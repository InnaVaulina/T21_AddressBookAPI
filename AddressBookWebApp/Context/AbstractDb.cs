using AddressBookWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressBookWebApp.Context
{
    public interface IAddressBookDBContex
    {
        DbSet<Note> Note { get; set; }

        Task<int> SaveChangesAsync();

        int SaveChanges();
    }

    public class AbstractDb : IAddressBookDBContex
    {
        private readonly AddressBookContext _context;

        public AbstractDb(AddressBookContext context)
        {
            _context = context;
        }

        public DbSet<Note> Note
        {
            get { return _context.Note; }
            set { _context.Note = value; }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }

}
