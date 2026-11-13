using System;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServicesApp.DataStructures
{
    public class WeightedGraph
    {
        public sealed record Edge(string From, string To, int Weight);
        private readonly Dictionary<string, List<(string To, int Weight)>> adjacency = new(StringComparer.OrdinalIgnoreCase);

        public IEnumerable<string> Vertices => adjacency.Keys;

        public void AddEdge(string from, string to, int weight, bool bidirectional = true)
        {
            if (!adjacency.ContainsKey(from)) adjacency[from] = new();
            if (!adjacency.ContainsKey(to)) adjacency[to] = new();
            adjacency[from].Add((to, weight));
            if (bidirectional) adjacency[to].Add((from, weight));
        }

        public List<string> BreadthFirstPath(string start, string target)
        {
            if (!adjacency.ContainsKey(start) || !adjacency.ContainsKey(target)) return new();

            var queue = new Queue<string>();
            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { start };
            var parent = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase) { [start] = null };
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                string current = queue.Dequeue();
                if (current.Equals(target, StringComparison.OrdinalIgnoreCase)) break;

                foreach (var edge in adjacency[current])
                {
                    if (visited.Add(edge.To))
                    {
                        parent[edge.To] = current;
                        queue.Enqueue(edge.To);
                    }
                }
            }

            if (!parent.ContainsKey(target)) return new();
            var path = new List<string>();
            string? node = target;
            while (node != null)
            {
                path.Add(node);
                node = parent[node];
            }
            path.Reverse();
            return path;
        }

        public List<string> DepthFirstTraversal(string start)
        {
            var result = new List<string>();
            if (!adjacency.ContainsKey(start)) return result;
            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            Dfs(start, visited, result);
            return result;
        }

        private void Dfs(string node, HashSet<string> visited, List<string> result)
        {
            if (!visited.Add(node)) return;
            result.Add(node);
            foreach (var edge in adjacency[node])
                Dfs(edge.To, visited, result);
        }

        public (List<Edge> Edges, int TotalWeight) MinimumSpanningTree(string start)
        {
            var result = new List<Edge>();
            if (!adjacency.ContainsKey(start)) return (result, 0);

            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { start };
            var candidates = new PriorityQueue<Edge, int>();

            foreach (var edge in adjacency[start])
                candidates.Enqueue(new Edge(start, edge.To, edge.Weight), edge.Weight);

            int total = 0;
            while (candidates.Count > 0 && visited.Count < adjacency.Count)
            {
                Edge edge = candidates.Dequeue();
                if (visited.Contains(edge.To)) continue;

                visited.Add(edge.To);
                result.Add(edge);
                total += edge.Weight;

                foreach (var next in adjacency[edge.To])
                {
                    if (!visited.Contains(next.To))
                        candidates.Enqueue(new Edge(edge.To, next.To, next.Weight), next.Weight);
                }
            }

            return (result, total);
        }
    }
}
