using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace FYP_Management.HelperClasses
{
    public static class Group_Helper
    {
        public static DataTable GetGroupDetails()
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.GetGroupDetails", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable("Groups");
            da.Fill(dt);
            return dt;
        }

        public static DataTable GetGroupsNotAssignedProject()
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.GetGroupsNotAssignedProject", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable("Groups");
            da.Fill(dt);
            return dt;
        }

        public static DataTable SearchGroup(int groupId)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.SearchGroup", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@GroupId", groupId);

            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable("Groups");
            da.Fill(dt);
            return dt;
        }

        public static void AddGroup(DateTime createdOn)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.AddGroup", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@CreatedOn", createdOn);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static int GetLastGroupId()
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.GetLastGroupId", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            con.Open();
            var result = cmd.ExecuteScalar();
            con.Close();

            return Convert.ToInt32(result);
        }

        public static void AddStudentToGroup(int groupId, int studentId, bool status, DateTime assignmentDate)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.AddStudentToGroup", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@GroupId", groupId);
            cmd.Parameters.AddWithValue("@StudentId", studentId);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@AssignmentDate", assignmentDate);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static DataTable GetStudentsFromGroup(int groupId)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.GetStudentsFromGroup", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@GroupId", groupId);

            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable("Students");
            da.Fill(dt);
            return dt;
        }

        public static DataTable GetGroupsNotEvaluated(string evaluationNamePattern)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.GetGroupsNotEvaluated", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@EvalNamePattern", evaluationNamePattern);

            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable("Groups");
            da.Fill(dt);
            return dt;
        }

        public static DataTable SearchGroupsNotEvaluated(string evaluationNamePattern, int groupId)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.SearchGroupsNotEvaluated", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@EvalNamePattern", evaluationNamePattern);
            cmd.Parameters.AddWithValue("@GroupId", groupId);

            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable("Groups");
            da.Fill(dt);
            return dt;
        }

        public static DataTable GetGroupsEvaluated(string evaluationNamePattern)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.GetGroupsEvaluated", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@EvalNamePattern", evaluationNamePattern);

            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable("Groups");
            da.Fill(dt);
            return dt;
        }

        public static DataTable SearchGroupsEvaluated(string evaluationNamePattern, int groupId)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.SearchGroupsEvaluated", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@EvalNamePattern", evaluationNamePattern);
            cmd.Parameters.AddWithValue("@GroupId", groupId);

            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable("Groups");
            da.Fill(dt);
            return dt;
        }

        public static void AssignProjectToGroup(int groupId, int projectId, DateTime assignDate)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.AssignProjectToGroup", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@GroupId", groupId);
            cmd.Parameters.AddWithValue("@ProjectId", projectId);
            cmd.Parameters.AddWithValue("@AssignDate", assignDate);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static DataTable GetStudentsNotInAnyGroup(string searchTerm)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.GetStudentsNotInAnyGroup", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable("Students");
            da.Fill(dt);
            return dt;
        }

        public static DataTable GetGroupWithProjects()
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.GetGroupWithProjects", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable("GroupProjects");
            da.Fill(dt);
            return dt;
        }

        public static void UpdateGroup(int groupId, DateTime createdOn)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.UpdateGroup", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id", groupId);
            cmd.Parameters.AddWithValue("@CreatedOn", createdOn);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void DeleteStudentFromGroup(int groupId, int studentId)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.DeleteStudentFromGroup", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@GroupId", groupId);
            cmd.Parameters.AddWithValue("@StudentId", studentId);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static DataTable GetGroupDetailsWithMembers()
        {
            using (var con = Config.GetConnection())
            using (var cmd = new SqlCommand("usp_GetGroupDetailsWithMembers", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;  // or bind to your DataGrid
            }
        }
    }
}
