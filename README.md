# Municipal Services Application - Windows Forms Version

## Overview

This folder contains the completed Windows Forms implementation for Part 1 of the Municipal Services Application for AAPD7112.

The application allows residents to report municipal issues through a simple graphical user interface.

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
- Consistent colour scheme and responsive layout

## Requirements

This Windows Forms version must be run on Windows.

Recommended:
- Windows 10 or Windows 11
- Visual Studio with the .NET desktop development workload
- .NET 10 SDK if using the included project file

## How to Compile

Open `MunicipalServicesApp.csproj` in Visual Studio.

Build the solution by selecting:

`Build > Build Solution`

or press:

`Ctrl + Shift + B`

The application can also be compiled from a Windows terminal by running:

`dotnet build`

## How to Run

In Visual Studio, select the green Start button.

Alternatively, from a Windows terminal in the project folder, run:

`dotnet run`

## How to Use the Application

1. Start the application.
2. The Main Menu displays three municipal service options.
3. Select **Report Issues**.
4. Enter the location of the issue.
5. Select an issue category.
6. Enter a detailed description of the issue.
7. Select **Choose File** if you want to attach an image or document.
8. Follow the progress indicator while completing the form.
9. Select **Submit Report**.
10. The application checks that all required fields have been completed.
11. If information is missing, a MessageBox explains what must be corrected.
12. After successful submission, a MessageBox displays the generated reference number and report summary.
13. Select **Back to Main Menu** to return to the startup screen.

## Validation

The application validates the required issue information before a report is submitted.

The following fields are required:
- Location
- Category
- Description

If one of these fields is missing, the application displays a clear warning message and allows the resident to correct the information.

The media attachment is optional.

## Data Structure

Submitted issues are stored using a:

`List<IssueReport>`

Each `IssueReport` stores:
- Reference number
- Location
- Category
- Description
- Attachment path
- Date reported

Using a List makes it simple to store and manage multiple issue reports during the application session.

## User Engagement Strategy

The application supports active citizen participation and feedback.

The reporting process includes:
- progress messages
- a ProgressBar
- clear validation feedback
- submission confirmation
- a unique reference number
- an encouraging thank-you message

These features help residents understand where they are in the reporting process and confirm that their information has been received.

## Interface Design

The application uses a consistent colour scheme and layout across the Main Menu and Report Issues forms.

The interface uses:
- a dark navigation/header colour
- a pink accent colour for important actions
- light backgrounds for readability
- clear labels and buttons
- layouts that resize with the application window

The two features planned for later parts of the project are visible but disabled so that users can clearly see that they are not yet available.

## Development and Testing

The application logic was initially developed and tested on macOS using a console prototype.

The final Windows Forms version was then built and tested on Windows 11 using Visual Studio.

The final version was tested for:
- successful compilation
- startup navigation
- issue reporting
- input validation
- category selection
- file attachment
- progress feedback
- successful submission
- reference number generation
- navigation back to the Main Menu

## Future Development

The same application will be extended in later assessments with:

- Local Events and Announcements
- Service Request Status

## Developer

Leonard Langa
