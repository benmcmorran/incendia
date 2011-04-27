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
            Global.Textures.Add("Player", Content.Load<Texture2D>(@"Images\Guy"));
            Global.Textures.Add("Empty", Content.Load<Texture2D>(@"Images\Empty"));
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
            Global.Textures.Add("Water", Content.Load<Texture2D>(@"Images\ParticleProxy")); //This image MUST be square
            Global.Textures.Add("Fire", Content.Load<Texture2D>(@"Images\FireProxy")); //This image MUST be square
            Global.Textures.Add("HpBarOutline", Content.Load<Texture2D>(@"Images\HpBarOutline")); 
            Global.Textures.Add("HpBit", Content.Load<Texture2D>(@"Images\HpBit"));
            Global.Textures.Add("HpBitFlash", Content.Load<Texture2D>(@"Images\HpBitFlash")); 



            Global.Textures.Add("Victim1", Content.Load<Texture2D>(@"Images\VictimProxy")); //This image MUST be square

            Global.Textures.Add("Fade", Content.Load<Texture2D>(@"Images\Fade"));

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
            GraphicsDevice.Clear(Color.White);

            stateManager.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
