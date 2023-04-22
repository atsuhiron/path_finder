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
        public List<uint> NodeIndecies { get; init; }

        public Nodes()
        {
            Edges = new List<IEdge>();
            NodeIndecies = new List<uint>();
        }

        public Nodes(List<IEdge> edges)
        {
            Edges = edges;
            var _ni = new HashSet<uint>(edges.Select(e => e.Start));
            _ni.UnionWith(edges.Select(e => e.End));
            NodeIndecies = _ni.ToList();
            NodeIndecies.Sort();
        }

        public Nodes(List<IEdge> edges, List<uint> nodeIndecies)
        {
            Edges = edges;
            NodeIndecies = nodeIndecies;
            if (! CheckAllEdge())
            {
                throw new ArgumentException("未定義の Node が Edge の定義に含まれています。");
            }
        }

        private bool CheckAllEdge()
        {
            var nodeSet = new HashSet<uint>(NodeIndecies);
            return Edges.All(e => nodeSet.Contains(e.Start)) && Edges.All(e => nodeSet.Contains(e.End));
        }

        private bool CheckEdge(IEdge edge)
        {
            return NodeIndecies.Contains(edge.Start) && NodeIndecies.Contains(edge.End);
        }
    }
}
