using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using System.Collections;

namespace Pigment.Engine.Octree
{
    /// <summary>
    /// Enumerated child position offsets for an Octree
    /// </summary>
    public enum ChildPosition
    {
        /// <summary>
        /// The bottom left back position
        /// </summary>
        BottomLeftBack,
        /// <summary>
        /// The bottom right back position
        /// </summary>
        BottomRightBack,
        /// <summary>
        /// The top left back position
        /// </summary>
        TopLeftBack,
        /// <summary>
        /// The top right back position
        /// </summary>
        TopRightBack,
        /// <summary>
        /// The bottom left front position
        /// </summary>
        BottomLeftFront,
        /// <summary>
        /// The bottom right front position
        /// </summary>
        BottomRightFront,
        /// <summary>
        /// The top left front position
        /// </summary>
        TopLeftFront,
        /// <summary>
        /// The top right front position
        /// </summary>
        TopRightFront
    }

    /// <summary>
    /// Exception thrown when you request the contents of a node which is not a leaf
    /// </summary>
    [Serializable]
    public class NotALeafException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotALeafException" /> class.
        /// </summary>
        public NotALeafException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotALeafException" /> class.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public NotALeafException(string message) : base(message)
        {  
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotALeafException" /> class.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="inner">The inner exception.</param>
        public NotALeafException(string message, Exception inner) : base(message,inner)
        {
        }
    }

