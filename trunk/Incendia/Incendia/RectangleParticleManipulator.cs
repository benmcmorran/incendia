using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Incendia
{
    class RectangleParticleManipulator : IParticleManipulator
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public RectangleParticleManipulator(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public void ManipulateParticle(Particle particle)
        {
            if (   Position.X < particle.Position.X && particle.Position.X < Position.X + Size.X
                && Position.Y < particle.Position.Y && particle.Position.Y < Position.Y + Size.Y)
                particle.Age = particle.Lifetime + 1;
        }
    }
}
