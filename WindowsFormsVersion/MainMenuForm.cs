using System.Drawing;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public class MainMenuForm : Form
    {
        private readonly Button btnReportIssues;
        private readonly Button btnLocalEvents;
        private readonly Button btnServiceStatus;

        private readonly Color accent = ColorTranslator.FromHtml("#ff3295");
        private readonly Color dark = ColorTranslator.FromHtml("#0f172a");
        private readonly Color pageBg = ColorTranslator.FromHtml("#eef2f7");
        private readonly Color white = ColorTranslator.FromHtml("#ffffff");
        private readonly Color subtitle = ColorTranslator.FromHtml("#555555");
        private readonly Color disabled = ColorTranslator.FromHtml("#999999");
        private readonly Color lightBorder = ColorTranslator.FromHtml("#dddddd");

        public MainMenuForm()
        {
            Text = "Municipal Services Application";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(820, 600);
            Size = new Size(980, 720);
            BackColor = pageBg;
            AutoScaleMode = AutoScaleMode.Dpi;
            MaximizeBox = true;

            var header = BuildHeader();

            var content = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(45, 30, 45, 28),
                ColumnCount = 1,
                RowCount = 4,
                BackColor = pageBg
            };

            content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 34));
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));

            btnReportIssues = CreateMenuButton(
                "Report Issues",
                "Report potholes, water leaks, electricity faults and other issues."
            );
            btnReportIssues.BackColor = white;
            btnReportIssues.ForeColor = dark;
            btnReportIssues.FlatAppearance.BorderColor = accent;
            btnReportIssues.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#fff1f7");
            btnReportIssues.Click += BtnReportIssues_Click;

            btnLocalEvents = CreateMenuButton(
                "Local Events and Announcements",
                "Browse, search and receive recommendations for upcoming local activities."
            );
            btnLocalEvents.BackColor = accent;
            btnLocalEvents.ForeColor = white;
            btnLocalEvents.FlatAppearance.BorderColor = accent;
            btnLocalEvents.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#e82987");
            btnLocalEvents.Click += BtnLocalEvents_Click;

            btnServiceStatus = CreateMenuButton(
                "Service Request Status",
                "Coming Soon - this feature will be implemented in the final PoE."
            );
            btnServiceStatus.Enabled = false;
            btnServiceStatus.BackColor = white;
            btnServiceStatus.ForeColor = disabled;
            btnServiceStatus.FlatAppearance.BorderColor = lightBorder;

            var footer = new Label
            {
                Text = "Part 2 - Local Events and Announcements",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = subtitle,
                Font = new Font("Segoe UI", 9)
            };

            content.Controls.Add(btnReportIssues, 0, 0);
            content.Controls.Add(btnLocalEvents, 0, 1);
            content.Controls.Add(btnServiceStatus, 0, 2);
            content.Controls.Add(footer, 0, 3);

            Controls.Add(content);
            Controls.Add(header);
        }

        private Control BuildHeader()
        {
            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 132,
                BackColor = dark,
                Padding = new Padding(34, 14, 34, 9)
            };

            var textLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = dark,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            textLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            textLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 62));
            textLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 38));

            var title = new Label
            {
                Text = "MUNICIPAL SERVICES",
                Dock = DockStyle.Fill,
                ForeColor = white,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                Margin = Padding.Empty
            };

            var subtitleLabel = new Label
            {
                Text = "Report issues, discover local events and access municipal services",
                Dock = DockStyle.Fill,
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.TopLeft,
                AutoEllipsis = true,
                Margin = new Padding(2, 0, 0, 0)
            };

            textLayout.Controls.Add(title, 0, 0);
            textLayout.Controls.Add(subtitleLabel, 0, 1);

            var accentLine = new Panel
            {
                BackColor = accent,
                Height = 5,
                Dock = DockStyle.Bottom
            };

            header.Controls.Add(textLayout);
            header.Controls.Add(accentLine);
            return header;
        }

        private static Button CreateMenuButton(string title, string description)
        {
            return new Button
            {
                Text = $"{title}\n{description}",
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 8, 0, 8),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance =
                {
                    BorderSize = 1
                },
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(22, 8, 22, 8),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
        }

        private void BtnReportIssues_Click(object? sender, System.EventArgs e)
        {
            using var form = new ReportIssueForm();
            Hide();
            form.ShowDialog();
            Show();
        }

        private void BtnLocalEvents_Click(object? sender, System.EventArgs e)
        {
            using var form = new LocalEventsForm();
            Hide();
            form.ShowDialog();
            Show();
        }
    }
}
