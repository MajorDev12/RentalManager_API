using RentalManager.Data;
using RentalManager.Models;

namespace RentalManager.Repositories.InvoiceRepository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }



        public Task<Invoice> AddAsync(Invoice transaction)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Invoice transaction)
        {
            throw new NotImplementedException();
        }

        public Task<Invoice?> FindAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Invoice>?> GetAllAsync()
        {
            var invoices = await _context.Invoices.ToListAsync();
            return invoices;
        }

        public Task<Invoice?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
