using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureInputCode
{
    // Role-Based Authorization (RBAC)
    public class AuthorizationHandler
    {
        public static bool Authorize(string username, string requiredRole, Dictionary<string, string> userRoles)
        {
            return userRoles.ContainsKey(username) && userRoles[username] == requiredRole;
        }
    }
}
