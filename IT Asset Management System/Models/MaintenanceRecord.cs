using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace IT_Asset_Management_System.Models
{
    [Table("maintenance_records")]
    public class MaintenanceRecord : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("asset_id")]
        public Guid AssetId { get; set; }

        [Column("service_date")]
        public DateOnly ServiceDate { get; set; }

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("cost")]
        public decimal? Cost { get; set; }

        [Column("technician")]
        public string? Technician { get; set; }

        [Column("repair_status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RepairStatus RepairStatus { get; set; } = RepairStatus.Pending;

        [Column("notes")]
        public string? Notes { get; set; }
    }
}
