namespace RentalManager.Authorization.Permissions
{
    public static class PermissionList
    {
        public static List<(string Name, string Category)> All = new()
    {
        // Property
        ("Property.Read", "Property"),
        ("Property.Create", "Property"),
        ("Property.Assign", "Property"),
        ("Property.Update", "Property"),
        ("Property.Delete", "Property"),

        // Utilities
        ("UtilityBill.Read", "UtilityBill"),
        ("UtilityBill.Create", "UtilityBill"),
        ("UtilityBill.Update", "UtilityBill"),
        ("UtilityBill.Delete", "UtilityBill"),

        // Unit Type
        ("UnitType.Read", "UnitType"),
        ("UnitType.Create", "UnitType"),
        ("UnitType.Update", "UnitType"),
        ("UnitType.Delete", "UnitType"),

        // Unit
        ("Unit.Read", "Unit"),
        ("Unit.Create", "Unit"),
        ("Unit.Update", "Unit"),
        ("Unit.Delete", "Unit"),

        // Tenant
        ("Tenant.Read.All", "Tenant"),
        ("Tenant.Read.Self", "Tenant"),
        ("Tenant.Create", "Tenant"),
        ("Tenant.Update", "Tenant"),
        ("Tenant.Assign", "Tenant"),
        ("Tenant.Delete", "Tenant"),

        // Transaction
        ("Transaction.Read.All", "Transaction"),
        ("Transaction.Read.Self", "Transaction"),
        ("Transaction.Create", "Transaction"),
        ("Transaction.Update", "Transaction"),
        ("Transaction.Delete", "Transaction"),

        // Expense
        ("Expense.Read", "Expense"),
        ("Expense.Create", "Expense"),
        ("Expense.Update", "Expense"),
        ("Expense.Delete", "Expense"),

        // Report
        ("Report.Read", "Report"),
        ("Report.Create", "Report"),
        ("Report.Update", "Report"),
        ("Report.Delete", "Report"),

        // Notification
        ("Notification.Read", "Notification"),
    };
    }
}
