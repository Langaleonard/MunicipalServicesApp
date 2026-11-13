using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    public class BasicTreeNode<T>
    {
        public T Value { get; }
        public List<BasicTreeNode<T>> Children { get; } = new();

        public BasicTreeNode(T value)
        {
            Value = value;
        }

        public BasicTreeNode<T> AddChild(T value)
        {
            var child = new BasicTreeNode<T>(value);
            Children.Add(child);
            return child;
        }

        public int CountNodes()
        {
            int count = 1;
            foreach (var child in Children)
            {
                count += child.CountNodes();
            }
            return count;
        }
    }
}
