using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllulExpressClientApi.Models
{
    public class Drivers
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        [Phone]
        [Required]
        public string? Phonenum1 { get; set; }
        public string Phonenum2 { get; set; } = string.Empty;
        public string? Password { get; set; }

        public double Paymentpayed { get; set; }
        public double Paymentremained { get; set; }
        public int Arrivedpost { get; set; }
        public int Remainedpost { get; set; }
        public string? Vehicledetail { get; set; }
        public List<Cities>? Cities { get; set; } = new();
        public bool IsActive { get; set; }
        public string? IDimagefront { get; set; }
        public string? IDimageback { get; set; }
        public DateTime Savedate { get; set; }
        public string Note { get; set; } = string.Empty;
        public bool Enabled { get; set; }

        public string? Language { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }

        public DateTime? LastUpdated { get; set; }


    }
    public class DriverLocationDto
    {
        public int Id { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }

}