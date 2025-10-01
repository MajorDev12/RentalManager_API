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
                var lineEntity = new List<InvoiceLine>();
                var invoiceAddedEntity = new Invoice();

                if (transactionExists == null)
                    return new ApiResponse<READInvoiceDto>(null, "Transaction Assigned Does Not Exist");

                if (invoiceAdded.Combine)
                {
                    
                    invoiceAddedEntity = await _repo.FindByMonthAsync(transactionExists.MonthFor, transactionExists.YearFor);

                    if (invoiceAddedEntity == null)
                        invoiceAddedEntity = await CreateInvoice(invoiceAdded);

                    lineEntity = lineDto.Select(u => u.ToEntityLineDto(invoiceAddedEntity)).ToList();
                }
                else
                {
                    invoiceAddedEntity = await CreateInvoice(invoiceAdded);

                    // add InvoiceLines
                    lineEntity = lineDto.Select(u => u.ToEntityLineDto(invoiceAddedEntity)).ToList();
                }


                var lines = await _invoicelinerepo.AddRangeAsync(lineEntity);
                return new ApiResponse<READInvoiceDto>(invoiceAddedEntity.ToReadDto(), "Invoice Added Successfully");

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




        public async Task<Invoice> CreateInvoice(CREATEInvoiceDto invoiceAdded)
        {
            string invoiceNumber = Generator.InvoiceNumberGenerator();
            var invoiceEntity = invoiceAdded.ToEntity(invoiceNumber);
            return await _repo.AddAsync(invoiceEntity);
        }




        public async Task<Invoice?> CombineInvoice(int monthFor, int YearFor)
        {
            // get transaction for this month and year Then check where Invoice.Combine is false
            return await _repo.FindByMonthAsync(monthFor, YearFor);

        }




    }
}
