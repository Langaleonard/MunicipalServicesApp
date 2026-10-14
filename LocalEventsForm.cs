using MunicipalServicesApp.Models;

namespace MunicipalServicesApp
{
    // Console test harness for macOS.
    // The final Windows Forms version is inside WindowsFormsVersion/.
    public class LocalEventsForm
    {
        private readonly List<EventItem> allEvents = new();
        private readonly SortedDictionary<DateTime, List<EventItem>> eventsByDate = new();
        private readonly Dictionary<string, List<EventItem>> eventsByCategory = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, int> categorySearchCounts = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> uniqueCategories = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<DateTime> uniqueEventDates = new();
        private readonly PriorityQueue<EventItem, DateTime> upcomingEventQueue = new();
        private readonly Stack<string> recentSearches = new();

        public LocalEventsForm()
        {
            SeedEvents();
            BuildDataStructures();
        }

        public void Start()
        {
            while (true)
            {
                Console.Clear();
                WriteHeading();
                DisplayEvents(GetUpcomingEvents().Take(8).ToList(), "Upcoming Events and Announcements");

                Console.WriteLine();
                Console.WriteLine("1. Search events");
                Console.WriteLine("2. View recommendations");
                Console.WriteLine("3. View categories and dates");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine();
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine() ?? string.Empty;

                switch (choice)
                {
                    case "1":
                        SearchEvents();
                        break;
                    case "2":
                        ShowRecommendations();
                        break;
                    case "3":
                        ShowOrganisationSummary();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        Pause();
                        break;
                }
            }
        }

        private void WriteHeading()
        {
            Console.WriteLine("Local Events and Announcements");
            Console.WriteLine("==============================");
            Console.WriteLine("Search municipal events by keyword, category and date.");
            Console.WriteLine();
        }

