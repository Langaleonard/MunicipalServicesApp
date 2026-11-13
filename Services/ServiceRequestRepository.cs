using MunicipalServicesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServicesApp.Services
{
    public static class ServiceRequestRepository
    {
        private static readonly List<ServiceRequest> requests = new();
        private static bool seeded;

        public static IReadOnlyList<ServiceRequest> GetAll()
        {
            EnsureSeeded();
            return requests;
        }

        public static void AddFromIssue(IssueReport issue)
        {
            EnsureSeeded();
            if (requests.Any(r => r.ReferenceNumber.Equals(issue.ReferenceNumber, StringComparison.OrdinalIgnoreCase)))
                return;

            requests.Add(new ServiceRequest
            {
                ReferenceNumber = issue.ReferenceNumber,
                Category = issue.Category,
                Location = issue.Location,
                Description = issue.Description,
                DateSubmitted = issue.DateReported,
                Status = "Submitted",
                Priority = DeterminePriority(issue.Category, issue.Description),
                AssignedDepartment = MapDepartment(issue.Category),
                ProgressPercent = 15,
                LastUpdated = issue.DateReported
            });
        }

        private static void EnsureSeeded()
        {
            if (seeded) return;
            seeded = true;
            DateTime today = DateTime.Today;

            requests.AddRange(new[]
            {
                new ServiceRequest
                {
                    ReferenceNumber = $"MSA-{today.AddDays(-8):yyyyMMdd}0001",
                    Category = "Water",
                    Location = "Sandton",
                    Description = "Major water leak reported near the main intersection.",
                    DateSubmitted = today.AddDays(-8).AddHours(9),
                    Status = "In Progress",
                    Priority = "Urgent",
                    AssignedDepartment = "Water Services",
                    ProgressPercent = 70,
                    LastUpdated = today.AddDays(-1).AddHours(14)
                },
                new ServiceRequest
                {
                    ReferenceNumber = $"MSA-{today.AddDays(-6):yyyyMMdd}0002",
                    Category = "Roads",
                    Location = "Rosebank",
                    Description = "Large pothole affecting two lanes of traffic.",
                    DateSubmitted = today.AddDays(-6).AddHours(11),
                    Status = "Assigned",
                    Priority = "High",
                    AssignedDepartment = "Roads and Transport",
                    ProgressPercent = 40,
                    LastUpdated = today.AddDays(-2).AddHours(10)
                },
                new ServiceRequest
                {
                    ReferenceNumber = $"MSA-{today.AddDays(-4):yyyyMMdd}0003",
                    Category = "Electricity",
                    Location = "Randburg",
                    Description = "Streetlights not working on the residential road.",
                    DateSubmitted = today.AddDays(-4).AddHours(16),
                    Status = "Submitted",
                    Priority = "Normal",
                    AssignedDepartment = "Electricity Services",
                    ProgressPercent = 15,
                    LastUpdated = today.AddDays(-4).AddHours(16)
                },
                new ServiceRequest
                {
                    ReferenceNumber = $"MSA-{today.AddDays(-12):yyyyMMdd}0004",
                    Category = "Sanitation",
                    Location = "Braamfontein",
                    Description = "Overflowing refuse collection point reported.",
                    DateSubmitted = today.AddDays(-12).AddHours(8),
                    Status = "Resolved",
                    Priority = "High",
                    AssignedDepartment = "Waste and Sanitation",
                    ProgressPercent = 100,
                    LastUpdated = today.AddDays(-3).AddHours(15)
                },
                new ServiceRequest
                {
                    ReferenceNumber = $"MSA-{today.AddDays(-2):yyyyMMdd}0005",
                    Category = "Roads",
                    Location = "Johannesburg CBD",
                    Description = "Damaged traffic sign requires replacement.",
                    DateSubmitted = today.AddDays(-2).AddHours(13),
                    Status = "In Progress",
                    Priority = "Normal",
                    AssignedDepartment = "Roads and Transport",
                    ProgressPercent = 70,
                    LastUpdated = today.AddDays(-1).AddHours(9)
                },
                new ServiceRequest
                {
                    ReferenceNumber = $"MSA-{today.AddDays(-1):yyyyMMdd}0006",
                    Category = "Other",
                    Location = "Newtown",
                    Description = "Public park bench damaged and unsafe.",
                    DateSubmitted = today.AddDays(-1).AddHours(10),
                    Status = "Assigned",
                    Priority = "Low",
                    AssignedDepartment = "Community Services",
                    ProgressPercent = 40,
                    LastUpdated = today.AddDays(-1).AddHours(15)
                }
            });
        }

        private static string DeterminePriority(string category, string description)
        {
            string combined = $"{category} {description}".ToLowerInvariant();
            if (combined.Contains("electric") || combined.Contains("water leak") || combined.Contains("danger")) return "Urgent";
            if (combined.Contains("road") || combined.Contains("sanitation")) return "High";
            return "Normal";
        }

        private static string MapDepartment(string category) => category.ToLowerInvariant() switch
        {
            "water" => "Water Services",
            "roads" => "Roads and Transport",
            "electricity" => "Electricity Services",
            "sanitation" => "Waste and Sanitation",
            _ => "Community Services"
        };
    }
}
