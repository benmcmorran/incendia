using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;


namespace Incendia
{
    public enum Nozzle {wide, medium, narrow }
    public class PlayState : IGraph<int>
    {
        public Character _player;
        public Vector2 WorldLimits { get; set; }
        public Tile[,] Grid { get; private set; }
        Camera2D camera;
        Viewport viewport;
        public List<Character> Victims = new List<Character>();
        Nozzle selectedNozzle;
        ParticleSystem fireHose;
        ParticleSystem fire;
        bool shootingWater;

        public PlayState(string name, Viewport viewport)
        {
            StreamReader reader = new StreamReader(System.Environment.CurrentDirectory + "\\Levels\\" + name + ".txt");
            string[] lines = reader.ReadToEnd().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            reader.Close();

            string[] playerPosition = lines[0].Split(new char[] { ',' });
            _player = Generator.PlayerSprite(new Vector2(Convert.ToInt32(playerPosition[0].Trim()), Convert.ToInt32(playerPosition[1].Trim())));
        
            string[] size = lines[1].Split(new char[] { ',' });
            WorldLimits = new Vector2(Convert.ToInt32(size[0].Trim()), Convert.ToInt32(size[1].Trim()));
            Grid = new Tile[Convert.ToInt32(size[0].Trim()), Convert.ToInt32(size[1].Trim())];
            selectedNozzle = Nozzle.narrow;

            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    switch (lines[y + 2][x])
                    {
                        case '1':
                            Grid[x, y] = Generator.Carpet1();
                            break;
                        case '2':
                            Grid[x, y] = Generator.Carpet2();
                            break;
                        case '3':
                            Grid[x, y] = Generator.TiledFloor1();
                            break;
                        case '4':
                            Grid[x, y] = Generator.TiledFloor2();
                            break;
                        case '5':
                            Grid[x, y] = Generator.WoodenFloor();
                            break;
                        case 'G':
                            Grid[x, y] = Generator.GraniteWall();
                            break;
                        case 'W':
                            Grid[x, y] = Generator.WoodenWall();
                            break;
                    }
                }
            }

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
            fireHose = new ParticleSystem(Global.Textures["Water"], _player.PositionCenter, 0, 0, .5f, .7f, 500, Utils.ConstantCurve(500), .10f, Utils.ConstantCurve(0), 0, c, .05f, Utils.ConstantCurve(1), Utils.ConstantCurve(1), Utils.ConstantCurve(1), a);

            a = new Curve();
            a.Keys.Add(new CurveKey(0, 0));
            a.Keys.Add(new CurveKey(.3f, .5f));
            a.Keys.Add(new CurveKey(.7f, .5f));
            a.Keys.Add(new CurveKey(1, 0));
            
            fire = new ParticleSystem(Global.Textures["Fire"], new List<Vector2>(), 0, (float)Math.PI * 2, .5f, 1f, 10, Utils.ConstantCurve(100), .10f, Utils.ConstantCurve(0), 0, Utils.ConstantCurve(3), .05f, Utils.ConstantCurve(1), Utils.ConstantCurve(1), Utils.ConstantCurve(1), a);

