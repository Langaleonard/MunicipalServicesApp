namespace MunicipalServicesApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MainMenuForm mainMenu = new MainMenuForm();

            while (true)
            {
                Console.Clear();
                mainMenu.ShowMenu();
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine() ?? string.Empty;

                switch (choice)
                {
                    case "1":
                        new ReportIssueForm().StartReport();
                        Pause();
                        break;

                    case "2":
                        new LocalEventsForm().Start();
                        break;

                    case "3":
                        Console.WriteLine();
                        Console.WriteLine("Service Request Status will be implemented in the final PoE.");
                        Pause();
                        break;

                    case "0":
                        Console.WriteLine("Goodbye.");
                        return;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("Invalid option. Please choose 0, 1, 2 or 3.");
                        Pause();
                        break;
                }
            }
        }

        private static void Pause()
        {
            Console.WriteLine();
            Console.Write("Press Enter to return to the main menu...");
            Console.ReadLine();
        }
    }
}
