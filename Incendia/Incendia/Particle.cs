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

        public void Update(GameTime gameTime, PlayState map)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += delta * Velocity;
            Age += delta;
            CollideWithWalls(gameTime, map);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            spriteBatch.Draw(Texture, Position, null, Color, Rotation, origin, Scale, SpriteEffects.None, 0);
        }

        //Assuming you are not already colliding with a tile
        private void CollideWithWalls(GameTime gameTime, PlayState map)
        {
            if (Velocity.X > 0)
            {
                //Find the set of tiles that our movement will intersect with
                Rectanglef movementBox = new Rectanglef(Position.X + Texture.Width, Position.Y, Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds, Texture.Height);
                int minX = (int)Math.Floor(movementBox.X);
                int maxX = (int)Math.Floor(movementBox.X + movementBox.Width);
                int minY = (int)Math.Floor(movementBox.Y);
                int maxY = (int)Math.Floor(movementBox.Y + movementBox.Height);

                //Itterate through every tile that we will cross through and see if one of them is solid
                for (int x = minX; x <= maxX; x++)
                {
                    for (int y = minY; y <= maxY; y++)
                    {
                        if (map.TileIsSolid(x, y))
                        {
                            Velocity += new Vector2(-Velocity.X,0);
                            Position += new Vector2(-Texture.Width - 0.001f, 0);
                            maxX = -999999;
                            break;
                        }
                    }
                }
            }

            else if (Velocity.X < 0)
            {
                //Find the set of tiles that our movement will intersect with
                Rectanglef movementBox = new Rectanglef(Position.X + (Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds), Position.Y, -Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds, Texture.Height);
                int maxX = (int)Math.Floor(movementBox.X);
                int minX = (int)Math.Floor(movementBox.X + movementBox.Width);
                int minY = (int)Math.Floor(movementBox.Y);
                int maxY = (int)Math.Floor(movementBox.Y + movementBox.Height);

                //Itterate through every tile that we will cross through and see if one of them is solid
                for (int x = maxX; x >= minX; x--)
                {
                    for (int y = minY; y <= maxY; y++)
                    {
                        if (map.TileIsSolid(x, y))
                        {
                            Velocity += new Vector2(-Velocity.X,0);
                            Position += new Vector2(1.001f, 0);
                            minX = 999999;
                            break;
                        }
                    }
                }
            }

            if (Velocity.Y > 0)
            {
                //Find the set of tiles that our movement will intersect with
                Rectanglef movementBox = new Rectanglef(Position.X, Position.Y + Texture.Height, Texture.Width, Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
                int minX = (int)Math.Floor(movementBox.X);
                int maxX = (int)Math.Floor(movementBox.X + movementBox.Width);
                int minY = (int)Math.Floor(movementBox.Y);
                int maxY = (int)Math.Floor(movementBox.Y + movementBox.Height);

                //Itterate through every tile that we will cross through and see if one of them is solid
                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        if (map.TileIsSolid(x, y))
                        {
                            Velocity += new Vector2(0,-Velocity.Y);
                            Position += new Vector2(0,-Texture.Height - 0.001f);
                            maxY = -999999;
                            break;
                        }
                    }
                }
            }

            else if (Velocity.Y < 0)
            {
                //Find the set of tiles that our movement will intersect with
                Rectanglef movementBox = new Rectanglef(Position.X, Position.Y + (Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds), Texture.Width, -Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
                int minX = (int)Math.Floor(movementBox.X);
                int maxX = (int)Math.Floor(movementBox.X + movementBox.Width);
                int minY = (int)Math.Floor(movementBox.Y);
                int maxY = (int)Math.Floor(movementBox.Y + movementBox.Height);

                //Itterate through every tile that we will cross through and see if one of them is solid
                for (int y = maxY; y >= minY; y--)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        if (map.TileIsSolid(x, y))
                        {
                            Velocity += new Vector2(0,-Velocity.Y);
                            Position += new Vector2(0,1.001f);
                            //get us out of these loops
                            minY = 999999;
                            break;
                        }
                    }
                }
            }
        }
    }
}
