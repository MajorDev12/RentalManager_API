using RentalManager.DTOs.Transaction;
using RentalManager.Models;

namespace RentalManager.Services.TransactionService
{
    public interface ITransactionService
    {
        Task<ApiResponse<List<READTransactionDto>>> GetAll();

    }
}
