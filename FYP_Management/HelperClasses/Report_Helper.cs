using FYP_Management.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FYP_Management.HelperClasses
{
    public static class Report_Helper
    {
        public static GroupReportModel GetGroupReportData(int groupId)
        {
            var model = new GroupReportModel { GroupId = groupId };

            using (var con = Config.GetConnection())
            {
                using var cmd = new SqlCommand("dbo.GetGroupReportData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GroupId", groupId);
                con.Open();

                using var reader = cmd.ExecuteReader();
                // 1. Read Project/Group Info (First Result Set)
                if (reader.Read())
                {
                    model.ProjectTitle = reader["ProjectTitle"]?.ToString() ?? "Not Assigned";
                    model.GroupCreatedOn = (System.DateTime)reader["GroupCreatedOn"];
                }

                // 2. Read Student Members (Second Result Set)
                reader.NextResult();
                while (reader.Read())
                {
                    model.Members.Add(new StudentMember(
                        $"{reader["FirstName"]} {reader["LastName"]}",
                        reader["RegistrationNo"].ToString()
                    ));
                }

                // 3. Read Advisors (Third Result Set)
                reader.NextResult();
                while (reader.Read())
                {
                    model.Advisors.Add(new AdvisorInfo(
                        $"{reader["FirstName"]} {reader["LastName"]}",
                        reader["AdvisorRole"].ToString()
                    ));
                }

                // 4. Read Evaluations (Fourth Result Set)
                reader.NextResult();
                while (reader.Read())
                {
                    model.EvaluationDetails.Add(new EvaluationDetail(
                        reader["EvaluationName"].ToString(),
                        (int)reader["TotalMarks"],
                        (int)reader["TotalWeightage"],
                        (int)reader["ObtainedMarks"],
                        (double)reader["WeightedScore"]
                    ));
                }
            }
            return model;
        }
    }
}