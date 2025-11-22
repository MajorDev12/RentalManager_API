using RentalManager.DTOs.Transaction;
using RentalManager.Models;

namespace RentalManager.Services.TransactionService
{
    public interface ITransactionService
    {
        Task<ApiResponse<List<READTransactionDto>>> GetAll();

        Task<ApiResponse<READTransactionDto>> Add(CREATETransactionDto transaction);

        Task<ApiResponse<READTransactionDto>> AddCharge(CREATEIncoiceTransactionDto transaction);

        Task<ApiResponse<bool>> GenerateRentInvoices(int propertyId);

        Task<ApiResponse<bool>> GenerateUtilityBillInvoices(int propertyId);

        List<CREATEIncoiceTransactionDto> CreateRentBalanceInvoice(List<Tenant> tenants, List<Transaction> existingCharges);

        List<CREATEIncoiceTransactionDto> CreateFullRentInvoice(List<Tenant> tenants);

        List<CREATEIncoiceTransactionDto> CreateFullUtilityInvoices(List<Tenant> tenants, List<UtilityBill> utilities);

        List<CREATEIncoiceTransactionDto> CreateUtilityBalanceInvoices(List<Tenant> tenants, List<UtilityBill> utilities, List<Transaction> existingCharges);


        Task<ApiResponse<READTransactionDto>> AddPayment(CREATEPaymentDto payment);

        Task<ApiResponse<READTransactionDto>> Update(int id, UPDATETransactionDto transaction);

        Task<ApiResponse<READTransactionDto>> Delete(int id);

        Task<ApiResponse<List<TenantBalanceDto>>> GetRentBalances();

        Task<ApiResponse<Transaction>> AddReccuringPayment(Transaction payment);

    }
}
