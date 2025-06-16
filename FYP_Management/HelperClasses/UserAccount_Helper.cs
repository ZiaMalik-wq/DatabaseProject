using Microsoft.Data.SqlClient;

namespace FYP_Management.HelperClasses
{
    public static class UserAccount_Helper
    {
        // Verifies a user's login credentials
        public static bool VerifyLogin(string username, string password)
        {
            string storedHash = null;
            string storedSalt = null;

            using (var conn = Config.GetConnection())
            {
                string sql = "SELECT PasswordHash, PasswordSalt FROM UserAccount WHERE Username = @Username AND StatusId = 16";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    storedHash = reader["PasswordHash"].ToString();
                    storedSalt = reader["PasswordSalt"].ToString();
                }
            }

            // If username was not found, or there's no hash/salt, fail.
            if (string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(storedSalt))
            {
                return false;
            }

            // Verify the entered password against the stored hash and salt
            return PasswordHelper.VerifyPassword(password, storedHash, storedSalt);
        }

        // A helper method to create your first admin user
        public static void CreateAdminUser(int personId, string username, string password)
        {
            var (hash, salt) = PasswordHelper.HashPassword(password);
            const int adminRoleId = 15; // The ID you added to the Lookup table

            using (var conn = Config.GetConnection())
            {
                string sql = "INSERT INTO UserAccount (PersonId, Username, PasswordHash, PasswordSalt, RoleId) VALUES (@PersonId, @Username, @Hash, @Salt, @RoleId)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PersonId", personId);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Hash", hash);
                    cmd.Parameters.AddWithValue("@Salt", salt);
                    cmd.Parameters.AddWithValue("@RoleId", adminRoleId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void CreateAdminUser(int personId, string username, string password,
                                   SqlConnection con, SqlTransaction tran)
        {
            var (hash, salt) = PasswordHelper.HashPassword(password);
            const int adminRoleId = 15;
            const int activeStatusId = 16;

            string sql = "INSERT INTO UserAccount (PersonId, Username, PasswordHash, PasswordSalt, RoleId, StatusId) VALUES (@PersonId, @Username, @Hash, @Salt, @RoleId, @StatusId)";

            using var cmd = new SqlCommand(sql, con, tran);
            cmd.Parameters.AddWithValue("@PersonId", personId);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Hash", hash);
            cmd.Parameters.AddWithValue("@Salt", salt);
            cmd.Parameters.AddWithValue("@RoleId", adminRoleId);
            cmd.Parameters.AddWithValue("@StatusId", activeStatusId);
            cmd.ExecuteNonQuery();
        }
    }
}