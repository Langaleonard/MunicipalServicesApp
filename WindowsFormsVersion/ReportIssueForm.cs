using MunicipalServicesApp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public class ReportIssueForm : Form
    {
        private static readonly List<IssueReport> Issues = new();

        private readonly TextBox txtLocation;
        private readonly ComboBox cmbCategory;
        private readonly RichTextBox txtDescription;
        private readonly TextBox txtAttachment;
        private readonly Button btnAttach;
        private readonly Button btnSubmit;
        private readonly Button btnBack;
        private readonly ProgressBar progressBar;
        private readonly Label lblProgress;
        private readonly OpenFileDialog openFileDialog;

        public ReportIssueForm()
        {
            Text = "Report a Municipal Issue";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(760, 680);
            Size = new Size(860, 760);
            BackColor = Color.White;

            openFileDialog = new OpenFileDialog
            {
                Title = "Select an image or document",
                Filter = "Supported Files|*.jpg;*.jpeg;*.png;*.pdf;*.doc;*.docx|All Files|*.*"
            };

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 105,
                BackColor = Color.FromArgb(24, 73, 112)
            };

            var title = new Label
            {
                Text = "Report a Municipal Issue",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 20)
            };

            var subtitle = new Label
            {
                Text = "Give us enough detail to help the municipality respond.",
                ForeColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(33, 63)
            };

            header.Controls.Add(title);
            header.Controls.Add(subtitle);

            var formLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(45, 25, 45, 30),
                ColumnCount = 1,
                AutoScroll = true
            };

            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            txtLocation = new TextBox
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 11),
                Margin = new Padding(0, 5, 0, 14)
            };
            txtLocation.TextChanged += InputChanged;

            cmbCategory = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11),
                Margin = new Padding(0, 5, 0, 14)
            };
            cmbCategory.Items.AddRange(new object[]
            {
                "Roads",
                "Sanitation",
                "Water",
                "Electricity",
                "Other"
            });
            cmbCategory.SelectedIndexChanged += InputChanged;

            txtDescription = new RichTextBox
            {
                Dock = DockStyle.Top,
                Height = 130,
                Font = new Font("Segoe UI", 11),
                Margin = new Padding(0, 5, 0, 14)
            };
            txtDescription.TextChanged += InputChanged;

            var attachmentPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                Height = 42,
                Margin = new Padding(0, 5, 0, 14)
            };
            attachmentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            attachmentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));

            txtAttachment = new TextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "No file selected"
            };

            btnAttach = new Button
            {
                Text = "Choose File",
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(235, 240, 244),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnAttach.Click += BtnAttach_Click;

            attachmentPanel.Controls.Add(txtAttachment, 0, 0);
            attachmentPanel.Controls.Add(btnAttach, 1, 0);

            progressBar = new ProgressBar
            {
                Dock = DockStyle.Top,
                Height = 20,
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                Margin = new Padding(0, 5, 0, 7)
            };

            lblProgress = new Label
            {
                Text = "Progress: 0% - Start your report.",
                AutoSize = true,
                ForeColor = Color.FromArgb(24, 73, 112),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 20)
            };

            var buttons = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                Height = 52,
                Margin = new Padding(0, 12, 0, 0)
            };
            buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            btnBack = new Button
            {
                Text = "Back to Main Menu",
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 8, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnBack.Click += (_, _) => Close();

            btnSubmit = new Button
            {
                Text = "Submit Report",
                Dock = DockStyle.Fill,
                Margin = new Padding(8, 0, 0, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(24, 73, 112),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSubmit.Click += BtnSubmit_Click;

            buttons.Controls.Add(btnBack, 0, 0);
            buttons.Controls.Add(btnSubmit, 1, 0);

            formLayout.Controls.Add(CreateLabel("Location"));
            formLayout.Controls.Add(txtLocation);
            formLayout.Controls.Add(CreateLabel("Category"));
            formLayout.Controls.Add(cmbCategory);
            formLayout.Controls.Add(CreateLabel("Description"));
            formLayout.Controls.Add(txtDescription);
            formLayout.Controls.Add(CreateLabel("Attachment (optional)"));
            formLayout.Controls.Add(attachmentPanel);
            formLayout.Controls.Add(CreateLabel("Reporting progress"));
            formLayout.Controls.Add(progressBar);
            formLayout.Controls.Add(lblProgress);
            formLayout.Controls.Add(buttons);

            Controls.Add(formLayout);
            Controls.Add(header);

            UpdateProgress();
        }

        private static Label CreateLabel(string text)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 35, 35),
                Margin = new Padding(0, 4, 0, 0)
            };
        }

        private void InputChanged(object? sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void BtnAttach_Click(object? sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtAttachment.Text = openFileDialog.FileName;
                UpdateProgress();
            }
        }

        private void UpdateProgress()
        {
            int progress = 0;

            if (!string.IsNullOrWhiteSpace(txtLocation.Text))
                progress += 25;

            if (cmbCategory.SelectedIndex >= 0)
                progress += 25;

            if (!string.IsNullOrWhiteSpace(txtDescription.Text))
                progress += 25;

            // Attachment is optional, so the report reaches 100% once required fields are complete.
            if (progress == 75)
                progress = 100;

            progressBar.Value = progress;

            lblProgress.Text = progress switch
            {
                0 => "Progress: 0% - Start your report.",
                25 => "Progress: 25% - Good start. Keep going!",
                50 => "Progress: 50% - Halfway there!",
                100 => "Progress: 100% - Your report is ready to submit.",
                _ => $"Progress: {progress}%"
            };
        }

        private void BtnSubmit_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show(
                    "Please enter the location of the issue.",
                    "Missing Location",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                txtLocation.Focus();
                return;
            }

            if (cmbCategory.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Please select an issue category.",
                    "Missing Category",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                cmbCategory.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show(
                    "Please provide a description of the issue.",
                    "Missing Description",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                txtDescription.Focus();
                return;
            }

            string reference = $"MSA-{DateTime.Now:yyyyMMddHHmmssfff}";

            var issue = new IssueReport
            {
                ReferenceNumber = reference,
                Location = txtLocation.Text.Trim(),
                Category = cmbCategory.SelectedItem?.ToString() ?? string.Empty,
                Description = txtDescription.Text.Trim(),
                AttachmentPath = txtAttachment.Text.Trim(),
                DateReported = DateTime.Now
            };

            Issues.Add(issue);

            string attachmentText = string.IsNullOrWhiteSpace(issue.AttachmentPath)
                ? "None"
                : Path.GetFileName(issue.AttachmentPath);

            MessageBox.Show(
                $"Issue submitted successfully!\n\n" +
                $"Reference Number: {issue.ReferenceNumber}\n" +
                $"Location: {issue.Location}\n" +
                $"Category: {issue.Category}\n" +
                $"Attachment: {attachmentText}\n\n" +
                "Thank you for helping improve your community.",
                "Report Submitted",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            ClearForm();
        }

        private void ClearForm()
        {
            txtLocation.Clear();
            cmbCategory.SelectedIndex = -1;
            txtDescription.Clear();
            txtAttachment.Clear();
            UpdateProgress();
            txtLocation.Focus();
        }
    }
}
