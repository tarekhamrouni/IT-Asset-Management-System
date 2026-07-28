using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace IT_Asset_Management_System.Models
{
    [Table("assets")]
    public class Asset : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("asset_tag")]
        public string AssetTag { get; set; } = string.Empty;

        [Column("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AssetType Type { get; set; }

        [Column("brand")]
        public string Brand { get; set; } = string.Empty;

        [Column("model")]
        public string Model { get; set; } = string.Empty;

        [Column("serial_number")]
        public string SerialNumber { get; set; } = string.Empty;

        [Column("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AssetStatus Status { get; set; } = AssetStatus.Available;

        [Column("assigned_to")]
        public string? AssignedTo { get; set; }

        [Column("department")]
        public string? Department { get; set; }

        [Column("purchase_date")]
        public DateOnly? PurchaseDate { get; set; }

        [Column("warranty_expires")]
        public DateOnly? WarrantyExpires { get; set; }

        [Column("location")]
        public string? Location { get; set; }
    }
}