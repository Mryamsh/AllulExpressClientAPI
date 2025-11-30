namespace AllulExpressClientApi
{

    public class LoginRequest
    {
        public required string Phonenum1 { get; set; }
        public required string Password { get; set; }
    }

    public class PasswordResetRequest
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string OTP { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
    }

}