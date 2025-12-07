using System;
using Microsoft.AspNetCore.DataProtection;
using QRCoder;

namespace AllulExpressClientApi.Services
{
    public class QrCodeService
    {
        private readonly IDataProtector _protector;

        public QrCodeService(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("QR-Code-Protector");
        }

        // Encrypt data
        public string Encrypt(string plainText)
        {
            return _protector.Protect(plainText);
        }

        // Decrypt data (optional)
        public string Decrypt(string cipherText)
        {
            return _protector.Unprotect(cipherText);
        }



        // Combine BusinessName + Date + PostId, encrypt it → generate QR
        public string CreatePostQr(string businessName, DateTime date, int postId)
        {
            Console.WriteLine($"BusinessName: {businessName}");
            Console.WriteLine($"Date: {date:yyyy-MM-dd}");
            Console.WriteLine($"PostId: {postId}");
            string rawData = $"{businessName}|{date:yyyy-MM-dd}|{postId}";
            Console.WriteLine($"Raw Data: {rawData}");
            string encrypted = Encrypt(rawData);

            byte[] qrBytes;
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                var qrData = qrGenerator.CreateQrCode(encrypted, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new PngByteQRCode(qrData);
                qrBytes = qrCode.GetGraphic(20);
            }

            return SaveQrImage(qrBytes, postId);
        }



        public string SaveQrImage(byte[] qrBytes, int postId)
        {
            string folderPath = Path.Combine("wwwroot", "qrcodes");

            // Create folder if not exists
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = $"{postId}.png";
            string filePath = Path.Combine(folderPath, fileName);

            File.WriteAllBytes(filePath, qrBytes);

            // Return accessible URL
            return $"/qrcodes/{fileName}";
        }

    }


}
