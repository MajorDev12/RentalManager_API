using RentalManager.DTOs.SystemCodeItem;

namespace RentalManager.Constants
{
    public static class SystemCodeMetadata
    {
        public static readonly Dictionary<string, SystemCodeItemMetadata>
    UtilityBill = new()
    {
        [SystemCodeNames.Item.UtilityBill.Water] = new()
        {
            Name = "Water",
            IconKey = "water",
            Color = "#2196F3",
            SortOrder = 1
        },

        [SystemCodeNames.Item.UtilityBill.Electricity] = new()
        {
            Name = "Electricity",
            IconKey = "electric-bolt",
            Color = "#FFC107",
            SortOrder = 2
        },

        [SystemCodeNames.Item.UtilityBill.Gas] = new()
        {
            Name = "Gas",
            IconKey = "gas-cylinder",
            Color = "#FF7043",
            SortOrder = 3
        },

        [SystemCodeNames.Item.UtilityBill.Garbage] = new()
        {
            Name = "Garbage",
            IconKey = "trash",
            Color = "#9E9E9E",
            SortOrder = 4
        },

        [SystemCodeNames.Item.UtilityBill.Sewer] = new()
        {
            Name = "Sewer",
            IconKey = "water-drop",
            Color = "#607D8B",
            SortOrder = 5
        },

        [SystemCodeNames.Item.UtilityBill.Internet] = new()
        {
            Name = "Internet",
            IconKey = "wifi",
            Color = "#4CAF50",
            SortOrder = 6
        },

        [SystemCodeNames.Item.UtilityBill.CableTv] = new()
        {
            Name = "Cable TV",
            IconKey = "tv",
            Color = "#3F51B5",
            SortOrder = 7
        },

        [SystemCodeNames.Item.UtilityBill.Security] = new()
        {
            Name = "Security",
            IconKey = "shield",
            Color = "#673AB7",
            SortOrder = 8
        },

        [SystemCodeNames.Item.UtilityBill.Cleaning] = new()
        {
            Name = "Cleaning",
            IconKey = "broom",
            Color = "#009688",
            SortOrder = 9
        },

        [SystemCodeNames.Item.UtilityBill.Parking] = new()
        {
            Name = "Parking",
            IconKey = "parking",
            Color = "#795548",
            SortOrder = 10
        },

        [SystemCodeNames.Item.UtilityBill.Maintenance] = new()
        {
            Name = "Maintenance",
            IconKey = "tools",
            Color = "#F44336",
            SortOrder = 11
        }
    };
    }
}
