using RentalManager.Models;

namespace RentalManager.Repositories.InvoiceLineRepository
{
    public interface IInvoiceLineRepository
    {
        Task<List<InvoiceLine>?> GetAllAsync();

        Task<InvoiceLine?> GetByIdAsync(int id);

        Task<InvoiceLine> AddAsync(InvoiceLine line);

        Task<List<InvoiceLine>> AddRangeAsync(List<InvoiceLine> lines);

        Task<int> UpdateAsync();

        Task DeleteAsync(InvoiceLine line);

        Task<Invoice?> FindAsync(int id);
    }
}
