﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;


namespace Incendia
{
    /// <summary>
    /// The dreaded Global class holds global, static fields. They should be set at the beginning of the game in Game1 and NEVER AGAIN!!!! If you want to set any value in here
    /// outside of initiallization, let everyone know!
    /// </summary>
    class Global
    {
        public static Dictionary<string, Texture2D> Textures;
        public static Dictionary<string, SoundEffect> Sounds;
        static public Random rand = new Random();
        public static int screenWidth, screenHeight;
        public static float SmokeSpread = .005f;
        public static SpriteFont Font;
        public static  float PixelsPerTile = 50;
    }
}
