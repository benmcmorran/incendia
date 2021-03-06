﻿using System;
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
            f.Add(new Frame("Victim" + Global.rand.Next(1, 4), new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0)));
            Animation a = new Animation(.05, f);
            
            return a;
        }


        public static Character PlayerSprite(Vector2 location)
        {
            Character c = new Character(location * Global.PixelsPerTile, PlayerAnimation(), -1, 194);
            return c;
        }

        public static Character VictimSprite(Vector2 location)
        {
            Character c = new Character(location * Global.PixelsPerTile, VictimAnimation(), -1, 75);
            return c;
        }
    }
}
