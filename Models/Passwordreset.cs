public class ResetPasswordRequest
{
    public string Phone { get; set; }
    public string NewPassword { get; set; }
}
public class VerifyOtpRequest
{
    public string Phone { get; set; }
    public string Otp { get; set; }
}

