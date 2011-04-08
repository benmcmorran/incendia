﻿using System;
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
            _player = Generator.PlayerSprite(new Vector2(0, 0));
            WorldLimits = new Vector2((float)horizontalTiles, (float)verticalTiles);
            grid = new uint[horizontalTiles,verticalTiles];

            //Here is where we define our grid for testing purposes only
            grid[0, 1] = 1;
            grid[0, 0] = 1;
            grid[1, 0] = 1;
            grid[19, 14] = 1;
            grid[18, 13] = 1;

            camera = new Camera2D();
            this.viewport = viewport;
        }

        public void Update(GameTime gameTime)
        {
            TakeInput();
            _player.Update(gameTime, this);
            camera.Location = _player.Position * Global.PixelsPerTile;
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
                _player.SetVelocityY(-3);
            else if (Input.KeyHeld(Keys.S))
                _player.SetVelocityY(3);
            else
                _player.SetVelocityY(0);
            if (Input.KeyHeld(Keys.D))
                _player.SetVelocityX(3);
            else if (Input.KeyHeld(Keys.A))
                _player.SetVelocityX(-3);
            else
                _player.SetVelocityX(0);
        }
    }
}