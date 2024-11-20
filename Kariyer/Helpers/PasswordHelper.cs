using System;
using System.Linq;
using System.Text.RegularExpressions;
using BCrypt.Net;

namespace Kariyer.Helpers
{
    public static class PasswordHelper
    {
        private const int MinLength = 8;
        private static readonly string Karakter = "!@#$%^&*()_+-=<>?/[]{}|";

        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));
            }

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            if (enteredPassword == null || storedHash == null)
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }

        public static bool ValidatePassword(string parola)
        {
            if (string.IsNullOrEmpty(parola))
                return false;

      
            if (parola.Length < MinLength)
                return false;

            
            if (!parola.Any(char.IsUpper))
                return false;

            
            if (!parola.Any(char.IsLower))
                return false;

            
            if (!parola.Any(char.IsDigit))
                return false;

           
            if (!parola.Any(c => Karakter.Contains(c)))
                return false;

            return true;
        }
    }
}
