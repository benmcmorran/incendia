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
    /// Represents the possible smoke states in a FireSimulation.
    /// </summary>
    public enum SmokeState
    {
        /// <summary>
        /// A cell that contains smoke.
        /// </summary>
        WithSmoke,

        /// <summary>
        /// A cell that does not contain smoke.
        /// </summary>
        WithoutSmoke
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
            int[,] material = new int[width, height];
            FireState[,] state = new FireState[width, height];
            SmokeState[,] smoke = new SmokeState[width, height];
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Set new material and state equal to present by default (nothing happens)
                    material[x, y] = map[x, y].Material;
                    state[x, y] = map[x, y].State;
                    smoke[x, y] = map[x, y].Smoke;

                    switch (map[x, y].State)
                    {
                        case FireState.Nonflammable:
                            break;

                        case FireState.Unburned:
                            // If so, this cell may start burning based on its flammability
                            if (HasNeighbor(x, y, map, t => t.State == FireState.Burning)
                                && Global.rand.NextDouble() < map[x, y].Flammability)
                                state[x, y] = FireState.Burning;
                            break;

                        case FireState.Burning:
                            material[x, y]--;
                            if (material[x, y] <= 0)
                                state[x, y] = FireState.Burnt;
                            break;

                        case FireState.Burnt:
                            break;
                    }

                    switch (map[x, y].Smoke)
                    {
                        case SmokeState.WithSmoke:
                            break;

                        case SmokeState.WithoutSmoke:
                            if (((HasNeighbor(x, y, map, t => t.Smoke == SmokeState.WithSmoke)
                                  && Global.rand.NextDouble() < Global.SmokeSpread)
                                 || state[x, y] == FireState.Burning)
                                && !map[x, y].Solid)
                                smoke[x, y] = SmokeState.WithSmoke;
                            break;
                    }
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y].UpdateBurning(material[x, y], state[x, y], smoke[x, y]);
                }
            }
        }

        private static bool HasNeighbor(int x, int y, Tile[,] map, Predicate<Tile> predicate)
        {
            for (int offsetX = -1; offsetX <= 1; offsetX++)
                for (int offsetY = -1; offsetY <= 1; offsetY++)
                {
                    if (IsValidCell(x + offsetX, y + offsetY, map)
                        && predicate(map[x + offsetX, y + offsetY]))
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
