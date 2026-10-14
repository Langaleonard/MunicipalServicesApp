# Municipal Services Application - Part 2

## Overview

This project continues the AAPD7112 Municipal Services Application from Part 1.

Part 2 adds **Local Events and Announcements** while keeping the existing **Report Issues** feature. **Service Request Status** remains unavailable until the final PoE.

The project contains two versions so development can continue on macOS while the final graphical application remains a Windows Forms application:

- The project in this root folder is a **macOS-compatible console test harness** targeting `net10.0`.
- The `WindowsFormsVersion` folder contains the **final Windows Forms application** targeting `net10.0-windows`.

## Part 2 Features

- Report Issues from Part 1 remains available.
- Local Events and Announcements is now available.
- Service Request Status remains marked as Coming Soon.
- Upcoming local events and municipal announcements are displayed.
- Events can be searched using a keyword, category and date.
- Recommendations change according to the user's search behaviour.
- Event information is organised with advanced data structures required by Part 2.

## Advanced Data Structures

The Local Events feature uses the following structures:

- `PriorityQueue<EventItem, DateTime>` to order upcoming events by date.
- `SortedDictionary<DateTime, List<EventItem>>` to organise events chronologically.
- `Dictionary<string, List<EventItem>>` to retrieve events efficiently by category.
- `Dictionary<string, int>` to record category search frequency for recommendations.
- `HashSet<string>` to store unique event categories.
- `HashSet<DateTime>` to store unique event dates.
- `Stack<string>` to keep recent searches.

## Recommendation Feature

The application records category-based search activity. The category searched most often becomes the user's preferred category. Upcoming events from that category are then recommended. Before any searches are performed, the application recommends the next upcoming events.

## Testing on macOS

The root console project can be compiled and run on macOS.

From Terminal in the `MunicipalServicesApp` folder run:

```bash
dotnet clean
dotnet restore
dotnet build
dotnet run
```

Use the console menu to test:

1. Report Issues
2. Local Events and Announcements
3. Service Request Status - Coming Soon

The console version exists so the Part 2 logic and data structures can be tested without Windows.

## Final Windows Forms Version

The final graphical application is located in:

```text
WindowsFormsVersion/
```

It includes:

- `MainMenuForm.cs`
- `ReportIssueForm.cs`
- `LocalEventsForm.cs`
- `Models/IssueReport.cs`
- `Models/EventItem.cs`
- `MunicipalServicesApp.csproj`

On Windows, open the `WindowsFormsVersion` folder in Visual Studio or run:

```powershell
dotnet restore
dotnet build
dotnet run
```

The Windows Forms interface uses the same colour scheme as Part 1 and includes a responsive Local Events and Announcements screen.

## Developer

Leonard Langa
