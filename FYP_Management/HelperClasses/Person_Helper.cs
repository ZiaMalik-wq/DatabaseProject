using System;
using Microsoft.Data.SqlClient;
using System.Data;
using FYP_Management.Models;
namespace FYP_Management.HelperClasses
{
    public static class Person_Helper
    {
        public static void AddPerson(Person person)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("usp_AddPerson", con)
            {
                CommandType = CommandType.StoredProcedure,
            };

            cmd.Parameters.AddWithValue("@FirstName", person.FirstName);
            cmd.Parameters.AddWithValue("@LastName", person.LastName);
            cmd.Parameters.AddWithValue("@Contact", person.Contact);
            cmd.Parameters.AddWithValue("@Email", person.Email);
            cmd.Parameters.AddWithValue("@DateOfBirth", person.DateOfBirth);
            cmd.Parameters.AddWithValue("@Gender", person.Gender);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public static int getLastPersonId()
        {
            string insertSql = "SELECT MAX(ID) AS LastID FROM Person";
            var con = Config.GetConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand(insertSql, con);
            int ans = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return ans;
        }

        public static void UpdatePerson(Person person)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("usp_UpdatePerson", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@FirstName", person.FirstName);
            cmd.Parameters.AddWithValue("@LastName", person.LastName);
            cmd.Parameters.AddWithValue("@Contact", person.Contact);
            cmd.Parameters.AddWithValue("@Email", person.Email);
            cmd.Parameters.AddWithValue("@DateOfBirth", person.DateOfBirth);
            cmd.Parameters.AddWithValue("@Gender", person.Gender);
            cmd.Parameters.AddWithValue("@PersonId", person.Id);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public static DataTable GetFullTable()
        {
            var con = Config.GetConnection();
            SqlCommand cmd = new SqlCommand("Select * from person", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;
        }
        
    }
}
