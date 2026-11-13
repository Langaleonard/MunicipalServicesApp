using MunicipalServicesApp.DataStructures;
using MunicipalServicesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServicesApp.Services
{
    public class ServiceRequestIndex
    {
        public ServiceRequestBinarySearchTree BinarySearchTree { get; } = new();
        public ServiceRequestAvlTree AvlTree { get; } = new();
        public ServiceRequestRedBlackTree RedBlackTree { get; } = new();
        public ServiceRequestMinHeap PriorityHeap { get; } = new();
        public WeightedGraph WorkflowGraph { get; } = new();
        public BasicTreeNode<string> ServiceHierarchy { get; private set; } = new("Municipal Services");
        public BinaryTree<string> StatusBinaryTree { get; private set; } = new();

        public ServiceRequestIndex(IEnumerable<ServiceRequest> requests)
        {
            Build(requests);
        }

        public void Build(IEnumerable<ServiceRequest> requests)
        {
            BuildWorkflowGraph();
            BuildServiceHierarchy();
            BuildStatusBinaryTree();

            foreach (ServiceRequest request in requests)
            {
                BinarySearchTree.Insert(request);
                AvlTree.Insert(request);
                RedBlackTree.Insert(request);
                if (!request.Status.Equals("Resolved", StringComparison.OrdinalIgnoreCase))
                    PriorityHeap.Insert(request);
            }
        }

        public ServiceRequest? FindByReference(string reference)
        {
            return AvlTree.Find(reference.Trim());
        }

        public IReadOnlyList<ServiceRequest> GetSortedRequests()
        {
            return RedBlackTree.InOrder();
        }

        public string GetRouteToResolution(string currentStatus)
        {
            if (currentStatus.Equals("Resolved", StringComparison.OrdinalIgnoreCase))
                return "Resolved";

            var path = WorkflowGraph.BreadthFirstPath(currentStatus, "Resolved");
            return path.Count == 0 ? currentStatus : string.Join("  >  ", path);
        }

        public string GetTechnicalSummary()
        {
            ServiceRequest? next = PriorityHeap.Peek();
            var dfs = WorkflowGraph.DepthFirstTraversal("Submitted");
            var mst = WorkflowGraph.MinimumSpanningTree("Submitted");

            string nextPriority = next == null
                ? "No open requests"
                : $"{next.ReferenceNumber} ({next.Priority})";

            return
                $"BST index: {BinarySearchTree.Count} requests  |  " +
                $"AVL index: {AvlTree.Count}, height {AvlTree.Height}  |  " +
                $"Red-Black index: {RedBlackTree.Count}\n" +
                $"Min-Heap next priority: {nextPriority}\n" +
                $"Graph DFS visits {dfs.Count} workflow nodes  |  " +
                $"MST: {mst.Edges.Count} edges, total weight {mst.TotalWeight}\n" +
                $"Basic service tree: {ServiceHierarchy.CountNodes()} nodes  |  " +
                $"Binary status tree: {StatusBinaryTree.InOrder().Count} nodes";
        }

        private void BuildWorkflowGraph()
        {
            WorkflowGraph.AddEdge("Submitted", "Assigned", 2);
            WorkflowGraph.AddEdge("Assigned", "In Progress", 3);
            WorkflowGraph.AddEdge("In Progress", "Resolved", 2);
            WorkflowGraph.AddEdge("Submitted", "Review", 1);
            WorkflowGraph.AddEdge("Review", "Assigned", 1);
            WorkflowGraph.AddEdge("Assigned", "Escalated", 4);
            WorkflowGraph.AddEdge("Escalated", "In Progress", 1);
        }

        private void BuildServiceHierarchy()
        {
            ServiceHierarchy = new BasicTreeNode<string>("Municipal Services");
            ServiceHierarchy.AddChild("Water Services");
            ServiceHierarchy.AddChild("Roads and Transport");
            ServiceHierarchy.AddChild("Electricity Services");
            ServiceHierarchy.AddChild("Waste and Sanitation");
            ServiceHierarchy.AddChild("Community Services");
        }

        private void BuildStatusBinaryTree()
        {
            StatusBinaryTree = new BinaryTree<string>
            {
                Root = new BinaryTreeNode<string>("Assigned")
                {
                    Left = new BinaryTreeNode<string>("Submitted"),
                    Right = new BinaryTreeNode<string>("In Progress")
                    {
                        Right = new BinaryTreeNode<string>("Resolved")
                    }
                }
            };
        }
    }
}
