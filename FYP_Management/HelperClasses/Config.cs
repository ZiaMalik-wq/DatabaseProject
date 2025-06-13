using System;
using System.IO;
using Microsoft.Data.SqlClient;

namespace FYP_Management.HelperClasses
{
    public static class Config
    {
        public static SqlConnection GetConnection()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string databaseName = "db_FYP.mdf";
            string fullPath = Path.Combine(baseDir, databaseName);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Database file not found: " + fullPath);

            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={fullPath};Integrated Security=True;Connect Timeout=30;";
            return new SqlConnection(connectionString);
        }
    }
}

//using System;
//using Microsoft.Data.SqlClient;
//using System.IO;

//namespace FYP_Management.HelperClasses
//{
//    public static class Config
//    {
//        public static SqlConnection GetConnection()
//        {
//            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
//            string dbFolder = Path.Combine(appDataPath, "FYP_Management");
//            string mdfPath = Path.Combine(dbFolder, "db_FYP.mdf");

//            if (!Directory.Exists(dbFolder))
//                Directory.CreateDirectory(dbFolder);

//            if (!File.Exists(mdfPath))
//                throw new FileNotFoundException("Database file not found at: " + mdfPath);

//            string connectionString =
//             $@"Data Source=(LocalDB)\MSSQLLocalDB;
//             AttachDbFilename={mdfPath};
//             Integrated Security=True;
//             Connect Timeout=30;";

//            return new SqlConnection(connectionString);
//        }
//    }
//}
