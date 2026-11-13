using MunicipalServicesApp.Models;
using System;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    public class ServiceRequestBinarySearchTree
    {
        private sealed class Node
        {
            public ServiceRequest Value;
            public Node? Left;
            public Node? Right;

            public Node(ServiceRequest value) => Value = value;
        }

        private Node? root;
        public int Count { get; private set; }

        public void Insert(ServiceRequest request)
        {
            root = Insert(root, request);
        }

        private Node Insert(Node? node, ServiceRequest request)
        {
            if (node == null)
            {
                Count++;
                return new Node(request);
            }

            int comparison = string.Compare(
                request.ReferenceNumber,
                node.Value.ReferenceNumber,
                StringComparison.OrdinalIgnoreCase);

            if (comparison < 0)
                node.Left = Insert(node.Left, request);
            else if (comparison > 0)
                node.Right = Insert(node.Right, request);
            else
                node.Value = request;

            return node;
        }

        public ServiceRequest? Find(string reference)
        {
            Node? current = root;
            while (current != null)
            {
                int comparison = string.Compare(
                    reference,
                    current.Value.ReferenceNumber,
                    StringComparison.OrdinalIgnoreCase);

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

        private static void Traverse(Node? node, List<ServiceRequest> result)
        {
            if (node == null) return;
            Traverse(node.Left, result);
            result.Add(node.Value);
            Traverse(node.Right, result);
        }
    }
}
