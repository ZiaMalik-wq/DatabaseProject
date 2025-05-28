using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace FYP_MS.HelperClasses
{
    public static class Advisor_Helper
    {
        /* ---------- INSERT ---------- */
        public static void AddAdvisor(int id, int designation, int salary)
        {
            const string sql = "INSERT INTO Advisor (Id, Designation, Salary) VALUES (@id, @designation, @salary)";
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@designation", designation);
            cmd.Parameters.AddWithValue("@salary", salary);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        /* ---------- SIMPLE SELECT ---------- */
        public static DataTable GetAdvisorTable()
        {
            const string sql = "SELECT * FROM Advisor";
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand(sql, con);
            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static DataTable GetAdvisorTableDetails()
        {
            const string sql =
                @"SELECT p.Id, p.FirstName, p.LastName,
                         lo.Value   AS Designation,
                         a.Salary,
                         p.Contact, p.Email, p.DateOfBirth,
                         l.Value    AS Gender
                  FROM   Advisor  a
                  JOIN   Person   p ON a.Id          = p.Id
                  JOIN   Lookup   l ON p.Gender      = l.Id
                  JOIN   Lookup  lo ON a.Designation = lo.Id";

            using var con = Config.GetConnection();
            using var cmd = new SqlCommand(sql, con);
            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        /* ---------- UPDATE ---------- */
        public static void UpdateAdvisor(int designation, int salary, int advisorId)
        {
            const string sql =
                @"UPDATE Advisor
                    SET Designation = @designation,
                        Salary      = @salary
                  WHERE Id = @id";

            using var con = Config.GetConnection();
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@designation", designation);
            cmd.Parameters.AddWithValue("@salary", salary);
            cmd.Parameters.AddWithValue("@id", advisorId);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        /* ---------- SEARCH (returns DataTable) ---------- */
        public static DataTable Search(string term)
        {
            const string sql =
                @"SELECT p.Id, p.FirstName, p.LastName,
                         lo.Value AS Designation,
                         a.Salary, p.Contact, p.Email,
                         p.DateOfBirth, l.Value AS Gender
                  FROM   Advisor  a
                  JOIN   Person   p  ON a.Id          = p.Id
                  JOIN   Lookup   l  ON p.Gender      = l.Id
                  JOIN   Lookup  lo  ON a.Designation = lo.Id
                  WHERE  CONCAT(p.FirstName, p.LastName, p.Email,
                                lo.Value, p.Contact) LIKE @filter";

            using var con = Config.GetConnection();
            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@filter", $"%{term}%");

            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        /* ---------- LIGHT-WEIGHT LIST (id, name, designation, gender) ---------- */
        public static DataTable GetAdvisors(string term)
        {
            const string sql =
                @"SELECT p.Id, p.FirstName, p.LastName,
                         lo.Value AS Designation,
                         l.Value  AS Gender
                  FROM   Advisor  a
                  JOIN   Person   p  ON a.Id          = p.Id
                  JOIN   Lookup   l  ON p.Gender      = l.Id
                  JOIN   Lookup  lo  ON a.Designation = lo.Id
                  WHERE  CONCAT(p.FirstName, p.LastName, p.Email,
                                lo.Value, p.Contact) LIKE @filter";

            using var con = Config.GetConnection();
            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@filter", $"%{term}%");

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
    }
}
