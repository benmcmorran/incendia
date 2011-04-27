using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Incendia
{
    public class StateManager
    {
        IGameState state;
        IGameState transitionState;
        TimeSpan elapsedTransition;
        public bool isTransitioning = false;
        const float halfTransitionTime = 2f;

        public void SetState(IGameState state)
        {
            this.state = state;
        }
        
        public void SetTransitionState(IGameState state)
        {
            transitionState = state;
            elapsedTransition = TimeSpan.Zero;
            isTransitioning = true;
        }

        public void Update(GameTime gameTime)
        {
            if (isTransitioning)
            {
                elapsedTransition += gameTime.ElapsedGameTime;
                if (elapsedTransition.TotalSeconds > 2 * halfTransitionTime)
                    isTransitioning = false;
                else if (elapsedTransition.TotalSeconds > halfTransitionTime)
                    state = transitionState;
            }

            state.Update(gameTime);
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            state.Draw(spriteBatch);

            if (isTransitioning)
            {
                float alpha = 0;
                if (elapsedTransition.TotalSeconds < halfTransitionTime)
                    alpha = MathHelper.Lerp(0, 1, (float)(elapsedTransition.TotalSeconds / halfTransitionTime));
                else if (elapsedTransition.TotalSeconds <= 2 * halfTransitionTime)
                    alpha = MathHelper.Lerp(1, 0, (float)((elapsedTransition.TotalSeconds - halfTransitionTime) / halfTransitionTime));

                spriteBatch.Begin();
                spriteBatch.Draw(Global.Textures["Fade"], new Rectangle(0, 0, 640, 480), Color.White * alpha);
                spriteBatch.End();
            }
        }
    }
}
