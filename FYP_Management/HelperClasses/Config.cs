using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace FYP_MS.HelperClasses
{
    public static class Config
    {
        public static SqlConnection GetConnection() => new(App.Config.GetConnectionString("database"));
    }
}
