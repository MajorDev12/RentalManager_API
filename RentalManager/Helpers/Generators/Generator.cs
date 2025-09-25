using System;

namespace RentalManager.Helpers.Generators
{
    public class Generator
    {
        private readonly Random _random = new Random();

        public string InvoiceNumberGenerator()
        {
            string prefix = "INV";
            string datePart = DateTime.Now.ToString("yyyyMMdd");
            int randomPart = _random.Next(1000, 9999);           

            return $"{prefix}-{datePart}-{randomPart}";
        }
    }
}
