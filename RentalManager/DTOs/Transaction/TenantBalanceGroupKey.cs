namespace RentalManager.DTOs.Transaction
{
    public sealed record TenantBalanceGroupKey
(
    int UserId,
    int MonthFor,
    int YearFor,
    int CategoryId
);

}
