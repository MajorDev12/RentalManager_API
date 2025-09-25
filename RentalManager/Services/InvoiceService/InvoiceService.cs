using RentalManager.DTOs.Invoice;
using RentalManager.DTOs.InvoiceLine;
using RentalManager.Helpers.Generators;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.InvoiceLineRepository;
using RentalManager.Repositories.InvoiceRepository;
using RentalManager.Repositories.TransactionRepository;

namespace RentalManager.Services.InvoiceService
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _repo;
        private readonly ITransactionRepository _transactionrepo;
        private readonly IInvoiceLineRepository _invoicelinerepo;


        public InvoiceService(IInvoiceRepository invoiceRepository, ITransactionRepository transactionRepository)
        {
            _repo = invoiceRepository;
            _transactionrepo = transactionRepository;
        }


        public async Task<ApiResponse<List<READInvoiceDto>>> GetAll()
        {
            try
            {
                var invoices = await _repo.GetAllAsync();

                if (invoices == null || invoices.Count == 0)
                {
                    return new ApiResponse<List<READInvoiceDto>>(null, "Data Not Found.");
                }

                var invoiceDtos = invoices.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READInvoiceDto>>(invoiceDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READInvoiceDto>>($"Error Occurred: {ex.InnerException?.Message}");
            }
        }


        public Task<ApiResponse<READInvoiceDto>> GetById(int id)
        {
            throw new NotImplementedException();
        }



        public async Task<ApiResponse<READInvoiceDto>> Add(CREATEInvoiceDto invoiceAdded, List<CREATEInvoiceLineDto> lineDto)
        {
            try
            {
                // check if transactionId Exists
                var transactionExists = await _transactionrepo.FindAsync(invoiceAdded.TransactionId);

                if (transactionExists == null)
                    return new ApiResponse<READInvoiceDto>(null, "Transaction Assigned Does Not Exist");
                // create InvoiceNumber
                var generator = new Generator();
                string invoiceNumber = generator.InvoiceNumberGenerator();
                // change toEntity - calc balance
                var invoiceEntity = invoiceAdded.ToEntity(invoiceNumber);
                // save it
                var addedInvoice = await _repo.AddAsync(invoiceEntity);

                // add InvoiceLines
                var lineEnity = lineDto.Select(u => u.ToEntityLineDto(addedInvoice)).ToList();

                var lines = await _invoicelinerepo.AddRangeAsync(lineEnity);


                var invoiceDto = addedInvoice.ToReadDto();

                return new ApiResponse<READInvoiceDto>(invoiceDto, "Invoice Added Successfully");
            }
            catch (Exception ex) 
            {
                return new ApiResponse<READInvoiceDto>(null, $"Something went wrong {ex.InnerException?.Message}");
            }

        }



        public Task<ApiResponse<READInvoiceDto>> Update(int id, READInvoiceDto code)
        {
            throw new NotImplementedException();
        }



        public Task<ApiResponse<READInvoiceDto>> Delete(int id)
        {
            throw new NotImplementedException();
        }


    }
}
