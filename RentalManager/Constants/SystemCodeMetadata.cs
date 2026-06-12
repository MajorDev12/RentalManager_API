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
            DisplayName = "Water",
            IconKey = "water",
            Color = "#2196F3",
            SortOrder = 1
        },

        [SystemCodeNames.Item.UtilityBill.Electricity] = new()
        {
            DisplayName = "Electricity",
            IconKey = "electric-bolt",
            Color = "#FFC107",
            SortOrder = 2
        },

        [SystemCodeNames.Item.UtilityBill.Gas] = new()
        {
            DisplayName = "Gas",
            IconKey = "gas-cylinder",
            Color = "#FF7043",
            SortOrder = 3
        },

        [SystemCodeNames.Item.UtilityBill.Garbage] = new()
        {
            DisplayName = "Garbage",
            IconKey = "trash",
            Color = "#9E9E9E",
            SortOrder = 4
        },

        [SystemCodeNames.Item.UtilityBill.Sewer] = new()
        {
            DisplayName = "Sewer",
            IconKey = "water-drop",
            Color = "#607D8B",
            SortOrder = 5
        },

        [SystemCodeNames.Item.UtilityBill.Internet] = new()
        {
            DisplayName = "Internet",
            IconKey = "wifi",
            Color = "#4CAF50",
            SortOrder = 6
        },

        [SystemCodeNames.Item.UtilityBill.CableTv] = new()
        {
            DisplayName = "Cable TV",
            IconKey = "tv",
            Color = "#3F51B5",
            SortOrder = 7
        },

        [SystemCodeNames.Item.UtilityBill.Security] = new()
        {
            DisplayName = "Security",
            IconKey = "shield",
            Color = "#673AB7",
            SortOrder = 8
        },

        [SystemCodeNames.Item.UtilityBill.Cleaning] = new()
        {
            DisplayName = "Cleaning",
            IconKey = "broom",
            Color = "#009688",
            SortOrder = 9
        },

        [SystemCodeNames.Item.UtilityBill.Parking] = new()
        {
            DisplayName = "Parking",
            IconKey = "parking",
            Color = "#795548",
            SortOrder = 10
        },

        [SystemCodeNames.Item.UtilityBill.Maintenance] = new()
        {
            DisplayName = "Maintenance",
            IconKey = "tools",
            Color = "#F44336",
            SortOrder = 11
        }
    };

        public static readonly Dictionary<string, SystemCodeItemMetadata>
    UnitType = new()
{
    // =========================
    // APARTMENT
    // =========================

    [SystemCodeNames.Item.UnitType.BedSitter] = new()
    {
        DisplayName = "Bedsitter",
        IconKey = "bed",
        Color = "#2196F3",
        GroupKey = "Apartment",
        SortOrder = 1
    },

    [SystemCodeNames.Item.UnitType.Studio] = new()
    {
        DisplayName = "Studio",
        IconKey = "apartment",
        Color = "#03A9F4",
        GroupKey = "Apartment",
        SortOrder = 2
    },

    [SystemCodeNames.Item.UnitType.OneBedroom] = new()
    {
        DisplayName = "1 Bedroom",
        IconKey = "king_bed",
        Color = "#4CAF50",
        GroupKey = "Apartment",
        SortOrder = 3
    },

    [SystemCodeNames.Item.UnitType.TwoBedroom] = new()
    {
        DisplayName = "2 Bedroom",
        IconKey = "weekend",
        Color = "#66BB6A",
        GroupKey = "Apartment",
        SortOrder = 4
    },

    [SystemCodeNames.Item.UnitType.ThreeBedroom] = new()
    {
        DisplayName = "3 Bedroom",
        IconKey = "villa",
        Color = "#8BC34A",
        GroupKey = "Apartment",
        SortOrder = 5
    },

    [SystemCodeNames.Item.UnitType.FourBedroom] = new()
    {
        DisplayName = "4 Bedroom",
        IconKey = "home",
        Color = "#CDDC39",
        GroupKey = "Apartment",
        SortOrder = 6
    },

    [SystemCodeNames.Item.UnitType.FiveBedroom] = new()
    {
        DisplayName = "5 Bedroom",
        IconKey = "home_work",
        Color = "#FFC107",
        GroupKey = "Apartment",
        SortOrder = 7
    },

    [SystemCodeNames.Item.UnitType.SixBedroom] = new()
    {
        DisplayName = "6 Bedroom",
        IconKey = "holiday_village",
        Color = "#FFB300",
        GroupKey = "Apartment",
        SortOrder = 8
    },

    [SystemCodeNames.Item.UnitType.SevenBedroom] = new()
    {
        DisplayName = "7 Bedroom",
        IconKey = "holiday_village",
        Color = "#FFA000",
        GroupKey = "Apartment",
        SortOrder = 9
    },

    [SystemCodeNames.Item.UnitType.EightBedroom] = new()
    {
        DisplayName = "8 Bedroom",
        IconKey = "holiday_village",
        Color = "#FF8F00",
        GroupKey = "Apartment",
        SortOrder = 10
    },

    [SystemCodeNames.Item.UnitType.NineBedroom] = new()
    {
        DisplayName = "9 Bedroom",
        IconKey = "holiday_village",
        Color = "#FF6F00",
        GroupKey = "Apartment",
        SortOrder = 11
    },

    [SystemCodeNames.Item.UnitType.TenBedroom] = new()
    {
        DisplayName = "10 Bedroom",
        IconKey = "holiday_village",
        Color = "#E65100",
        GroupKey = "Apartment",
        SortOrder = 12
    },

    [SystemCodeNames.Item.UnitType.Duplex] = new()
    {
        DisplayName = "Duplex",
        IconKey = "domain",
        Color = "#9C27B0",
        GroupKey = "Apartment",
        SortOrder = 13
    },

    [SystemCodeNames.Item.UnitType.PentHouse] = new()
    {
        DisplayName = "Penthouse",
        IconKey = "apartment",
        Color = "#673AB7",
        GroupKey = "Apartment",
        SortOrder = 14
    },

    [SystemCodeNames.Item.UnitType.Loft] = new()
    {
        DisplayName = "Loft",
        IconKey = "stairs",
        Color = "#7E57C2",
        GroupKey = "Apartment",
        SortOrder = 15
    },

    // =========================
    // HOSTEL
    // =========================

    [SystemCodeNames.Item.UnitType.SingleRoom] = new()
    {
        DisplayName = "Single Room",
        IconKey = "single_bed",
        Color = "#607D8B",
        GroupKey = "Hostel",
        SortOrder = 1
    },

    [SystemCodeNames.Item.UnitType.DoubleRoom] = new()
    {
        DisplayName = "Double Room",
        IconKey = "bedroom_parent",
        Color = "#546E7A",
        GroupKey = "Hostel",
        SortOrder = 2
    },

    [SystemCodeNames.Item.UnitType.DormitoryBed] = new()
    {
        DisplayName = "Dormitory Bed",
        IconKey = "bunk_bed",
        Color = "#455A64",
        GroupKey = "Hostel",
        SortOrder = 3
    },

    // =========================
    // HOTEL / LODGE / AIRBNB
    // =========================

    [SystemCodeNames.Item.UnitType.StandardRoom] = new()
    {
        DisplayName = "Standard Room",
        IconKey = "hotel",
        Color = "#009688",
        GroupKey = "Hotel",
        SortOrder = 1
    },

    [SystemCodeNames.Item.UnitType.DeluxeRoom] = new()
    {
        DisplayName = "Deluxe Room",
        IconKey = "hotel_class",
        Color = "#00BCD4",
        GroupKey = "Hotel",
        SortOrder = 2
    },

    [SystemCodeNames.Item.UnitType.ExecutiveRoom] = new()
    {
        DisplayName = "Executive Room",
        IconKey = "star",
        Color = "#3F51B5",
        GroupKey = "Hotel",
        SortOrder = 3
    },

    [SystemCodeNames.Item.UnitType.FamilyRoom] = new()
    {
        DisplayName = "Family Room",
        IconKey = "family_restroom",
        Color = "#795548",
        GroupKey = "Hotel",
        SortOrder = 4
    },

    // =========================
    // OFFICE BUILDING
    // =========================

    [SystemCodeNames.Item.UnitType.Office] = new()
    {
        DisplayName = "Office",
        IconKey = "business",
        Color = "#3F51B5",
        GroupKey = "Office Building",
        SortOrder = 1
    },

    [SystemCodeNames.Item.UnitType.OfficeSuite] = new()
    {
        DisplayName = "Office Suite",
        IconKey = "corporate_fare",
        Color = "#5C6BC0",
        GroupKey = "Office Building",
        SortOrder = 2
    },

    [SystemCodeNames.Item.UnitType.ConferenceRoom] = new()
    {
        DisplayName = "Conference Room",
        IconKey = "groups",
        Color = "#0097A7",
        GroupKey = "Office Building",
        SortOrder = 3
    },

    // =========================
    // COMMERCIAL
    // =========================

    [SystemCodeNames.Item.UnitType.Shop] = new()
    {
        DisplayName = "Shop",
        IconKey = "storefront",
        Color = "#FF5722",
        GroupKey = "Commercial Building",
        SortOrder = 1
    },

    // =========================
    // WAREHOUSE
    // =========================

    [SystemCodeNames.Item.UnitType.WarehouseSpace] = new()
    {
        DisplayName = "Warehouse Space",
        IconKey = "warehouse",
        Color = "#795548",
        GroupKey = "Warehouse",
        SortOrder = 1
    }
};

    }
}
