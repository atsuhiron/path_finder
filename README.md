# path_finder
Finds the shortest path between any two nodes in a graph structure.

## Usage
1. Define the graph, either by loading a json file (1) or defining it on the fly (2).
1. Create pathfinding instance(3)
1. Specify the start and goal node indices to obtain a route. (4)
```CSharp
using PathFinder.IO;

var graph = FromJson.Load("path/to/file.json"); // (1)
/*
// The following graph will be produced.
//       2.0
//     0 --- 1
// 1.0 |     | 2.0
//     2-----3
//       1.5

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
*/

var finder = new Dijkstra(graph); // (3)
var route = finder.FindRoute(0, 3); // (4)

// Show result
Console.WriteLine($"Success: {route.Success}");
Console.WriteLine($"Iteration number: {route.Iteration}");
Console.WriteLine($"Total cost: {route.SumCost()}");
for (int i = 0; i < route.RouteNodeIndices.Count; i++) Console.WriteLine($"Node{i}: {route.RouteNodeIndices[i]}");
// Success: True
// Iteration number: 3
// Total cost: 2.5
// Node0: 0
// Node1: 2
// Node2: 3
```

## Sample of JSON
The graph generated in (2) above is represented by the following JSON.
```json
{
  "EdgeType": "NonDirectionalEdge",
  "NodeType": "CoreNode",
  "Edges": [
    {
      "Start": 0,
      "End": 1,
      "Cost": 2.0,
      "Directed": false
    },
    {
      "Start": 1,
      "End": 3,
      "Cost": 2.0,
      "Directed": false
    },
    {
      "Start": 0,
      "End": 2,
      "Cost": 1.0,
      "Directed": false
    },
    {
      "Start": 2,
      "End": 3,
      "Cost": 1.5,
      "Directed": false
    }
  ],
  "Nodes": [
    { "Index": 0 },
    { "Index": 1 },
    { "Index": 2 },
    { "Index": 3 }
  ]
}
```