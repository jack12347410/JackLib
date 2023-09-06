using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackLib
{
    public abstract class BinaryTree<T>
    {
        public T Value { get; set; }
        public abstract int Count { get; }
        public abstract int Depth { get; }
        public abstract BinaryTree<T> Parent { get; }
        public abstract BinaryTree<T> Left { get; }
        public abstract BinaryTree<T> Right { get; }
        public abstract int Level { get; }

        public BinaryTree(T value)
        {
            this.Value = value;
        }

        public abstract void AddLeft(T value);
        public abstract void AddRight(T value);
        public abstract void AddLeft(BinaryTree<T> tree);
        public abstract void AddRight(BinaryTree<T> tree);
        public abstract void Remove();
        public abstract IEnumerable<BinaryTree<T>> GetDescendants();
        public static void Copy(BinaryTree<T> srcTree, BinaryTree<T> destTree)
        {
            if (srcTree.Left != null)
            {
                T value = srcTree.Left.Value;
                destTree.AddLeft(value);
                Copy(srcTree.Left, destTree.Left);
            }
            if (srcTree.Right != null)
            {
                T value = srcTree.Right.Value;
                destTree.AddRight(value);
                Copy(srcTree.Right, destTree.Right);
            }
        }
    }
}
