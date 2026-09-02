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
            Color accent = ColorTranslator.FromHtml("#ff3295");
            Color dark = ColorTranslator.FromHtml("#0f172a");
            Color pageBg = ColorTranslator.FromHtml("#eef2f7");
            Color white = ColorTranslator.FromHtml("#ffffff");
            Color inputBorder = ColorTranslator.FromHtml("#cccccc");
            Color messageBg = ColorTranslator.FromHtml("#e0f2fe");
            Color messageText = ColorTranslator.FromHtml("#075985");
            Color securityBg = ColorTranslator.FromHtml("#f8fafc");
            Color lightBorder = ColorTranslator.FromHtml("#dddddd");

            Text = "Report a Municipal Issue";
            StartPosition = FormStartPosition.CenterScreen;

            MinimumSize = new Size(780, 560);
            Size = new Size(980, 680);

            BackColor = pageBg;

            AutoScaleMode = AutoScaleMode.Dpi;
            MaximizeBox = true;

            openFileDialog = new OpenFileDialog
            {
                Title = "Select an image or document",
                Filter =
                    "Supported Files|*.jpg;*.jpeg;*.png;*.pdf;*.doc;*.docx|" +
                    "All Files|*.*"
            };

            // HEADER
            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = dark
            };

            var title = new Label
            {
                Text = "Report a Municipal Issue",
                ForeColor = white,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 16)
            };

            var subtitleLabel = new Label
            {
                Text = "Tell us what happened so the municipality can respond.",
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(33, 60)
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

            // FORM LAYOUT
            var formLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(40, 18, 40, 22),
                ColumnCount = 1,
                RowCount = 12,
                AutoScroll = true,
                BackColor = pageBg
            };

            formLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100)
            );

            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // LOCATION
            txtLocation = new TextBox
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 11),
                Margin = new Padding(0, 5, 0, 14),
                BackColor = white
            };

            txtLocation.TextChanged += InputChanged;

            // CATEGORY
            cmbCategory = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11),
                Margin = new Padding(0, 5, 0, 14),
                BackColor = white
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

            // DESCRIPTION
            txtDescription = new RichTextBox
            {
                Dock = DockStyle.Fill,
                MinimumSize = new Size(0, 90),
                Font = new Font("Segoe UI", 11),
                Margin = new Padding(0, 5, 0, 14),
                BackColor = white,
                BorderStyle = BorderStyle.FixedSingle
            };

            txtDescription.TextChanged += InputChanged;

            // ATTACHMENT
            var attachmentPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                AutoSize = true,
                Margin = new Padding(0, 5, 0, 14)
            };

            attachmentPanel.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100)
            );

            attachmentPanel.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 160)
            );

            txtAttachment = new TextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "No file selected",
                BackColor = white
            };

            btnAttach = new Button
            {
                Text = "Choose File",
                Dock = DockStyle.Fill,
                MinimumSize = new Size(0, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = securityBg,
                ForeColor = dark,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnAttach.FlatAppearance.BorderColor = inputBorder;

            btnAttach.Click += BtnAttach_Click;

            attachmentPanel.Controls.Add(txtAttachment, 0, 0);
            attachmentPanel.Controls.Add(btnAttach, 1, 0);

            // PROGRESS
            progressBar = new ProgressBar
            {
                Dock = DockStyle.Top,
                Height = 22,
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                Margin = new Padding(0, 5, 0, 10)
            };

            lblProgress = new Label
            {
                Text = "Progress: 0% - Start your report.",
                Dock = DockStyle.Top,
                AutoSize = true,
                ForeColor = messageText,
                BackColor = messageBg,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Padding = new Padding(12),
                Margin = new Padding(0, 0, 0, 20)
            };

            // BUTTONS
            var buttons = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                AutoSize = true,
                Margin = new Padding(0, 12, 0, 0)
            };

            buttons.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 50)
            );

            buttons.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 50)
            );

            btnBack = new Button
            {
                Text = "Back to Main Menu",
                Dock = DockStyle.Fill,
                MinimumSize = new Size(0, 48),
                Margin = new Padding(0, 0, 8, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = white,
                ForeColor = dark,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnBack.FlatAppearance.BorderColor = lightBorder;
            btnBack.Click += (_, _) => Close();

            btnSubmit = new Button
            {
                Text = "Submit Report",
                Dock = DockStyle.Fill,
                MinimumSize = new Size(0, 48),
                Margin = new Padding(8, 0, 0, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = accent,
                ForeColor = white,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnSubmit.FlatAppearance.BorderColor = accent;
            btnSubmit.Click += BtnSubmit_Click;

            buttons.Controls.Add(btnBack, 0, 0);
            buttons.Controls.Add(btnSubmit, 1, 0);

            // ADD CONTROLS
            formLayout.Controls.Add(CreateLabel("Location", dark));
            formLayout.Controls.Add(txtLocation);

            formLayout.Controls.Add(CreateLabel("Category", dark));
            formLayout.Controls.Add(cmbCategory);

            formLayout.Controls.Add(CreateLabel("Description", dark));
            formLayout.Controls.Add(txtDescription);

            formLayout.Controls.Add(
                CreateLabel("Attachment (optional)", dark)
            );

            formLayout.Controls.Add(attachmentPanel);

            formLayout.Controls.Add(
                CreateLabel("Reporting progress", dark)
            );

            formLayout.Controls.Add(progressBar);
            formLayout.Controls.Add(lblProgress);
            formLayout.Controls.Add(buttons);

            Controls.Add(formLayout);
            Controls.Add(header);

            UpdateProgress();
        }

        private static Label CreateLabel(string text, Color textColor)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Font = new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold
                ),
                ForeColor = textColor,
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
            int completed = 0;

            if (!string.IsNullOrWhiteSpace(txtLocation.Text))
                completed++;

            if (cmbCategory.SelectedIndex >= 0)
                completed++;

            if (!string.IsNullOrWhiteSpace(txtDescription.Text))
                completed++;

            int progress = completed switch
            {
                0 => 0,
                1 => 30,
                2 => 60,
                3 => 100,
                _ => 0
            };

            progressBar.Value = progress;

            lblProgress.Text = progress switch
            {
                0 =>
                    "Progress: 0% - Start your report.",

                30 =>
                    "Progress: 30% - Good start. Keep going!",

                60 =>
                    "Progress: 60% - Almost there!",

                100 =>
                    "Progress: 100% - Ready to submit. Thank you for helping your community!",

                _ =>
                    $"Progress: {progress}%"
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

            string reference =
                $"MSA-{DateTime.Now:yyyyMMddHHmmssfff}";

            var issue = new IssueReport
            {
                ReferenceNumber = reference,
                Location = txtLocation.Text.Trim(),

                Category =
                    cmbCategory.SelectedItem?.ToString()
                    ?? string.Empty,

                Description = txtDescription.Text.Trim(),
                AttachmentPath = txtAttachment.Text.Trim(),
                DateReported = DateTime.Now
            };

            Issues.Add(issue);

            string attachmentText =
                string.IsNullOrWhiteSpace(issue.AttachmentPath)
                ? "None"
                : Path.GetFileName(issue.AttachmentPath);

            MessageBox.Show(
                $"Issue submitted successfully!\n\n" +
                $"Reference Number: {issue.ReferenceNumber}\n" +
                $"Location: {issue.Location}\n" +
                $"Category: {issue.Category}\n" +
                $"Date Reported: {issue.DateReported:dd MMMM yyyy HH:mm}\n" +
                $"Attachment: {attachmentText}\n\n" +
                $"Thank you for helping improve your community.",
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