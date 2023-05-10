using PathFinder.Geom;
using PathFinder.PathFinderAlgorithm;

namespace PathFinder
{
    internal class Program
    {
        static void Main(string[] _)
        {
            Console.WriteLine("Hello, World!");
            var edges = new List<IEdge>()
            {
                new NonDirectionalEdge(0, 1, 2.0f),
                new NonDirectionalEdge(1, 3, 2.0f),
                new NonDirectionalEdge(0, 2, 1.0f),
                new NonDirectionalEdge(2, 3, 1.5f)
            };
            var graph = new Graph(
                edges,
                (int index) => new CoreNode(index)); // (2)

            var finder = new Dijkstra(graph); // (3)
            var route = finder.FindRoute(0, 3); // (4)

            Console.WriteLine($"Success: {route.Success}");
            Console.WriteLine($"Iteration number: {route.Iteration}");
            Console.WriteLine($"Total cost: {route.SumCost()}");
            for (int i = 0; i < route.RouteNodeIndices.Count; i++) Console.WriteLine($"Node{i}: {route.RouteNodeIndices[i]}");
        }
    }
}