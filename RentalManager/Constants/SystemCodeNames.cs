namespace RentalManager.Constants
{
    public static class SystemCodeNames
    {
        public static class Code
        {
            public const string Gender = "GENDER";
            public const string UtilityBill = "UTILITYBILL";
            public const string UnitType = "UNITTYPE";
            public const string PaymentMethod = "PAYMENTMETHOD";
            public const string TenantStatus = "TENANTSTATUS";
            public const string UserStatus = "USERSTATUS";
            public const string UnitStatus = "UNITSTATUS";
            public const string LeaseStatus = "LEASESTATUS";
            public const string LogLevel = "LOGLEVEL";
            public const string BillingCycle = "BILLINGCYCLE";
            public const string TransactionCategory = "TRANSACTIONCATEGORY";
            public const string TransactionRelationType = "TRANSACTIONRELATIONTYPE";
            public const string TransactionType = "TRANSACTIONTYPE";
            public const string ExpenseCategory = "EXPENSECATEGORY";
            public const string PropertyType = "PROPERTYTYPE";
            public const string RentalType = "RENTALTYPE";
        }

        public static class Item
        {
            public static class Gender
            {
                public const string Male = "MALE";
                public const string Female = "FEMALE";
                public const string Other = "OTHER";
            }

            public static class UtilityBill
            {
                public const string Water = "WATER";
                public const string Electricity = "ELECTRICITY";
                public const string Gas = "GAS";
                public const string Garbage = "GARBAGE";
                public const string Sewer = "SEWER";
                public const string Internet = "INTERNET";
                public const string CableTv = "CABLE_TV";
                public const string Security = "SECURITY";
                public const string Cleaning = "CLEANING";
                public const string Parking = "PARKING";
                public const string Maintenance = "MAINTENANCE";
            }


            public static class RentalType
            {
                public const string ShortTerm = "Short_Term";
                public const string LongTerm = "Long_Term";
            }


            public static class UnitType
            {
                public const string SingleRoom = "Single Room";
                public const string DoubleRoom = "Double Room";
                public const string BedSitter = "BedSitter";
                public const string Loft = "Loft";
                public const string Shop = "Shop";
                public const string Office = "Office";
                public const string Duplex = "Duplex";
                public const string PentHouse = "PentHouse";
                public const string ConferenceRoom = "Conference Room";
                public const string OneBedroom = "1-BR";
                public const string TwoBedroom = "2-BR";
                public const string ThreeBedroom = "3-BR";
                public const string FourBedroom = "4-BR";
                public const string FiveBedroom = "5-BR";
                public const string SixBedroom = "6-BR";
                public const string SevenBedroom = "7-BR";
                public const string EightBedroom = "8-BR";
                public const string NineBedroom = "9-BR";
                public const string TenBedroom = "10-BR";
            }

            public static class TenantStatus
            {
                public const string Pending = "PENDING";
                public const string Active = "ACTIVE";
                public const string Vacated = "VACATED";
                public const string Evicted = "EVICTED";
                public const string Terminated = "TERMINATED";
            }

            public static class UserStatus
            {
                public const string Active = "ACTIVE";
                public const string INACTIVE = "INACTIVE";
            }

            public static class UnitStatus
            {
                public const string Vacant = "VACANT";
                public const string Occupied = "OCCUPIED";
            }

            public static class ExpenseCategory
            {
                public const string Salary = "SALARY";
                public const string Maintenance = "MAINTENANCE";
                public const string Legal = "LEGAL";
                public const string Insurance = "INSURANCE";
                public const string Security = "SECURITY";
            }

            public static class LeaseStatus
            {
                public const string Draft = "DRAFT";
                public const string Active = "ACTIVE";
                public const string Suspended = "SUSPENDED";
                public const string Expired = "EXPIRED";
                public const string Terminated = "TERMINATED";
            }


            public static class BillingCycle
            {
                public const string OneTime = "ONETIME";
                public const string Daily = "DAILY";
                public const string Weekly = "WEEKLY";
                public const string Monthly = "MONTHLY";
                public const string Quarterly = "QUARTERLY";
                public const string Yearly = "YEARLY";
            }


            public static class TransactionRelationType
            {
                public const string Adjustment = "ADJUSTMENT";
                public const string Refund = "REFUND";
                public const string Reversal = "REVERSAL";
            }


            public static class TransactionCategory
            {
                public const string General = "GENERAL"; // for payments
                public const string Rent = "RENT";
                public const string Utility = "UTILITY";
                public const string Expense = "EXPENSE";
                public const string Deposit = "DEPOSIT";
                public const string Advance = "ADVANCE";
                public const string Fee = "FEE";
            }

            public static class PaymentMethod
            {
                public const string Cash = "CASH";
                public const string Mpesa = "MPESA";
                public const string Bank = "BANK";
            }

            public static class TransactionType
            {
                public const string Charge = "CHARGE";
                public const string Expense = "EXPENSE";
                public const string Payment = "PAYMENT";
                public const string Deposit = "DEPOSIT";
                public const string Deposit_refund = "DEPOSIT_REFUND";
                public const string Deposit_Adjustment = "DEPOSIT_ADJUSTMENT";
                public const string Refund = "REFUND";
                public const string Adjustment = "ADJUSTMENT";
            }


            public static class PropertyType
            {
                public const string Apartment = "APARTMENT";
                public const string Bungalow = "BUNGALOW";
                public const string Maisonette = "MAISONETTE";
                public const string Commercial = "COMMERCIAL";
                public const string Office = "OFFICE";
                public const string MixedUse = "MIXEDUSE";
            }
            public static class LogLevel
            {
                public const string Info = "INFO";
                public const string Warning = "WARNING";
                public const string Error = "ERROR";
            }
        }
    }
}