using System;
using System.IO;
using Microsoft.Data.SqlClient;

namespace FYP_Management.HelperClasses
{
    public static class Config
    {
        public static SqlConnection GetConnection()
        {
            string path = Environment.CurrentDirectory;
            string databaseName = "db_FYP.mdf";
            string fullPath = Path.Combine(path, databaseName);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Database file not found: " + fullPath);

            var builder = new SqlConnectionStringBuilder
            {
                DataSource = @"(LocalDB)\MSSQLLocalDB",       // Full SQL Server instance
                InitialCatalog = "FYP",          // The DB name you attached
                IntegratedSecurity = true
            };

            return new SqlConnection(builder.ConnectionString);
        }
    }
}
