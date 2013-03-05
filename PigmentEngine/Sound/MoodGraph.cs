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
        List<MoodNode> nodes;

        public MoodGraph(List<MoodNode> nodes)
        {
            Contract.Requires<ArgumentNullException>(nodes != null, "nodes");
            Contract.Ensures(this.nodes != null, "this.nodes must not be null after this method executes.");
            this.nodes = nodes;
        }

        public MoodGraph()
        {
            Contract.Ensures(this.nodes != null, "this.nodes must not be null after this method executes.");
            this.nodes = new List<MoodNode>();
        }

        public void AddNode(MoodNode node)
        {
            Contract.Ensures(nodes.Contains(node), "nodes must contain the supplied node after this method executes.");
            nodes.Add(node);
        }

        public void AddDirectedEdge(MoodNode from, MoodNode to, int cost)
        {
            from.AdjacenciesTo.Add(new Tuple<MoodNode,int>(to,cost));
        }

        public void ComputeShortestPath(MoodNode from, MoodNode to)
        {
            Dictionary<MoodNode, double> distances = new Dictionary<MoodNode, double>();
            List<MoodNode> unvisited = new List<MoodNode>();
            foreach (MoodNode node in nodes)
            {
                if (node.Equals(from))
                {
                    distances.Add(node, 0);
                }
                else
                {
                    distances.Add(node, double.PositiveInfinity);
                    unvisited.Add(node);
                }
            }
            MoodNode current = from;
            foreach (Tuple<MoodNode, int> nodeCostPair in from.AdjacenciesTo)
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

        }

    }
}
