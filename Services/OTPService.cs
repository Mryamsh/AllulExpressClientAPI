public static class OtpService
{
    public static string GenerateOtp()
    {
        Random rnd = new Random();
        return rnd.Next(100000, 999999).ToString(); // 6 digits
    }
}
