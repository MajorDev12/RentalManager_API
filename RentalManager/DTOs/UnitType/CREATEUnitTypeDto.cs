using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.UnitType
{
    public class CREATEUnitTypeDto
    {
        public int NameId { get; set; }

        public string? Notes { get; set; }

        public int PropertyId { get; set; }
    }
}

