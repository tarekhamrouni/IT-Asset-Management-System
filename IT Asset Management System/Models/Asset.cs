namespace IT_Asset_Management_System.Models
{
    public class Asset
    {
        public Guid Id { get; set; }
        public string AssetTag { get; set; } = string.Empty;
        public AssetType Type { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public AssetStatus Status { get; set; } = AssetStatus.Available;
        public string? AssignedTo { get; set; }
        public string? Department { get; set; }
        public DateOnly? PurchaseDate { get; set; }
        public DateOnly? WarrantyExpires { get; set; }
        public string? Location { get; set; }
    }
}
