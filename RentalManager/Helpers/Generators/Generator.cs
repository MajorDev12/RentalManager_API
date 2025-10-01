using System;

namespace RentalManager.Helpers.Generators
{
    public class Generator
    {
        private static readonly Random _random = new Random();

        public static string InvoiceNumberGenerator()
        {
            string prefix = "INV";
            string datePart = DateTime.Now.ToString("yyyyMMdd");
            int randomPart = _random.Next(1000, 9999);           

            return $"{prefix}-{datePart}-{randomPart}";
        }
    }
}
