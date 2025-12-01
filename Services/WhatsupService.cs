using System;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

public class WhatsAppService
{
    private readonly TwilioSettings _settings;
    private bool _isInitialized = false;

    public WhatsAppService(IOptions<TwilioSettings> options)
    {
        _settings = options.Value;

        Console.WriteLine($"[WhatsAppService] Constructor start - {DateTime.UtcNow:O}");

        try
        {
            Console.WriteLine("[WhatsAppService] Initializing Twilio client...");
            Console.WriteLine("[WhatsAppService] AccountSid present? " + (!string.IsNullOrWhiteSpace(_settings?.AccountSid)));
            Console.WriteLine("[WhatsAppService] AuthToken present? " + (!string.IsNullOrWhiteSpace(_settings?.AuthToken)));
            Console.WriteLine("[WhatsAppService] WhatsAppNumber = " + _settings?.WhatsAppNumber);

            // Only init if credentials exist
            if (!string.IsNullOrWhiteSpace(_settings?.AccountSid) && !string.IsNullOrWhiteSpace(_settings?.AuthToken))
            {
                TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);
                _isInitialized = true;
                Console.WriteLine("[WhatsAppService] TwilioClient.Init succeeded.");
            }
            else
            {
                Console.WriteLine("[WhatsAppService] Twilio credentials missing — skipping Twilio init.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[WhatsAppService] Exception during Twilio init:");
            Console.WriteLine(ex.ToString());
            // Do not rethrow — keep service alive so app doesn't crash on startup
        }

        Console.WriteLine($"[WhatsAppService] Constructor finished - {DateTime.UtcNow:O}");
    }

    public void SendMessage(string to, string message)
    {
        Console.WriteLine($"[WhatsAppService] SendMessage start - {DateTime.UtcNow:O}");
        Console.WriteLine("[WhatsAppService] _isInitialized = " + _isInitialized);
        Console.WriteLine("[WhatsAppService] from (WhatsAppNumber) = " + _settings?.WhatsAppNumber);
        Console.WriteLine("[WhatsAppService] to (raw) = " + to);
        Console.WriteLine("[WhatsAppService] to (formatted) = " + $"whatsapp:{to}");
        Console.WriteLine("[WhatsAppService] message = " + (message?.Length > 100 ? message.Substring(0, 100) + "..." : message));

        if (!_isInitialized)
        {
            Console.WriteLine("[WhatsAppService] Twilio not initialized - skipping send.");
            Console.WriteLine($"[WhatsAppService] SendMessage end - {DateTime.UtcNow:O}");
            return;
        }

        try
        {
            var msg = MessageResource.Create(
                from: new PhoneNumber(_settings.WhatsAppNumber),
                to: new PhoneNumber($"whatsapp:{to}"),
                body: message
            );

            Console.WriteLine("[WhatsAppService] MessageResource.Create returned. Sid = " + msg?.Sid);
            Console.WriteLine("[WhatsAppService] Message status = " + msg?.Status);
        }
        catch (Exception ex)
        {
            Console.WriteLine("[WhatsAppService] Exception during SendMessage:");
            Console.WriteLine(ex.ToString());
        }

        Console.WriteLine($"[WhatsAppService] SendMessage finished - {DateTime.UtcNow:O}");
    }
}
