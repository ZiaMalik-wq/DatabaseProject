using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FYP_Management.HelperClasses
{
    public static class Lookup
    {
        public static List<string> getGenders()
        {
            var con = Config.GetConnection();
            SqlCommand cmd = new SqlCommand("Select value from Lookup where Category = 'Gender'", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            List<string> list=new List<string>();
            while (sdr.Read())
            {
                string gender = sdr.GetString(0);
                list.Add(gender);
            }
            con.Close();
            return list;
        }
        public static List<string> getDesignations()
        {
            var con = Config.GetConnection();
            SqlCommand cmd = new SqlCommand("Select value from Lookup where Category = 'DESIGNATION'", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            List<string> list=new List<string>();
            while (sdr.Read())
            {
                string gender = sdr.GetString(0);
                list.Add(gender);
            }
            con.Close();
            return list;
        }
        public static int getIndexFromValue(string str)
        {
            var con = Config.GetConnection();
            SqlCommand cmd = new SqlCommand($"Select Id from Lookup where value = '{str}'", con);
            con.Open();
            int id = (int)cmd.ExecuteScalar();
            con.Close();
            return id;
        }
    }
}
