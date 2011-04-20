using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Incendia
{
    public enum Nozzle {wide, medium, narrow }
    class PlayState : IGraph<int>
    {
        Character _player;
        public Vector2 WorldLimits { get; set; }
        uint[,] grid;
        Camera2D camera;
        Viewport viewport;
        List<Character> victims = new List<Character>();
        Nozzle selectedNozzle;
        ParticleSystem fireHose;
        bool shootingWater;

        public PlayState(uint horizontalTiles, uint verticalTiles, Viewport viewport)
        {
            _player = Generator.PlayerSprite(new Vector2(5, 5));
            WorldLimits = new Vector2((float)horizontalTiles, (float)verticalTiles);
            grid = new uint[horizontalTiles,verticalTiles];

            selectedNozzle = Nozzle.narrow;

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

            Curve c = new Curve();
            c.Keys.Add(new CurveKey(0, 0));
            c.Keys.Add(new CurveKey(.3f, 1));
            c.Keys.Add(new CurveKey(.7f, 1));
            c.Keys.Add(new CurveKey(1, 0));

            Curve a = new Curve();
            a.Keys.Add(new CurveKey(0, 0));
            a.Keys.Add(new CurveKey(.3f, 1));
            a.Keys.Add(new CurveKey(.7f, 1));
            a.Keys.Add(new CurveKey(1, 0));
            fireHose = new ParticleSystem(Global.Textures["Particle"], _player.PositionCenter, 0, 0, .5f, .7f, 500, Utils.ConstantCurve(500), .10f, Utils.ConstantCurve(0), 0, c, .05f, Utils.ConstantCurve(1), Utils.ConstantCurve(1), Utils.ConstantCurve(1), a); 


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
            
            UpdateVictims(gameTime);
            fireHose.EmitterLocation = (_player.PositionCenter + new Vector2((float)Math.Cos(_player.Rotation), (float)Math.Sin(_player.Rotation)) / 2) * Global.PixelsPerTile;
            fireHose.MinDirection = _player.Rotation - NozzleWidthInRadians();
            fireHose.MaxDirection = _player.Rotation + NozzleWidthInRadians();

            fireHose.Update(gameTime, this, shootingWater && !TileIsSolid((int)Math.Floor(fireHose.EmitterLocation.X), (int)Math.Floor(fireHose.EmitterLocation.Y)));
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.ViewTransformationMatrix(viewport));
            
            _player.Draw(batch);

            foreach (Character c in victims)
            {
                c.Draw(batch);
            }

            //Draw what is on the grid
            for (int x = 0; x < WorldLimits.X; x++ )
            {
                for (int y = 0; y < WorldLimits.Y; y++)
                {
                    if(grid[x,y] == 1)
                        batch.Draw(Global.Textures["Wall"], new Vector2(x,y) * Global.PixelsPerTile, null, Color.White, 0, Vector2.Zero, Global.PixelsPerTile / Global.Textures["Wall"].Width, SpriteEffects.None, 0);
                }
            }

            //batch.Draw(Global.Textures["Wall"], _player.PositionCenter * Global.PixelsPerTile, null, Color.White, 0, Vector2.Zero, new Vector2(1,1), SpriteEffects.None, 0);
            fireHose.Draw(batch);
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

            shootingWater = Input.MouseLeftClicked();
            
            if(Input.KeyJustPressed(Keys.Space))
                ItterateNozzle();
        }

        void UpdateVictims(GameTime gameTime)
        {
            for (int i = victims.Count - 1; i >= 0; i--)
            {
                if (victims[i].Hp <= 0)
                {
                    victims.RemoveAt(i);
                    continue;
                }

                victims[i].Update(gameTime, this);
                victims[i].Behave(this);
            }
        }

        public bool TileIsSolid(int x, int y)
        {
            x = (int)MathHelper.Clamp(x, 0, WorldLimits.X - 1);
            y = (int)MathHelper.Clamp(y, 0, WorldLimits.Y -1);
            return grid[x, y] != 0;
        }

        float NozzleWidthInRadians()
        {
            if (selectedNozzle == Nozzle.narrow)
                return (float)Math.PI / 32;
            else if (selectedNozzle == Nozzle.medium)
                return (float)Math.PI / 12;
            else
                return (float)Math.PI / 6;
        }

        void ItterateNozzle()
        {
            if (selectedNozzle == Nozzle.narrow)
                selectedNozzle = Nozzle.medium;
            else if (selectedNozzle == Nozzle.medium)
                selectedNozzle = Nozzle.wide;
            else
                selectedNozzle = Nozzle.narrow;
        }
        

        public float LeastCostEstimate(int start, int end)
        {
            // Manhattan estimate
            int startX, startY, endX, endY;
            CellFromInt(start, out startX, out startY);
            CellFromInt(end, out endX, out endY);
            return Math.Abs(endX - startX) + Math.Abs(endY - startY);
        }

        public Dictionary<int, float> AdjacentCost(int node)
        {
            int x, y;
            CellFromInt(node, out x, out y);

            Dictionary<int, float> neighbors = new Dictionary<int, float>(4);

            // N, S, E, and W cells have a cost of one
            if (y > 0 && TileIsSolid(x, y - 1))
                neighbors.Add(IntFromCell(x, y - 1), 1);
            if (y < grid.GetLength(1) - 1 && TileIsSolid(x, y + 1))
                neighbors.Add(IntFromCell(x, y + 1), 1);
            if (x > 0 && TileIsSolid(x - 1, y))
                neighbors.Add(IntFromCell(x - 1, y), 1);
            if (x < grid.GetLength(0) - 1 && TileIsSolid(x + 1, y))
                neighbors.Add(IntFromCell(x + 1, y), 1);

            return neighbors;
        }

        public void CellFromInt(int node, out int x, out int y)
        {
            x = node % grid.GetLength(0);
            y = node / grid.GetLength(0);
        }

        public int IntFromCell(int x, int y)
        {
            return y * grid.GetLength(0) + x;
        }
    }
}
