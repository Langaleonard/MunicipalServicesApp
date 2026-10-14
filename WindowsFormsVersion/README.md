# Municipal Services Application - Part 2

## Overview

This project is the Part 2 continuation of the Municipal Services Application for AAPD7112.

The application keeps the working **Report Issues** feature from Part 1 and adds the **Local Events and Announcements** feature required for Part 2. **Service Request Status** remains disabled because it will be implemented in the final PoE.

## Requirements

- Windows 10 or Windows 11
- Visual Studio with the .NET desktop development workload
- .NET 10 SDK

## Compile

Open `MunicipalServicesApp.csproj` in Visual Studio and select:

`Build > Build Solution`

or press:

`Ctrl + Shift + B`

From a Windows terminal in the project folder you can also run:

`dotnet build`

## Run

In Visual Studio, select the green **Start** button.

Alternatively:

`dotnet run`

## Main Menu

The main menu contains:

1. **Report Issues** - implemented in Part 1 and still functional.
2. **Local Events and Announcements** - implemented in Part 2.
3. **Service Request Status** - disabled until the final PoE.

## Local Events and Announcements

The Part 2 page allows residents to:

- View upcoming local events and municipal announcements.
- Search by title, location, category or keyword.
- Filter by a selected event category.
- Filter by a specific date.
- Reset the filters and return to all upcoming items.
- Receive event recommendations based on previous category searches.
- Return to the Main Menu.

The interface is responsive and uses the same colour scheme and layout style as Part 1.

## Advanced Data Structures

### PriorityQueue<EventItem, DateTime>

A priority queue is used for upcoming events. The event date is used as the priority so that the next upcoming event can be retrieved first. This provides efficient chronological organisation of event-related data.

### SortedDictionary<DateTime, List<EventItem>>

A sorted dictionary stores events by date. This allows events to remain organised in date order and makes date-based retrieval efficient when the user filters events using the date selector.

### Dictionary<string, List<EventItem>>

A dictionary stores events by category. It provides fast access to all events in a selected category such as Community, Business, Health or Municipal Notice.

### HashSet<string>

A set stores unique event categories. This prevents duplicate category names when the category filter is populated.

### HashSet<DateTime>

A second set keeps unique event dates. This demonstrates set-based handling of unique date values in the event collection.

### Stack<string>

A stack records recent user searches. The most recent search is stored at the top of the stack. This supports the analysis of recent user activity while demonstrating a LIFO data structure.

### Dictionary<string, int>

A search-preference dictionary counts how often users search for each category. These counts are used by the recommendation feature to determine the user's most frequently searched category.

## Recommendation Feature

The recommendation feature analyses the categories selected by the user during searches.

Each category search increases a count in `Dictionary<string, int>`. The category with the highest count is treated as the user's current preference. The system then retrieves upcoming events from that category and displays up to three recommendations in the **Recommended For You** section.

Before the user has performed any searches, the application displays the next three upcoming events as general recommendations.

This provides a simple and understandable recommendation algorithm that is directly based on user search behaviour.

## Event Data

The project contains sample municipal and community event information for demonstration and assessment purposes. Dates are generated relative to the current date so that the sample events remain upcoming when the application is tested.

## Developer

Leonard Langa

## GitHub

https://github.com/Langaleonard/MunicipalServicesApp
