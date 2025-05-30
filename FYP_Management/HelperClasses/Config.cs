using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
namespace FYP_Management.HelperClasses
{
    public static class Config
    {
        public static SqlConnection GetConnection()
        {
            string cs = App.Config.GetConnectionString("database");
            if (string.IsNullOrWhiteSpace(cs))
                throw new InvalidOperationException("Database connection string is missing.");
            return new SqlConnection(cs);
        }
    }
}
