using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Incendia
{
    /// <summary>
    /// A frame for animation. Holds a string which is an index for a texture in Global and a rectangle which is how much of the texture to draw, like a window on the texture.
    /// </summary>
    class Frame
    {
        string _textureIndex; //A key used to find a texture in Global
        Rectangle _rectangle;

        public string TextureIndex { get { return _textureIndex; } }
        public Rectangle Rectangle { get { return _rectangle; } } //In PIXILS

        //Add a rectangle with zero width and height to make it fit the entire texture
        public Frame(string textureIndex, Rectangle rectangle)
        {
            _textureIndex = textureIndex;
            _rectangle = rectangle;
            if (_rectangle.Width <= 0)
                _rectangle.Width = Global.Textures[_textureIndex].Width;
            if (_rectangle.Height <= 0)
                _rectangle.Height = Global.Textures[_textureIndex].Height;
        }

        public void Draw(Vector2 location, SpriteBatch batch, float rotation, float scale, SpriteEffects effect)
        {
            batch.Draw(Global.Textures[_textureIndex], location, _rectangle, Color.White, rotation, new Vector2(Global.Textures[_textureIndex].Width / 2, Global.Textures[_textureIndex].Height / 2), scale, effect, 0);
        }
    }
}
