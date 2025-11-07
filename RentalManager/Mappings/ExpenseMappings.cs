using RentalManager.DTOs.Expense;
using RentalManager.Models;

namespace RentalManager.Mappings
{
    public static class ExpenseMappings
    {
        public static Expense ToEntity(this CREATEExpenseDto dto) => new Expense
        {
            Name = dto.Name,
            Amount = dto.Amount,
            Notes = dto.Notes,
            PropertyId = dto.PropertyId
        };



        public static READExpenseDto ToReadDto(this Expense dto) => new READExpenseDto
        {
            Id = dto.Id,
            Name = dto.Name,
            Amount = dto.Amount,
            Notes = dto.Notes,
            PropertyId = dto.PropertyId,
            PropertyName = dto.Property.Name
        };



        public static UPDATEExpenseDto ToUpdateDto(this Expense dto) => new UPDATEExpenseDto
        {
            Name = dto.Name,
            Amount = dto.Amount,
            Notes = dto.Notes,
            PropertyId = dto.PropertyId
        };


        public static Expense ToUpdateEntity(this UPDATEExpenseDto dto) => new Expense
        {
            Name = dto.Name,
            Amount = dto.Amount,
            Notes = dto.Notes,
            PropertyId = dto.PropertyId
        };


        public static Expense UpdateEntity(this UPDATEExpenseDto dto, Expense existing)
        {
            existing.Name = dto.Name;
            existing.Amount = dto.Amount;
            existing.Notes = dto.Notes;
            existing.PropertyId = dto.PropertyId; 

            return existing;
        }


    }
}
