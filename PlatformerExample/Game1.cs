﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using PlatformLibrary;
using System;


namespace PlatformerExample
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        SpriteSheet sheet;
        SpriteFont spriteFont;
        Tileset tileset;
        Tilemap tilemap;
        Player player;
        List<Platform> platforms;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        AxisList world;
        public uint endX;
        public uint endY;
        GhostEnemy ghost;
        /// <summary>
        /// A random number generator used by the system 
        /// </summary>
        Random random = new Random();

        /// <summary>
        /// particle system for the ghost
        /// </summary>
        ParticleSystem GhostParticles;

        /// <summary>
        /// particle system for player dying
        /// </summary>
        ParticleSystem Rain;

        /// <summary>
        /// particle system for when player reaches the door and wins
        /// </summary>
        ParticleSystem DoorParticles;

        /// <summary>
        /// particle option 1, basic particle
        /// </summary>
        Texture2D NormalParticle;

        /// <summary>
        /// particle for explosion
        /// </summary>
        Texture2D RainParticle;

        /// <summary>
        /// particle for fire/ghost movement
        /// </summary>
        Texture2D FireParticle;


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

            //load in spritefont
            spriteFont = Content.Load<SpriteFont>("File");

            // TODO: use this.Content to load your game content here
            var t = Content.Load<Texture2D>("spritesheet");
            sheet = new SpriteSheet(t, 21, 21, 1, 2);

            
            // Load the level
            tilemap = Content.Load<Tilemap>("level5");

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
                else if(objectGroup.Name == "Ghost")
                {
                    GroupObject groupObject = objectGroup.Objects[0];
                    var GhostFrames = from index in Enumerable.Range(445, 449) select sheet[index];
                    ghost = new GhostEnemy(GhostFrames, player, groupObject.X, groupObject.Y);
                }
                else if(objectGroup.Name == "End") //get the ending location which will end the game
                {
                    GroupObject groupObject = objectGroup.Objects[0];
                    endX = groupObject.X;
                    endY = groupObject.Y;
                }
            }
            

            // Add the platforms to the axis list
            world = new AxisList();
            foreach (Platform platform in platforms)
            {
                world.AddGameObject(platform);
            }

            tileset = Content.Load<Tileset>("BetterSet2");

            // particle information for when player reaches the door
            
            NormalParticle = Content.Load<Texture2D>("particle");
            DoorParticles = new ParticleSystem(GraphicsDevice, 1000, NormalParticle);
            DoorParticles.Emitter = new Vector2(100, 100);
            DoorParticles.SpawnPerFrame = 4;
                    
                // Set the SpawnParticle method
            DoorParticles.SpawnParticle = (ref Particle particle) =>
            {
                particle.Position = new Vector2(player.Position.X, player.Position.Y);
                particle.Velocity = new Vector2(
                    MathHelper.Lerp(-200, 200, (float)random.NextDouble()), 
                    MathHelper.Lerp(-200, 200, (float)random.NextDouble()) 
                    );
                particle.Acceleration = 2.0f * new Vector2(0, (float)-random.NextDouble());
                particle.Color = Color.Gold;
                particle.Scale = 1.5f;
                particle.Life = 8.0f;
            };

                // Set the UpdateParticle method
            DoorParticles.UpdateParticle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += deltaT * particle.Acceleration;
                particle.Position += deltaT * particle.Velocity;
                particle.Scale -= deltaT;
                particle.Life -= deltaT;
            };


            // particle info for rain
            RainParticle = Content.Load<Texture2D>("rain drop");
            Rain = new ParticleSystem(GraphicsDevice, 5000, RainParticle);
            Rain.Emitter = new Vector2(100, 100);
            Rain.SpawnPerFrame = 4;

            // Set the SpawnParticle method
            Rain.SpawnParticle = (ref Particle particle) =>
            {
                particle.Position = new Vector2(
                    MathHelper.Lerp(0, 2100, (float)random.NextDouble()),
                    MathHelper.Lerp(0, 1, (float)random.NextDouble())
                    );
                particle.Velocity = new Vector2(
                    MathHelper.Lerp(0, 0, (float)random.NextDouble()), 
                    MathHelper.Lerp(-100, -25, (float)random.NextDouble()) 
                    );
                particle.Acceleration = 0.0f * new Vector2(0, (float)-random.NextDouble());
                particle.Color = Color.OrangeRed;
                particle.Scale = 0.5f;
                particle.Life = 200.0f;
            };

            // Set the UpdateParticle method
            Rain.UpdateParticle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += deltaT * particle.Acceleration;
                particle.Position -= deltaT * particle.Velocity;
                //particle.Scale = particle.Scale;
                particle.Life -= deltaT;
            };


            // particle info for ghost
            FireParticle = Content.Load<Texture2D>("Fire");
            GhostParticles = new ParticleSystem(GraphicsDevice, 1000, FireParticle);
            GhostParticles.Emitter = new Vector2(100, 100);
            GhostParticles.SpawnPerFrame = 4;

            // Set the SpawnParticle method
            GhostParticles.SpawnParticle = (ref Particle particle) =>
            {
                particle.Position = new Vector2(
                    MathHelper.Lerp(ghost.Position.X - 42, ghost.Position.X , (float)random.NextDouble()),
                    MathHelper.Lerp(ghost.Position.Y - 42, ghost.Position.Y, (float)random.NextDouble())
                    );
                particle.Velocity = new Vector2(
                    MathHelper.Lerp(-50, 50, (float)random.NextDouble()), 
                    MathHelper.Lerp(-50, 50, (float)random.NextDouble()) 
                    );
                particle.Acceleration = 1.0f * new Vector2(0, (float)-random.NextDouble());
                particle.Color = Color.GhostWhite;
                particle.Scale = 0.5f;
                particle.Life = 0.5f;
            };

            // Set the UpdateParticle method
            GhostParticles.UpdateParticle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += deltaT * particle.Acceleration;
                particle.Position += deltaT * particle.Velocity;
                particle.Scale -= deltaT;
                particle.Life -= deltaT;
            };

            // put player and ghost drawings on layer 2
            /*
            var playerLayer = new ParallaxLayer(this);
            playerLayer.Sprites.Add(player);
            playerLayer.Sprites.Add(ghost);
            playerLayer.DrawOrder = 3;
            Components.Add(playerLayer);
            */
            
            //fireballs background drawing           
            var FireTextures = new Texture2D[]
            {
                 Content.Load<Texture2D>("FireBalls2")
            };
           
            var FireSprites = new StaticSprite[]
            {
                new StaticSprite(FireTextures[0]),
                new StaticSprite(FireTextures[0], new Vector2(500, 0)),
                new StaticSprite(FireTextures[0], new Vector2(1000, 0)),
                new StaticSprite(FireTextures[0], new Vector2(1500, 0)),
                new StaticSprite(FireTextures[0], new Vector2(2000, 0))
            };
            var FireLayer = new ParallaxLayer(this);
            FireLayer.Sprites.AddRange(FireSprites);
            FireLayer.DrawOrder = 2;
            Components.Add(FireLayer);


            //Cloud layer background Drawing
            var CloudTextures = new Texture2D[]
            {
                 Content.Load<Texture2D>("Clouds2")                                
            };

            var CloudSprites = new StaticSprite[]
            {
                new StaticSprite(CloudTextures[0]),
                new StaticSprite(CloudTextures[0], new Vector2(500, 0)),
                new StaticSprite(CloudTextures[0], new Vector2(1000, 0)),
                new StaticSprite(CloudTextures[0], new Vector2(1500, 0)),
                new StaticSprite(CloudTextures[0], new Vector2(2000, 0))
            };

            var CloudLayer = new ParallaxLayer(this);
            CloudLayer.Sprites.AddRange(CloudSprites);
            CloudLayer.DrawOrder = 1;
            Components.Add(CloudLayer);

            //Mountain layer background Drawing
            var FlourTextures = new Texture2D[]
            {
                 Content.Load<Texture2D>("Flours")
            };

            var FlourSprites = new StaticSprite[]
            {
                new StaticSprite(FlourTextures[0]),
                new StaticSprite(FlourTextures[0], new Vector2(500, 0)),
                new StaticSprite(FlourTextures[0], new Vector2(1000, 0)),
                new StaticSprite(FlourTextures[0], new Vector2(1500, 0)),
                new StaticSprite(FlourTextures[0], new Vector2(2000, 0))
            };

            var FlourLayer = new ParallaxLayer(this);
            FlourLayer.Sprites.AddRange(FlourSprites);
            FlourLayer.DrawOrder = 0;
            Components.Add(FlourLayer);


            //speeds of each background and character layer
            FlourLayer.ScrollController = new PlayerTrackingScrollController(player, 0.1f);
            CloudLayer.ScrollController = new PlayerTrackingScrollController(player, 0.4f);
            //playerLayer.ScrollController = new PlayerTrackingScrollController(player, 1.0f);
            FireLayer.ScrollController = new PlayerTrackingScrollController(player, 1.0f);

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

            ghost.Update(gameTime);
            // Check for platform collisions
            var platformQuery = world.QueryRange(player.Bounds.X, player.Bounds.X + player.Bounds.Width);
            player.CheckForPlatformCollision(platformQuery);

            

            DoorParticles.Update(gameTime);
            Rain.Update(gameTime);
            GhostParticles.Update(gameTime);

            if (ghost.Bounds.CollidesWith(player.Bounds)) // check for collisions with the player
            {
                player.gameState = 2;
                /*             
                if (player.Bounds.Y + player.Bounds.Height <= Bounds.Y)
                {
                    //logic player bouncing off head, but ghost wont be dying, can bounce off ghost heads
                    player.Position.Y -= 1;
                    player.playerBounce = 1;
                }                   
                else if (player.Bounds.X >= Bounds.X + Bounds.Width) //&& player.Bounds.Y > Bounds.Y
                {
                    //logic for player death
                    player.gameState = 2;
                }
                else if (player.Bounds.X + player.Bounds.Width <= Bounds.X) //&& player.Bounds.Y > Bounds.Y - 18
                {
                    //logic for player death
                    player.gameState = 2;
                }
                */
            }
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
            ghost.Draw(spriteBatch);
            GhostParticles.Draw(t);
            Rain.Draw(t);




            if (player.gameState == 1)  //if you have won, draw the you win
            {
                //spriteBatch.Draw(YouWin, win, Color.White);
                spriteBatch.DrawString(spriteFont, "You Win! :)", player.Position, Color.Gold);
                DoorParticles.Draw(t);
            }

            if (player.gameState == 2) //if you have lost, draw the you lose
            {
                //spriteBatch.Draw(YouLose, lose, Color.White);
                spriteBatch.DrawString(spriteFont, "Ghostie got you :(", player.Position, Color.Red);              
            }

            if (player.gameState == 3) //if you have lost, draw the you lose
            {
                //spriteBatch.Draw(YouLose, lose, Color.White);
                spriteBatch.DrawString(spriteFont, "You fell to your death :(", player.Position, Color.Red);             
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
