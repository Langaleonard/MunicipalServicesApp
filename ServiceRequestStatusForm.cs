using MunicipalServicesApp.Models;
using MunicipalServicesApp.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public class ServiceRequestStatusForm : Form
    {
        private readonly TextBox txtReference;
        private readonly DataGridView gridRequests;
        private readonly Label lblResultCount;
        private readonly Label lblRequestTitle;
        private readonly Label lblRequestDetails;
        private readonly Label lblStatusRoute;
        private readonly Label lblStructureInfo;
        private readonly ProgressBar progressStatus;

        private readonly Label lblOpenCount;
        private readonly Label lblProgressCount;
        private readonly Label lblResolvedCount;
        private readonly Label lblPriorityCount;

        private ServiceRequestIndex requestIndex;
        private List<ServiceRequest> requests;

        private readonly Color accent = ColorTranslator.FromHtml("#ff3295");
        private readonly Color dark = ColorTranslator.FromHtml("#0f172a");
        private readonly Color pageBg = ColorTranslator.FromHtml("#eef2f7");
        private readonly Color white = ColorTranslator.FromHtml("#ffffff");
        private readonly Color muted = ColorTranslator.FromHtml("#555555");
        private readonly Color lightBorder = ColorTranslator.FromHtml("#dddddd");
        private readonly Color infoBg = ColorTranslator.FromHtml("#e0f2fe");
        private readonly Color infoText = ColorTranslator.FromHtml("#075985");
        private readonly Color successBg = ColorTranslator.FromHtml("#ecfdf5");
        private readonly Color successText = ColorTranslator.FromHtml("#065f46");

        public ServiceRequestStatusForm()
        {
            Text = "Service Request Status";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(900, 650);
            Size = new Size(1180, 820);
            BackColor = pageBg;
            AutoScaleMode = AutoScaleMode.Dpi;
            MaximizeBox = true;

            requests = ServiceRequestRepository.GetAll().ToList();
            requestIndex = new ServiceRequestIndex(requests);

            var header = BuildHeader();

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6,
                Padding = new Padding(28, 18, 28, 20),
                BackColor = pageBg
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 45));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 55));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var searchCard = new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                BackColor = white,
                Padding = new Padding(16),
                Margin = new Padding(0, 0, 0, 12)
            };

            var searchLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 4,
                BackColor = white
            };
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13));

            var searchLabel = new Label
            {
                Text = "Track by reference",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = dark
            };

            txtReference = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Enter a reference number, e.g. MSA-...",
                Margin = new Padding(0, 0, 10, 0)
            };
            txtReference.KeyDown += TxtReference_KeyDown;

            var btnTrack = CreateActionButton("Track Request", accent, white);
            btnTrack.Margin = new Padding(0, 0, 8, 0);
            btnTrack.Click += (_, _) => TrackRequest();

            var btnClear = CreateActionButton("Show All", white, dark);
            btnClear.FlatAppearance.BorderColor = lightBorder;
            btnClear.Click += (_, _) => ClearTracking();

            searchLayout.Controls.Add(searchLabel, 0, 0);
            searchLayout.Controls.Add(txtReference, 1, 0);
            searchLayout.Controls.Add(btnTrack, 2, 0);
            searchLayout.Controls.Add(btnClear, 3, 0);
            searchCard.Controls.Add(searchLayout);

            var summaryLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 4,
                Margin = new Padding(0, 0, 0, 12)
            };
            for (int i = 0; i < 4; i++)
                summaryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

            lblOpenCount = CreateSummaryValue();
            lblProgressCount = CreateSummaryValue();
            lblResolvedCount = CreateSummaryValue();
            lblPriorityCount = CreateSummaryValue();

            summaryLayout.Controls.Add(CreateSummaryCard("OPEN REQUESTS", lblOpenCount), 0, 0);
            summaryLayout.Controls.Add(CreateSummaryCard("IN PROGRESS", lblProgressCount), 1, 0);
            summaryLayout.Controls.Add(CreateSummaryCard("RESOLVED", lblResolvedCount), 2, 0);
            summaryLayout.Controls.Add(CreateSummaryCard("URGENT / HIGH", lblPriorityCount), 3, 0);

            var listHeader = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                Margin = new Padding(0, 0, 0, 6)
            };
            listHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            listHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));

            listHeader.Controls.Add(new Label
            {
                Text = "Submitted Service Requests",
                AutoSize = true,
                Font = new Font("Segoe UI", 12.5f, FontStyle.Bold),
                ForeColor = dark
            }, 0, 0);

            lblResultCount = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 9),
                ForeColor = muted
            };
            listHeader.Controls.Add(lblResultCount, 1, 0);

            gridRequests = BuildGrid();
            gridRequests.SelectionChanged += (_, _) => ShowSelectedRequest();

            var detailLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0, 12, 0, 12)
            };
            detailLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58));
            detailLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42));

            var trackingCard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = white,
                Padding = new Padding(18),
                Margin = new Padding(0, 0, 8, 0)
            };

            var trackingLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                BackColor = white
            };
            trackingLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            trackingLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            trackingLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            trackingLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            trackingLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            lblRequestTitle = new Label
            {
                Text = "Select a service request",
                AutoSize = true,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = dark,
                Margin = new Padding(0, 0, 0, 5)
            };

            lblRequestDetails = new Label
            {
                Text = "Choose a row above or enter a reference number to view tracking information.",
                AutoSize = true,
                MaximumSize = new Size(650, 0),
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = muted,
                Margin = new Padding(0, 0, 0, 10)
            };

            progressStatus = new ProgressBar
            {
                Dock = DockStyle.Top,
                Height = 22,
                Minimum = 0,
                Maximum = 100,
                Margin = new Padding(0, 0, 0, 7)
            };

            lblStatusRoute = new Label
            {
                Text = "Status route will appear here.",
                AutoSize = true,
                MaximumSize = new Size(650, 0),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = infoText,
                BackColor = infoBg,
                Padding = new Padding(10),
                Margin = new Padding(0, 0, 0, 8)
            };

            var statusHint = new Label
            {
                Text = "Submitted  >  Assigned  >  In Progress  >  Resolved",
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                ForeColor = muted
            };

            trackingLayout.Controls.Add(lblRequestTitle, 0, 0);
            trackingLayout.Controls.Add(lblRequestDetails, 0, 1);
            trackingLayout.Controls.Add(progressStatus, 0, 2);
            trackingLayout.Controls.Add(lblStatusRoute, 0, 3);
            trackingLayout.Controls.Add(statusHint, 0, 4);
            trackingCard.Controls.Add(trackingLayout);

            var technicalCard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = white,
                Padding = new Padding(18),
                Margin = new Padding(8, 0, 0, 0)
            };

            var technicalLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = white
            };
            technicalLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            technicalLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            technicalLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            technicalLayout.Controls.Add(new Label
            {
                Text = "Data Structure Insights",
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = dark,
                Margin = new Padding(0, 0, 0, 6)
            }, 0, 0);

            lblStructureInfo = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(520, 0),
                Font = new Font("Consolas", 9),
                ForeColor = infoText,
                BackColor = infoBg,
                Padding = new Padding(10),
                Margin = new Padding(0, 0, 0, 8)
            };
            technicalLayout.Controls.Add(lblStructureInfo, 0, 1);

            technicalLayout.Controls.Add(new Label
            {
                Text = "The indexes are rebuilt from the current repository so a newly reported issue becomes trackable immediately.",
                AutoSize = true,
                MaximumSize = new Size(520, 0),
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = muted
            }, 0, 2);

            technicalCard.Controls.Add(technicalLayout);
            detailLayout.Controls.Add(trackingCard, 0, 0);
            detailLayout.Controls.Add(technicalCard, 1, 0);

            var footer = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2
            };
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            var btnBack = CreateActionButton("Back to Main Menu", white, dark);
            btnBack.Dock = DockStyle.Left;
            btnBack.Width = 190;
            btnBack.FlatAppearance.BorderColor = lightBorder;
            btnBack.Click += (_, _) => Close();

            var lblFooter = new Label
            {
                Text = "Final PoE - Service Request Status",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 9),
                ForeColor = muted
            };

            footer.Controls.Add(btnBack, 0, 0);
            footer.Controls.Add(lblFooter, 1, 0);

            mainLayout.Controls.Add(searchCard, 0, 0);
            mainLayout.Controls.Add(summaryLayout, 0, 1);
            mainLayout.Controls.Add(listHeader, 0, 2);
            mainLayout.Controls.Add(gridRequests, 0, 3);
            mainLayout.Controls.Add(detailLayout, 0, 4);
            mainLayout.Controls.Add(footer, 0, 5);

            Controls.Add(mainLayout);
            Controls.Add(header);

            RefreshRequests();
        }

        private Control BuildHeader()
        {
            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 108,
                BackColor = dark,
                Padding = new Padding(28, 10, 28, 8)
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = dark,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 62));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 38));

            layout.Controls.Add(new Label
            {
                Text = "Service Request Status",
                Dock = DockStyle.Fill,
                ForeColor = white,
                Font = new Font("Segoe UI", 21, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                Margin = Padding.Empty
            }, 0, 0);

            layout.Controls.Add(new Label
            {
                Text = "Track municipal requests by unique reference number and follow their progress.",
                Dock = DockStyle.Fill,
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 9.5f),
                TextAlign = ContentAlignment.TopLeft,
                AutoEllipsis = true,
                Margin = new Padding(2, 0, 0, 0)
            }, 0, 1);

            var accentLine = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 5,
                BackColor = accent
            };

            header.Controls.Add(layout);
            header.Controls.Add(accentLine);
            return header;
        }

        private DataGridView BuildGrid()
        {
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = white,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 34,
                RowTemplate = { Height = 34 }
            };

            grid.ColumnHeadersDefaultCellStyle.BackColor = dark;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = white;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            grid.DefaultCellStyle.ForeColor = dark;
            grid.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#fff1f7");
            grid.DefaultCellStyle.SelectionForeColor = dark;
            grid.GridColor = lightBorder;

            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Reference",
                HeaderText = "Reference Number",
                DataPropertyName = "ReferenceNumber",
                FillWeight = 150
            });
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Category",
                DataPropertyName = "Category",
                FillWeight = 80
            });
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Location",
                DataPropertyName = "Location",
                FillWeight = 100
            });
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Priority",
                DataPropertyName = "Priority",
                FillWeight = 65
            });
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Status",
                DataPropertyName = "Status",
                FillWeight = 85
            });
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Submitted",
                DataPropertyName = "DateSubmitted",
                FillWeight = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd MMM yyyy" }
            });

            return grid;
        }

        private void RefreshRequests()
        {
            requests = ServiceRequestRepository.GetAll().ToList();
            requestIndex = new ServiceRequestIndex(requests);

            gridRequests.DataSource = null;
            gridRequests.DataSource = requestIndex.GetSortedRequests().ToList();
            lblResultCount.Text = $"{requests.Count} request(s) indexed";

            lblOpenCount.Text = requests.Count(r => !r.Status.Equals("Resolved", StringComparison.OrdinalIgnoreCase)).ToString();
            lblProgressCount.Text = requests.Count(r => r.Status.Equals("In Progress", StringComparison.OrdinalIgnoreCase)).ToString();
            lblResolvedCount.Text = requests.Count(r => r.Status.Equals("Resolved", StringComparison.OrdinalIgnoreCase)).ToString();
            lblPriorityCount.Text = requests.Count(r => r.Priority is "Urgent" or "High").ToString();
            lblStructureInfo.Text = requestIndex.GetTechnicalSummary();

            if (gridRequests.Rows.Count > 0)
                gridRequests.Rows[0].Selected = true;
        }

        private void TrackRequest()
        {
            string reference = txtReference.Text.Trim();
            if (string.IsNullOrWhiteSpace(reference))
            {
                MessageBox.Show(
                    "Please enter a service request reference number.",
                    "Reference Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtReference.Focus();
                return;
            }

            ServiceRequest? request = requestIndex.FindByReference(reference);
            if (request == null)
            {
                MessageBox.Show(
                    "No service request was found with that reference number.\n\nCheck the reference and try again.",
                    "Request Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            foreach (DataGridViewRow row in gridRequests.Rows)
            {
                if (row.DataBoundItem is ServiceRequest item &&
                    item.ReferenceNumber.Equals(request.ReferenceNumber, StringComparison.OrdinalIgnoreCase))
                {
                    row.Selected = true;
                    gridRequests.CurrentCell = row.Cells[0];
                    break;
                }
            }

            DisplayRequest(request);
        }

        private void ClearTracking()
        {
            txtReference.Clear();
            RefreshRequests();
        }

        private void ShowSelectedRequest()
        {
            if (gridRequests.CurrentRow?.DataBoundItem is ServiceRequest request)
                DisplayRequest(request);
        }

        private void DisplayRequest(ServiceRequest request)
        {
            lblRequestTitle.Text = $"{request.ReferenceNumber}  -  {request.Status}";
            lblRequestDetails.Text =
                $"Category: {request.Category}\n" +
                $"Location: {request.Location}\n" +
                $"Priority: {request.Priority}\n" +
                $"Assigned to: {request.AssignedDepartment}\n" +
                $"Submitted: {request.DateSubmitted:dd MMM yyyy HH:mm}\n" +
                $"Last updated: {request.LastUpdated:dd MMM yyyy HH:mm}\n" +
                $"Description: {request.Description}";

            progressStatus.Value = Math.Max(0, Math.Min(100, request.ProgressPercent));
            lblStatusRoute.Text = $"Graph route to resolution: {requestIndex.GetRouteToResolution(request.Status)}";
            lblStatusRoute.BackColor = request.Status.Equals("Resolved", StringComparison.OrdinalIgnoreCase)
                ? successBg
                : infoBg;
            lblStatusRoute.ForeColor = request.Status.Equals("Resolved", StringComparison.OrdinalIgnoreCase)
                ? successText
                : infoText;
        }

        private void TxtReference_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                TrackRequest();
            }
        }

        private Label CreateSummaryValue()
        {
            return new Label
            {
                Text = "0",
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = accent
            };
        }

        private Control CreateSummaryCard(string title, Label valueLabel)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 78,
                BackColor = white,
                Padding = new Padding(14, 9, 14, 9),
                Margin = new Padding(0, 0, 10, 0)
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = white
            };
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            layout.Controls.Add(new Label
            {
                Text = title,
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = muted
            }, 0, 0);
            layout.Controls.Add(valueLabel, 0, 1);
            card.Controls.Add(layout);
            return card;
        }

        private static Button CreateActionButton(string text, Color background, Color foreground)
        {
            return new Button
            {
                Text = text,
                Dock = DockStyle.Fill,
                MinimumSize = new Size(0, 38),
                FlatStyle = FlatStyle.Flat,
                BackColor = background,
                ForeColor = foreground,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
        }
    }
}
