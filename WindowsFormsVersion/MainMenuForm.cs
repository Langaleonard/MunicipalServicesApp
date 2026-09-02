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
            Text = "Municipal Services Application";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(720, 520);
            Size = new Size(820, 600);
            BackColor = Color.White;

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 130,
                BackColor = Color.FromArgb(24, 73, 112)
            };

            var title = new Label
            {
                Text = "MUNICIPAL SERVICES",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(35, 28)
            };

            var subtitle = new Label
            {
                Text = "Report and access municipal services in your community",
                ForeColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 11),
                AutoSize = true,
                Location = new Point(38, 78)
            };

            header.Controls.Add(title);
            header.Controls.Add(subtitle);

            var content = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(55, 35, 55, 35),
                ColumnCount = 1,
                RowCount = 4
            };

            content.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));

            btnReportIssues = CreateMenuButton(
                "Report Issues",
                "Report potholes, water leaks, electricity faults and other issues."
            );
            btnReportIssues.Click += BtnReportIssues_Click;

            btnLocalEvents = CreateMenuButton(
                "Local Events and Announcements",
                "Coming Soon"
            );
            btnLocalEvents.Enabled = false;

            btnServiceStatus = CreateMenuButton(
                "Service Request Status",
                "Coming Soon"
            );
            btnServiceStatus.Enabled = false;

            var footer = new Label
            {
                Text = "Part 1 - Report Issues",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.DimGray,
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
                FlatAppearance = { BorderSize = 1 },
                BackColor = Color.FromArgb(245, 248, 250),
                ForeColor = Color.FromArgb(30, 30, 30),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 20, 0)
            };
        }

        private void BtnReportIssues_Click(object? sender, System.EventArgs e)
        {
            using var form = new ReportIssueForm();
            Hide();
            form.ShowDialog();
            Show();
        }
    }
}
