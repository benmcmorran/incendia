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
        bool showingLevels = false;
        private string[] levels;
        private int currentLevel;
        private int currentSelection;

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
            if (showingLevels)
            {
                if (Input.KeyJustPressed(Keys.Down))
                    currentLevel = currentLevel + 1 >= levels.Length ? 0 : currentLevel + 1;

                if (Input.KeyJustPressed(Keys.Up))
                    currentLevel = currentLevel - 1 < 0 ? levels.Length - 1 : currentLevel - 1;
            }
            else
            {
                if (Input.KeyJustPressed(Keys.Down))
                    currentSelection = currentSelection + 1 >= 3 ? 0 : currentSelection + 1;

                if (Input.KeyJustPressed(Keys.Up))
                    currentSelection = currentSelection - 1 < 0 ?  2 : currentSelection - 1;
            }

            if (Input.KeyJustPressed(Keys.Enter))
            {
                if (showingLevels)
                {
                    PlayState playState = new PlayState(manager, levels[currentLevel], viewport);
                    manager.SetTransitionState(playState);
                }
                else
                {
                    switch (currentSelection)
                    {
                        case 0: showingLevels = true; break;
                        case 1: manager.SetTransitionState(new MessageState(manager, this, "Help", "")); break;
                        case 2: manager.SetTransitionState(new MessageState(manager, this, "Game Info", "")); break;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Global.Textures["Splash"], new Rectangle(0, 0, Global.screenWidth, Global.screenHeight), Color.White);

            if (showingLevels)
            {
                for (int i = 0; i < levels.Length; i++)
                {
                    Color color = currentLevel == i ? Color.Red : Color.White;
                    string button = currentLevel == i ? "Button User Levels" : "Button User Levelsb";
                    
                    spriteBatch.Draw(Global.Textures[button], new Rectangle(100, 175 + 55 * i, 500, 40), Color.White);
                    spriteBatch.DrawString(Global.Font, levels[i], new Vector2(110, 180 + 55 * i), color);
                }
            }
            else
            {
                spriteBatch.Draw(Global.Textures["Button Play" + TextureEnding(0)], new Vector2(100, 175), Color.White);
                spriteBatch.Draw(Global.Textures["Button Help" + TextureEnding(1)], new Vector2(100, 275), Color.White);
                spriteBatch.Draw(Global.Textures["Button Info" + TextureEnding(2)], new Vector2(100, 375), Color.White);
            }
            spriteBatch.End();
        }

        private string TextureEnding(int num)
        {
            return num == currentSelection ? "b" : "";
        }
    }
}
