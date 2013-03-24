using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pigment.Engine.Sound
{
    /// <summary>
    /// A uni-directional edge in a graph system
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// The destination
        /// </summary>
        private IGraphNode destination;

        /// <summary>
        /// Gets or sets the destination.
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
        public IGraphNode Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        /// <summary>
        /// The cost
        /// </summary>
        private int cost;

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>
        /// The cost.
        /// </value>
        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> class.
        /// </summary>
        public Edge() : this(null,0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> class.
        /// </summary>
        /// <param name="destination">The destination.</param>
        public Edge(IGraphNode destination) : this(destination,0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> class.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="cost">The cost.</param>
        public Edge(IGraphNode destination, int cost)
        {
            this.destination = destination;
            this.cost = cost;
        }
    }
}
