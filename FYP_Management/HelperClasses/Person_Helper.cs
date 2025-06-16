#nullable enable
using System;
using Microsoft.Data.SqlClient;
using System.Data;
using FYP_Management.Models;

namespace FYP_Management.HelperClasses
{
    public static class Person_Helper
    {
        /// <summary>
        /// Adds a new person to the database and optionally returns their new ID.
        /// This is the new, safe primary method for adding a person.
        /// </summary>
        public static int AddPerson(Person person)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.AddPersonAndGetId", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@FirstName", person.FirstName);
            cmd.Parameters.AddWithValue("@LastName", person.LastName);
            cmd.Parameters.AddWithValue("@Contact", person.Contact);
            cmd.Parameters.AddWithValue("@Email", person.Email);
            cmd.Parameters.AddWithValue("@DateOfBirth", person.DateOfBirth);
            cmd.Parameters.AddWithValue("@Gender", person.Gender);

            // OUTPUT parameter must match the proc's @PersonId
            var outputParam = new SqlParameter("@PersonId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            con.Open();
            cmd.ExecuteNonQuery();

            // pull the new ID back out
            return Convert.ToInt32(outputParam.Value);
        }


        public static void UpdatePerson(Person person)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.UpdatePerson", con)
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

        /// <summary>
        /// Gets all people from the table. Avoids using 'SELECT *'.
        /// </summary>
        public static DataTable GetFullTable()
        {
            // Explicitly list columns instead of using SELECT *
            const string sql = "SELECT Id, FirstName, LastName, Contact, Email, DateOfBirth, Gender FROM dbo.Person";

            using var con = Config.GetConnection();
            // Use 'using' block for SqlCommand as well
            using var cmd = new SqlCommand(sql, con);

            var dt = new DataTable();
            con.Open();
            using (var sdr = cmd.ExecuteReader())
            {
                dt.Load(sdr);
            }
            return dt;
        }

        /// <summary>
        /// Adds a person as part of a larger database transaction.
        /// This method is perfect and requires no changes.
        /// </summary>
        public static int AddPersonAndGetId(string firstName, string lastName, string contact, string email, DateTime dob, int gender,
                                            SqlConnection con, SqlTransaction tran)
        {
            using (var cmd = new SqlCommand("dbo.AddPersonAndGetId", con, tran))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Contact", contact);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@DateOfBirth", dob);
                cmd.Parameters.AddWithValue("@Gender", gender);

                var outputParam = new SqlParameter("@PersonId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                cmd.ExecuteNonQuery();

                return (int)outputParam.Value;
            }
        }
    }
}