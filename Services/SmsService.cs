using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.Extensions.Configuration;

public class SmsService
{
    private readonly IConfiguration _config;

    public SmsService(IConfiguration config)
    {
        _config = config;

        TwilioClient.Init(
            _config["Twilio:AccountSid"],
            _config["Twilio:AuthToken"]


        );
    }

    public async Task SendAsync(string phone, string message)
    {
        var normalizedPhone = NormalizeIraqiPhone(phone);
        Console.WriteLine("otp " + normalizedPhone);
        await MessageResource.CreateAsync(
            from: new Twilio.Types.PhoneNumber(_config["Twilio:FromPhone"]),
            to: new Twilio.Types.PhoneNumber(normalizedPhone),
            body: message
        );
    }


    private string NormalizeIraqiPhone(string phone)
    {
        phone = phone.Trim();

        // Remove spaces
        phone = phone.Replace(" ", "");

        // If already international
        if (phone.StartsWith("+964"))
            return phone;

        // If starts with 0 (local format)
        if (phone.StartsWith("0"))
            return "+964" + phone.Substring(1);

        throw new Exception("Invalid phone number format");
    }

}
