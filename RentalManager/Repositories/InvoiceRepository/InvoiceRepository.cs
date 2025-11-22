using Microsoft.EntityFrameworkCore;
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



        public async Task<Invoice> AddAsync(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }



        public Task DeleteAsync(Invoice transaction)
        {
            throw new NotImplementedException();
        }



        public Task<Invoice?> FindAsync(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<Invoice?> FindByMonthAsync(int month, int year)
        {
            return await _context.Invoices
                .Where(u => u.Transactions.MonthFor == month && u.Transactions.YearFor == year && u.isMain == true)
                .FirstOrDefaultAsync();
        }


        public async Task<List<Invoice>?> GetAllAsync()
        {
            return await _context.Invoices
                .Include(u => u.Transactions)
                .Include(u => u.Transactions.Property)
                .Include(u => u.Transactions.User)
                .Include(u => u.Transactions.TransactionType)
                .ToListAsync();
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
