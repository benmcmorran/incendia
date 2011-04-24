using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Incendia
{
    /// <summary>
    /// This class will generate animations, sprites and other game objects
    /// </summary>
    class Generator
    {
        public static Animation PlayerAnimation()
        {
            List<Frame> f = new List<Frame>();
            f.Add(new Frame("Player", new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0)));
            Animation a = new Animation(.05, f);
            return a;
        }

        public static Animation VictimAnimation()
        {
            List<Frame> f = new List<Frame>();
            f.Add(new Frame("Victim1", new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0)));
            Animation a = new Animation(.05, f);
            
            return a;
        }


        public static Character PlayerSprite(Vector2 location)
        {
            Character c = new Character(location * Global.PixelsPerTile, PlayerAnimation(), -1, 1000);
            return c;
        }

        public static Character VictimSprite(Vector2 location)
        {
            Character c = new Character(location * Global.PixelsPerTile, VictimAnimation(), -1, 1000000);
            return c;
        }

        public static Tile Carpet1()
        {
            return new Tile(.004f, Global.rand.Next(2000, 3000), FireState.Unburned, "Carpet 1", false);
        }

        public static Tile Carpet2()
        {
            return new Tile(.004f, Global.rand.Next(2000, 3000), FireState.Unburned, "Carpet 2", false);
        }

        public static Tile TiledFloor1()
        {
            return new Tile(.004f, Global.rand.Next(2000, 3000), FireState.Unburned, "Tiled Floor 1", false);
        }

        public static Tile TiledFloor2()
        {
            return new Tile(.004f, Global.rand.Next(2000, 3000), FireState.Unburned, "Tiled Floor 2", false);
        }

        public static Tile WoodenFloor()
        {
            return new Tile(.004f, Global.rand.Next(2000, 3000), FireState.Unburned, "Wooden Floor", false);
        }

        public static Tile GraniteWall()
        {
            return new Tile(0, 0, FireState.Nonflammable, "Granite Wall", true);
        }

        public static Tile WoodenWall()
        {
            return new Tile(.0001f, Global.rand.Next(2000, 3000), FireState.Unburned, "Wooden Wall", true);
        }
    }
}
