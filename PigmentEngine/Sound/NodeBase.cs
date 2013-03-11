using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace PigmentEngine.Sound
{
    public class NodeBase<T>
    {
        /// <summary>
        /// The next hop to take get to a different mood
        /// </summary>
        private Dictionary<T, NodeBase<T>> nextHopTo;

        /// <summary>
        /// Gets or sets the next hop to.
        /// </summary>
        /// <value>
        /// The next hop to.
        /// </value>
        public Dictionary<T, NodeBase<T>> NextHopTo
        {
            get
            {
                return nextHopTo;
            }
            set
            {
                nextHopTo = value;
            }
        }

        /// <summary>
        /// The content of the node
        /// </summary>
        private T content;

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
	    public T Content
	    {
		    get { return content;}
		    set { content = value;}
	    }
	

        /// <summary>
        /// The adjacencies
        /// </summary>
        private List<Tuple<NodeBase<T>, int>> adjacencies;

        /// <summary>
        /// Gets or sets the adjacencies.
        /// </summary>
        /// <value>
        /// The adjacencies.
        /// </value>
        public List<Tuple<NodeBase<T>, int>> Adjacencies
        {
            get { return adjacencies; }
            set { adjacencies = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoodNode" /> class.
        /// </summary>
        /// <param name="mood">The mood.</param>
        /// <param name="phrase">The phrase.</param>
        public NodeBase()
        {
            Contract.Ensures(adjacencies != null, "adjacencies must not be null after this method executes.");
            Contract.Ensures(nextHopTo != null, "nextHopTo must not be null after this method executes.");
            adjacencies = new List<Tuple<NodeBase<T>, int>>();
            nextHopTo = new Dictionary<T, NodeBase<T>>();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoodNode" /> class.
        /// </summary>
        /// <param name="mood">The mood.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="adjacencies">The adjacent nodes with costs.</param>
        public NodeBase(List<Tuple<NodeBase<T>, int>> adjacencies) : this()
        {
            Contract.Requires<ArgumentNullException>(adjacencies != null, "adjacencies");
            this.adjacencies = adjacencies;
        }
    }
}
