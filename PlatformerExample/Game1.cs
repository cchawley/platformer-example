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
        SpriteSheet sheet;
        Tileset tileset;
        Tilemap tilemap;
        Player player;
        List<Platform> platforms;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
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
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 1000;
            graphics.ApplyChanges();
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
            sheet = new SpriteSheet(t, 21, 21, 1, 2);

            
            // Load the level
            tilemap = Content.Load<Tilemap>("level3");

            foreach(ObjectGroup objectGroup in tilemap.ObjectGroups)
            {
                if(objectGroup.Name == "Platforms") //create all of the tiles from the tile objects
                {
                    foreach (GroupObject groupObject in objectGroup.Objects)
                    {
                        BoundingRectangle bounds = new BoundingRectangle(
                            groupObject.X,
                            groupObject.Y,
                            groupObject.Width,
                            groupObject.Height
                        );
                        platforms.Add(new Platform(bounds, sheet[groupObject.SheetIndex - 1]));
                    }

                    world = new AxisList();
                    foreach (Platform platform in platforms)
                    {
                        world.AddGameObject(platform);
                    }
                }
                else if(objectGroup.Name == "Spawn")// give the player the starting point from the object
                {
                    GroupObject groupObject = objectGroup.Objects[0];
                    // Create the player with the corresponding frames from the spritesheet
                    var playerFrames = from index in Enumerable.Range(139, 150) select sheet[index];
                    //List<Sprite> playerFramesList = playerFrames.ToList();
                    //playerFramesList.Add(sheet[112]);
                    player = new Player(playerFrames, groupObject.X, groupObject.Y);
                }
                else if(objectGroup.Name == "End") //get the ending location which will end the game
                {
                    GroupObject groupObject = objectGroup.Objects[0];
                }
            }
            

            // Add the platforms to the axis list
            world = new AxisList();
            foreach (Platform platform in platforms)
            {
                world.AddGameObject(platform);
            }

            tileset = Content.Load<Tileset>("BetterSet2"); 
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
            var offset = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2) - player.Position;
            var t = Matrix.CreateTranslation(offset.X, offset.Y, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null,null, t);

            // Draw the tilemap 
            tilemap.Draw(spriteBatch);

            // Draw the platforms 
            var platformQuery = world.QueryRange(player.Position.X - GraphicsDevice.Viewport.Width / 2, player.Position.X + GraphicsDevice.Viewport.Width / 2);
            foreach (Platform platform in platformQuery)
            {   
                platform.Draw(spriteBatch);
            }
            Debug.WriteLine($"{platformQuery.Count()} Platforms rendered");
            
            // Draw the player
            player.Draw(spriteBatch);
            
            


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
