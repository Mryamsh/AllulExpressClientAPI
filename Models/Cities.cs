using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AllulExpressClientApi.Models
{
    public class Cities
    {
        public int Id { get; set; }
        public string? City { get; set; }
        public double totalfee { get; set; }
        public double Driverfee { get; set; }
        public double benifit { get; set; }
        [JsonIgnore]
        public List<Drivers> Drivers { get; set; } = new();


    }
}