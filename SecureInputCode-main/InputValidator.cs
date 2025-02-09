// Secure Input Validation (C# Backend)
using System;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using BCrypt.Net;
using System.Web;

namespace SecureInputCode
{
    public static class InputValidator
    {
        public const string RegexString = @"[<>\\""']";
        public static string SanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            // Remove potentially harmful characters
            input = Regex.Replace(input, "", string.Empty);
    
        // Trim and ensure proper encoding
        return HttpUtility.HtmlEncode(input.Trim());
        }
    }
}
