using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Data.SqlClient;
using SkiaSharp;
using System.Collections.Generic;
using System.Data; // Required for CommandType
using FYP_Management.HelperClasses;

namespace FYP_Management.ViewModel
{
    public class StudentGenderViewModel
    {
        public ISeries[] Series { get; set; }

        public StudentGenderViewModel()
        {
            var genderData = new List<(string Gender, int Count)>();

            // --- Call the Stored Procedure ---
            using (var conn = Config.GetConnection())
            {
                // Set up the command to call the stored procedure
                using (var cmd = new SqlCommand("dbo.GetStudentGenderDistribution", conn))
                {
                    // This line is CRITICAL. It tells ADO.NET to treat the command
                    // text as the name of a stored procedure, not a SQL query.
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // The column names match the aliases in our stored procedure
                            genderData.Add((
                                reader["GenderName"].ToString(),
                                (int)reader["StudentCount"]
                            ));
                        }
                    }
                }
            }

            // --- Configure the LiveCharts Pie Series (This part is identical to before) ---
            var seriesList = new List<ISeries>();
            foreach (var dataPoint in genderData)
            {
                seriesList.Add(new PieSeries<int>
                {
                    Name = dataPoint.Gender,
                    Values = new[] { dataPoint.Count },
                    DataLabelsPaint = new SolidColorPaint(SKColors.White),
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = p => $"{p.Model:N0}" // Format number with commas
                });
            }

            Series = seriesList.ToArray();
        }
    }
}