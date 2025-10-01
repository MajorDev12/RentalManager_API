using RentalManager.DTOs.Transaction;
using RentalManager.Models;

namespace RentalManager.Services.TransactionService
{
    public interface ITransactionService
    {
        Task<ApiResponse<List<READTransactionDto>>> GetAll();

        Task<ApiResponse<READTransactionDto>> Add(CREATETransactionDto transaction);

        Task<ApiResponse<READTransactionDto>> AddPayment(CREATEPaymentDto payment);

        Task<ApiResponse<READTransactionDto>> Update(int id, UPDATETransactionDto transaction);

        Task<ApiResponse<READTransactionDto>> Delete(int id);

        Task<ApiResponse<List<TenantBalanceDto>>> GetRentBalances();

    }
}
