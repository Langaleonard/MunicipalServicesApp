using System.Drawing;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public class MainMenuForm : Form
    {
        private readonly Button btnReportIssues;
        private readonly Button btnLocalEvents;
        private readonly Button btnServiceStatus;

        public MainMenuForm()
        {
            Color accent = ColorTranslator.FromHtml("#ff3295");
            Color dark = ColorTranslator.FromHtml("#0f172a");
            Color pageBg = ColorTranslator.FromHtml("#eef2f7");
            Color white = ColorTranslator.FromHtml("#ffffff");
            Color subtitle = ColorTranslator.FromHtml("#555555");
            Color disabled = ColorTranslator.FromHtml("#999999");
            Color lightBorder = ColorTranslator.FromHtml("#dddddd");

            Text = "Municipal Services Application";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(760, 540);
            Size = new Size(900, 640);
            BackColor = pageBg;
            AutoScaleMode = AutoScaleMode.Dpi;

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 140,
                BackColor = dark
            };

            var title = new Label
            {
                Text = "MUNICIPAL SERVICES",
                ForeColor = white,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(35, 28)
            };

            var subtitleLabel = new Label
            {
                Text = "Report issues, discover local events and access municipal services",
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 11),
                AutoSize = true,
                Location = new Point(38, 80)
            };

            var accentLine = new Panel
            {
                BackColor = accent,
                Height = 5,
                Dock = DockStyle.Bottom
            };

            header.Controls.Add(title);
            header.Controls.Add(subtitleLabel);
            header.Controls.Add(accentLine);

            var content = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(55, 35, 55, 35),
                ColumnCount = 1,
                RowCount = 4,
                BackColor = pageBg
            };

            content.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));

            btnReportIssues = CreateMenuButton(
                "Report Issues",
                "Report potholes, water leaks, electricity faults and other issues."
            );
            btnReportIssues.BackColor = white;
            btnReportIssues.ForeColor = dark;
            btnReportIssues.FlatAppearance.BorderColor = accent;
            btnReportIssues.Click += BtnReportIssues_Click;

            btnLocalEvents = CreateMenuButton(
                "Local Events and Announcements",
                "Browse, search and receive recommendations for upcoming local activities."
            );
            btnLocalEvents.BackColor = accent;
            btnLocalEvents.ForeColor = white;
            btnLocalEvents.FlatAppearance.BorderColor = accent;
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
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 20, 0),
                Cursor = Cursors.Hand
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
