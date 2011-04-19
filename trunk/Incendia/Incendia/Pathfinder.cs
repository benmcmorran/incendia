using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinding
{
    class Pathfinder<T>
    {
        private IGraph<T> graph;

        public Pathfinder(IGraph<T> graph)
        {
            this.graph = graph;
        }

        public List<T> Solve(T start, List<T> end)
        {
            HashSet<T> openSet = new HashSet<T>();
            openSet.Add(start);
            HashSet<T> closedSet = new HashSet<T>();
            Dictionary<T, T> parent = new Dictionary<T,T>();

            Dictionary<T, float> gScore = new Dictionary<T,float>();
            Dictionary<T, float> hScore = new Dictionary<T,float>();
            Dictionary<T, float> fScore = new Dictionary<T,float>();

            gScore[start] = 0;
            hScore[start] = MinLeastCostEstimate(start, end);
            fScore[start] = hScore[start];

            while (openSet.Count != 0)
            {
                T node = (from T x in openSet
                          orderby fScore[x] ascending
                          select x).First();

                foreach (T endNode in end)
                    if (EqualityComparer<T>.Default.Equals(node, endNode))
                        return ReconstructPath(parent, endNode);

                openSet.Remove(node);
                closedSet.Add(node);

                Dictionary<T, float> neighbors = graph.AdjacentCost(node);
                foreach (T neighbor in neighbors.Keys)
                {
                    if (closedSet.Contains(neighbor)) continue;

                    float newGScore = gScore[node] + neighbors[neighbor];
                    bool isRouteBetter;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                        isRouteBetter = true;
                    }
                    else if (newGScore < gScore[neighbor])
                        isRouteBetter = true;
                    else
                        isRouteBetter = false;

                    if (isRouteBetter)
                    {
                        parent[neighbor] = node;
                        gScore[neighbor] = newGScore;
                        hScore[neighbor] = MinLeastCostEstimate(neighbor, end);
                        fScore[neighbor] = gScore[neighbor] + hScore[neighbor];
                    }
                }
            }

            return new List<T>();
        }

        private List<T> ReconstructPath(Dictionary<T, T> parent, T end)
        {
            List<T> path;

            if (parent.ContainsKey(end))
                path = ReconstructPath(parent, parent[end]);
            else
                path = new List<T>();

            path.Add(end);
            return path;
        }

        private float MinLeastCostEstimate(T start, List<T> end)
        {
            float minCost = graph.LeastCostEstimate(start, end.First());
            for (int i = 1; i < end.Count; i++)
            {
                float newCost = graph.LeastCostEstimate(start, end[i]);
                minCost = newCost < minCost ? newCost : minCost;
            }
            return minCost;
        }
    }
}
