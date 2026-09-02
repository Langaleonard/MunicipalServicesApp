using MunicipalServicesApp.Models;

namespace MunicipalServicesApp
{
    public class ReportIssueForm
    {
        private readonly List<IssueReport> _issues = new();

        public void StartReport()
        {
            Console.Clear();
            Console.WriteLine("Municipal Services Application");
            Console.WriteLine("==============================");
            Console.WriteLine("Report a Municipal Issue");
            Console.WriteLine();
            ShowProgress(0, "Let's get your report started.");

            string location = GetRequiredInput("Enter location: ");
            ShowProgress(25, "Great, the location has been added.");

            Console.WriteLine();
            Console.WriteLine("Select a category:");
            Console.WriteLine("1. Roads");
            Console.WriteLine("2. Sanitation");
            Console.WriteLine("3. Water");
            Console.WriteLine("4. Electricity");
            Console.WriteLine("5. Other");

            string category = GetCategory();
            ShowProgress(50, "Category selected. You're halfway there!");

            Console.WriteLine();
            string description = GetRequiredInput("Describe the issue: ");
            ShowProgress(75, "Description added. Almost done!");

            string attachmentPath = GetAttachmentPath();
            ShowProgress(100, "Your report is ready to submit.");

            IssueReport issue = new IssueReport
            {
                ReferenceNumber = GenerateReferenceNumber(),
                Location = location,
                Category = category,
                Description = description,
                AttachmentPath = attachmentPath,
                DateReported = DateTime.Now
            };

            _issues.Add(issue);

            Console.WriteLine();
            Console.WriteLine("================================");
            Console.WriteLine("Issue submitted successfully!");
            Console.WriteLine("================================");
            Console.WriteLine($"Reference Number : {issue.ReferenceNumber}");
            Console.WriteLine($"Location         : {issue.Location}");
            Console.WriteLine($"Category         : {issue.Category}");
            Console.WriteLine($"Description      : {issue.Description}");
            Console.WriteLine($"Date Reported    : {issue.DateReported:dd MMMM yyyy HH:mm}");

            if (!string.IsNullOrWhiteSpace(issue.AttachmentPath))
            {
                Console.WriteLine($"Attachment       : {issue.AttachmentPath}");
            }
            else
            {
                Console.WriteLine("Attachment       : None");
            }

            Console.WriteLine();
            Console.WriteLine("Thank you for helping improve your community.");
        }

        private string GetRequiredInput(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine() ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }

                Console.WriteLine("This field is required. Please try again.");
            }
        }

        private string GetCategory()
        {
            while (true)
            {
                Console.Write("Category: ");
                string choice = Console.ReadLine() ?? string.Empty;

                switch (choice)
                {
                    case "1":
                        return "Roads";
                    case "2":
                        return "Sanitation";
                    case "3":
                        return "Water";
                    case "4":
                        return "Electricity";
                    case "5":
                        return "Other";
                    default:
                        Console.WriteLine("Invalid category. Please choose a number from 1 to 5.");
                        break;
                }
            }
        }

        private string GetAttachmentPath()
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write("Enter an image/document path, or press Enter to skip: ");
                string path = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(path))
                {
                    Console.WriteLine("No attachment added. You can continue without one.");
                    return string.Empty;
                }

                path = path.Trim().Trim('"');

                if (File.Exists(path))
                {
                    Console.WriteLine("Attachment added successfully.");
                    return path;
                }

                Console.WriteLine("File not found.");
                Console.Write("Would you like to try another path? (Y/N): ");
                string retry = Console.ReadLine() ?? string.Empty;

                if (!retry.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Attachment skipped.");
                    return string.Empty;
                }
            }
        }

        private void ShowProgress(int percentage, string message)
        {
            Console.WriteLine();
            Console.WriteLine($"Progress: {percentage}% - {message}");
        }

        private string GenerateReferenceNumber()
        {
            return $"MSA-{DateTime.Now:yyyyMMddHHmmssfff}";
        }
    }
}