    /// <summary>
    /// An Octree comprised of Octrees, where leaf nodes contain zero or more contents of type I
    /// </summary>
    /// <typeparam name="I">The type of item this tree holds</typeparam>
    public class OcTree<I> : IEnumerable<I>, IPositioned
        where I:IPositioned
    {
        /// <summary>
        /// The child nodes of this node
        /// </summary>
        protected Dictionary<ChildPosition, OcTree<I>> children;
        /// <summary>
        /// The contents of this node
        /// </summary>
        protected List<I> contents;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is a leaf.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is a leaf; otherwise, <c>false</c>.
        /// </value>
        public bool IsALeaf { get; set; }

        /// <summary>
        /// Gets the bounding area of this node.
        /// </summary>
        /// <value>
        /// The size of the node.
        /// </value>
        public Vector3 Size {get; private set;}
        /// <summary>
        /// Gets the max number of contents this node can hold.
        /// </summary>
        /// <value>
        /// The max contents for this node.
        /// </value>
        public int MaxContents {get; private set;}

        /// <summary>
        /// Gets or sets the <see cref="OcTree{" /> with the specified child position.
        /// </summary>
        /// <value>
        /// The <see cref="OcTree{" />.
        /// </value>
        /// <param name="cp">The child position.</param>
        /// <returns></returns>
        public OcTree<I> this[ChildPosition cp]
        {
            get
            {
                return children[cp];
            }
            set
            {
                children[cp] = value;
            }
        }

        public delegate void AddOrRemoveItemDelegate(OcTree<I> sender, I item);

        protected event AddOrRemoveItemDelegate OnItemAdd;
        protected event AddOrRemoveItemDelegate OnItemRemove;

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<I> GetEnumerator()
        {
            if (IsALeaf) return contents.GetEnumerator();
            else
            {
                Queue<OcTree<I>> nodesToSearch = new Queue<OcTree<I>>();
                List<I> childContents = new List<I>();
                foreach(OcTree<I> child in children.Values)
                {
                    nodesToSearch.Enqueue(child);
                }
                while (nodesToSearch.Count > 0)
                {
                    OcTree<I> currentNode = nodesToSearch.Dequeue();
                    if (currentNode.IsALeaf)
                    {
                        childContents.AddRange(currentNode.contents);
                    }
                    else
                    {
                        foreach (OcTree<I> child in currentNode.children.Values)
                        {
                            nodesToSearch.Enqueue(child);
                        }
                    }
                }
                return childContents.GetEnumerator();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return contents.GetEnumerator();
        }

        /// <summary>
        /// Gets the position of the node.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector3 Position {get; private set;}

        /// <summary>
        /// Initializes a new instance of the <see cref="OcTree{I}" /> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        /// <param name="maxContents">The max contents.</param>
        public OcTree (Vector3 position, Vector3 size, int maxContents)
        {
            this.Position = position;
            this.Size = size;
            this.MaxContents = maxContents;
        }

        /// <summary>
        /// Gets the child which encompasses the given position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public OcTree<I> GetChildAtPosition(Vector3 position)
        {
            if (Position.X < position.X)
            {
                if (Position.Y < position.Y)
                {
                    if (Position.Z < position.Z)
                    {
                        return children[ChildPosition.BottomLeftBack];
                    }
                    else
                    {
                        return children[ChildPosition.BottomLeftFront];
                    }
                }
                else
                {
                    if (Position.Z < position.Z)
                    {
                        return children[ChildPosition.TopLeftBack];
                    }
                    else
                    {
                        return children[ChildPosition.TopLeftFront];
                    }
                }
            }
            else
            {
                if (Position.Y < position.Y)
                {
                    if (Position.Z < position.Z)
                    {
                        return children[ChildPosition.BottomRightBack];
                    }
                    else
                    {
                        return children[ChildPosition.BottomRightFront];
                    }
                }
                else
                {
                    if (Position.Z < position.Z)
                    {
                        return children[ChildPosition.TopRightBack];
                    }
                    else
                    {
                        return children[ChildPosition.TopRightFront];
                    }
                }
            }
        }

        /// <summary>
        /// Creates the children of this node.
        /// </summary>
        public virtual void CreateChildren()
        {
            children = new Dictionary<ChildPosition, OcTree<I>>();
            children.Add(ChildPosition.BottomLeftBack, new OcTree<I>(Position - Size/4,Size/2,MaxContents));
            children.Add(ChildPosition.BottomLeftFront, new OcTree<I>(Position + new Vector3(-Size.X / 4,-Size.Y/4,Size.Z/4), Size / 2, MaxContents));
            children.Add(ChildPosition.BottomRightBack, new OcTree<I>(Position + new Vector3(Size.X / 4,-Size.Y/4,-Size.Z/4),Size/2,MaxContents));
            children.Add(ChildPosition.BottomRightFront, new OcTree<I>(Position+ new Vector3(Size.X / 4,-Size.Y/4,Size.Z/4),Size/2,MaxContents));
            children.Add(ChildPosition.TopLeftBack, new OcTree<I>(Position + new Vector3(-Size.X / 4,Size.Y/4,-Size.Z/4),Size/2,MaxContents));
            children.Add(ChildPosition.TopLeftFront, new OcTree<I>(Position + new Vector3(-Size.X / 4,Size.Y/4,Size.Z/4),Size/2,MaxContents));
            children.Add(ChildPosition.TopRightBack, new OcTree<I>(Position + new Vector3(Size.X / 4,Size.Y/4,-Size.Z/4),Size/2,MaxContents));
            children.Add(ChildPosition.TopRightFront, new OcTree<I>(Position + Size/4,Size/2,MaxContents));
        }

        /// <summary>
        /// Adds the specified item to this node, or finds the correct child node to add it to.
        /// </summary>
        /// <param name="item">The item.</param>
        public virtual void Add(I item)
        {
            OcTree<I> currentNode = this;
            while (!currentNode.IsALeaf)
            {
                currentNode = currentNode.GetChildAtPosition(item.Position);
            }
            if (currentNode.contents.Count < MaxContents)
            {
                currentNode.contents.Add(item);
                if (OnItemAdd != null) OnItemAdd(currentNode, item);
            }
            else
            {
                currentNode.IsALeaf = false;
                currentNode.CreateChildren();
                currentNode.Add(item);
                foreach (I item2 in currentNode)
                {
                    currentNode.Add(item2);
                }
                currentNode.contents.Clear();
            }
        }

        /// <summary>
        /// Finds the child node containing the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public OcTree<I> FindNodeOf(I item)
        {
            return GetChildAtPosition(item.Position);
        }

        /// <summary>
        /// Deletes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Delete(I item)
        {
            GetChildAtPosition(item.Position).contents.Remove(item);
        }
    }
}
