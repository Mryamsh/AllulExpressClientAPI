using System.ComponentModel.DataAnnotations;

namespace AllulExpressClientApi.Models
{
    public class Clients
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Business { get; set; }
        public string? Email { get; set; }
        [Phone]
        [Required]
        public string? Phonenum1 { get; set; }
        public string Phonenum2 { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string? Equipment { get; set; }
        public double Cashmoney { get; set; }
        public double Balance { get; set; }
        public double Totalpaymentpayed { get; set; }
        public int Totalposts { get; set; }
        public int Returnedposts { get; set; }
        public DateTime Savedate { get; set; }
        public string Note { get; set; } = string.Empty;
        public bool Enabled { get; set; }
        public string? Language { get; set; }
        public List<Posts> Posts { get; set; } = new();

        public string? OtpCode { get; set; }
        public DateTime? OtpExpiry { get; set; }

    }




}