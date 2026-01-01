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
        await MessageResource.CreateAsync(
            from: new Twilio.Types.PhoneNumber(_config["Twilio:FromPhone"]),
            to: new Twilio.Types.PhoneNumber(phone),
            body: message
        );
    }
}
