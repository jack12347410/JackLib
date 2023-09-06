using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackLib
{
    public class LinkedBinaryTree<T> : BinaryTree<T>
    {
        #region 外部成員
        /// <summary>
        /// 總節點數
        /// </summary>
        public override int Count
        {
            get
            {
                return count;
            }
        }
        /// <summary>
        /// 深度
        /// </summary>
        public override int Depth
        {
            get
            {
                return depth;
            }
        }
        /// <summary>
        /// 父樹
        /// </summary>
        public override BinaryTree<T> Parent
        {
            get
            {
                return parent;
            }
        }
        /// <summary>
        /// 左子樹
        /// </summary>
        public override BinaryTree<T> Left
        {
            get
            {
                return left;
            }
        }
        /// <summary>
        /// 右子樹
        /// </summary>
        public override BinaryTree<T> Right
        {
            get
            {
                return right;
            }
        }
        /// <summary>
        /// 層級
        /// </summary>
        public override int Level
        {
            get
            {
                return level;
            }
        }

        #endregion 外部成員

        #region 內部成員
        private int count;

        private int depth;

        private LinkedBinaryTree<T> parent;

        private LinkedBinaryTree<T> left;

        private LinkedBinaryTree<T> right;

        private int level;
        #endregion 內部成員

        #region 建構子
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="value"></param>
        public LinkedBinaryTree(T value)
            : base(value)
        {
            count = 1;
            depth = 1;
            level = 1;
        }

        #endregion 建構子 

        #region 新增
        /// <summary>
        /// 新增左節點
        /// </summary>
        /// <param name="value"></param>
        public override void AddLeft(T value)
        {
            Add(ref left, value);
        }

        /// <summary>
        /// 新增右節點
        /// </summary>
        /// <param name="value"></param>
        public override void AddRight(T value)
        {
            Add(ref right, value);
        }

        /// <summary>
        /// 新增左子樹
        /// </summary>
        /// <param name="tree"></param>
        public override void AddLeft(BinaryTree<T> tree)
        {
            Add(ref left, tree);
        }

        /// <summary>
        /// 新增右子數
        /// </summary>
        /// <param name="tree"></param>
        public override void AddRight(BinaryTree<T> tree)
        {
            Add(ref right, tree);
        }
        #endregion 新增

        #region 移除
        /// <summary>
        /// 移除
        /// </summary>
        public override void Remove()
        {
            LinkedBinaryTree<T> sibling;
            if (parent == null)
                return;
            else if (parent.left == this)
            {
                parent.left = null;
                sibling = parent.right;
            }
            else if (parent.right == this)
            {
                parent.right = null;
                sibling = parent.left;
            }
            else
                return;

            if (depth + 1 == parent.depth)
            {
                if (sibling == null || sibling.depth < depth)
                    parent.UpdateDepth();
            }
            parent.count -= count;
            parent.BubbleCount(-count);
            parent = null;
        }
        #endregion 移除

        /// <summary>
        /// 獲取子孫
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<BinaryTree<T>> GetDescendants()
        {
            var nodes = new Stack<LinkedBinaryTree<T>>(new[] { this });
            while (nodes.Any())
            {
                LinkedBinaryTree<T> node = nodes.Pop();
                yield return node;
                if (node.Left != null) nodes.Push((LinkedBinaryTree<T>)node.Left);
                if (node.Right != null) nodes.Push((LinkedBinaryTree<T>)node.Right);
            }
        }

        protected void Add(ref LinkedBinaryTree<T> child, T value)
        {
            Add(ref child, new LinkedBinaryTree<T>(value));
        }

        protected void Add(ref LinkedBinaryTree<T> child, BinaryTree<T> tree)
        {
            if (child != null)
                throw new Exception("Child is existed");
            LinkedBinaryTree<T> tmpTree = tree as LinkedBinaryTree<T>;
            if (tmpTree == null)
            {
                tmpTree = new LinkedBinaryTree<T>(tree.Value);
                BinaryTree<T>.Copy(tree, tmpTree);
            }
            child = tmpTree;
            child.level = level + 1;
            child.parent = this;
            if (depth == 1)
            {
                depth = 2;
                BubbleDepth();
            }
            ++count;
            BubbleCount(1);
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

            if (left != null)
                depth = left.depth + 1;
            if (right != null && right.depth + 1 > depth)
                depth = right.depth + 1;

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
    }
}
