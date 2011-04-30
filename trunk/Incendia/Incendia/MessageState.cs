using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Incendia
{
    class MessageState : IGameState
    {
        public string Background { get; set; }
        public string Text { get; set; }
        private StateManager manager;
        private IGameState returnState;

        public MessageState(StateManager manager, IGameState returnState, string background, string text)
        {
            this.manager = manager;
            this.returnState = returnState;
            Background = background;
            Text = text;
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().GetPressedKeys().Length > 0 && !manager.isTransitioning)
                manager.SetTransitionState(returnState);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Global.Textures[Background], new Rectangle(0, 0, Global.screenWidth, Global.screenHeight), Color.White);
            Vector2 size = Global.Font.MeasureString(Text);
            spriteBatch.DrawString(Global.Font, Text, new Vector2((Global.screenWidth - size.X) / 2, (Global.screenHeight - size.Y) / 2), Color.White);
            spriteBatch.End();
        }
    }
}
