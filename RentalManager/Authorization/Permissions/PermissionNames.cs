namespace RentalManager.Authorization.Permissions
{
    public static class PermissionNames
    {
        public static class Property
        {
            public const string Read = "Property.Read";
            public const string Create = "Property.Create";
            public const string Assign = "Property.Assign";
            public const string Update = "Property.Update";
            public const string Delete = "Property.Delete";
        }

        public static class UtilityBill
        {
            public const string Read = "UtilityBill.Read";
            public const string Create = "UtilityBill.Create";
            public const string Update = "UtilityBill.Update";
            public const string Delete = "UtilityBill.Delete";
        }

        public static class UnitType
        {
            public const string Read = "UnitType.Read";
            public const string Create = "UnitType.Create";
            public const string Update = "UnitType.Update";
            public const string Delete = "UnitType.Delete";
        }

        public static class Unit
        {
            public const string Read = "Unit.Read";
            public const string Create = "Unit.Create";
            public const string Update = "Unit.Update";
            public const string Delete = "Unit.Delete";
        }


        public static class Tenant
        {
            public const string ReadAll = "Tenant.Read.All";
            public const string ReadSelf = "Tenant.Read.Self";
            public const string Create = "Tenant.Create";
            public const string Update = "Tenant.Update";
            public const string Assign = "Tenant.Assign";
            public const string Delete = "Tenant.Delete";
        }


        public static class Transaction
        {
            public const string ReadAll = "Transaction.Read.All";
            public const string ReadSelf = "Transaction.Read.Self";
            public const string Create = "Transaction.Create";
            public const string Update = "Transaction.Update";
            public const string Assign = "Transaction.Assign";
            public const string Delete = "Transaction.Delete";
        }

        public static class Expense
        {
            public const string Read = "Expense.Read";
            public const string Create = "Expense.Create";
            public const string Update = "Expense.Update";
            public const string Delete = "Expense.Delete";
        }

        public static class Report
        {
            public const string Read = "Report.Read";
            public const string Create = "Report.Create";
            public const string Update = "Report.Update";
            public const string Delete = "Report.Delete";
        }

        public static class Notification
        {
            public const string Read = "Notification.Read";
        }
    }
}
