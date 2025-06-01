using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks.Dataflow;

namespace FYP_Management.HelperClasses
{
    public static class Advisor_Helper
    {
        /* ---------- INSERT ---------- */
        public static void AddAdvisor(int id, int designation, int salary)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.AddAdvisor", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@designation", designation);
            cmd.Parameters.AddWithValue("@salary", salary);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public static DataTable GetAdvisorTableDetails()
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.GetAdvisorTableDetails", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        /* ---------- UPDATE ---------- */
        public static void UpdateAdvisor(int designation, int salary, int advisorId)
        { 
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.UpdateAdvisor", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@designation", designation);
            cmd.Parameters.AddWithValue("@salary", salary);
            cmd.Parameters.AddWithValue("@id", advisorId);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        /* ---------- SEARCH (returns DataTable) ---------- */
        public static DataTable Search(string term)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.SearchAdvisors", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Term", term);
            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        /* ---------- LIGHT-WEIGHT LIST (id, name, designation, gender) ---------- */
        public static DataTable GetAdvisors(string term)
        {
            using var con = Config.GetConnection();

            using var cmd = new SqlCommand("dbo.GetAdvisors", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Pass the raw term; the proc adds the '%' wild-cards.
            cmd.Parameters.Add("@filter", SqlDbType.NVarChar, 200).Value =
                string.IsNullOrWhiteSpace(term) ? string.Empty : term;

            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);

            return dt;
        }


        public static DataTable GetIndustryAdvisors(string term)
        {
            using var con = Config.GetConnection();

            using var cmd = new SqlCommand("dbo.GetIndustryAdvisor", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@filter", term ?? string.Empty);
            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);

            return dt;
        }


        /* ---------- ASSIGN ADVISOR TO PROJECT ---------- */
        public static void AssignAdvisor(int advisorId, int projectId, int advisorRole, DateTime assignedOn)
        {
            const string sql =
                @"INSERT INTO ProjectAdvisor (AdvisorId, ProjectId, AdvisorRole, AssignmentDate)
                  VALUES (@aid, @pid, @arole, @date)";

            using var con = Config.GetConnection();
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@aid", advisorId);
            cmd.Parameters.AddWithValue("@pid", projectId);
            cmd.Parameters.AddWithValue("@arole", advisorRole);
            cmd.Parameters.AddWithValue("@date", assignedOn);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public static void DeleteAdvisor(int advisorId)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.DeleteAdvisor", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@id", advisorId);
            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
