using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Incendia
{
    class PlayState
    {
        Character _player;
        public Vector2 WorldLimits { get; set; }
        uint[,] grid;
        Camera2D camera;
        Viewport viewport;

        public PlayState(uint horizontalTiles, uint verticalTiles, Viewport viewport)
        {
            _player = Generator.PlayerSprite(new Vector2(5, 5));
            WorldLimits = new Vector2((float)horizontalTiles, (float)verticalTiles);
            grid = new uint[horizontalTiles,verticalTiles];

            //Here is where we define our grid for testing purposes only
            grid[0, 1] = 1;
            grid[0, 0] = 1;
            grid[1, 0] = 1;
            grid[19, 14] = 1;
            grid[18, 13] = 1;
            grid[10, 10] = 1;
            grid[10, 11] = 1;
            grid[10, 12] = 1;
            grid[10, 13] = 1;
            grid[13, 10] = 1;
            grid[12, 11] = 1;
            grid[13, 12] = 1;
            grid[13, 13] = 1;




            camera = new Camera2D();
            this.viewport = viewport;
        }

        public void Update(GameTime gameTime)
        {
            TakeInput();
            _player.Update(gameTime, this);
            //Locked camera
            //camera.Location = _player.Position * Global.PixelsPerTile;

            //Smooth camera
            camera.Location += ((_player.PositionCenter * Global.PixelsPerTile) - camera.Location) * 0.1f;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.ViewTransformationMatrix(viewport));
            
            _player.Draw(batch);

            //Draw what is on the grid
            for (int x = 0; x < WorldLimits.X; x++ )
            {
                for (int y = 0; y < WorldLimits.Y; y++)
                {
                    if(grid[x,y] == 1)
                        batch.Draw(Global.Textures["Wall"], new Vector2(x,y) * Global.PixelsPerTile, null, Color.White, 0, Vector2.Zero, Global.PixelsPerTile / Global.Textures["Wall"].Width, SpriteEffects.None, 0);
                }
            }

            batch.End();
        }

        void TakeInput()
        {
            if (Input.DistanceToMouse(_player.PositionCenter * Global.PixelsPerTile) > Global.PixelsPerTile * _player.Visual.Height / 2)
                _player.Rotation = Input.AngleToMouse(Vector2.Transform(_player.PositionCenter * Global.PixelsPerTile, camera.ViewTransformationMatrix(viewport)));
            if (Input.KeyHeld(Keys.W))
                _player.SetVelocityY(-5);
            else if (Input.KeyHeld(Keys.S))
                _player.SetVelocityY(5);
            else
                _player.SetVelocityY(0);
            if (Input.KeyHeld(Keys.D))
                _player.SetVelocityX(5);
            else if (Input.KeyHeld(Keys.A))
                _player.SetVelocityX(-5);
            else
                _player.SetVelocityX(0);
        }

        public bool TileIsSolid(int x, int y)
        {
            x = (int)MathHelper.Clamp(x, 0, WorldLimits.X - 1);
            y = (int)MathHelper.Clamp(y, 0, WorldLimits.Y -1);
            return grid[x, y] != 0;
        }
    }
}
