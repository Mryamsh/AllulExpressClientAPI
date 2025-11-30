using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

public class WhatsAppService
{
    private readonly TwilioSettings _settings;

    public WhatsAppService(IOptions<TwilioSettings> options)
    {
        _settings = options.Value;
        TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);
    }

    public void SendMessage(string to, string message)
    {
        var msg = MessageResource.Create(
            from: new PhoneNumber(_settings.FromWhatsApp),
            to: new PhoneNumber(to),
            body: message
        );
    }
}
