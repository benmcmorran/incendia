using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace Incendia
{
    class MenuState : IGameState
    {
        private StateManager manager;
        private Viewport viewport;
        private string[] levels;
        private int currentLevel;

        public MenuState(StateManager manager, Viewport viewport)
        {
            this.manager = manager;
            this.viewport = viewport;

            levels = Directory.GetFiles(Environment.CurrentDirectory + "\\Levels", "*.txt");
            for (int i = 0; i < levels.Length; i++)
                levels[i] = Path.GetFileNameWithoutExtension(levels[i]);
            currentLevel = 0;
        }
        
        public void Update(GameTime gameTime)
        {
            if (Input.KeyJustPressed(Keys.Down))
                currentLevel = currentLevel + 1 >= levels.Length ? 0 : currentLevel + 1;

            if (Input.KeyJustPressed(Keys.Up))
                currentLevel = currentLevel - 1 < 0 ? levels.Length - 1 : currentLevel - 1;

            if (Input.KeyJustPressed(Keys.Enter))
            {
                PlayState playState = new PlayState(manager, levels[currentLevel], viewport);
                playState.Grid[5, 5].State = FireState.Burning;
                manager.SetState(playState);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(Global.Font, "Menu (enter to select)", new Vector2(100, 70), Color.DarkRed);

            for (int i = 0; i < levels.Length; i++)
            {
                Color color = Color.Black;
                if (currentLevel == i)
                    color = Color.Red;
                spriteBatch.DrawString(Global.Font, levels[i], new Vector2(100, 100 + 30 * i), color);
            }
            spriteBatch.End();
        }
    }
}
