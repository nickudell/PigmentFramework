using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace Pigment.Engine.Sound
{
    /// <summary>
    /// Sparse-graph
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// The nodes that make up the graph
        /// </summary>
        protected List<IGraphNode> nodes;

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes that make up the graph.
        /// </value>
        public List<IGraphNode> Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        public Graph(List<IGraphNode> nodes)
        {
            Contract.Requires<ArgumentNullException>(nodes != null, "nodes");
            Contract.Ensures(this.nodes != null, "this.nodes must not be null after this method executes.");
            this.nodes = nodes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        public Graph()
        {
            Contract.Ensures(this.nodes != null, "this.nodes must not be null after this method executes.");
            this.nodes = new List<IGraphNode>();
        }

        /// <summary>
        /// Adds the node to the graph.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void AddNode(IGraphNode node)
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
        public void AddDirectedEdge(IGraphNode from, IGraphNode to, int cost)
        {
            from.Adjacencies.Add(new Edge(to,cost));
        }
        
        public delegate bool IsDestinationDelegate(IGraphNode node);

        /// <summary>
        /// Computes the shortest path between two nodes using Djikstra's algorithm.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public Queue<Edge> PathTo(IGraphNode from, IsDestinationDelegate destinationTest)
        {
            Contract.Requires<ArgumentNullException>(from != null, "from");
            Contract.Requires<ArgumentNullException>(destinationTest != null, "destinationTest");
            Contract.Requires<ArgumentException>(!destinationTest(from), "from is the destination.");

            Dictionary<IGraphNode, double> distances = new Dictionary<IGraphNode, double>();
            List<IGraphNode> unvisited = new List<IGraphNode>();
            Dictionary<IGraphNode, IGraphNode> previous = new Dictionary<IGraphNode, IGraphNode>();
            Queue<Edge> path = new Queue<Edge>();

            foreach (IGraphNode node in nodes)
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
                IGraphNode current = null;
                double minDist = double.PositiveInfinity;
                //pick unvisited node with smallest distance
                foreach (IGraphNode node in unvisited)
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
                foreach (Edge edge in from.Adjacencies)
                {
                    if (unvisited.Contains(edge.Destination))
                    {
                        double distance = distances[from] + edge.Cost;
                        if (distance < distances[edge.Destination])
                        {
                            distances[edge.Destination] = distance;
                        }
                    }
                }
                if (destinationTest(current)) //found the destination, build the node queue
                {
                    Stack<Edge> reversedPath = new Stack<Edge>();
                    while (previous.ContainsKey(current))
                    {
                    int cost = previous[current].Adjacencies.Find((o) => { return o.Destination.Equals(current); }).Cost;
                    reversedPath.Push(new Edge(current,cost));
                    current = previous[current];
                    }
                    while(reversedPath.Count > 0)
                    {
                        path.Enqueue(reversedPath.Pop());
                    }
                }
                else
                {
                    unvisited.Remove(current);
                }
            }
            return path;
        }
    }

    public class Graph<T> : Graph
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        public Graph(List<IGraphNode<T>> nodes)
        {
            Contract.Requires<ArgumentNullException>(nodes != null, "nodes");
            Contract.Ensures(this.nodes != null, "this.nodes must not be null after this method executes.");
            this.nodes = new List<IGraphNode>();
            foreach(IGraphNode<T> node in nodes)
            {
                this.nodes.Add(node);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        public Graph()
        {
            Contract.Ensures(this.nodes != null, "this.nodes must not be null after this method executes.");
            this.nodes = new List<IGraphNode>();
        }

        /// <summary>
        /// Computes the nearest hops for all nodes in the graph
        /// </summary>
        public void ComputeNearestHops()
        {
            //reset the next hops
            foreach (IGraphNode<T> node in nodes)
            {
                foreach (T content in node.NextHopTo.Keys)
                {
                    node.NextHopTo[content] = null;
                }
            }
            foreach (IGraphNode<T> node in nodes)
            {
                foreach (T content in node.NextHopTo.Keys)
                {
                    //only make paths for un-mapped entities
                    if (node.NextHopTo[content] == null)
                    {
                        Queue<Edge> path = PathTo(node, (o)=>{
                            IGraphNode<T> ot = o as IGraphNode<T>;
                            if(ot == null)
                            {
                                return false;
                            }
                            else
                            {
                                return ot.Equals(content);
                            }});
                        Edge edge = path.Dequeue();
                        IGraphNode<T> current = (IGraphNode<T>)edge.Destination;
                        while (path.Count > 0)
                        {
                            IGraphNode<T> next = (IGraphNode<T>)path.Peek().Destination;
                            current.NextHopTo[content] = new Edge(next,edge.Cost);
                            current = next;
                        }
                        current.NextHopTo[content] = new Edge(current,0);
                    }
                }
            }
        }

    }
}
