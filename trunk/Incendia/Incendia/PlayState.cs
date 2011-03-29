using System;
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

        public PlayState(Vector2 worldLimits)
        {
            _player = Generator.PlayerSprite(new Vector2(100, 100));
            WorldLimits = worldLimits;
        }

        public void Update(GameTime gameTime)
        {
            TakeInput();
            _player.Update(gameTime);
        }

        public void Draw(SpriteBatch batch)
        {
            _player.Draw(batch);
        }

        void TakeInput()
        {
            if (Input.DistanceToMouse(_player.Position) > _player.Visual.Height / 2)
                _player.Rotation = Input.AngleToMouse(_player.Position);
            if (Input.KeyHeld(Keys.W))
                _player.SetVelocityY(-100);
            else if (Input.KeyHeld(Keys.S))
                _player.SetVelocityY(100);
            else
                _player.SetVelocityY(0);
            if (Input.KeyHeld(Keys.D))
                _player.SetVelocityX(100);
            else if (Input.KeyHeld(Keys.A))
                _player.SetVelocityX(-100);
            else
                _player.SetVelocityX(0);
        }
    }
}
