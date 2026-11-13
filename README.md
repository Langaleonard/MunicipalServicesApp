# Municipal Services Application - Final PoE

## Student

Leonard Langa  
Student Number: ST10125760  
Module: AAPD7112  
GitHub: https://github.com/Langaleonard/MunicipalServicesApp

## Overview

This is the final version of the Municipal Services Application. It combines all three stages of the project:

1. Report Issues, completed in Part 1.
2. Local Events and Announcements, completed in Part 2.
3. Service Request Status, implemented for the final PoE.

The final application is a C# Windows Forms application using the same responsive navy, pink and light-grey interface across all forms.

## Requirements

- Windows 10 or Windows 11
- Visual Studio with the .NET desktop development workload
- .NET 10 SDK

## Compile

Open `MunicipalServicesApp.csproj` in Visual Studio and select:

`Build > Build Solution`

or press:

`Ctrl + Shift + B`

From a Windows terminal in the project folder:

```text
dotnet restore
dotnet build
```

## Run

In Visual Studio select the green Start button, or run:

```text
dotnet run
```

## Main Menu

The final main menu contains three working features:

- Report Issues
- Local Events and Announcements
- Service Request Status

## Report Issues

Residents can:

- enter the issue location;
- select a category;
- enter a detailed description;
- optionally attach a document or image;
- follow completion progress;
- submit the report and receive a unique reference number.

A newly submitted issue is automatically added to the Service Request repository. The same reference number can then be searched in Service Request Status.

## Local Events and Announcements

Residents can:

- view upcoming municipal events and announcements;
- search by title, location or keyword;
- filter by category;
- filter by date;
- receive recommendations based on previous category searches.

Part 2 data structures such as PriorityQueue, SortedDictionary, Dictionary, HashSet and Stack remain integrated.

## Service Request Status

The final feature displays submitted service requests in an organised table containing:

- reference number;
- category;
- location;
- priority;
- status;
- submitted date.

Residents can enter a unique reference number and select **Track Request**. The application retrieves the matching request, highlights it in the table, shows request details, displays progress and calculates the remaining workflow path to resolution.

## Final PoE Data Structures

### Basic Tree

`BasicTreeNode<string>` represents the municipal service hierarchy. The root is Municipal Services and child nodes represent departments such as Water Services, Roads and Transport, Electricity Services, Waste and Sanitation and Community Services. This provides a hierarchical view of the service environment.

### Binary Tree

`BinaryTree<string>` models the main service status progression in a binary structure. It demonstrates binary traversal and is included in the Service Request index summary.

### Binary Search Tree

`ServiceRequestBinarySearchTree` indexes requests using their unique reference numbers. It supports ordered storage and lookup of service requests.

### AVL Tree

`ServiceRequestAvlTree` is a self-balancing binary search tree. It maintains its height after insertions using rotations. The Service Request Status feature uses the AVL index for the main reference-number lookup, reducing the risk of an unbalanced search tree.

### Red-Black Tree

`ServiceRequestRedBlackTree` provides another self-balancing tree index. It is used to maintain the service requests in sorted reference-number order for display in the Service Request Status table.

### Min-Heap

`ServiceRequestMinHeap` stores unresolved requests according to urgency. Urgent requests receive the highest processing priority, followed by High, Normal and Low. The Data Structure Insights panel shows the next request selected by the heap.

### Graph

`WeightedGraph` models the service-request workflow as connected status nodes, including Submitted, Review, Assigned, Escalated, In Progress and Resolved.

### Breadth-First Search

Breadth-first search is used when tracking a request to calculate a direct route from the request's current status to Resolved. The route is displayed to the resident in the tracking panel.

### Depth-First Search

Depth-first traversal explores the workflow graph and is used in the technical summary to demonstrate traversal across the status network.

### Minimum Spanning Tree

Prim's minimum spanning tree algorithm is applied to the weighted workflow graph. It selects a low-cost set of connections that reaches all workflow nodes. The number of MST edges and total weight are shown in the Data Structure Insights panel.

## Integration Between Features

When a resident submits a new issue in Report Issues:

1. an `IssueReport` is created;
2. a unique `MSA-...` reference number is generated;
3. the issue is stored in the Part 1 list;
4. `ServiceRequestRepository.AddFromIssue()` converts it into a trackable service request;
5. the resident can use the same reference under Service Request Status.

This connects the final feature directly to the functionality completed in Part 1.

## Sample Data

The repository contains sample service requests so the final feature can be demonstrated immediately. New reports created during the same application session are also added to the repository.

## Testing

Before submission, test the following on Windows:

- project builds with 0 errors;
- all three main menu options open;
- Report Issues still submits successfully;
- Local Events search and recommendations still work;
- Service Request Status displays the request list;
- valid references are found;
- invalid references show a clear message;
- request progress and graph route display correctly;
- Data Structure Insights display BST, AVL, Red-Black Tree, Heap, Graph, DFS and MST information;
- a newly submitted issue can be tracked using its generated reference number.

## Developer

Leonard Langa
