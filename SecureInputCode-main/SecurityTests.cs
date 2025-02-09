// Unit Tests for SQL Injection, XSS, Authentication, and Authorization
using NUnit.Framework;
using System.Collections.Generic;

namespace SecureInputCode
{
    [TestFixture]
    public class SecurityTests
    {
        [Test]
        public void TestForSQLInjection()
        {
            string maliciousInput = "' OR 1=1 --";
            string sanitizedInput = InputValidator.SanitizeInput(maliciousInput);
            Assert.That(sanitizedInput.Contains("'"), Is.False);
        }

        [Test]
        public void TestForXSS()
        {
            string maliciousInput = "<script>alert('XSS')</script>";
            string sanitizedInput = InputValidator.SanitizeInput(maliciousInput);
            Assert.That(sanitizedInput, Is.Not.EqualTo(maliciousInput));

        }

        [Test]
        public void TestAuthentication()
        {
            string password = "SecurePass123";
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            Assert.That(BCrypt.Net.BCrypt.Verify(password, hash), Is.True);
        }

        [Test]
        public void TestAuthorization()
        {
            var userRoles = new Dictionary<string, string>
            {
                { "adminUser", "admin" },
                { "regularUser", "user" }
            };
            Assert.That(AuthorizationHandler.Authorize("adminUser", "admin", userRoles), Is.True);
            Assert.That(AuthorizationHandler.Authorize("regularUser", "admin", userRoles), Is.False);
        }
    }

}
