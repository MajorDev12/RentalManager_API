using RentalManager.DTOs.Transaction;
using RentalManager.Models;

namespace RentalManager.Repositories.InvoiceRepository
{
    public interface IInvoiceRepository
    {
        Task<List<Invoice>?> GetAllAsync();

        Task<Invoice?> GetByIdAsync(int id);

        Task<Invoice> AddAsync(Invoice transaction);

        Task<int> UpdateAsync();

        Task DeleteAsync(Invoice transaction);

        Task<Invoice?> FindAsync(int id);

        Task<Invoice?> FindByMonthAsync(int month, int year);

    }
}
