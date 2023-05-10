# path_finder
グラフ構造内での任意2ノード間の最短経路を探索します。

## 使い方
1. グラフを定義します。json ファイルを読み込むか(1)、その場で定義します(2)。
1. 経路探索インスタンスを生成する。(3)
1. スタートとゴールのノード番号を指定すれば、経路が得られます。(4)
```CSharp
using PathFinder.IO;

var graph = FromJson.Load("path/to/file.json"); // (1)
/*
// 以下のようなグラフが出来上がる。
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

// 結果の表示
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

## JSON のサンプル
上記の (2) で生成しているグラフは以下のような JSON で表されます。
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