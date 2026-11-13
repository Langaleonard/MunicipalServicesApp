using MunicipalServicesApp.Models;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    public class ServiceRequestMinHeap
    {
        private readonly List<ServiceRequest> items = new();
        public int Count => items.Count;

        public void Clear() => items.Clear();

        public void Insert(ServiceRequest request)
        {
            items.Add(request);
            HeapifyUp(items.Count - 1);
        }

        public ServiceRequest? Peek() => items.Count == 0 ? null : items[0];

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (Compare(items[index], items[parent]) >= 0) break;
                (items[index], items[parent]) = (items[parent], items[index]);
                index = parent;
            }
        }

        private static int Compare(ServiceRequest a, ServiceRequest b)
        {
            int priority = a.PriorityRank.CompareTo(b.PriorityRank);
            if (priority != 0) return priority;
            return a.DateSubmitted.CompareTo(b.DateSubmitted);
        }
    }
}
