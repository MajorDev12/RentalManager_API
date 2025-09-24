using RentalManager.DTOs.Invoice;
using RentalManager.DTOs.SystemCode;
using RentalManager.DTOs.Transaction;
using RentalManager.Models;

namespace RentalManager.Services.InvoiceService
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepo;


        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepo = invoiceRepository;
        }





        public Task<ApiResponse<READSystemCodeDto>> Add(CREATESystemCodeDto code)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<READSystemCodeDto>> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<READInvoiceDto>>> GetAll()
        {
            try
            {
                var transactions = await _repo.GetAllAsync();

                if (transactions == null || transactions.Count == 0)
                {
                    return new ApiResponse<List<READInvoiceDto>>(null, "Data Not Found.");
                }

                var transactionDtos = transactions.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READInvoiceDto>>(transactionDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READInvoiceDto>>($"Error Occurred: {ex.InnerException?.Message}");
            }
        }

        public Task<ApiResponse<READSystemCodeDto>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<READSystemCodeDto>> Update(int id, UPDATESystemCodeDto code)
        {
            throw new NotImplementedException();
        }
    }
}
