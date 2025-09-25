using RentalManager.Data;
using RentalManager.Models;

namespace RentalManager.Repositories.InvoiceLineRepository
{
    public class InvoiceLineRepository : IInvoiceLineRepository
    {
        private readonly ApplicationDbContext _context;


        public InvoiceLineRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<InvoiceLine> AddAsync(InvoiceLine line)
        {
             _context.InvoiceLines.Add(line);
            await _context.SaveChangesAsync();
            return line;
        }


        public async Task<List<InvoiceLine>> AddRangeAsync(List<InvoiceLine> lines)
        {
            _context.InvoiceLines.AddRange(lines);
            await _context.SaveChangesAsync();
            return lines;
        }


        public Task DeleteAsync(InvoiceLine line)
        {
            throw new NotImplementedException();
        }

        public Task<Invoice?> FindAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<InvoiceLine>?> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<InvoiceLine?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
