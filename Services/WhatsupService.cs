using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

public class WhatsAppService
{
    private readonly TwilioSettings _twilio;

    public WhatsAppService(IOptions<TwilioSettings> options)
    {
        _twilio = options.Value;
    }

    public void SendOtp(string phone, string otp)
    {
        var formatted = $"whatsapp:+964{phone.Substring(1)}";
        TwilioClient.Init(_twilio.AccountSid, _twilio.AuthToken);

        MessageResource.Create(
            from: new PhoneNumber("whatsapp:+14155238886"),
            to: new PhoneNumber(formatted),
            body: "Your OTP is " + otp
        );
    }
}
