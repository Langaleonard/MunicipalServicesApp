# Municipal Services Application

## Project Overview

The Municipal Services Application is a C# application developed for South African municipal service reporting.

The purpose of the application is to allow residents to report municipal issues in a simple and user-friendly way.

This first part of the project focuses on the Report Issues feature.

## Current Features

- Main menu
- Report Issues option
- Location input
- Issue category selection
- Detailed issue description
- Optional image or document attachment
- Input validation
- Progress and engagement messages
- Unique reference number generation
- Report confirmation
- Storage of submitted reports using a List

The following features will be implemented later:

- Local Events and Announcements
- Service Request Status

## Requirements

- .NET SDK
- Visual Studio Code or Visual Studio
- C#

The application was developed using .NET 10.

## How to Compile the Application

Open a terminal inside the project folder and run:

dotnet build

If the application compiles successfully, the terminal will display:

Build succeeded

## How to Run the Application

Run the following command:

dotnet run

The Municipal Services Application main menu will appear.

## How to Use the Application

1. Start the application.
2. Select option 1 to report a municipal issue.
3. Enter the location of the issue.
4. Select a category.
5. Enter a description of the issue.
6. Add an image or document path if required, or press Enter to skip.
7. Follow the progress messages displayed by the application.
8. The application will submit the report and generate a unique reference number.
9. A summary of the submitted issue will be displayed.

## Validation

The application validates required information.

The user cannot leave required fields empty.

The category selection is also validated to ensure that a valid option is selected.

## Data Structure

The application uses a List<IssueReport> to store submitted municipal issues.

Each report contains:

- Reference number
- Location
- Category
- Description
- Attachment path
- Date reported

## User Engagement Strategy

The application uses active citizen participation and feedback as its main user engagement strategy.

Progress messages are shown while the resident completes the report.

The application also gives confirmation after successful submission and thanks the resident for helping improve the community.

## Developer

Leonard Langa