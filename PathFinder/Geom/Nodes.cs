using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder.Geom
{
    public class Nodes
    {
        public List<IEdge> Edges { get; init; }
        public List<uint> NodeIndices { get; init; }

        public Nodes()
        {
            Edges = new List<IEdge>();
            NodeIndices = new List<uint>();
        }

        public Nodes(List<IEdge> edges)
        {
            Edges = edges;
            var _ni = new HashSet<uint>(edges.Select(e => e.Start));
            _ni.UnionWith(edges.Select(e => e.End));
            NodeIndices = _ni.ToList();
            NodeIndices.Sort();
        }

        public Nodes(List<IEdge> edges, List<uint> nodeIndices)
        {
            Edges = edges;
            NodeIndices = nodeIndices;
            if (! CheckAllEdge())
            {
                throw new ArgumentException("未定義の Node が Edge の定義に含まれています。");
            }
        }

        private bool CheckAllEdge()
        {
            var nodeSet = new HashSet<uint>(NodeIndices);
            return Edges.All(e => nodeSet.Contains(e.Start)) && Edges.All(e => nodeSet.Contains(e.End));
        }

        private bool CheckEdge(IEdge edge)
        {
            return NodeIndices.Contains(edge.Start) && NodeIndices.Contains(edge.End);
        }
    }
}
