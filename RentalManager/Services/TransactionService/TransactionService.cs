using RentalManager.DTOs.Transaction;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.TransactionRepository;

namespace RentalManager.Services.TransactionService
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repo;

        public TransactionService(ITransactionRepository repo) {
            _repo = repo;
        }


        public async Task<ApiResponse<List<READTransactionDto>>> GetAll()
        {
            try
            {
                var transactions = await _repo.GetAllAsync();

                if (transactions == null || transactions.Count == 0)
                {
                    return new ApiResponse<List<READTransactionDto>>(null, "Data Not Found.");
                }

                var transactionDtos = transactions.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READTransactionDto>>(transactionDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READTransactionDto>>($"Error Occurred: {ex.InnerException?.Message}");
            }
        }
    }
}
