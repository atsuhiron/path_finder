﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder.Geom
{
    public class Nodes
    {
        public List<IEdge> Edges { get; init; }
        public List<int> NodeIndices { get; init; }

        public Nodes()
        {
            Edges = new List<IEdge>();
            NodeIndices = new List<int>();
        }

        public Nodes(List<IEdge> edges)
        {
            Edges = new HashSet<IEdge>(edges).ToList();
            var _ni = new HashSet<int>(edges.Select(e => e.Start));
            _ni.UnionWith(edges.Select(e => e.End));
            NodeIndices = _ni.ToList();
            NodeIndices.Sort();
        }

        public Nodes(List<IEdge> edges, List<int> nodeIndices)
        {
            Edges = new HashSet<IEdge>(edges).ToList();
            NodeIndices = nodeIndices;
            NodeIndices.Sort();
            if (! CheckAllEdge())
            {
                throw new ArgumentException("未定義の Node が Edge の定義に含まれています。");
            }
        }

        public void AddEdge(IEdge edge, bool addNode)
        {
            if (Edges.Contains(edge)) return;

            if (CheckEdge(edge))
            {
                Edges.Add(edge);
                return;
            }

            if (! addNode)
            {
                throw new ArgumentException("未定義の Node が Edge の定義に含まれています。");
            }

            if (! NodeIndices.Contains(edge.Start)) NodeIndices.Add(edge.Start);
            if (! NodeIndices.Contains(edge.End)) NodeIndices.Add(edge.End);
            NodeIndices.Sort();
            Edges.Add(edge);
        }

        private bool CheckAllEdge()
        {
            var nodeSet = new HashSet<int>(NodeIndices);
            return Edges.All(e => nodeSet.Contains(e.Start)) && Edges.All(e => nodeSet.Contains(e.End));
        }

        private bool CheckEdge(IEdge edge)
        {
            return NodeIndices.Contains(edge.Start) && NodeIndices.Contains(edge.End);
        }

        public static Nodes CreateGrid(int x, int y)
        {
            var edges = new List<IEdge>();

            foreach (int xi in Enumerable.Range(0, x))
            {
                foreach (int yi in Enumerable.Range(0, y))
                {
                    var cur = x * yi + xi;
                    if (xi != x - 1) edges.Add(new NonDirectionalEdge(cur, cur + 1));
                    if (yi != y - 1) edges.Add(new NonDirectionalEdge(cur, cur + x));
                }
            }

            return new Nodes(edges);
        }
    }
}
