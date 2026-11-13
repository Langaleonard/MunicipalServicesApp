using MunicipalServicesApp.Models;
using System;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    public class ServiceRequestRedBlackTree
    {
        private enum NodeColor { Red, Black }

        private sealed class Node
        {
            public ServiceRequest Value;
            public Node? Left;
            public Node? Right;
            public Node? Parent;
            public NodeColor Color = NodeColor.Red;

            public Node(ServiceRequest value) => Value = value;
        }

        private Node? root;
        public int Count { get; private set; }

        public void Insert(ServiceRequest request)
        {
            Node? parent = null;
            Node? current = root;
            int comparison = 0;

            while (current != null)
            {
                parent = current;
                comparison = string.Compare(request.ReferenceNumber, current.Value.ReferenceNumber, StringComparison.OrdinalIgnoreCase);
                if (comparison == 0)
                {
                    current.Value = request;
                    return;
                }
                current = comparison < 0 ? current.Left : current.Right;
            }

            var node = new Node(request) { Parent = parent };
            if (parent == null)
                root = node;
            else if (string.Compare(request.ReferenceNumber, parent.Value.ReferenceNumber, StringComparison.OrdinalIgnoreCase) < 0)
                parent.Left = node;
            else
                parent.Right = node;

            Count++;
            FixInsert(node);
        }

        public ServiceRequest? Find(string reference)
        {
            Node? current = root;
            while (current != null)
            {
                int comparison = string.Compare(reference, current.Value.ReferenceNumber, StringComparison.OrdinalIgnoreCase);
                if (comparison == 0) return current.Value;
                current = comparison < 0 ? current.Left : current.Right;
            }
            return null;
        }

        public List<ServiceRequest> InOrder()
        {
            var result = new List<ServiceRequest>();
            Traverse(root, result);
            return result;
        }

        private void FixInsert(Node node)
        {
            while (node != root && node.Parent?.Color == NodeColor.Red)
            {
                Node parent = node.Parent!;
                Node grand = parent.Parent!;

                if (parent == grand.Left)
                {
                    Node? uncle = grand.Right;
                    if (uncle?.Color == NodeColor.Red)
                    {
                        parent.Color = NodeColor.Black;
                        uncle.Color = NodeColor.Black;
                        grand.Color = NodeColor.Red;
                        node = grand;
                    }
                    else
                    {
                        if (node == parent.Right)
                        {
                            node = parent;
                            RotateLeft(node);
                            parent = node.Parent!;
                            grand = parent.Parent!;
                        }
                        parent.Color = NodeColor.Black;
                        grand.Color = NodeColor.Red;
                        RotateRight(grand);
                    }
                }
                else
                {
                    Node? uncle = grand.Left;
                    if (uncle?.Color == NodeColor.Red)
                    {
                        parent.Color = NodeColor.Black;
                        uncle.Color = NodeColor.Black;
                        grand.Color = NodeColor.Red;
                        node = grand;
                    }
                    else
                    {
                        if (node == parent.Left)
                        {
                            node = parent;
                            RotateRight(node);
                            parent = node.Parent!;
                            grand = parent.Parent!;
                        }
                        parent.Color = NodeColor.Black;
                        grand.Color = NodeColor.Red;
                        RotateLeft(grand);
                    }
                }
            }

            if (root != null) root.Color = NodeColor.Black;
        }

        private void RotateLeft(Node x)
        {
            Node y = x.Right!;
            x.Right = y.Left;
            if (y.Left != null) y.Left.Parent = x;
            y.Parent = x.Parent;
            if (x.Parent == null) root = y;
            else if (x == x.Parent.Left) x.Parent.Left = y;
            else x.Parent.Right = y;
            y.Left = x;
            x.Parent = y;
        }

        private void RotateRight(Node y)
        {
            Node x = y.Left!;
            y.Left = x.Right;
            if (x.Right != null) x.Right.Parent = y;
            x.Parent = y.Parent;
            if (y.Parent == null) root = x;
            else if (y == y.Parent.Left) y.Parent.Left = x;
            else y.Parent.Right = x;
            x.Right = y;
            y.Parent = x;
        }

        private static void Traverse(Node? node, List<ServiceRequest> result)
        {
            if (node == null) return;
            Traverse(node.Left, result);
            result.Add(node.Value);
            Traverse(node.Right, result);
        }
    }
}
