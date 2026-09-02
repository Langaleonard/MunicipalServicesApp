# Municipal Services Application - Windows Forms Version

## Overview

This folder contains the Windows Forms version of the Municipal Services Application for Part 1 of AAPD7112.

The application allows a resident to report a municipal issue using a graphical user interface.

## Implemented Part 1 Features

- Main Menu Windows Form
- Report Issues option
- Local Events and Announcements shown as disabled / coming soon
- Service Request Status shown as disabled / coming soon
- Location TextBox
- Category ComboBox
- Description RichTextBox
- Optional media/document attachment using OpenFileDialog
- Submit button
- Back to Main Menu button
- ProgressBar and encouraging progress messages
- Validation using MessageBox feedback
- Unique reference number generation
- Submitted reports stored using List<IssueReport>
- Consistent layout and colour scheme

## Requirements

This Windows Forms version must be run on Windows.

Recommended:
- Windows 10 or Windows 11
- Visual Studio 2022 or later with the .NET desktop development workload
- .NET 10 SDK if using the included project file

## Compile

Open the project in Visual Studio and build the solution.

Alternatively, from a Windows terminal in this folder:

dotnet build

## Run

From Visual Studio, select Start.

Or from a Windows terminal:

dotnet run

## How to Use

1. Start the application.
2. Select Report Issues.
3. Enter the issue location.
4. Select an issue category.
5. Enter a detailed description.
6. Select Choose File if you want to attach an image or document.
7. Follow the progress indicator.
8. Select Submit Report.
9. The application validates the required fields.
10. A success MessageBox displays the generated reference number and report summary.
11. Select Back to Main Menu to return to the startup form.

## Data Structure

Submitted issues are stored in a List<IssueReport>.

Each IssueReport stores:
- Reference number
- Location
- Category
- Description
- Attachment path
- Date reported

## User Engagement Strategy

The application supports active citizen participation and feedback.

It uses:
- progress messages,
- a ProgressBar,
- validation feedback,
- submission confirmation,
- and an encouraging thank-you message.

## macOS Development Note

The application logic was developed and tested on macOS using a console prototype because Windows Forms is Windows-specific.

This folder contains the Windows Forms implementation prepared for the required Windows graphical interface.

Developer: Leonard Langa
