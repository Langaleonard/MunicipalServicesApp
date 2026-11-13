using MunicipalServicesApp.Models;
using System;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    public class ServiceRequestAvlTree
    {
        private sealed class Node
        {
            public ServiceRequest Value;
            public Node? Left;
            public Node? Right;
            public int Height = 1;

            public Node(ServiceRequest value) => Value = value;
        }

        private Node? root;
        public int Count { get; private set; }
        public int Height => GetHeight(root);

        public void Insert(ServiceRequest request)
        {
            bool added = false;
            root = Insert(root, request, ref added);
            if (added) Count++;
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

        private static Node Insert(Node? node, ServiceRequest request, ref bool added)
        {
            if (node == null)
            {
                added = true;
                return new Node(request);
            }

            int comparison = string.Compare(request.ReferenceNumber, node.Value.ReferenceNumber, StringComparison.OrdinalIgnoreCase);
            if (comparison < 0)
                node.Left = Insert(node.Left, request, ref added);
            else if (comparison > 0)
                node.Right = Insert(node.Right, request, ref added);
            else
            {
                node.Value = request;
                return node;
            }

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
            int balance = GetBalance(node);

            if (balance > 1 && string.Compare(request.ReferenceNumber, node.Left!.Value.ReferenceNumber, StringComparison.OrdinalIgnoreCase) < 0)
                return RotateRight(node);

            if (balance < -1 && string.Compare(request.ReferenceNumber, node.Right!.Value.ReferenceNumber, StringComparison.OrdinalIgnoreCase) > 0)
                return RotateLeft(node);

            if (balance > 1 && string.Compare(request.ReferenceNumber, node.Left!.Value.ReferenceNumber, StringComparison.OrdinalIgnoreCase) > 0)
            {
                node.Left = RotateLeft(node.Left!);
                return RotateRight(node);
            }

            if (balance < -1 && string.Compare(request.ReferenceNumber, node.Right!.Value.ReferenceNumber, StringComparison.OrdinalIgnoreCase) < 0)
            {
                node.Right = RotateRight(node.Right!);
                return RotateLeft(node);
            }

            return node;
        }

        private static Node RotateRight(Node y)
        {
            Node x = y.Left!;
            Node? t2 = x.Right;
            x.Right = y;
            y.Left = t2;
            y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));
            x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));
            return x;
        }

        private static Node RotateLeft(Node x)
        {
            Node y = x.Right!;
            Node? t2 = y.Left;
            y.Left = x;
            x.Right = t2;
            x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));
            y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));
            return y;
        }

        private static int GetHeight(Node? node) => node?.Height ?? 0;
        private static int GetBalance(Node? node) => node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);

        private static void Traverse(Node? node, List<ServiceRequest> result)
        {
            if (node == null) return;
            Traverse(node.Left, result);
            result.Add(node.Value);
            Traverse(node.Right, result);
        }
    }
}
