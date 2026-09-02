using System;

namespace MunicipalServicesApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MainMenuForm mainMenu = new MainMenuForm();
            mainMenu.ShowMenu();

            Console.WriteLine();
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine() ?? string.Empty;

            if (choice == "1")
            {
                ReportIssueForm reportIssueForm = new ReportIssueForm();
                reportIssueForm.StartReport();
            }
            else
            {
                Console.WriteLine("This feature is not available yet.");
            }
        }
    }
}