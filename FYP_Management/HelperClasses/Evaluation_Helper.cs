#nullable enable
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace FYP_Management.HelperClasses
{
    public static class Evaluation_Helper
    {
        private static DataTable LoadDataTable(SqlCommand cmd)
        {
            using var con = cmd.Connection;
            if (con.State != ConnectionState.Open) con.Open();

            using var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            var dt = new DataTable();
            dt.Load(rdr);
            return dt;
        }

        private static int ToInt32OrZero(object? value) =>
            value == null || value == DBNull.Value ? 0 : Convert.ToInt32(value);

        public static DataTable GetEvaluations(string str = "")
        {
            var con = Config.GetConnection();
            using var cmd = new SqlCommand(
                "SELECT * FROM Evaluation WHERE Name LIKE @str", con);
            cmd.Parameters.AddWithValue("@str", $"%{str}%");

            return LoadDataTable(cmd);
        }

        public static void insertEvaluation(string name, int totalM, int tWeightage)
        {
            int currentSum = GetAssigedPercentageEvaluations();
            if (currentSum + tWeightage > 100)
                throw new InvalidOperationException(
                    $"Cannot insert: weightage would reach {currentSum + tWeightage} (>-100).");

            using var con = Config.GetConnection();
            con.Open();
            using var cmd = new SqlCommand(
                "INSERT INTO Evaluation (Name, TotalMarks, TotalWeightage) " +
                "VALUES (@name, @tot, @weight)", con);

            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@tot", totalM);
            cmd.Parameters.AddWithValue("@weight", tWeightage);

            cmd.ExecuteNonQuery();
        }

        public static void UpdateEvaluation(string name, int totalM, int tWeightage, int id)
        {
            using var con = Config.GetConnection();
            con.Open();

            int currentSum;
            int oldWeightage;

            using (var cmdSum = new SqlCommand(
                       "SELECT SUM(TotalWeightage) FROM Evaluation", con))
                currentSum = ToInt32OrZero(cmdSum.ExecuteScalar());

            using (var cmdOld = new SqlCommand(
                       "SELECT TotalWeightage FROM Evaluation WHERE Id = @id", con))
            {
                cmdOld.Parameters.AddWithValue("@id", id);
                oldWeightage = ToInt32OrZero(cmdOld.ExecuteScalar());
            }

            /* — 2. Simulate new total — */
            int prospectiveTotal = currentSum - oldWeightage + tWeightage;
            if (prospectiveTotal > 100)
                throw new InvalidOperationException(
                    $"Cannot update: total weightage would become {prospectiveTotal} (>-100).");

            /* — 3. Safe to update — */
            using var cmd = new SqlCommand(
                @"UPDATE Evaluation
                     SET Name          = @name,
                         TotalMarks    = @tot,
                         TotalWeightage = @weight
                   WHERE Id = @id", con);

            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@tot", totalM);
            cmd.Parameters.AddWithValue("@weight", tWeightage);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }

        public static DataTable GetEvaluationFromGid(int Gid)
        {
            var con = Config.GetConnection();
            using var cmd = new SqlCommand(@"
                SELECT  E.Id,
                        GE.GroupId,
                        E.Name,
                        GE.ObtainedMarks,
                        E.TotalMarks,
                        E.TotalWeightage,
                        GE.EvaluationDate
                FROM    Evaluation       E
                JOIN    GroupEvaluation  GE ON E.Id = GE.EvaluationId
                WHERE   GE.GroupId = @gid", con);

            cmd.Parameters.AddWithValue("@gid", Gid);
            return LoadDataTable(cmd);
        }

        public static List<string> getEvaluationName()
        {
            var names = new List<string>();

            using var con = Config.GetConnection();
            con.Open();
            using var cmd = new SqlCommand(
                "SELECT Name FROM Evaluation", con);
            using var rdr = cmd.ExecuteReader();

            while (rdr.Read())
                names.Add(rdr.GetString(0));

            return names;
        }

        public static int GetEvaluationIndex(int index)
        {
            var ids = new List<int>();

            using var con = Config.GetConnection();
            con.Open();
            using var cmd = new SqlCommand(
                "SELECT Id FROM Evaluation ORDER BY Id", con);
            using var rdr = cmd.ExecuteReader();

            while (rdr.Read())
                ids.Add(rdr.GetInt32(0));

            if (index < 0 || index >= ids.Count)
                throw new ArgumentOutOfRangeException(nameof(index),
                    "Index is outside the bounds of the Evaluation list.");

            return ids[index];
        }

        public static void AddGroupEvaluation(int GId, int EId, int OMarks, DateTime dtime)
        {
            using var con = Config.GetConnection();
            con.Open();
            using var cmd = new SqlCommand(
                @"INSERT INTO GroupEvaluation
                     (GroupId, EvaluationId, ObtainedMarks, EvaluationDate)
                  VALUES (@gid, @eid, @omarks, @dtime)", con);

            cmd.Parameters.AddWithValue("@gid", GId);
            cmd.Parameters.AddWithValue("@eid", EId);
            cmd.Parameters.AddWithValue("@omarks", OMarks);
            cmd.Parameters.AddWithValue("@dtime", dtime);

            cmd.ExecuteNonQuery();
        }

        public static void UpdateGroupEvaluation(int GId, int EId, int OMarks, DateTime dtime)
        {
            using var con = Config.GetConnection();
            con.Open();
            using var cmd = new SqlCommand(
                @"UPDATE GroupEvaluation
                     SET ObtainedMarks = @omarks,
                         EvaluationDate = @dtime
                   WHERE GroupId      = @gid
                     AND EvaluationId = @eid", con);

            cmd.Parameters.AddWithValue("@gid", GId);
            cmd.Parameters.AddWithValue("@eid", EId);
            cmd.Parameters.AddWithValue("@omarks", OMarks);
            cmd.Parameters.AddWithValue("@dtime", dtime);

            cmd.ExecuteNonQuery();
        }

        public static string GetTotalMarksofEvaluation(int Eid)
        {
            using var con = Config.GetConnection();
            con.Open();
            using var cmd = new SqlCommand(
                "SELECT TotalMarks FROM Evaluation WHERE Id = @id", con);
            cmd.Parameters.AddWithValue("@id", Eid);

            return ToInt32OrZero(cmd.ExecuteScalar()).ToString();
        }

        public static int GetAssigedPercentageEvaluations()
        {
            using var con = Config.GetConnection();
            con.Open();
            using var cmd = new SqlCommand(
                "SELECT SUM(TotalWeightage) FROM Evaluation", con);

            return ToInt32OrZero(cmd.ExecuteScalar());
        }
    }
}
