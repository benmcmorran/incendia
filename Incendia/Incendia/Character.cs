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
    class Character : Sprite
    {
        public Character(Vector2 position, Animation defaultAnimation, double lifeTime)
            : base(position, defaultAnimation, lifeTime)
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            base.Draw(batch);
        }


    }
}
