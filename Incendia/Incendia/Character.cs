using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Incendia
{
    /// <summary>
    /// A person, such as the player or victims
    /// </summary>
    class Character : Sprite
    {
        public Character(Vector2 position, Animation defaultAnimation, double lifeTime)
            : base(position, defaultAnimation, lifeTime)
        {
        }

        public override void Update(GameTime gameTime, PlayState map) 
        {
            CollideWithWalls(gameTime, map);
            base.Update(gameTime, map);
            KeepInBounds(map);
            CollideWithWalls(gameTime, map);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            base.Draw(batch);
        }

        private void KeepInBounds(PlayState map)
        {
            //Place the character within the bounds of this world
            if (Position.X < 0)
                SetPositionX(0);
            else if (Position.X + Visual.Width > map.WorldLimits.X)
                SetPositionX(map.WorldLimits.X - Visual.Width);
            if (Position.Y < 0)
                SetPositionY(0);
            else if (Position.Y + Visual.Height > map.WorldLimits.Y)
                SetPositionY(map.WorldLimits.Y - Visual.Height);
        }

        //Assuming you are not already colliding with a tile
        private void CollideWithWalls(GameTime gameTime, PlayState map)
        {
            if (Velocity.X > 0)
            {
                //Find the set of tiles that our movement will intersect with
                Rectanglef movementBox = new Rectanglef(Position.X + Visual.Width, Position.Y, Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds, Visual.Height);
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
                            SetVelocityX(0);
                            SetPositionX((float)x - Visual.Width - 0.001f);
                            maxX = -999999;
                            break;
                        }
                    }
                }
            }

            else if (Velocity.X < 0)
            {
                //Find the set of tiles that our movement will intersect with
                Rectanglef movementBox = new Rectanglef(Position.X + (Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds), Position.Y, -Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds, Visual.Height);
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
                            SetVelocityX(0);
                            SetPositionX(x + 1.001f);
                            minX = 999999;
                            break;
                        }
                    }
                }
            }

            if (Velocity.Y > 0)
            {
                //Find the set of tiles that our movement will intersect with
                Rectanglef movementBox = new Rectanglef(Position.X, Position.Y + Visual.Height, Visual.Width,  Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
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
                            SetVelocityY(0);
                            SetPositionY((float)y - Visual.Height - 0.001f);
                            maxY = -999999;
                            break;
                        }
                    }
                }
            }

            else if (Velocity.Y < 0)
            {
                //Find the set of tiles that our movement will intersect with
                Rectanglef movementBox = new Rectanglef(Position.X, Position.Y + (Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds), Visual.Width, -Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
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
                            SetVelocityY(0);
                            SetPositionY(y + 1.001f);
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
