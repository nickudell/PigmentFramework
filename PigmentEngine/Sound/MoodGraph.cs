using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace PigmentEngine.Sound
{
    class MoodGraph
    {
        /// <summary>
        /// The nodes that make up the graph
        /// </summary>
        private List<MoodNode> nodes;

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes that make up the graph.
        /// </value>
        public List<MoodNode> Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoodGraph"/> class.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        public MoodGraph(List<MoodNode> nodes)
        {
            Contract.Requires<ArgumentNullException>(nodes != null, "nodes");
            Contract.Ensures(this.nodes != null, "this.nodes must not be null after this method executes.");
            this.nodes = nodes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoodGraph"/> class.
        /// </summary>
        public MoodGraph()
        {
            Contract.Ensures(this.nodes != null, "this.nodes must not be null after this method executes.");
            this.nodes = new List<MoodNode>();
        }

        /// <summary>
        /// Adds the node to the graph.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void AddNode(MoodNode node)
        {
            Contract.Ensures(nodes.Contains(node), "nodes must contain the supplied node after this method executes.");
            nodes.Add(node);
        }

        /// <summary>
        /// Adds a directed edge conntecting the from node to the to node, with the specified cost
        /// </summary>
        /// <param name="from">The from node.</param>
        /// <param name="to">The to node.</param>
        /// <param name="cost">The cost of the edge.</param>
        public void AddDirectedEdge(MoodNode from, MoodNode to, int cost)
        {
            from.Adjacencies.Add(new Tuple<NodeBase<Mood>,int>((NodeBase<Mood>)to,cost));
            foreach (MoodNode node in nodes)
            {
                ComputeShortestPath(node, to.Content);
            }
        }

        /// <summary>
        /// Computes the shortest path.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public void ComputeShortestPath(MoodNode from, Mood to)
        {
            Contract.Requires<ArgumentNullException>(from != null, "from");
            Contract.Requires<ArgumentNullException>(to != null, "to");
            Contract.Requires<ArgumentException>(!from.Content.Equals(to), "from and to are equal");

            Dictionary<NodeBase<Mood>, double> distances = new Dictionary<NodeBase<Mood>, double>();
            List<NodeBase<Mood>> unvisited = new List<NodeBase<Mood>>();
            Dictionary<NodeBase<Mood>, NodeBase<Mood>> previous = new Dictionary<NodeBase<Mood>, NodeBase<Mood>>();
            foreach (NodeBase<Mood> node in nodes)
            {
                if (node.Equals(from))
                {
                    distances.Add(node, 0);
                }
                else
                {
                    distances.Add(node, double.PositiveInfinity);
                }
                unvisited.Add(node);
            }
            while (unvisited.Count > 0)
            {
                NodeBase<Mood> current = null;
                double minDist = double.PositiveInfinity;
                //pick unvisited node with smallest distance
                foreach (NodeBase<Mood> node in unvisited)
                {
                    if (minDist > distances[node])
                    {
                        minDist = distances[node];
                        if (previous.ContainsKey(node))
                        {
                            previous[node] = current;
                        }

                        else
                        {
                            previous.Add(node, current);
                        }
                        current = node;
                    }
                }
                Contract.Assert(current != null);
                if (minDist == double.PositiveInfinity)
                {
                    break;
                }
                //find rough distance to next node
                foreach (Tuple<NodeBase<Mood>, int> nodeCostPair in from.Adjacencies)
                {
                    if (unvisited.Contains(nodeCostPair.Item1))
                    {
                        double distance = distances[from] + nodeCostPair.Item2;
                        if (distance < distances[nodeCostPair.Item1])
                        {
                            distances[nodeCostPair.Item1] = distance;
                        }
                    }
                }
                if (current.Content.Equals(to))
                {
                    current.NextHopTo[to] = null;
                    while (previous.ContainsKey(current))
                    {
                        NodeBase<Mood> prevNode = previous[current];
                        prevNode.NextHopTo[to] = current;
                        current = prevNode;
                    }
                }
                else
                {
                    unvisited.Remove(current);
                }
            }
        }

    }
}
