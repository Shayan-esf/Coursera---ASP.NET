// Secure Database Access with Parameterized Queries
using System.Data;
using System.Data.SqlClient;

namespace SecureInputCode
{
    public class DatabaseHandler
    {
        private readonly string _connectionString;

        public DatabaseHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void InsertUser(string username, string email, string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            string query = "INSERT INTO Users (Username, Email, PasswordHash) VALUES (@Username, @Email, @PasswordHash)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.VarChar) { Value = InputValidator.SanitizeInput(username) });
                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar) { Value = InputValidator.SanitizeInput(email) });
                cmd.Parameters.Add(new SqlParameter("@PasswordHash", SqlDbType.VarChar) { Value = hashedPassword });

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool AuthenticateUser(string username, string password)
        {
            string query = "SELECT PasswordHash FROM Users WHERE Username = @Username";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.VarChar) { Value = InputValidator.SanitizeInput(username) });

                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    string storedHash = result.ToString();
                    return BCrypt.Net.BCrypt.Verify(password, storedHash);
                }
            }
            return false;
        }
    }
}
