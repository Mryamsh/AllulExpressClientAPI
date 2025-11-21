public class WhatsAppService
{
    public async Task SendOtpAsync(string phoneNumber, string otp)
    {
        // Twilio WhatsApp API example
        // Use your account SID, auth token, and WhatsApp number
        var message = $"Your OTP is: {otp}";
        // Twilio code to send message...
    }
}
