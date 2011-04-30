using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace Incendia
{
    public enum Nozzle {wide, medium, narrow }
    public class PlayState : IGraph<int>, IGameState
    {
        public Character _player;
        bool cheating = false;
        public Vector2 WorldLimits { get; set; }
        public Tile[,] Grid { get; private set; }
        Camera2D camera;
        Viewport viewport;
        public List<Character> Victims = new List<Character>();
        Nozzle selectedNozzle;
        ParticleSystem fireHose;
        ParticleSystem fire;
        ParticleSystem explosions;
        bool shootingWater;
        StateManager manager;
        public double hpBarFlashing = 0;
        List<float> explosionTime = new List<float>();
        bool _showMiniMap;
        SoundEffectInstance hoseSound = Global.Sounds["Hose"].CreateInstance();
        SoundEffectInstance fireSound = Global.Sounds["Fire"].CreateInstance();

        public PlayState(StateManager manager, string name, Viewport viewport)
        {
            this.manager = manager;

            StreamReader reader = new StreamReader(System.Environment.CurrentDirectory + "\\Levels\\" + name + ".txt");
            string[] lines = reader.ReadToEnd().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            reader.Close();

            string[] playerPosition = lines[0].Split(new char[] { ',' });
            _player = Generator.PlayerSprite(new Vector2(Convert.ToInt32(playerPosition[0].Trim()), Convert.ToInt32(playerPosition[1].Trim())));

            string[] size = lines[1].Split(new char[] { ',' });
            WorldLimits = new Vector2(Convert.ToInt32(size[0].Trim()), Convert.ToInt32(size[1].Trim()));
            Grid = new Tile[Convert.ToInt32(size[0].Trim()), Convert.ToInt32(size[1].Trim())];
            selectedNozzle = Nozzle.narrow;

            string[] victimPositions = lines[2].Split(new char[] { ';' });
            foreach (string victimPosition in victimPositions)
            {
                string[] position = victimPosition.Split(new char[] { ',' });
                Victims.Add(Generator.VictimSprite(new Vector2(Convert.ToInt32(position[0].Trim()), Convert.ToInt32(position[1].Trim()))));
            }

            string[] firePositionList = lines[3].Split(new char[] { ',' });
            Vector2 firePosition = new Vector2(Convert.ToInt32(firePositionList[0].Trim()), Convert.ToInt32(firePositionList[1].Trim()));


            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    Layer1TileType layer1 = Layer1TileType.Outside;
                    Layer2TileType layer2 = Layer2TileType.Empty;
                    Layer3TileType layer3 = Layer3TileType.Empty;

                    switch (lines[y + 4][x])
                    {
                        case 'O': layer1 = Layer1TileType.Outside; break;
                        case '1': layer1 = Layer1TileType.Carpet1; break;
                        case '2': layer1 = Layer1TileType.Carpet2; break;
                        case '3': layer1 = Layer1TileType.Tile1; break;
                        case '4': layer1 = Layer1TileType.Tile2; break;
                        case '5': layer1 = Layer1TileType.WoodFloor; break;
                        case 'G': layer1 = Layer1TileType.GraniteWall; break;
                        case 'W': layer1 = Layer1TileType.WoodWall; break;
                    }

                    switch (lines[y + Grid.GetLength(1) + 5][x])
                    {
                        case '-': layer2 = Layer2TileType.Empty; break;
                        case 'E': layer2 = Layer2TileType.Desk; break;
                        case 'S': layer2 = Layer2TileType.Sofa; break;
                        case 'P': layer2 = Layer2TileType.Plant; break;
                        case 'C': layer2 = Layer2TileType.OpenVerticalDoor; break;
                        case 'D': layer2 = Layer2TileType.ClosedVerticalDoor; break;
                        case 'c': layer2 = Layer2TileType.OpenHorizontalDoor; break;
                        case 'd': layer2 = Layer2TileType.ClosedHorizontalDoor; break;
                        case 'T': layer2 = Layer2TileType.TrashCan; break;
                        case 'N': layer2 = Layer2TileType.Newspaper; break;
                        case 'F': layer2 = Layer2TileType.Flammables; break;
                    }

                    switch (lines[y + 2 * Grid.GetLength(1) + 6][x])
                    {
                        case 'C': layer3 = Layer3TileType.Computer; break;
                        case 'B': layer3 = Layer3TileType.Blotter; break;
                        case '-': layer3 = Layer3TileType.Empty; break;
                        case 'P': layer3 = Layer3TileType.Plant; break;
                        case 'L': layer3 = Layer3TileType.Laptop; break;
                    }

                    Grid[x, y] = new Tile(layer1, layer2, layer3);
                }
            }

            Grid[(int)firePosition.X, (int)firePosition.Y].State = FireState.Burning;

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
            fireHose = new ParticleSystem(Global.Textures["Water"], _player.PositionCenter, 0, 0, .5f, .7f, 500, Utils.ConstantCurve(500), .10f, Utils.ConstantCurve(0), 0, c, .05f, a, a, a, a, false, 1);

            a = new Curve();
            a.Keys.Add(new CurveKey(0, 0));
            a.Keys.Add(new CurveKey(.3f, .5f));
            a.Keys.Add(new CurveKey(.7f, .5f));
            a.Keys.Add(new CurveKey(1, 0));
            Curve preMultColor = new Curve();
            preMultColor.Keys.Add(new CurveKey(0, 1));
            preMultColor.Keys.Add(new CurveKey(.7f, 1));
            preMultColor.Keys.Add(new CurveKey(1, 0));
            fire = new ParticleSystem(Global.Textures["Fire"], new List<Vector2>(), 0, (float)Math.PI * 2, .5f, 1f, 10, Utils.ConstantCurve(100), .10f, Utils.ConstantCurve(0), 0, Utils.ConstantCurve(1), .05f, preMultColor, preMultColor, preMultColor, a, true, 1);
            explosions = new ParticleSystem(Global.Textures["Fire"], new List<Vector2>(), 0, (float)Math.PI * 2, 1f, 2f, 500, Utils.ConstantCurve(500), .10f, Utils.ConstantCurve(0), 0, Utils.ConstantCurve(1), .05f, preMultColor, preMultColor, preMultColor, a, true, 10);

            hoseSound.IsLooped = true;
            fireSound.IsLooped = true;
            fireSound.Play();
            PlayVictimCount();

            _showMiniMap = true;
            camera = new Camera2D();
            this.viewport = viewport;
        }

        public void Update(GameTime gameTime)
        {
            if (_player.Hp <= 0 && !manager.isTransitioning && !cheating)
            {
                manager.SetTransitionState(new MenuState(manager, viewport));
            }

            if (camera.Shake > 0)
                camera.Shake--;

            TakeInput();
            FireSimulation.Step(Grid);
            _player.Update(gameTime, this);
            //Locked camera
            //camera.Location = _player.Position * Global.PixelsPerTile;

            //Smooth camera
            camera.Location += ((_player.PositionCenter * Global.PixelsPerTile) - camera.Location) * 0.03f;

            if (hpBarFlashing > 0)
                hpBarFlashing -= gameTime.ElapsedGameTime.TotalSeconds;

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
                    if (Grid[x, y].ReadyToExplode)
                        EXPLODE(x,y, gameTime);
                    if (Grid[x,y].State == FireState.Burning && camera.IsInView(new Rectangle((int)(x * Global.PixelsPerTile), (int)(y * Global.PixelsPerTile), (int)Global.PixelsPerTile, (int)Global.PixelsPerTile), viewport))
                    {
                        fire.EmitterLocations.Add(new Vector2((float)x + .5f, (float) y + .5f) * Global.PixelsPerTile);
                    }
                }
            }

            if (shootingWater)
                hoseSound.Play();
            else
                hoseSound.Pause();

            fireHose.Update(gameTime, this, shootingWater && !TileIsSolid((int)Math.Floor(fireHose.EmitterLocations[0].X / Global.PixelsPerTile), (int)Math.Floor(fireHose.EmitterLocations[0].Y / Global.PixelsPerTile)));
            fire.Update(gameTime, this, true);
            UpdateExplosions(gameTime);
           
            foreach (Particle p in fireHose.ParticleReturner)
            {
                p.KeepinBounds(this);
                if (p.Age < p.Lifetime && Grid[(int)Math.Floor(p.Position.X / Global.PixelsPerTile), (int)Math.Floor(p.Position.Y / Global.PixelsPerTile)].HitByWater())
                    p.Age = (p.Lifetime - p.Age) / 2;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.ViewTransformationMatrix(viewport));

            //Draw what is on the Grid
            for (int x = 0; x < WorldLimits.X; x++)
            {
                for (int y = 0; y < WorldLimits.Y; y++)
                {
                    
                    if (camera.IsInView(new Rectangle((int)(x * Global.PixelsPerTile), (int)(y * Global.PixelsPerTile), (int)Global.PixelsPerTile, (int)Global.PixelsPerTile), viewport))
                    {
                        batch.Draw(Global.Textures[Grid[x, y].Texture1], new Vector2(x, y) * Global.PixelsPerTile, null, Color.White, 0, Vector2.Zero, Global.PixelsPerTile / Global.Textures[Grid[x, y].Texture1].Width, SpriteEffects.None, 0);
                        batch.Draw(Global.Textures[Grid[x, y].Texture2], new Vector2(x, y) * Global.PixelsPerTile, null, Color.White, 0, Vector2.Zero, Global.PixelsPerTile / Global.Textures[Grid[x, y].Texture2].Width, SpriteEffects.None, 0);
                        batch.Draw(Global.Textures[Grid[x, y].Texture3], new Vector2(x, y) * Global.PixelsPerTile, null, Color.White, 0, Vector2.Zero, Global.PixelsPerTile / Global.Textures[Grid[x, y].Texture3].Width, SpriteEffects.None, 0);
                        if (Grid[x, y].Smoke == SmokeState.WithSmoke)
                            batch.Draw(Global.Textures["Smoke"], new Vector2(x, y) * Global.PixelsPerTile, null, Color.White, 0, Vector2.Zero, Global.PixelsPerTile / Global.Textures["Smoke"].Width, SpriteEffects.None, 0);
                    }
                }
            }


            fireHose.Draw(batch);
            fire.Draw(batch);
            explosions.Draw(batch);
            _player.Draw(batch);


            foreach (Character c in Victims)
            {
                c.Draw(batch);
            }

            //batch.Draw(Global.Textures["Wall"], _player.PositionCenter * Global.PixelsPerTile, null, Color.White, 0, Vector2.Zero, new Vector2(1,1), SpriteEffects.None, 0);
            _player.Draw(batch);


            batch.End();

            batch.Begin();
            batch.Draw(Global.Textures["HpBarOutline"], new Vector2(50,50), null, Color.White, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0);
            for (int hp = 0; hp <= _player.Hp; hp++)
            {
                if (hpBarFlashing > 0)
                    batch.Draw(Global.Textures["HpBitFlash"], new Vector2(53 + hp, 53), null, Color.White, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0);
                else
                    batch.Draw(Global.Textures["HpBit"], new Vector2(53 + hp, 53), null, Color.White, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0);
            }
            if(_showMiniMap)
                DrawMiniMap(batch);
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
            if (Input.KeyJustPressed(Keys.M))
                _showMiniMap = !_showMiniMap;
            if (Input.KeyJustPressed(Keys.I))
                cheating = !cheating;

            if (Input.KeyJustPressed(Keys.Escape))
                manager.SetTransitionState(new MenuState(manager, viewport));
        }

        void UpdateVictims(GameTime gameTime)
        {
            bool victimsRescued = false;
            for (int i = Victims.Count - 1; i >= 0; i--)
            {
                if (Victims[i].Hp <= 0)
                {
                    Victims.RemoveAt(i);
                    if (!manager.isTransitioning)
                        manager.SetTransitionState(new MessageState(manager, new MenuState(manager, viewport), "Empty", "You Lost!"));
                    continue;
                }

                for (int x = (int)Math.Floor(Victims[i].Position.X); x <= Math.Floor(Victims[i].Position.X + Victims[i].Visual.Width); x++)
                {
                    for (int y = (int)Math.Floor(Victims[i].Position.Y); y <= Math.Floor(Victims[i].Position.Y + Victims[i].Visual.Height); y++)
                    {
                        if (Grid[x, y].Outside)
                        {
                            Victims[i].Escaped = true;
                            Victims[i].Rescued = true;
                            x = 9999;
                            break;
                        }
                    }
                }

                if (Victims[i].Rescued)
                {
                    Victims.RemoveAt(i);
                    victimsRescued = true;
                    continue;
                }

                Victims[i].Update(gameTime, this);
                Victims[i].Behave(this);
            }

            if (victimsRescued)
                PlayVictimCount();
            if (!manager.isTransitioning && Victims.Count == 0)
                manager.SetTransitionState(new MessageState(manager, new MenuState(manager, viewport), "Empty", "You Won...\nwith a score of " + CalculateScore() + "."));

        }

        private int CalculateScore()
        {
            int cellCount = Grid.GetLength(0) * Grid.GetLength(1);
            int unburnedCells = 0;

            for (int x = 0; x < Grid.GetLength(0); x++)
                for (int y = 0; y < Grid.GetLength(1); y++)
                    if (Grid[x, y].State == FireState.Unburned)
                        unburnedCells++;

            return (int)((float)unburnedCells / cellCount * 10000);
        }

        private void PlayVictimCount()
        {
            Global.Sounds[Victims.Count + " Left"].Play();
        }

        void UpdateExplosions(GameTime time)
        {
            for (int i = 0; i <= explosionTime.Count - 1; i++)
            {
                if (explosionTime[i] + 1 < time.TotalGameTime.TotalSeconds)
                {
                    explosions.EmitterLocations.RemoveAt(explosions.EmitterLocations.Count - 1);
                    explosionTime.RemoveAt(i);
                    continue;
                }
            }
            explosions.Update(time, this, true);
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

        void DrawMiniMap(SpriteBatch batch)
        {
            Vector2 startingCorner = new Vector2(Global.screenWidth - 50, Global.screenHeight - 50);
            int blockSize = 5;

            for (int x = (int)WorldLimits.X - 1; x >= 0; x--)
            {
                for (int y = (int)WorldLimits.Y - 1; y >= 0; y--)
                {
                    Vector2 location = startingCorner - new Vector2(WorldLimits.X - 1 - x, WorldLimits.Y - 1 - y) * blockSize;
                    switch (Grid[x, y].State)
                    {
                        case(FireState.Burning):
                            if (Grid[x, y].Solid)
                                batch.Draw(Global.Textures["BlackBlock"], location, null, Color.White, 0, Vector2.Zero, new Vector2(1, 1) * blockSize / Global.Textures["BlackBlock"].Width, SpriteEffects.None, 0);
                            else
                                batch.Draw(Global.Textures["RedBlock"], location, null, Color.White, 0, Vector2.Zero, new Vector2(1, 1) * blockSize / Global.Textures["RedBlock"].Width, SpriteEffects.None, 0);
                            break;
                        case(FireState.Burnt):
                            batch.Draw(Global.Textures["GreyBlock"], location, null, Color.White, 0, Vector2.Zero, new Vector2(1, 1) * blockSize / Global.Textures["GreyBlock"].Width, SpriteEffects.None, 0);
                            break;
                        default:
                            if(Grid[x,y].Solid)
                                batch.Draw(Global.Textures["BlackBlock"], location, null, Color.White, 0, Vector2.Zero, new Vector2(1, 1) * blockSize / Global.Textures["BlackBlock"].Width, SpriteEffects.None, 0);
                            else
                                batch.Draw(Global.Textures["WhiteBlock"], location, null, Color.White, 0, Vector2.Zero, new Vector2(1, 1) * blockSize / Global.Textures["WhiteBlock"].Width, SpriteEffects.None, 0);
                            break;

                    }

                }
            }

            batch.Draw(Global.Textures["GreenBlock"], startingCorner - new Vector2(WorldLimits.X - 1 - _player.Position.X, WorldLimits.Y - 1 - _player.Position.Y) * blockSize, null, Color.White, 0, Vector2.Zero, new Vector2(1, 1) * blockSize / Global.Textures["GreenBlock"].Width, SpriteEffects.None, 0);

        }

        void EXPLODE(float x, float y, GameTime time)
        {
            explosions.EmitterLocations.Add(new Vector2(x + .5f,y + .5f) * Global.PixelsPerTile);
            explosionTime.Add((float)time.TotalGameTime.TotalSeconds);
            Grid[(int)x, (int)y].EXPLODES = false;
            camera.Shake = 70;
            Global.Sounds["Boom"].Play();
        }
    }
}

