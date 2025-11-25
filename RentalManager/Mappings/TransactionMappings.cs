using RentalManager.DTOs.Invoice;
using RentalManager.DTOs.Tenant;
using RentalManager.DTOs.Transaction;
using RentalManager.Models;

namespace RentalManager.Mappings
{
    public static class TransactionMappings
    {
        public static Transaction ToEntity(this CREATETransactionDto dto) => new Transaction
        {
            Id = dto.Id,
            UserId = dto.UserId,
            PropertyId = dto.PropertyId,
            UnitId = dto.UnitId,
            UtilityBillId = dto.UtilityBillId,
            TransactionTypeId = dto.TransactionTypeId,
            TransactionCategoryId = dto.TransactionCategoryId,
            Amount = dto.Amount,
            PaymentMethodId = dto.PaymentMethodId,
            TransactionDate = dto.TransactionDate,
            MonthFor = dto.MonthFor,
            YearFor = dto.YearFor,
            Notes = dto.Notes
        };


        public static Transaction ToEntity(this CREATEIncoiceTransactionDto dto, CREATETransactionDto transactionDto) => new Transaction
        {
            UserId = dto.UserId,
            PropertyId = transactionDto.PropertyId,
            UnitId = transactionDto.UnitId,
            UtilityBillId = transactionDto.UtilityBillId,
            TransactionTypeId = transactionDto.TransactionTypeId,
            TransactionCategoryId = transactionDto.TransactionCategoryId,
            Amount = transactionDto.Amount,
            TransactionDate = DateTime.UtcNow,
            MonthFor = dto.MonthFor,
            YearFor = dto.YearFor,
            Notes = dto.Notes
        };

        public static Transaction ToEntity(this CREATEIncoiceTransactionDto dto, InvoiceMappingContext ctx)
        {
            return new Transaction
            {
                UserId = dto.UserId,
                PropertyId = ctx.PropertyId,
                UnitId = ctx.UnitId,
                TransactionTypeId = ctx.TransactionTypeId,
                TransactionCategoryId = ctx.TransactionCategoryId,
                Amount = dto.Item.Sum(i => i.Amount),
                TransactionDate = dto.InvoiceDate,
                MonthFor = dto.MonthFor,
                YearFor = dto.YearFor,
                Notes = dto.Notes
            };
        }



        public static Transaction ToEntity(this CREATEIncoiceTransactionDto dto,
                                   InvoiceMappingContext ctx,
                                   decimal amount)
        {
            return new Transaction
            {
                UserId = dto.UserId,
                PropertyId = ctx.PropertyId,
                UnitId = ctx.UnitId,
                UtilityBillId = ctx.UtilityBillId,
                TransactionTypeId = ctx.TransactionTypeId,
                TransactionCategoryId = ctx.TransactionCategoryId,
                Amount = amount,
                TransactionDate = DateTime.UtcNow,
                MonthFor = dto.MonthFor,
                YearFor = dto.YearFor,
                Notes = dto.Notes
            };
        }






        public static Transaction ToPaymentEntity(this CREATEPaymentDto dto, Tenant tenant) => new Transaction
        {
            UserId = tenant.UserId,
            PropertyId = tenant.User.PropertyId,
            UnitId = tenant.UnitId,
            Amount = dto.Amount,
            PaymentMethodId = dto.PaymentMethodId,
            TransactionDate = dto.TransactionDate,
            Notes = dto.Notes
        };



        public static Transaction ToTransactionEntity(this ASSIGNUnitDto dto) => new Transaction
        {
            UserId = dto.tenantId,
            UnitId = dto.unitId,
            Amount = dto.AmountPaid,
            PaymentMethodId = dto.PaymentMethodId,
            TransactionDate = dto.PaymentDate
        };



        public static READTransactionDto ToReadDto(this Transaction dto) => new READTransactionDto
        {
            Id = dto.Id,
            UserId = dto.UserId,
            UserName = $"{dto.User?.FirstName}  {dto.User?.LastName}",
            PropertyId = dto.PropertyId,
            PropertyName = dto.Property.Name,
            UnitId = dto.Unit?.Id,
            Unit = dto.Unit?.Name,
            UtilityBillId = dto.UtilityBillId,
            UtilityBill = dto.UtilityBill?.Name,
            TransactionTypeId = dto.TransactionTypeId,
            TransactionType = dto.TransactionType.Item,
            TransactionCategory = dto.TransactionCategory.Item,
            Amount = dto.Amount,
            PaymentMethodId = dto.PaymentMethodId,
            PaymentMethod = dto.PaymentMethod?.Item,
            TransactionDate = dto.TransactionDate,
            MonthFor = dto.MonthFor,
            YearFor = dto.YearFor,
            Notes = dto?.Notes
        };


        public static Transaction UpdateEntity(this UPDATETransactionDto updated, Transaction existing)
        {
            existing.UserId = updated.UserId;
            existing.PropertyId = updated.PropertyId;
            existing.UnitId = updated.UnitId;
            existing.Amount = updated.Amount;
            existing.TransactionTypeId = updated.TransactionTypeId;
            existing.MonthFor = updated.MonthFor;
            existing.YearFor = updated.YearFor;
            existing.PaymentMethodId = updated.PaymentMethodId;
            existing.Notes = updated.Notes;

            return existing;
        }


        public static CREATEInvoiceDto ToInvoice(this Transaction transaction) => new CREATEInvoiceDto
        {
            TotalAmount = transaction.Amount,
            Status = transaction.Status,
            Combine = true,
            TransactionId = transaction.Id,
        };


        public static CREATETransactionDto ToCreateTransaction(this CREATEIncoiceTransactionDto dto) => new CREATETransactionDto
        {
            UserId = dto.UserId,
            TransactionDate = dto.InvoiceDate,
            MonthFor = dto.MonthFor,
            YearFor = dto.YearFor,
            Notes = dto.Notes
        };



    }
}
