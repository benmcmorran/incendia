using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Incendia
{
    class Particle
    {
        public Texture2D Texture { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float SpeedMultiplier { get; set; }

        public float Rotation { get; set; }
        public float RotationMultiplier { get; set; }

        public Color Color { get; set; }

        public float Scale { get; set; }
        public float ScaleMultiplier { get; set; }

        public float Age { get; set; }
        public float Lifetime { get; set; }

        public Particle(Texture2D texture,
            Vector2 position, Vector2 velocity, float speedMultiplier,
            float rotation, float rotationMultiplier,
            Color color,
            float scale, float scaleMultiplier,
            float lifetime)
        {
            Texture = texture;

            Position = position;
            Velocity = velocity;
            SpeedMultiplier = speedMultiplier;

            Rotation = rotation;
            RotationMultiplier = rotationMultiplier;

            Color = color;

            Scale = scale;
            ScaleMultiplier = scaleMultiplier;

            Lifetime = lifetime;
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += delta * Velocity;
            Age += delta;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            spriteBatch.Draw(Texture, Position, null, Color, Rotation, origin, Scale, SpriteEffects.None, 0);
        }
    }
}
