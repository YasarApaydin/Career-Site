using System;
using System.Security.Cryptography;
using System.Text;



namespace Kariyer.Helpers
{
    public static class TokenHelper
    {
        public static (string token, DateTime expirationTime) GenerateToken(int size = 32, int expirationMinutes = 10)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var tokenData = new byte[size];
                rng.GetBytes(tokenData);
                var token = Convert.ToBase64String(tokenData);
                var expirationTime = DateTime.UtcNow.AddMinutes(expirationMinutes); 
                return (token, expirationTime);
            }
        }

        public static bool ValidateToken(string token, string expectedToken, DateTime expirationTime)
        {
            if (DateTime.UtcNow > expirationTime) 
            {
                return false;
            }

            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var expectedTokenBytes = Encoding.UTF8.GetBytes(expectedToken);
            return CryptographicOperations.FixedTimeEquals(tokenBytes, expectedTokenBytes);
        }
    }
}
