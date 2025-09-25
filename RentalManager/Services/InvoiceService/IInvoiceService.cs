using RentalManager.DTOs.Invoice;
using RentalManager.DTOs.InvoiceLine;
using RentalManager.Models;

namespace RentalManager.Services.InvoiceService
{
    public interface IInvoiceService
    {
        Task<ApiResponse<List<READInvoiceDto>>> GetAll();
        Task<ApiResponse<READInvoiceDto>> GetById(int id);
        Task<ApiResponse<READInvoiceDto>> Add(CREATEInvoiceDto invoice, List<CREATEInvoiceLineDto> lineDto);
        Task<ApiResponse<READInvoiceDto>> Update(int id, READInvoiceDto code);
        Task<ApiResponse<READInvoiceDto>> Delete(int id);
    }
}