            camera = new Camera2D();
            this.viewport = viewport;
        }
        
        public PlayState(int horizontalTiles, int verticalTiles, Viewport viewport)
        {
            _player = Generator.PlayerSprite(new Vector2(5, 5));
            WorldLimits = new Vector2((float)horizontalTiles, (float)verticalTiles);
            Grid = new Tile[horizontalTiles,verticalTiles];
            selectedNozzle = Nozzle.narrow;

            for (int x = horizontalTiles - 1; x >= 0; x--)
            {
                for (int y = verticalTiles - 1; y >= 0; y--)
                {
                    Grid[x,y] = Generator.WoodenFloor();
                }
            }

            //Here is where we define our grid for testing purposes only
            Grid[0, 1] = Generator.WoodenWall();
            Grid[0, 0] = Generator.WoodenWall();
            Grid[1, 0] = Generator.WoodenWall();
            Grid[19, 14] = Generator.WoodenWall();
            Grid[18, 13] = Generator.WoodenWall();
            Grid[10, 10] = Generator.WoodenWall();
            Grid[10, 11] = Generator.WoodenWall();
            Grid[10, 12] = Generator.WoodenWall();
            Grid[10, 13] = Generator.WoodenWall();
            Grid[13, 10] = Generator.WoodenWall();
            Grid[12, 11] = Generator.WoodenWall();
            Grid[13, 12] = Generator.WoodenWall();
            Grid[13, 13] = Generator.WoodenWall();

            Grid[4, 5].State = FireState.Burning;

            Victims.Add(Generator.VictimSprite(new Vector2(4, 14)));

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
            fireHose = new ParticleSystem(Global.Textures["Water"], _player.PositionCenter, 0, 0, .5f, .7f, 500, Utils.ConstantCurve(500), .10f, Utils.ConstantCurve(0), 0, c, .05f, Utils.ConstantCurve(1), Utils.ConstantCurve(1), Utils.ConstantCurve(1), a, false);

            a = new Curve();
            a.Keys.Add(new CurveKey(0, 0));
            a.Keys.Add(new CurveKey(.3f, .5f));
            a.Keys.Add(new CurveKey(.7f, .5f));
            a.Keys.Add(new CurveKey(1, 0));
            
            fire = new ParticleSystem(Global.Textures["Fire"], new List<Vector2>(), 0, (float)Math.PI * 2, .5f, 1f, 8, Utils.ConstantCurve(100), .10f, Utils.ConstantCurve(0), 0, Utils.ConstantCurve(3), .05f, Utils.ConstantCurve(1), Utils.ConstantCurve(1), Utils.ConstantCurve(1), a, true);



            camera = new Camera2D();
            this.viewport = viewport;
        }

        public void Update(GameTime gameTime)
        {
            TakeInput();
            FireSimulation.Step(Grid);
            _player.Update(gameTime, this);
            //Locked camera
            //camera.Location = _player.Position * Global.PixelsPerTile;

            //Smooth camera
            camera.Location += ((_player.PositionCenter * Global.PixelsPerTile) - camera.Location) * 0.1f;
            
            UpdateVictims(gameTime);
            fireHose.EmitterLocations.Clear(); 
            fireHose.EmitterLocations.Add((_player.PositionCenter + new Vector2((float)Math.Cos(_player.Rotation), (float)Math.Sin(_player.Rotation)) / 2) * Global.PixelsPerTile);
            fireHose.MinDirection = _player.Rotation - NozzleWidthInRadians();
            fireHose.MaxDirection = _player.Rotation + NozzleWidthInRadians();

            fire.EmitterLocations.Clear();
            for (int x = 0; x < WorldLimits.X; x++)
            {
                for (int y = 0; y < WorldLimits.Y; y++)
                {

                    if (Grid[x,y].State == FireState.Burning && camera.IsInView(new Rectangle((int)(x * Global.PixelsPerTile), (int)(y * Global.PixelsPerTile), (int)Global.PixelsPerTile, (int)Global.PixelsPerTile), viewport))
                    {
                        fire.EmitterLocations.Add(new Vector2((float)x + .5f, (float) y + .5f) * Global.PixelsPerTile);
                    }
                }
            }

            fireHose.Update(gameTime, this, shootingWater && !TileIsSolid((int)Math.Floor(fireHose.EmitterLocations[0].X / Global.PixelsPerTile), (int)Math.Floor(fireHose.EmitterLocations[0].Y / Global.PixelsPerTile)));
            fire.Update(gameTime, this, true);
           
            foreach (Particle p in fireHose.ParticleReturner)
            {
                p.KeepinBounds(this);
                if (p.Age < p.Lifetime && grid[(int)Math.Floor(p.Position.X / Global.PixelsPerTile), (int)Math.Floor(p.Position.Y / Global.PixelsPerTile)].HitByWater())
                    p.Age = (p.Lifetime - p.Age) / 2;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.ViewTransformationMatrix(viewport));

            //Draw what is on the grid
            for (int x = 0; x < WorldLimits.X; x++)
            {
                for (int y = 0; y < WorldLimits.Y; y++)
                {
                    
                    if (camera.IsInView(new Rectangle((int)(x * Global.PixelsPerTile), (int)(y * Global.PixelsPerTile), (int)Global.PixelsPerTile, (int)Global.PixelsPerTile), viewport))
                    {
                            batch.Draw(Global.Textures[Grid[x, y].Texture], new Vector2(x, y) * Global.PixelsPerTile, null, Color.White, 0, Vector2.Zero, Global.PixelsPerTile / Global.Textures[Grid[x, y].Texture].Width, SpriteEffects.None, 0);
                    }
                }
            }


            fireHose.Draw(batch);
            fire.Draw(batch);
            _player.Draw(batch);


            foreach (Character c in Victims)
            {
                c.Draw(batch);
            }

            

            //batch.Draw(Global.Textures["Wall"], _player.PositionCenter * Global.PixelsPerTile, null, Color.White, 0, Vector2.Zero, new Vector2(1,1), SpriteEffects.None, 0);
            _player.Draw(batch);


            batch.End();

            batch.Begin();
            batch.DrawString(Global.Font, _player.Hp.ToString(), new Vector2(50, 50), Color.Bisque);
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

            _player.Velocity.Normalize();
            _player.SetVelocityX(_player.Velocity.X * 1.5f);
            _player.SetVelocityY(_player.Velocity.Y * 1.5f);


            shootingWater = Input.MouseLeftClicked();
            
            if(Input.KeyJustPressed(Keys.Space))
                ItterateNozzle();
        }

        void UpdateVictims(GameTime gameTime)
        {
            for (int i = Victims.Count - 1; i >= 0; i--)
            {
                if (Victims[i].Hp <= 0)
                {
                    Victims.RemoveAt(i);
                    continue;
                }
                else if (Victims[i].Escaped)
                {
                    Victims.RemoveAt(i);
                    continue;
                }
                else if (Victims[i].Rescued)
                {
                    Victims.RemoveAt(i);
                    continue;
                }

                Victims[i].Update(gameTime, this);
                Victims[i].Behave(this);
            }
        }

        public bool TileIsSolid(int x, int y)
        {
            x = (int)MathHelper.Clamp(x, 0, WorldLimits.X - 1);
            y = (int)MathHelper.Clamp(y, 0, WorldLimits.Y -1);
            return Grid[x,y].Solid;
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
            if (y < Grid.GetLength(1) - 1 && TileIsSolid(x, y + 1))
                neighbors.Add(IntFromCell(x, y + 1), 1);
            if (x > 0 && TileIsSolid(x - 1, y))
                neighbors.Add(IntFromCell(x - 1, y), 1);
            if (x < Grid.GetLength(0) - 1 && TileIsSolid(x + 1, y))
                neighbors.Add(IntFromCell(x + 1, y), 1);

            return neighbors;
        }

        public void CellFromInt(int node, out int x, out int y)
        {
            x = node % Grid.GetLength(0);
            y = node / Grid.GetLength(0);
        }

        public int IntFromCell(int x, int y)
        {
            return y * Grid.GetLength(0) + x;
        }
    }
}
