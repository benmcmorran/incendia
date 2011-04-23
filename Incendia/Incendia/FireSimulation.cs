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
    enum FireState
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
    class FireSimulation
    {
        /// <summary>
        /// Defines the flammability each cells, expressed as the probability of the
        /// given cell igniting in one time step if it has a burning neighbor.
        /// </summary>
        public float[,] Flammability { get; private set; }
        
        /// <summary>
        /// Defines the amount of material in each cell. A burning cell will burn one material
        /// in one time step.
        /// </summary>
        public int[,] Material { get; private set; }

        /// <summary>
        /// Defines the state of each cell.
        /// </summary>
        public FireState[,] State { get; private set; }

        private int width;
        private int height;

        private Random random = new Random();

        /// <summary>
        /// Creates a new FireSimulation.
        /// </summary>
        /// <param name="width">Width of the simulation grid, in cells.</param>
        /// <param name="height">Height of the simulation grid, in cells.</param>
        public FireSimulation(int width, int height)
        {
            Flammability = new float[width, height];

            Material = new int[width, height];
            State = new FireState[width, height];

            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Advances the simulation one time step.
        /// </summary>
        public void Step()
        {
            int[,] newMaterial = new int[width, height];
            FireState[,] newState = new FireState[width, height];
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Set new material and state equal to present by default (nothing happens)
                    newMaterial[x, y] = Material[x, y];
                    newState[x, y] = State[x, y];

                    switch (State[x, y])
                    {
                        case FireState.Nonflammable:
                            break;

                        case FireState.Unburned:
                            // If so, this cell may start burning based on its flammability
                            if (HasBurningNeighbor(x, y) && random.NextDouble() < Flammability[x, y])
                                newState[x, y] = FireState.Burning;
                            break;

                        case FireState.Burning:
                            newMaterial[x, y]--;
                            if (newMaterial[x, y] <= 0)
                                newState[x, y] = FireState.Burnt;
                            break;

                        case FireState.Burnt:
                            break;
                    }

                }
            }

            Material = newMaterial;
            State = newState;
        }

        /// <summary>
        /// Determines if the fire has gone out.
        /// </summary>
        /// <returns>Returns true if nothing is burning.</returns>
        public bool IsDead()
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (State[x, y] == FireState.Burning)
                        return false;

            return true;
        }

        private bool HasBurningNeighbor(int x, int y)
        {
            for (int offsetX = -1; offsetX <= 1; offsetX++)
                for (int offsetY = -1; offsetY <= 1; offsetY++)
                {
                    if (IsValidCell(x + offsetX, y + offsetY)
                        && State[x + offsetX, y + offsetY] == FireState.Burning)
                        return true;
                }

            return false;
        }

        private bool IsValidCell(int x, int y)
        {
            return (0 <= x && x < width && 0 <= y && y < height);
        }
    }
}
