using MunicipalServicesApp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public class LocalEventsForm : Form
    {
        // Advanced data structures required for Part 2.
        private readonly SortedDictionary<DateTime, List<EventItem>> eventsByDate = new();
        private readonly Dictionary<string, List<EventItem>> eventsByCategory = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, int> categorySearchCounts = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> uniqueCategories = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<DateTime> uniqueEventDates = new();
        private readonly PriorityQueue<EventItem, DateTime> upcomingEventQueue = new();
        private readonly Stack<string> recentSearches = new();

        private readonly List<EventItem> allEvents = new();

        private readonly TextBox txtSearch;
        private readonly ComboBox cmbCategory;
        private readonly DateTimePicker dtpDate;
        private readonly FlowLayoutPanel eventsPanel;
        private readonly FlowLayoutPanel recommendationsPanel;
        private readonly Label lblResultCount;
        private readonly Label lblRecommendationInfo;

        private readonly Color accent = ColorTranslator.FromHtml("#ff3295");
        private readonly Color dark = ColorTranslator.FromHtml("#0f172a");
        private readonly Color pageBg = ColorTranslator.FromHtml("#eef2f7");
        private readonly Color white = ColorTranslator.FromHtml("#ffffff");
        private readonly Color muted = ColorTranslator.FromHtml("#555555");
        private readonly Color lightBorder = ColorTranslator.FromHtml("#dddddd");
        private readonly Color infoBg = ColorTranslator.FromHtml("#e0f2fe");
        private readonly Color infoText = ColorTranslator.FromHtml("#075985");

        public LocalEventsForm()
        {
            Text = "Local Events and Announcements";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(900, 650);
            Size = new Size(1100, 780);
            BackColor = pageBg;
            AutoScaleMode = AutoScaleMode.Dpi;
            MaximizeBox = true;

            SeedEvents();
            BuildDataStructures();

            var header = BuildHeader();

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(35, 22, 35, 25),
                BackColor = pageBg
            };

            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 68));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 32));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var filterCard = new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                BackColor = white,
                Padding = new Padding(18),
                Margin = new Padding(0, 0, 0, 14)
            };

            var filters = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 5,
                BackColor = white
            };

            filters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
            filters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22));
            filters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22));
            filters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11));
            filters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11));

            txtSearch = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Search title, location or keyword...",
                Margin = new Padding(0, 0, 10, 0)
            };

            cmbCategory = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10),
                Margin = new Padding(0, 0, 10, 0)
            };

            cmbCategory.Items.Add("All Categories");
            foreach (string category in uniqueCategories.OrderBy(c => c))
            {
                cmbCategory.Items.Add(category);
            }
            cmbCategory.SelectedIndex = 0;

            dtpDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Short,
                ShowCheckBox = true,
                Checked = false,
                Margin = new Padding(0, 0, 10, 0)
            };

            var btnSearch = CreateActionButton("Search", accent, white);
            btnSearch.Margin = new Padding(0, 0, 8, 0);
            btnSearch.Click += (_, _) => ApplySearch();

            var btnReset = CreateActionButton("Reset", white, dark);
            btnReset.FlatAppearance.BorderColor = lightBorder;
            btnReset.Click += (_, _) => ResetSearch();

            filters.Controls.Add(txtSearch, 0, 0);
            filters.Controls.Add(cmbCategory, 1, 0);
            filters.Controls.Add(dtpDate, 2, 0);
            filters.Controls.Add(btnSearch, 3, 0);
            filters.Controls.Add(btnReset, 4, 0);
            filterCard.Controls.Add(filters);

            var resultHeader = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                Margin = new Padding(0, 0, 0, 8)
            };
            resultHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            resultHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));

            var lblEventsHeading = new Label
            {
                Text = "Upcoming Events & Announcements",
                AutoSize = true,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = dark
            };

            lblResultCount = new Label
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 9),
                ForeColor = muted
            };

            resultHeader.Controls.Add(lblEventsHeading, 0, 0);
            resultHeader.Controls.Add(lblResultCount, 1, 0);

            eventsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = pageBg,
                Padding = new Padding(0, 0, 8, 0),
                Margin = new Padding(0, 0, 0, 15)
            };
            eventsPanel.SizeChanged += (_, _) => ResizeEventCards();

            var recommendationsContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = white,
                Padding = new Padding(16),
                Margin = new Padding(0, 0, 0, 14)
            };

            var recLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = white
            };
            recLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            recLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            recLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var recHeading = new Label
            {
                Text = "Recommended For You",
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = dark,
                Margin = new Padding(0, 0, 0, 3)
            };

            lblRecommendationInfo = new Label
            {
                Text = "Search by category to build personalised recommendations.",
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                ForeColor = infoText,
                BackColor = infoBg,
                Padding = new Padding(8),
                Margin = new Padding(0, 0, 0, 8)
            };

            recommendationsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = white
            };
            recommendationsPanel.SizeChanged += (_, _) => ResizeRecommendationCards();

            recLayout.Controls.Add(recHeading, 0, 0);
            recLayout.Controls.Add(lblRecommendationInfo, 0, 1);
            recLayout.Controls.Add(recommendationsPanel, 0, 2);
            recommendationsContainer.Controls.Add(recLayout);

            var btnBack = CreateActionButton("Back to Main Menu", white, dark);
            btnBack.Dock = DockStyle.Left;
            btnBack.Width = 190;
            btnBack.FlatAppearance.BorderColor = lightBorder;
            btnBack.Click += (_, _) => Close();

            mainLayout.Controls.Add(filterCard, 0, 0);
            mainLayout.Controls.Add(resultHeader, 0, 1);
            mainLayout.Controls.Add(eventsPanel, 0, 2);
            mainLayout.Controls.Add(recommendationsContainer, 0, 3);
            mainLayout.Controls.Add(btnBack, 0, 4);

            Controls.Add(mainLayout);
            Controls.Add(header);

            txtSearch.KeyDown += TxtSearch_KeyDown;

            DisplayEvents(GetUpcomingEvents());
            UpdateRecommendations();
        }

        private Panel BuildHeader()
        {
            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 110,
                BackColor = dark
            };

            var title = new Label
            {
                Text = "Local Events & Announcements",
                ForeColor = white,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 18)
            };

            var subtitle = new Label
            {
                Text = "Find upcoming community events and important municipal announcements.",
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(33, 65)
            };

            var accentLine = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 5,
                BackColor = accent
            };

            header.Controls.Add(title);
            header.Controls.Add(subtitle);
            header.Controls.Add(accentLine);
            return header;
        }

        private Button CreateActionButton(string text, Color backColor, Color foreColor)
        {
            return new Button
            {
                Text = text,
                Dock = DockStyle.Fill,
                MinimumSize = new Size(0, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = backColor,
                ForeColor = foreColor,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatAppearance =
                {
                    BorderSize = 1,
                    BorderColor = backColor
                }
            };
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
                    Location = "Soweto Community Hall",
                    Description = "Join residents for a neighbourhood clean-up and recycling drive.",
                    IsAnnouncement = false
                },
                new EventItem
                {
                    Id = 2,
                    Title = "Planned Water Maintenance",
                    Category = "Municipal Notice",
                    EventDate = today.AddDays(5),
                    Location = "Sandton and surrounding areas",
                    Description = "Scheduled maintenance may cause temporary interruptions to the water supply.",
                    IsAnnouncement = true
                },
                new EventItem
                {
                    Id = 3,
                    Title = "Local Business Support Workshop",
                    Category = "Business",
                    EventDate = today.AddDays(7),
                    Location = "Johannesburg Civic Centre",
                    Description = "A practical workshop for local entrepreneurs and small business owners.",
                    IsAnnouncement = false
                },
                new EventItem
                {
                    Id = 4,
                    Title = "Youth Sports Fun Day",
                    Category = "Sport",
                    EventDate = today.AddDays(10),
                    Location = "Roodepoort Sports Complex",
                    Description = "Free community sports activities for young residents and families.",
                    IsAnnouncement = false
                },
                new EventItem
                {
                    Id = 5,
                    Title = "Electricity Network Upgrade Notice",
                    Category = "Municipal Notice",
                    EventDate = today.AddDays(12),
                    Location = "Johannesburg South",
                    Description = "Residents are advised of planned electricity network upgrades in selected areas.",
                    IsAnnouncement = true
                },
                new EventItem
                {
                    Id = 6,
                    Title = "Community Health Awareness Day",
                    Category = "Health",
                    EventDate = today.AddDays(14),
                    Location = "Alexandra Community Centre",
                    Description = "Health information, basic screenings and wellness support for residents.",
                    IsAnnouncement = false
                },
                new EventItem
                {
                    Id = 7,
                    Title = "Heritage Community Market",
                    Category = "Community",
                    EventDate = today.AddDays(18),
                    Location = "Newtown",
                    Description = "A community market featuring local food, crafts and cultural activities.",
                    IsAnnouncement = false
                },
                new EventItem
                {
                    Id = 8,
                    Title = "Library Digital Skills Session",
                    Category = "Education",
                    EventDate = today.AddDays(21),
                    Location = "Johannesburg City Library",
                    Description = "A free session covering basic digital skills and online municipal services.",
                    IsAnnouncement = false
                }
            });
        }

        private void BuildDataStructures()
        {
            foreach (EventItem item in allEvents)
            {
                DateTime eventDate = item.EventDate.Date;

                if (!eventsByDate.ContainsKey(eventDate))
                {
                    eventsByDate[eventDate] = new List<EventItem>();
                }
                eventsByDate[eventDate].Add(item);

                if (!eventsByCategory.ContainsKey(item.Category))
                {
                    eventsByCategory[item.Category] = new List<EventItem>();
                }
                eventsByCategory[item.Category].Add(item);

                uniqueCategories.Add(item.Category);
                uniqueEventDates.Add(eventDate);

                if (item.EventDate >= DateTime.Today)
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

        private void ApplySearch()
        {
            string keyword = txtSearch.Text.Trim();
            string category = cmbCategory.SelectedItem?.ToString() ?? "All Categories";
            bool filterByDate = dtpDate.Checked;
            DateTime selectedDate = dtpDate.Value.Date;

            TrackSearch(keyword, category, filterByDate ? selectedDate : null);

            IEnumerable<EventItem> results;

            if (category != "All Categories" && eventsByCategory.TryGetValue(category, out List<EventItem>? categoryEvents))
            {
                results = categoryEvents;
            }
            else
            {
                results = allEvents;
            }

            results = results.Where(e => e.EventDate.Date >= DateTime.Today);

            if (filterByDate)
            {
                if (eventsByDate.TryGetValue(selectedDate, out List<EventItem>? dateEvents))
                {
                    HashSet<int> idsForDate = dateEvents.Select(e => e.Id).ToHashSet();
                    results = results.Where(e => idsForDate.Contains(e.Id));
                }
                else
                {
                    results = Enumerable.Empty<EventItem>();
                }
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                results = results.Where(e =>
                    e.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    e.Location.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    e.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    e.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            DisplayEvents(results.OrderBy(e => e.EventDate).ToList());
            UpdateRecommendations();
        }

        private void TrackSearch(string keyword, string category, DateTime? selectedDate)
        {
            string description = category;

            if (category != "All Categories")
            {
                if (!categorySearchCounts.ContainsKey(category))
                {
                    categorySearchCounts[category] = 0;
                }
                categorySearchCounts[category]++;
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                description += $" | {keyword}";

                foreach (string matchingCategory in uniqueCategories.Where(c =>
                    c.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                {
                    if (!categorySearchCounts.ContainsKey(matchingCategory))
                    {
                        categorySearchCounts[matchingCategory] = 0;
                    }
                    categorySearchCounts[matchingCategory]++;
                }
            }

            if (selectedDate.HasValue)
            {
                description += $" | {selectedDate.Value:dd MMM yyyy}";
            }

            recentSearches.Push(description);

            while (recentSearches.Count > 8)
            {
                // Rebuild the stack with only the eight newest searches.
                string[] newest = recentSearches.Take(8).Reverse().ToArray();
                recentSearches.Clear();
                foreach (string item in newest)
                {
                    recentSearches.Push(item);
                }
            }
        }

        private void ResetSearch()
        {
            txtSearch.Clear();
            cmbCategory.SelectedIndex = 0;
            dtpDate.Checked = false;
            DisplayEvents(GetUpcomingEvents());
            UpdateRecommendations();
        }

        private void TxtSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ApplySearch();
                e.SuppressKeyPress = true;
            }
        }

        private void DisplayEvents(IReadOnlyCollection<EventItem> items)
        {
            eventsPanel.SuspendLayout();
            eventsPanel.Controls.Clear();

            if (items.Count == 0)
            {
                var empty = new Label
                {
                    Text = "No matching events were found. Try another category, date or keyword.",
                    AutoSize = false,
                    Height = 70,
                    Width = Math.Max(500, eventsPanel.ClientSize.Width - 35),
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = muted,
                    BackColor = white,
                    Font = new Font("Segoe UI", 10),
                    Margin = new Padding(0, 5, 0, 5)
                };
                eventsPanel.Controls.Add(empty);
            }
            else
            {
                foreach (EventItem item in items)
                {
                    eventsPanel.Controls.Add(CreateEventCard(item));
                }
            }

            lblResultCount.Text = $"{items.Count} result{(items.Count == 1 ? string.Empty : "s")}";
            eventsPanel.ResumeLayout();
            ResizeEventCards();
        }

        private Control CreateEventCard(EventItem item)
        {
            var card = new Panel
            {
                BackColor = white,
                Height = 118,
                Width = Math.Max(650, eventsPanel.ClientSize.Width - 35),
                Margin = new Padding(0, 0, 0, 10),
                Padding = new Padding(16),
                BorderStyle = BorderStyle.FixedSingle,
                Tag = "EventCard"
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                BackColor = white
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var lblTitle = new Label
            {
                Text = item.Title,
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = dark,
                Margin = new Padding(0, 0, 0, 4)
            };

            var lblDate = new Label
            {
                Text = item.EventDate.ToString("ddd, dd MMM yyyy"),
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopRight,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = accent
            };

            string typeText = item.IsAnnouncement ? "ANNOUNCEMENT" : "EVENT";
            var lblMeta = new Label
            {
                Text = $"{typeText}  |  {item.Category}  |  {item.Location}",
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                ForeColor = muted,
                Margin = new Padding(0, 0, 0, 4)
            };

            var lblDescription = new Label
            {
                Text = item.Description,
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9),
                ForeColor = dark,
                Padding = new Padding(0, 3, 0, 0)
            };

            layout.Controls.Add(lblTitle, 0, 0);
            layout.Controls.Add(lblDate, 1, 0);
            layout.Controls.Add(lblMeta, 0, 1);
            layout.SetColumnSpan(lblMeta, 2);
            layout.Controls.Add(lblDescription, 0, 2);
            layout.SetColumnSpan(lblDescription, 2);

            card.Controls.Add(layout);
            return card;
        }

        private void UpdateRecommendations()
        {
            recommendationsPanel.SuspendLayout();
            recommendationsPanel.Controls.Clear();

            List<EventItem> recommendations;

            if (categorySearchCounts.Count == 0)
            {
                recommendations = GetUpcomingEvents().Take(3).ToList();
                lblRecommendationInfo.Text = "Popular upcoming items are shown until you search for a category.";
            }
            else
            {
                string preferredCategory = categorySearchCounts
                    .OrderByDescending(pair => pair.Value)
                    .ThenBy(pair => pair.Key)
                    .First().Key;

                recommendations = eventsByCategory.TryGetValue(preferredCategory, out List<EventItem>? preferredEvents)
                    ? preferredEvents
                        .Where(e => e.EventDate.Date >= DateTime.Today)
                        .OrderBy(e => e.EventDate)
                        .Take(3)
                        .ToList()
                    : new List<EventItem>();

                lblRecommendationInfo.Text =
                    $"Based on your recent searches, you may be interested in {preferredCategory} events.";
            }

            foreach (EventItem item in recommendations)
            {
                recommendationsPanel.Controls.Add(CreateRecommendationCard(item));
            }

            recommendationsPanel.ResumeLayout();
            ResizeRecommendationCards();
        }

        private Control CreateRecommendationCard(EventItem item)
        {
            var card = new Panel
            {
                BackColor = pageBg,
                Width = 290,
                Height = 82,
                Margin = new Padding(0, 0, 10, 8),
                Padding = new Padding(10),
                Tag = "RecommendationCard"
            };

            var title = new Label
            {
                Text = item.Title,
                AutoEllipsis = true,
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 38,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = dark
            };

            var meta = new Label
            {
                Text = $"{item.EventDate:dd MMM yyyy} | {item.Category}",
                AutoSize = false,
                Dock = DockStyle.Bottom,
                Height = 24,
                Font = new Font("Segoe UI", 8),
                ForeColor = muted
            };

            card.Controls.Add(title);
            card.Controls.Add(meta);
            return card;
        }

        private void ResizeEventCards()
        {
            int width = Math.Max(500, eventsPanel.ClientSize.Width - 30);
            foreach (Control control in eventsPanel.Controls)
            {
                if (Equals(control.Tag, "EventCard") || control is Label)
                {
                    control.Width = width;
                }
            }
        }

        private void ResizeRecommendationCards()
        {
            int available = Math.Max(260, recommendationsPanel.ClientSize.Width - 30);
            int cardWidth = available >= 900 ? (available / 3) - 12 : available >= 580 ? (available / 2) - 12 : available;

            foreach (Control control in recommendationsPanel.Controls)
            {
                if (Equals(control.Tag, "RecommendationCard"))
                {
                    control.Width = Math.Max(250, cardWidth);
                }
            }
        }
    }
}