        private void SearchEvents()
        {
            Console.Clear();
            WriteHeading();

            Console.Write("Keyword (press Enter for any): ");
            string keyword = (Console.ReadLine() ?? string.Empty).Trim();

            Console.WriteLine();
            Console.WriteLine("Categories:");
            Console.WriteLine("0. All Categories");

            List<string> categories = uniqueCategories.OrderBy(c => c).ToList();
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i]}");
            }

            Console.Write("Category number: ");
            string categoryInput = Console.ReadLine() ?? "0";

            string selectedCategory = "All Categories";
            if (int.TryParse(categoryInput, out int categoryIndex) &&
                categoryIndex >= 1 &&
                categoryIndex <= categories.Count)
            {
                selectedCategory = categories[categoryIndex - 1];
            }

            Console.Write("Date yyyy-MM-dd (press Enter for any): ");
            string dateInput = (Console.ReadLine() ?? string.Empty).Trim();

            DateTime? selectedDate = null;
            if (!string.IsNullOrWhiteSpace(dateInput))
            {
                if (DateTime.TryParse(dateInput, out DateTime parsedDate))
                {
                    selectedDate = parsedDate.Date;
                }
                else
                {
                    Console.WriteLine("Invalid date. The date filter will be ignored.");
                }
            }

            IEnumerable<EventItem> results = allEvents
                .Where(e => e.EventDate.Date >= DateTime.Today);

            if (!selectedCategory.Equals("All Categories", StringComparison.OrdinalIgnoreCase))
            {
                results = eventsByCategory.TryGetValue(selectedCategory, out List<EventItem>? categoryEvents)
                    ? categoryEvents.Where(e => e.EventDate.Date >= DateTime.Today)
                    : Enumerable.Empty<EventItem>();
            }

            if (selectedDate.HasValue)
            {
                results = results.Where(e => e.EventDate.Date == selectedDate.Value);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                results = results.Where(e =>
                    e.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    e.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    e.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    e.Location.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            TrackSearch(keyword, selectedCategory, selectedDate);

            Console.Clear();
            WriteHeading();
            DisplayEvents(results.OrderBy(e => e.EventDate).ToList(), "Search Results");
            Console.WriteLine();
            DisplayRecommendations(GetRecommendations());
            Pause();
        }

        private void ShowRecommendations()
        {
            Console.Clear();
            WriteHeading();
            DisplayRecommendations(GetRecommendations());

            if (recentSearches.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Recent Searches (Stack)");
                Console.WriteLine("-----------------------");
                foreach (string search in recentSearches.Take(5))
                {
                    Console.WriteLine($"- {search}");
                }
            }

            Pause();
        }

        private void ShowOrganisationSummary()
        {
            Console.Clear();
            WriteHeading();

            Console.WriteLine("Unique Categories (HashSet)");
            Console.WriteLine("---------------------------");
            foreach (string category in uniqueCategories.OrderBy(c => c))
            {
                Console.WriteLine($"- {category}");
            }

            Console.WriteLine();
            Console.WriteLine("Event Dates (SortedDictionary)");
            Console.WriteLine("------------------------------");
            foreach (KeyValuePair<DateTime, List<EventItem>> pair in eventsByDate)
            {
                Console.WriteLine($"{pair.Key:dd MMM yyyy}: {pair.Value.Count} item(s)");
            }

            Console.WriteLine();
            Console.WriteLine($"Unique event dates stored in HashSet: {uniqueEventDates.Count}");
            Console.WriteLine($"Upcoming items indexed in PriorityQueue: {upcomingEventQueue.Count}");
            Pause();
        }

        private void SeedEvents()
        {
            DateTime today = DateTime.Today;

            allEvents.AddRange(new[]
            {
                new EventItem
                {
                    Id = 1,
                    Title = "Community Clean-Up Day",
                    Category = "Community",
                    EventDate = today.AddDays(3),
                    Location = "Johannesburg Civic Centre",
                    Description = "Residents are invited to join a community clean-up and recycling drive.",
                    IsAnnouncement = false
                },
                new EventItem
                {
                    Id = 2,
                    Title = "Scheduled Water Maintenance",
                    Category = "Municipal Notice",
                    EventDate = today.AddDays(5),
                    Location = "Sandton",
                    Description = "Planned water maintenance may cause temporary interruptions between 09:00 and 15:00.",
                    IsAnnouncement = true
                },
                new EventItem
                {
                    Id = 3,
                    Title = "Local Business Support Workshop",
                    Category = "Business",
                    EventDate = today.AddDays(7),
                    Location = "Braamfontein Library",
                    Description = "A free workshop for local entrepreneurs covering municipal support and business resources.",
                    IsAnnouncement = false
                },
                new EventItem
                {
                    Id = 4,
                    Title = "Youth Sports Day",
                    Category = "Sports",
                    EventDate = today.AddDays(9),
                    Location = "Patterson Park",
                    Description = "A community sports day with football, netball and family activities.",
                    IsAnnouncement = false
                },
                new EventItem
                {
                    Id = 5,
                    Title = "Electricity Network Upgrade Notice",
                    Category = "Municipal Notice",
                    EventDate = today.AddDays(11),
                    Location = "Randburg",
                    Description = "Residents are advised of a planned electricity network upgrade in selected areas.",
                    IsAnnouncement = true
                },
                new EventItem
                {
                    Id = 6,
                    Title = "Community Safety Forum",
                    Category = "Community",
                    EventDate = today.AddDays(14),
                    Location = "Rosebank Community Hall",
                    Description = "Meet community representatives and discuss local safety concerns and initiatives.",
                    IsAnnouncement = false
                },
                new EventItem
                {
                    Id = 7,
                    Title = "Public Library Digital Skills Session",
                    Category = "Education",
                    EventDate = today.AddDays(16),
                    Location = "Johannesburg City Library",
                    Description = "A beginner-friendly session covering online municipal services and basic digital skills.",
                    IsAnnouncement = false
                },
                new EventItem
                {
                    Id = 8,
                    Title = "Park Maintenance Announcement",
                    Category = "Municipal Notice",
                    EventDate = today.AddDays(18),
                    Location = "Emmarentia",
                    Description = "Sections of the public park will be temporarily closed while maintenance is completed.",
                    IsAnnouncement = true
                },
                new EventItem
                {
                    Id = 9,
                    Title = "Heritage Community Market",
                    Category = "Community",
                    EventDate = today.AddDays(21),
                    Location = "Newtown",
                    Description = "A local market featuring food, crafts, music and small businesses from the community.",
                    IsAnnouncement = false
                },
                new EventItem
                {
                    Id = 10,
                    Title = "Environmental Awareness Talk",
                    Category = "Education",
                    EventDate = today.AddDays(24),
                    Location = "Zoo Lake",
                    Description = "Learn practical ways households can reduce waste and support a cleaner city.",
                    IsAnnouncement = false
                }
            });
        }

        private void BuildDataStructures()
        {
            foreach (EventItem item in allEvents.OrderBy(e => e.EventDate))
            {
                DateTime date = item.EventDate.Date;

                if (!eventsByDate.TryGetValue(date, out List<EventItem>? dateEvents))
                {
                    dateEvents = new List<EventItem>();
                    eventsByDate[date] = dateEvents;
                }
                dateEvents.Add(item);

                if (!eventsByCategory.TryGetValue(item.Category, out List<EventItem>? categoryEvents))
                {
                    categoryEvents = new List<EventItem>();
                    eventsByCategory[item.Category] = categoryEvents;
                }
                categoryEvents.Add(item);

                uniqueCategories.Add(item.Category);
                uniqueEventDates.Add(date);

                if (date >= DateTime.Today)
                {
                    upcomingEventQueue.Enqueue(item, item.EventDate);
                }
            }
        }

        private List<EventItem> GetUpcomingEvents()
        {
            var copy = new PriorityQueue<EventItem, DateTime>();

            foreach (EventItem item in allEvents.Where(e => e.EventDate.Date >= DateTime.Today))
            {
                copy.Enqueue(item, item.EventDate);
            }

            var ordered = new List<EventItem>();
            while (copy.Count > 0)
            {
                ordered.Add(copy.Dequeue());
            }

            return ordered;
        }

        private void TrackSearch(string keyword, string category, DateTime? selectedDate)
        {
            var parts = new List<string>();

            if (!category.Equals("All Categories", StringComparison.OrdinalIgnoreCase))
            {
                parts.Add(category);
                categorySearchCounts[category] = categorySearchCounts.GetValueOrDefault(category) + 1;
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                parts.Add($"keyword: {keyword}");

                foreach (string matchingCategory in uniqueCategories.Where(c =>
                    c.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                {
                    categorySearchCounts[matchingCategory] = categorySearchCounts.GetValueOrDefault(matchingCategory) + 1;
                }
            }

            if (selectedDate.HasValue)
            {
                parts.Add(selectedDate.Value.ToString("dd MMM yyyy"));
            }

            string description = parts.Count == 0 ? "All upcoming events" : string.Join(" | ", parts);
            recentSearches.Push(description);

            if (recentSearches.Count > 8)
            {
                string[] newest = recentSearches.Take(8).Reverse().ToArray();
                recentSearches.Clear();
                foreach (string item in newest)
                {
                    recentSearches.Push(item);
                }
            }
        }

        private List<EventItem> GetRecommendations()
        {
            if (categorySearchCounts.Count == 0)
            {
                return GetUpcomingEvents().Take(3).ToList();
            }

            string preferredCategory = categorySearchCounts
                .OrderByDescending(pair => pair.Value)
                .ThenBy(pair => pair.Key)
                .First().Key;

            return eventsByCategory.TryGetValue(preferredCategory, out List<EventItem>? preferredEvents)
                ? preferredEvents
                    .Where(e => e.EventDate.Date >= DateTime.Today)
                    .OrderBy(e => e.EventDate)
                    .Take(3)
                    .ToList()
                : new List<EventItem>();
        }

        private void DisplayEvents(IReadOnlyCollection<EventItem> items, string heading)
        {
            Console.WriteLine(heading);
            Console.WriteLine(new string('-', heading.Length));

            if (items.Count == 0)
            {
                Console.WriteLine("No matching events were found.");
                return;
            }

            foreach (EventItem item in items)
            {
                string type = item.IsAnnouncement ? "ANNOUNCEMENT" : "EVENT";
                Console.WriteLine($"[{type}] {item.Title}");
                Console.WriteLine($"Date     : {item.EventDate:ddd, dd MMM yyyy}");
                Console.WriteLine($"Category : {item.Category}");
                Console.WriteLine($"Location : {item.Location}");
                Console.WriteLine($"Details  : {item.Description}");
                Console.WriteLine();
            }
        }

        private void DisplayRecommendations(IReadOnlyCollection<EventItem> recommendations)
        {
            Console.WriteLine("Recommended For You");
            Console.WriteLine("-------------------");

            if (categorySearchCounts.Count == 0)
            {
                Console.WriteLine("Popular upcoming items are shown until you search for a category.");
            }
            else
            {
                string preferredCategory = categorySearchCounts
                    .OrderByDescending(pair => pair.Value)
                    .ThenBy(pair => pair.Key)
                    .First().Key;

                Console.WriteLine($"Based on your searches, you may be interested in {preferredCategory} events.");
            }

            foreach (EventItem item in recommendations)
            {
                Console.WriteLine($"- {item.EventDate:dd MMM yyyy} | {item.Title} | {item.Category}");
            }
        }

        private static void Pause()
        {
            Console.WriteLine();
            Console.Write("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
