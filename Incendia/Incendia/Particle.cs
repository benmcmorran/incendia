﻿using System;
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

        public Vector2 TopRightCorner { 
            get { return new Vector2(Position.X - Texture.Width / 2 * Scale, Position.Y - Texture.Height / 2 * Scale); }
            set { Position = new Vector2(value.X + Texture.Width / 2 * Scale, value.Y + Texture.Height * Scale); }
        }

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
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Velocity.X > 0)
            {
                //Find the set of tiles that our movement will intersect with
                Rectanglef movementBox = new Rectanglef((TopRightCorner.X + Texture.Width * Scale), TopRightCorner.Y, Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds, Texture.Height * Scale);
                movementBox.Multiply(1 / Global.PixelsPerTile);
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
                            //s += new Vector2(-Velocity.X, -Velocity.Y / 2);
                            //TopRightCorner += new Vector2(-Texture.Width * Scale - 0.001f, 0);
                            maxX = -999999;
                            Age += (Lifetime - Age) / 1.5f;
                            Velocity = new Vector2(-Velocity.X, Global.rand.Next(-500,500));

                            break;
                        }
                    }
                }
            }

            else if (Velocity.X < 0)
            {
                //Find the set of tiles that our movement will intersect with
                Rectanglef movementBox = new Rectanglef((TopRightCorner.X + (Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds)), TopRightCorner.Y, -Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds, Texture.Height * Scale);
                movementBox.Multiply(1 / Global.PixelsPerTile);
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
                            //Velocity += new Vector2(-Velocity.X, -Velocity.Y / 2);
                            //TopRightCorner += new Vector2(1.001f, 0);
                            minX = 999999;

                            Age += (Lifetime - Age) / 1.5f;
                            Velocity = new Vector2(-Velocity.X, Global.rand.Next(-500, 500));


                            break;
                        }
                    }
                }
            }

            if (Velocity.Y > 0)
            {
                //Find the set of tiles that our movement will intersect with
                Rectanglef movementBox = new Rectanglef(TopRightCorner.X, TopRightCorner.Y + Texture.Height * Scale, Texture.Width * Scale, Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
                movementBox.Multiply(1 / Global.PixelsPerTile);
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
                            //Velocity += new Vector2(-Velocity.X / 2, -Velocity.Y);
                            //TopRightCorner += new Vector2(0, -Texture.Height * Scale - 0.001f);
                            maxY = -999999;

                            Age += (Lifetime - Age) / 1.5f;
                            Velocity = new Vector2(Global.rand.Next(-500, 500), -Velocity.Y);

                            break;
                        }
                    }
                }
            }

            else if (Velocity.Y < 0)
            {
                //Find the set of tiles that our movement will intersect with
                Rectanglef movementBox = new Rectanglef(TopRightCorner.X, TopRightCorner.Y + (Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds), Texture.Width * Scale, -Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
                movementBox.Multiply(1 / Global.PixelsPerTile);
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
                            //Velocity += new Vector2(-Velocity.X / 2,-Velocity.Y);
                            //TopRightCorner += new Vector2(0, 1.001f);
                            //get us out of these loops
                            minY = 999999;
                            Velocity = new Vector2(Global.rand.Next(-500, 500), -Velocity.Y);
                            Age += (Lifetime - Age) / 1.5f;

                            break;
                        }
                    }
                }
            }
        }
    }
}
