using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pigment.Engine.Sound
{
    /// <summary>
    /// Interface for graph nodes
    /// </summary>
    public interface IGraphNode
    {
        /// <summary>
        /// Gets or sets the adjacencies.
        /// </summary>
        /// <value>
        /// The adjacencies.
        /// </value>
        List<Edge> Adjacencies
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Interface for graph nodes which are equatable by some T object, incorporating next-hop-to buffers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGraphNode<T> : IGraphNode, IEquatable<T>
    {
        /// <summary>
        /// Gets or sets the next hop to.
        /// </summary>
        /// <value>
        /// The next hop to.
        /// </value>
        Dictionary<T, Edge> NextHopTo
        {
            get;
            set;
        }
    }
}
