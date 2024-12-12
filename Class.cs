namespace WinFormsApp1
{
    
    public class Vertex
    {
        public int Id { get; set; }

        public Vertex(int id)
        {
            Id = id;
        }
    }

    public class Edge
    {
        public Vertex From { get; set; }
        public Vertex To { get; set; }

        public Vertex Vertex
        {
            get => default;
            set
            {
            }
        }

        public Edge(Vertex from, Vertex to)
        {
            From = from;
            To = to;
        }
    }

    public class Graph
    {
        private List<Vertex> vertices;
        private List<Edge> edges;

        private const int MaxVertices = 20;
        private const int MaxEdges = 50;

        public List<Vertex> Vertices
        {
            get { return vertices; }
        }

        public List<Edge> Edges
        {
            get { return edges; }
        }

        public Edge Edge
        {
            get => default;
            set
            {
            }
        }

        public Vertex Vertex
        {
            get => default;
            set
            {
            }
        }

        public Graph()
        {
            vertices = new List<Vertex>();
            edges = new List<Edge>();
        }

        public void AddVertex(Vertex vertex)
        {
            if (vertices.Count >= MaxVertices)
            {
                throw new Exception($"Не можна додати більше ніж {MaxVertices} вершин");
            }
            vertices.Add(vertex);
        }

        public void AddEdge(Edge edge)
        {
            if (edges.Count >= MaxEdges)
            {
                throw new Exception($"Не можна додати більше ніж {MaxEdges} ребер");
            }
            edges.Add(edge);
        }

        public void RemoveLoops()
        {
            edges.RemoveAll(edge => edge.From == edge.To);
        }

        public int CountCentralVertices()
        {
            Dictionary<Vertex, int> eccentricities = new Dictionary<Vertex, int>();
            foreach (Vertex v in vertices)
            {
                eccentricities[v] = GetEccentricity(v);
            }
            int minEccentricity = eccentricities.Values.Min();
            int centralVerticesCount = eccentricities.Count(kv => kv.Value == minEccentricity);
            return centralVerticesCount;
        }

        public int GetEccentricity(Vertex vertex)
        {
            int maxDistance = 0;
            foreach (Vertex v in vertices)
            {
                if (v != vertex)
                {
                    int distance = GetShortestPath(vertex, v).Count;
                    maxDistance = Math.Max(maxDistance, distance);
                }
            }
            return maxDistance;
        }


        public List<Vertex> GetShortestPath(Vertex start, Vertex end)
        {
            Dictionary<Vertex, Vertex> previous = new Dictionary<Vertex, Vertex>();
            Dictionary<Vertex, int> distances = new Dictionary<Vertex, int>();
            List<Vertex> nodes = new List<Vertex>();

            List<Vertex> path = new List<Vertex>();


            foreach (var vertex in vertices)
            {
                if (vertex == start)
                {
                    distances[vertex] = 0;
                }
                else
                {
                    distances[vertex] = int.MaxValue;
                }

                nodes.Add(vertex);
            }

            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => distances[x] - distances[y]);

                var smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest == end)
                {
                    path = new List<Vertex>();
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }

                foreach (var neighbor in GetNeighbors(smallest))
                {
                    var alt = distances[smallest] + GetDistance(smallest, neighbor);
                    if (alt < distances[neighbor])
                    {
                        distances[neighbor] = alt;
                        previous[neighbor] = smallest;
                    }
                }
            }

            return path;
        }

        public List<Vertex> GetNeighbors(Vertex node)
        {
            List<Vertex> neighbors = new List<Vertex>();

            foreach (Edge edge in edges)
            {
                if (edge.From == node)
                {
                    neighbors.Add(edge.To);
                }
                else if (edge.To == node)
                {
                    neighbors.Add(edge.From);
                }
            }

            return neighbors;
        }
        public int GetDistance(Vertex nodeA, Vertex nodeB)
        {
            foreach (Edge edge in edges)
            {
                if ((edge.From == nodeA && edge.To == nodeB) || (edge.From == nodeB && edge.To == nodeA))
                {
                    return 1;
                }
            }

            // Поверніть велике число, якщо вершини не з'єднані.
            return vertices.Count;
        }


    }
}
