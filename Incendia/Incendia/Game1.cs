using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Incendia
{
    //Hey there! This class is where the game begins! 

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        StateManager stateManager;

        public Game1()
        {
            IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
            Global.screenWidth = graphics.PreferredBackBufferWidth;
            Global.screenHeight = graphics.PreferredBackBufferHeight;
            //graphics.IsFullScreen = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Global.Textures = new Dictionary<string, Texture2D>();
            Global.Textures.Add("Player", Content.Load<Texture2D>(@"Images\Fireman1"));
            Global.Textures.Add("Empty", Content.Load<Texture2D>(@"Images\Empty"));
            Global.Textures.Add("Emptyb", Content.Load<Texture2D>(@"Images\Empty"));

            Global.Textures.Add("Outside", Content.Load<Texture2D>(@"Images\Grass"));
            Global.Textures.Add("Outsideb", Content.Load<Texture2D>(@"Images\Grass"));
            Global.Textures.Add("Carpet 1", Content.Load<Texture2D>(@"Images\Carpet")); //This image MUST be square
            Global.Textures.Add("Carpet 1b", Content.Load<Texture2D>(@"Images\Carpetb")); //This image MUST be square
            Global.Textures.Add("Carpet 2", Content.Load<Texture2D>(@"Images\Carpet2")); //This image MUST be square
            Global.Textures.Add("Carpet 2b", Content.Load<Texture2D>(@"Images\Carpetb")); //This image MUST be square
            Global.Textures.Add("Tiled Floor 1", Content.Load<Texture2D>(@"Images\Tiles")); //This image MUST be square
            Global.Textures.Add("Tiled Floor 1b", Content.Load<Texture2D>(@"Images\Tiles")); //This image MUST be square
            Global.Textures.Add("Tiled Floor 2", Content.Load<Texture2D>(@"Images\Tiles2")); //This image MUST be square
            Global.Textures.Add("Tiled Floor 2b", Content.Load<Texture2D>(@"Images\Tiles2")); //This image MUST be square
            Global.Textures.Add("Wooden Floor", Content.Load<Texture2D>(@"Images\woodenfloor")); //This image MUST be square
            Global.Textures.Add("Wooden Floorb", Content.Load<Texture2D>(@"Images\woodenfloorb")); //This image MUST be square
            Global.Textures.Add("Granite Wall", Content.Load<Texture2D>(@"Images\granitewall")); //This image MUST be square
            Global.Textures.Add("Granite Wallb", Content.Load<Texture2D>(@"Images\granitewall")); //This image MUST be square
            Global.Textures.Add("Wooden Wall", Content.Load<Texture2D>(@"Images\woodwall")); //This image MUST be square
            Global.Textures.Add("Wooden Wallb", Content.Load<Texture2D>(@"Images\woodwallb")); //This image MUST be square

            Global.Textures.Add("Desk", Content.Load<Texture2D>(@"Images\Table")); //This image MUST be square
            Global.Textures.Add("Deskb", Content.Load<Texture2D>(@"Images\Empty")); //This image MUST be square
            Global.Textures.Add("Couch", Content.Load<Texture2D>(@"Images\Couch")); //This image MUST be square
            Global.Textures.Add("Couchb", Content.Load<Texture2D>(@"Images\Couchb")); //This image MUST be square
            Global.Textures.Add("Plant", Content.Load<Texture2D>(@"Images\Plant")); //This image MUST be square
            Global.Textures.Add("Plantb", Content.Load<Texture2D>(@"Images\Plantburnt")); //This image MUST be square
            Global.Textures.Add("Open Horizontal Door", Content.Load<Texture2D>(@"Images\dooropentop")); //This image MUST be square
            Global.Textures.Add("Open Horizontal Doorb", Content.Load<Texture2D>(@"Images\Empty")); //This image MUST be square
            Global.Textures.Add("Open Vertical Door", Content.Load<Texture2D>(@"Images\dooropenRight")); //This image MUST be square
            Global.Textures.Add("Open Vertical Doorb", Content.Load<Texture2D>(@"Images\Empty")); //This image MUST be square
            Global.Textures.Add("Closed Horizontal Door", Content.Load<Texture2D>(@"Images\doortop")); //This image MUST be square
            Global.Textures.Add("Closed Horizontal Doorb", Content.Load<Texture2D>(@"Images\Empty")); //This image MUST be square
            Global.Textures.Add("Closed Vertical Door", Content.Load<Texture2D>(@"Images\doorRight")); //This image MUST be square
            Global.Textures.Add("Closed Vertical Doorb", Content.Load<Texture2D>(@"Images\Empty")); //This image MUST be square
            Global.Textures.Add("Trash Can", Content.Load<Texture2D>(@"Images\trashcan")); //This image MUST be square
            Global.Textures.Add("Trash Canb", Content.Load<Texture2D>(@"Images\trashcanb")); //This image MUST be square
            Global.Textures.Add("Newspaper", Content.Load<Texture2D>(@"Images\newspaper")); //This image MUST be square
            Global.Textures.Add("Newspaperb", Content.Load<Texture2D>(@"Images\newspaperb")); //This image MUST be square
            Global.Textures.Add("Flammables", Content.Load<Texture2D>(@"Images\GasCan"));
            Global.Textures.Add("Flammablesb", Content.Load<Texture2D>(@"Images\GasCanb")); 


            Global.Textures.Add("Computer", Content.Load<Texture2D>(@"Images\Computer")); //This image MUST be square
            Global.Textures.Add("Computerb", Content.Load<Texture2D>(@"Images\Computerb")); //This image MUST be square
            Global.Textures.Add("Blotter", Content.Load<Texture2D>(@"Images\deskblotter")); //This image MUST be square
            Global.Textures.Add("Blotterb", Content.Load<Texture2D>(@"Images\Empty")); //This image MUST be square
            Global.Textures.Add("Desk Plant", Content.Load<Texture2D>(@"Images\deskplant")); //This image MUST be square
            Global.Textures.Add("Desk Plantb", Content.Load<Texture2D>(@"Images\Empty")); //This image MUST be square
            Global.Textures.Add("Laptop", Content.Load<Texture2D>(@"Images\laptop")); //This image MUST be square
            Global.Textures.Add("Laptopb", Content.Load<Texture2D>(@"Images\laptopb")); //This image MUST be square

            Global.Textures.Add("Water", Content.Load<Texture2D>(@"Images\ParticleProxy")); //This image MUST be square
            Global.Textures.Add("Fire", Content.Load<Texture2D>(@"Images\FireProxy")); //This image MUST be square
            Global.Textures.Add("Smoke", Content.Load<Texture2D>(@"Images\Smoke"));
            Global.Textures.Add("HpBarOutline", Content.Load<Texture2D>(@"Images\HpBarOutline")); 
            Global.Textures.Add("HpBit", Content.Load<Texture2D>(@"Images\HpBit"));
            Global.Textures.Add("HpBitFlash", Content.Load<Texture2D>(@"Images\HpBitFlash"));

            Global.Textures.Add("BlackBlock", Content.Load<Texture2D>(@"Images\BlackBlock"));
            Global.Textures.Add("RedBlock", Content.Load<Texture2D>(@"Images\RedBlock"));
            Global.Textures.Add("WhiteBlock", Content.Load<Texture2D>(@"Images\WhiteBlock"));
            Global.Textures.Add("GreyBlock", Content.Load<Texture2D>(@"Images\GreyBlock"));
            Global.Textures.Add("GreenBlock", Content.Load<Texture2D>(@"Images\GreenBlock")); 



            Global.Textures.Add("Victim1", Content.Load<Texture2D>(@"Images\People1")); //This image MUST be square
            Global.Textures.Add("Victim2", Content.Load<Texture2D>(@"Images\People2")); //This image MUST be square
            Global.Textures.Add("Victim3", Content.Load<Texture2D>(@"Images\People3")); //This image MUST be square

            Global.Textures.Add("Fade", Content.Load<Texture2D>(@"Images\Fade"));

            Global.Textures.Add("Splash", Content.Load<Texture2D>(@"Images\UI\Incendia"));
            Global.Textures.Add("Game Info", Content.Load<Texture2D>(@"Images\UI\Gameinfo"));
            Global.Textures.Add("Help", Content.Load<Texture2D>(@"Images\UI\Help"));
            Global.Textures.Add("Button Help", Content.Load<Texture2D>(@"Images\UI\ButtonHelp"));
            Global.Textures.Add("Button Helpb", Content.Load<Texture2D>(@"Images\UI\ButtonHelpb"));
            Global.Textures.Add("Button Info", Content.Load<Texture2D>(@"Images\UI\ButtonInfo"));
            Global.Textures.Add("Button Infob", Content.Load<Texture2D>(@"Images\UI\ButtonInfob"));
            Global.Textures.Add("Button Levels", Content.Load<Texture2D>(@"Images\UI\ButtonLevels"));
            Global.Textures.Add("Button Levelsb", Content.Load<Texture2D>(@"Images\UI\ButtonLevelsb"));
            Global.Textures.Add("Button Play", Content.Load<Texture2D>(@"Images\UI\ButtonPlay"));
            Global.Textures.Add("Button Playb", Content.Load<Texture2D>(@"Images\UI\ButtonPlayb"));
            Global.Textures.Add("Button User Levels", Content.Load<Texture2D>(@"Images\UI\ButtonUserLevels"));
            Global.Textures.Add("Button User Levelsb", Content.Load<Texture2D>(@"Images\UI\ButtonUserLevelsb"));

            Global.Sounds = new Dictionary<string, SoundEffect>();
            Global.Sounds.Add("Fire", Content.Load<SoundEffect>(@"Sound\FIRE!"));
            Global.Sounds.Add("Hose", Content.Load<SoundEffect>(@"Sound\hose"));
            Global.Sounds.Add("5 Left", Content.Load<SoundEffect>(@"Sound\Chief\5left"));
            Global.Sounds.Add("4 Left", Content.Load<SoundEffect>(@"Sound\Chief\4left"));
            Global.Sounds.Add("3 Left", Content.Load<SoundEffect>(@"Sound\Chief\3left"));
            Global.Sounds.Add("2 Left", Content.Load<SoundEffect>(@"Sound\Chief\2left"));
            Global.Sounds.Add("1 Left", Content.Load<SoundEffect>(@"Sound\Chief\1left"));
            Global.Sounds.Add("0 Left", Content.Load<SoundEffect>(@"Sound\Chief\0left"));
            Global.Sounds.Add("Boom", Content.Load<SoundEffect>(@"Sound\Boom"));

            Global.Font = Content.Load<SpriteFont>(@"SpriteFont1");

            stateManager = new StateManager();
            stateManager.SetState(new MenuState(stateManager, GraphicsDevice.Viewport));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            stateManager.Update(gameTime);
            Input.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            stateManager.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
