using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Incendia
{
    /// <summary>
    /// Represents the possible states of a cell in a FireSimulation.
    /// </summary>
    public enum FireState
    {
        /// <summary>
        /// Nonflammable cells are not affected by simulation.
        /// </summary>
        Nonflammable,

        /// <summary>
        /// Unburned cells can ignite if they have a burning neighbor.
        /// </summary>
        Unburned,

        /// <summary>
        /// Burning cells are losing material, and can ignite other cells.
        /// </summary>
        Burning,

        /// <summary>
        /// Burnt cells are not affected by simulation. A from Buring to Burnt when it runs out
        /// of material.
        /// </summary>
        Burnt
    }
    
    /// <summary>
    /// Simulates the spread of a fire based on a cellular automata model.
    /// </summary>
    static class FireSimulation
    {
        /// <summary>
        /// Advances the simulation one time step.
        /// </summary>
        public static void Step(Tile[,] map)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Set new material and state equal to present by default (nothing happens)
                    int material = map[x, y].Material;
                    FireState state = map[x, y].State;

                    switch (map[x, y].State)
                    {
                        case FireState.Nonflammable:
                            break;

                        case FireState.Unburned:
                            // If so, this cell may start burning based on its flammability
                            if (HasBurningNeighbor(x, y, map) && Global.rand.NextDouble() < map[x, y].Flammability)
                                state = FireState.Burning;
                            break;

                        case FireState.Burning:
                            material--;
                            if (material <= 0)
                                state = FireState.Burnt;
                            break;

                        case FireState.Burnt:
                            break;
                    }

                    map[x, y].UpdateBurning(material, state);
                }
            }
        }

        private static bool HasBurningNeighbor(int x, int y, Tile[,] map)
        {
            for (int offsetX = -1; offsetX <= 1; offsetX++)
                for (int offsetY = -1; offsetY <= 1; offsetY++)
                {
                    if (IsValidCell(x + offsetX, y + offsetY, map)
                        && map[x + offsetX, y + offsetY].State == FireState.Burning)
                        return true;
                }

            return false;
        }

        private static bool IsValidCell(int x, int y, Tile[,] map)
        {
            return (0 <= x && x < map.GetLength(0) && 0 <= y && y < map.GetLength(1));
        }
    }
}
