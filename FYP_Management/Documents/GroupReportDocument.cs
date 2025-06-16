using FYP_Management.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq;

namespace FYP_Management.Documents
{
    public class GroupReportDocument : IDocument
    {
        private readonly GroupReportModel _model;

        public GroupReportDocument(GroupReportModel model)
        {
            _model = model;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(50);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                });
            });
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Final Year Project Grade Report").Bold().FontSize(20);
                    column.Item().Text($"Project: {_model.ProjectTitle}").FontSize(14);
                    column.Item().Text($"Group ID: {_model.GroupId}").FontSize(12);
                });
                row.ConstantItem(100).Text($"Date: {System.DateTime.Now:d}");
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                // Members and Advisors
                column.Item().Row(row =>
                {
                    row.RelativeItem().Element(c => ComposeList(c, "Group Members", _model.Members.Select(m => $"{m.FullName} ({m.RegistrationNo})").ToList()));
                    row.RelativeItem().Element(c => ComposeList(c, "Advisors", _model.Advisors.Select(a => $"{a.FullName} ({a.Role})").ToList()));
                });

                // Grades Table
                column.Item().PaddingTop(25).Element(ComposeGradesTable);

                // Final Score
                var finalScore = _model.EvaluationDetails.Sum(e => e.WeightedScore);
                column.Item().AlignRight().PaddingTop(10).Text($"Final Score: {finalScore:F2} / 100").Bold().FontSize(14);

                // Signature Lines
                column.Item().PaddingTop(80).Row(row =>
                {
                    row.RelativeItem().Element(container => ComposeSignatureLine(container, "Main Advisor"));
                    row.RelativeItem().Element(container => ComposeSignatureLine(container, "Coordinator"));
                });
            });
        }

        void ComposeList(IContainer container, string title, System.Collections.Generic.List<string> items)
        {
            container.Column(column =>
            {
                column.Item().Text(title).SemiBold();
                foreach (var item in items)
                    column.Item().Text(item);
            });
        }

        void ComposeGradesTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3); // Evaluation Name
                    columns.RelativeColumn();  // Total Marks
                    columns.RelativeColumn();  // Weightage
                    columns.RelativeColumn();  // Obtained Marks
                    columns.RelativeColumn();  // Weighted Score
                });

                table.Header(header =>
                {
                    header.Cell().Text("Evaluation").Bold();
                    header.Cell().Text("Total Marks").Bold();
                    header.Cell().Text("Weightage %").Bold();
                    header.Cell().Text("Obtained").Bold();
                    header.Cell().Text("Score").Bold();
                });

                foreach (var item in _model.EvaluationDetails)
                {
                    table.Cell().Text(item.Name);
                    table.Cell().Text(item.TotalMarks.ToString());
                    table.Cell().Text(item.Weightage.ToString());
                    table.Cell().Text(item.ObtainedMarks.ToString());
                    table.Cell().Text($"{item.WeightedScore:F2}");
                }
            });
        }

        void ComposeSignatureLine(IContainer container, string title)
        {
            container.Column(column =>
            {
                column.Item().BorderBottom(1).PaddingBottom(5).Text("");
                column.Item().Text(title).SemiBold();
            });
        }
    }
}