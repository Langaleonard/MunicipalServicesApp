using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    public class BinaryTreeNode<T>
    {
        public T Value { get; set; }
        public BinaryTreeNode<T>? Left { get; set; }
        public BinaryTreeNode<T>? Right { get; set; }

        public BinaryTreeNode(T value)
        {
            Value = value;
        }
    }

    public class BinaryTree<T>
    {
        public BinaryTreeNode<T>? Root { get; set; }

        public List<T> InOrder()
        {
            var values = new List<T>();
            Traverse(Root, values);
            return values;
        }

        private static void Traverse(BinaryTreeNode<T>? node, List<T> values)
        {
            if (node == null) return;
            Traverse(node.Left, values);
            values.Add(node.Value);
            Traverse(node.Right, values);
        }
    }
}
