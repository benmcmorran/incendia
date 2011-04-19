using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incendia
{
    interface IGraph<T>
    {
        float LeastCostEstimate(T start, T end);
        Dictionary<T, float> AdjacentCost(T node);
    }
}
