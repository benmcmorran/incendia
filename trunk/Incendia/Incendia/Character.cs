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
    public class Character : Sprite
    {
        public float Hp { get; set; }
        public bool Rescued = false;
        public bool Escaped = false;

        public Character(Vector2 position, Animation defaultAnimation, double lifeTime, int hp)
            : base(position, defaultAnimation, lifeTime)
        {
            Hp = hp;
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

        /// <summary>
        /// For victims ONLY!
        /// </summary>
        /// <param name="map"></param>
        public void Behave(PlayState map)
        {
            Rectanglef r = new Rectanglef(map._player.Position.X, map._player.Position.Y, map._player.Visual.Width, map._player.Visual.Height);
            if (Position.X + Visual.Width * Scale >= r.X && Position.X <= r.X + r.Width && Position.Y + Visual.Height * Scale >= r.Y && Position.Y <= r.Y + r.Height)
                Rescued = false;
            _velocity = Vector2.Zero;
            for (int x = (int)map.WorldLimits.X - 1; x >= 0; x--)
            {
                for (int y = (int)map.WorldLimits.Y - 1; y >= 0; y--)
                {
                    float angle = (float)Math.Atan2(y - (double)Position.Y, x - (double)Position.X);
                    if(map.Grid[x,y].State == FireState.Burning)
                        _velocity -= new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) / (float)Math.Sqrt((x - Position.X) * (x - Position.X) + (y - Position.Y) * (y - Position.Y));
                }
            }
            if (Math.Sqrt((map._player.Position.X - Position.X) * (map._player.Position.X - Position.X) + (map._player.Position.Y - Position.Y) * (map._player.Position.Y - Position.Y)) <= 50)
            {
                float anglep = (float)Math.Atan2(map._player.Position.Y - (double)Position.Y, map._player.Position.X - (double)Position.X);
                _velocity += new Vector2((float)Math.Cos(anglep), (float)Math.Sin(anglep)) * 20 / (float)Math.Sqrt((map._player.Position.X - Position.X) * (map._player.Position.X - Position.X) + (map._player.Position.Y - Position.Y) * (map._player.Position.Y - Position.Y));
            }

            _velocity.Normalize();
            _velocity *= 2;

        }

    }
}
