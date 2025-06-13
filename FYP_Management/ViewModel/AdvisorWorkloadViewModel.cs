using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FYP_Management.HelperClasses;

namespace FYP_Management.ViewModel
{
    public class AdvisorWorkloadViewModel
    {
        public ISeries[] Series { get; set; } 
        public Axis[] XAxes { get; set; } // Define your Axes in the ViewModel

        public AdvisorWorkloadViewModel()
        {
            // 1. Pull the data
            var rows = new List<(string Advisor, int ProjectCount)>();

                const string sql = @"
                SELECT TOP 10
                       CONCAT(p.FirstName, ' ', p.LastName) AS Advisor,
                       COUNT(DISTINCT pa.ProjectId) AS Projects
                FROM   ProjectAdvisor pa
                JOIN   Advisor a ON a.Id = pa.AdvisorId
                JOIN   Person p ON p.Id = a.Id
                GROUP BY p.FirstName, p.LastName
                ORDER BY Projects DESC;"; // Order by the count to get the highest

            // Using a helper method to get connection is good practice
            using var conn = Config.GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();

            using var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (rdr.Read())
            {
                rows.Add((rdr.GetString(0), rdr.GetInt32(1)));
            }

            // 2. Feed LiveCharts
            Series =
            [
                new ColumnSeries<int>
                {
                    Name = "Projects",
                    Values = rows.Select(r => r.ProjectCount).ToList(), // Explicitly convert to IReadOnlyCollection<int>
                }
            ];

            XAxes =
            [
                new Axis
                {
                    // Use the advisor names for the labels on the X-axis
                    Labels = rows.Select(r => r.Advisor).ToArray(),
                    LabelsRotation = 45, // Rotate labels if they overlap
                    SeparatorsPaint = null // Hides the vertical grid lines
                }
            ];
        }
    }
}