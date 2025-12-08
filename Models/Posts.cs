using System.Text.Json.Serialization;

namespace AllulExpressClientApi.Models
{
    public class Posts
    {
        public int Id { get; set; }
        public string? Businessname { get; set; }
        public string? City { get; set; }
        public string? Phonenum1 { get; set; }
        public string Phonenum2 { get; set; } = string.Empty;
        public double Price { get; set; }
        public double Shipmentfee { get; set; }
        public int Postnum { get; set; }
        public string? Exactaddress { get; set; }
        public string? Poststatus { get; set; }
        public bool ChangeOrReturn { get; set; }
        public int Numberofpieces { get; set; }
        public DateTime Savedate { get; set; }
        public string Note { get; set; } = string.Empty;
        public int ClientId { get; set; }

        // Navigation property
        [JsonIgnore]
        public Clients? Client { get; set; }
        public int? DriverID { get; set; }

        // Navigation property
        [JsonIgnore]
        public Drivers? driver { get; set; }
        public string? Qrcode { get; internal set; }
        public string? Qrcodetext { get; internal set; }







    }
}