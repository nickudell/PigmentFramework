using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace Pigment.Engine.Sound
{
    class Graph<T>
    {
        /// <summary>
        /// The nodes that make up the graph
        /// </summary>
        private List<Node<T>> nodes;

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes that make up the graph.
        /// </value>
        public List<Node<T>> Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        public Graph(List<Node<T>> nodes)
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
            this.nodes = new List<Node<T>>();
        }

        /// <summary>
        /// Adds the node to the graph.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void AddNode(Node<T> node)
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
        public void AddDirectedEdge(Node<T> from, Node<T> to, int cost)
        {
            from.Adjacencies.Add(new Tuple<Node<T>,int>((Node<T>)to,cost));
            foreach (Node<T> node in nodes)
            {
                ComputeShortestPath(node, to.Content);
            }
        }

        /// <summary>
        /// Computes the nearest hops for all nodes in the graph
        /// </summary>
        public void ComputeNearestHops()
        {
            //reset the next hops
            foreach (Node<T> node in nodes)
            {
                foreach (T content in node.NextHopTo.Keys)
                {
                    node.NextHopTo[content] = null;
                }
            }
            foreach (Node<T> node in nodes)
            {
                foreach (T content in node.NextHopTo.Keys)
                {
                    //only make paths for un-mapped entities
                    if (node.NextHopTo[content] == null)
                    {
                        Queue<Node<T>> path = ComputeShortestPath(node, content);
                        Node<T> current = path.Dequeue();
                        while (path.Count > 0)
                        {
                            Node<T> next = path.Dequeue();
                            current.NextHopTo[content] = next;
                            current = next;
                        }
                        current.NextHopTo[content] = current;
                    }
                }
            }
        }

        /// <summary>
        /// Computes the shortest path.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public Queue<Node<T>> ComputeShortestPath(Node<T> from, T to)
        {
            Contract.Requires<ArgumentNullException>(from != null, "from");
            Contract.Requires<ArgumentNullException>(to != null, "to");
            Contract.Requires<ArgumentException>(!from.Content.Equals(to), "from and to are equal");

            Dictionary<Node<T>, double> distances = new Dictionary<Node<T>, double>();
            List<Node<T>> unvisited = new List<Node<T>>();
            Dictionary<Node<T>, Node<T>> previous = new Dictionary<Node<T>, Node<T>>();
            Queue<Node<T>> path = new Queue<Node<T>>();
            foreach (Node<T> node in nodes)
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
                Node<T> current = null;
                double minDist = double.PositiveInfinity;
                //pick unvisited node with smallest distance
                foreach (Node<T> node in unvisited)
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
                foreach (Tuple<Node<T>, int> nodeCostPair in from.Adjacencies)
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
                if (current.Content.Equals(to)) //found the destination, build the node queue
                {
                    Stack<Node<T>> reversedPath = new Stack<Node<T>>();
                    reversedPath.Push(current);
                    while (previous.ContainsKey(current))
                    {
                        reversedPath.Push(previous[current]);
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
}
