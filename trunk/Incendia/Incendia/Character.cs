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

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, PlayState map) 
        {
            base.Update(gameTime, map);
            KeepInBounds(map);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            base.Draw(batch);
        }

        private void KeepInBounds(PlayState map)
        {
            //Place the character within the bounds of the world
            if (Position.X < 0)
                SetPositionX(0);
            else if (Position.X + Visual.Width > map.WorldLimits.X)
                SetPositionX(map.WorldLimits.X - Visual.Width);
            if (Position.Y < 0)
                SetPositionY(0);
            else if (Position.Y + Visual.Height > map.WorldLimits.Y)
                SetPositionY(map.WorldLimits.Y - Visual.Height);
        }


    }
}
