using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackLib
{
    public abstract class Tree<T>
    {
        public T Value { get; set; }

        public abstract Tree<T> Parent { get; }
        public abstract TreeList<T> Children { get; }
        public abstract int Count { get; }
        public abstract int Degree { get; }
        public abstract int Depth { get; }
        public abstract int Level { get; }

        public Tree(T value)
        {
            this.Value = value;
        }

        public abstract void Add(T value);
        public abstract void Add(Tree<T> tree);
        public abstract void Remove();
        public abstract Tree<T> Clone();
        public abstract IEnumerable<Tree<T>> GetDescendants();
    }

    public abstract class TreeList<T> : IEnumerable<Tree<T>>
    {
        public abstract int Count { get; }
        public abstract IEnumerator<Tree<T>> GetEnumerator();

        IEnumerator<Tree<T>> IEnumerable<Tree<T>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
