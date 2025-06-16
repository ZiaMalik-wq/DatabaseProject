using System;
using System.Collections.Generic;

namespace FYP_Management.Models
{
    // Main container for all report data
    public class GroupReportModel
    {
        public string ProjectTitle { get; set; } = "N/A";
        public DateTime GroupCreatedOn { get; set; }
        public int GroupId { get; set; }

        public List<StudentMember> Members { get; set; } = new List<StudentMember>();
        public List<AdvisorInfo> Advisors { get; set; } = new List<AdvisorInfo>();
        public List<EvaluationDetail> EvaluationDetails { get; set; } = new List<EvaluationDetail>();
    }

    // Using 'record' is a modern, concise way to define simple data-holding types
    public record StudentMember(string FullName, string RegistrationNo);
    public record AdvisorInfo(string FullName, string Role);
    public record EvaluationDetail(string Name, int TotalMarks, int Weightage, int ObtainedMarks, double WeightedScore);
}