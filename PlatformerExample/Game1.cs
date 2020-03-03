using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using PlatformLibrary;

namespace PlatformerExample
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteSheet sheet;
        Tileset tileset;
        Tilemap tilemap;
        Player player;
        List<Platform> platforms;
        AxisList world;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            platforms = new List<Platform>();
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
#if VISUAL_DEBUG
            VisualDebugging.LoadContent(Content);
#endif
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            var t = Content.Load<Texture2D>("spritesheet");
            sheet = new SpriteSheet(t, 21, 21, 3, 2);

            // Create the player with the corresponding frames from the spritesheet
            var playerFrames = from index in Enumerable.Range(19, 30) select sheet[index];
            player = new Player(playerFrames);

            // Load the level
            tilemap = Content.Load<Tilemap>("level1");


            platforms.Add(new Platform(new BoundingRectangle(80, 300, 105, 21), sheet[1]));
            platforms.Add(new Platform(new BoundingRectangle(280, 400, 84, 21), sheet[2]));
            platforms.Add(new Platform(new BoundingRectangle(160, 200, 42, 21), sheet[3]));

            // Add the platforms to the axis list
            world = new AxisList();
            foreach (Platform platform in platforms)
            {
                world.AddGameObject(platform);
            }

            tileset = Content.Load<Tileset>("tiledspritesheet"); 
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            player.Update(gameTime);

            // Check for platform collisions
            var platformQuery = world.QueryRange(player.Bounds.X, player.Bounds.X + player.Bounds.Width);
            player.CheckForPlatformCollision(platformQuery);
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Calculate and apply the world/view transform
            var offset = new Vector2(200, 300) - player.Position;
            var t = Matrix.CreateTranslation(offset.X, offset.Y, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null,null, t);

            // Draw the tilemap 
            tilemap.Draw(spriteBatch);

            // Draw the platforms 
            var platformQuery = world.QueryRange(player.Position.X - 221, player.Position.X + 400);
            foreach(Platform platform in platformQuery)
            {   
                platform.Draw(spriteBatch);
            }
            Debug.WriteLine($"{platformQuery.Count()} Platforms rendered");
            
            // Draw the player
            player.Draw(spriteBatch);
            
            // Draw an arbitrary range of sprites and corresponding tiles
            for(var i = 17; i < 30; i++)
            {
                sheet[i].Draw(spriteBatch, new Vector2(i*25, 25), Color.White);
                tileset[i].Draw(spriteBatch, new Vector2(i * 25, 50), Color.White);
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
