using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackLib
{
    public class LinkedTree<T> : Tree<T>
    {
        #region 外部成員
        public override Tree<T> Parent
        {
            get
            {
                return parent;
            }
        }
        public override TreeList<T> Children
        {
            get
            {
                return children;
            }
        }
        public override int Degree
        {
            get
            {
                return childrenList.Count;
            }
        }
        public override int Count
        {
            get
            {
                return count;
            }
        }
        public override int Depth
        {
            get
            {
                return depth;
            }
        }
        public override int Level
        {
            get
            {
                return level;
            }
        }

        #endregion 外部成員

        #region 內部成員
        protected LinkedList<LinkedTree<T>> childrenList;

        protected LinkedTree<T> parent;

        protected LinkedTreeList<T> children;


        protected int count;

        protected int depth;

        protected int level;
        #endregion 內部成員

        #region 建構子
        public LinkedTree(T value)
            : base(value)
        {
            childrenList = new LinkedList<LinkedTree<T>>();
            children = new LinkedTreeList<T>(childrenList);
            depth = 1;
            level = 1;
            count = 1;
        }

        #endregion 建構子

        #region 新增
        /// <summary>
        /// 新增子節點
        /// </summary>
        /// <param name="value"></param>
        public override void Add(T value)
        {
            Add(new LinkedTree<T>(value));
        }
        /// <summary>
        /// 新增子樹
        /// </summary>
        /// <param name="tree"></param>
        public override void Add(Tree<T> tree)
        {
            LinkedTree<T> gtree = (LinkedTree<T>)tree;
            if (gtree.Parent != null)
                gtree.Remove();
            gtree.parent = this;
            if (gtree.depth + 1 > depth)
            {
                depth = gtree.depth + 1;
                BubbleDepth();
            }
            gtree.level = level + 1;
            gtree.UpdateLevel();
            childrenList.AddLast(gtree);
            count += tree.Count;
            BubbleCount(tree.Count);
        }
        #endregion 新增

        #region 刪除
        public override void Remove()
        {
            if (parent == null)
                return;
            parent.childrenList.Remove(this);
            if (depth + 1 == parent.depth)
                parent.UpdateDepth();
            parent.count -= count;
            parent.BubbleCount(-count);
            parent = null;
        }

        #endregion 刪除

        #region 複製
        public override Tree<T> Clone()
        {
            return Clone(1);
        }

        #endregion 複製

        #region 獲取
        /// <summary>
        /// 獲取子孫
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Tree<T>> GetDescendants()
        {
            var nodes = new Stack<LinkedTree<T>>(new[] { this });
            while (nodes.Any())
            {
                LinkedTree<T> node = nodes.Pop();
                yield return node;
                foreach (var n in node.Children) nodes.Push((LinkedTree<T>)n);
            }
        }
        #endregion 獲取

        protected LinkedTree<T> Clone(int level)
        {
            LinkedTree<T> cloneTree = new LinkedTree<T>(Value);
            cloneTree.depth = depth;
            cloneTree.level = level;
            cloneTree.count = count;
            foreach (LinkedTree<T> child in childrenList)
            {
                LinkedTree<T> cloneChild = child.Clone(level + 1);
                cloneChild.parent = cloneTree;
                cloneTree.childrenList.AddLast(cloneChild);
            }
            return cloneTree;
        }

        protected void BubbleDepth()
        {
            if (parent == null)
                return;

            if (depth + 1 > parent.depth)
            {
                parent.depth = depth + 1;
                parent.BubbleDepth();
            }
        }

        protected void UpdateDepth()
        {
            int tmpDepth = depth;
            depth = 1;
            foreach (LinkedTree<T> child in childrenList)
                if (child.depth + 1 > depth)
                    depth = child.depth + 1;
            if (tmpDepth == depth || parent == null)
                return;
            if (tmpDepth + 1 == parent.depth)
                parent.UpdateDepth();
        }

        protected void BubbleCount(int diff)
        {
            if (parent == null)
                return;

            parent.count += diff;
            parent.BubbleCount(diff);
        }

        protected void UpdateLevel()
        {
            int childLevel = level + 1;
            foreach (LinkedTree<T> child in childrenList)
            {
                child.level = childLevel;
                child.UpdateLevel();
            }
        }

    }

    public class LinkedTreeList<T> : TreeList<T>
    {
        protected LinkedList<LinkedTree<T>> list;

        public LinkedTreeList(LinkedList<LinkedTree<T>> list)
        {
            this.list = list;
        }

        public override int Count
        {
            get
            {
                return list.Count;
            }
        }

        public override IEnumerator<Tree<T>> GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
