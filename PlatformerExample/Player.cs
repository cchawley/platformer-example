using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using PlatformLibrary;

namespace PlatformerExample
{
    /// <summary>
    /// An enumeration of possible player animation states
    /// </summary>
    enum PlayerState
    {
        Idle,
        JumpingLeft,
        JumpingRight,
        WalkingLeft,
        WalkingRight,
        FallingLeft,
        FallingRight
    }

    /// <summary>
    /// An enumeration of possible player veritcal movement states
    /// </summary>
    enum VerticalMovementState
    {
        OnGround,
        Jumping,
        Falling
    }

    /// <summary>
    /// A class representing the player
    /// </summary>
    public class Player : ISprite
    {
        // The speed of the walking animation
        const int FRAME_RATE = 500;

        Game1 game;

        public int gameState = 0;  // 0 for normal, 1 for winning, and 2 for dying

        // The duration of a player's jump, in milliseconds
        const int JUMP_TIME = 500;

        // The player sprite frames
        Sprite[] frames;

        // The currently rendered frame
        int currentFrame = 0;

        // The player's animation state
        PlayerState animationState = PlayerState.Idle;

        // The player's speed
        public int speed = 3;

        // The player's vertical movement state
        VerticalMovementState verticalState = VerticalMovementState.OnGround;

        // A timer for jumping
        TimeSpan jumpTimer;

        // A timer for animations
        TimeSpan animationTimer;

        // The currently applied SpriteEffects
        SpriteEffects spriteEffects = SpriteEffects.None;

        // The color of the sprite
        Color color = Color.White;

        // The origin of the sprite (centered on its feet)
        Vector2 origin = new Vector2(10, 20);

        Vector2 BoxCalc = new Vector2(9, 22);

        public int playerBounce;

        public Vector2 StartingPosition;

        /// <summary>
        /// Gets and sets the position of the player on-screen
        /// </summary>
        public Vector2 Position = new Vector2();

        public BoundingRectangle Bounds => new BoundingRectangle(Position - 1.8f * BoxCalc, 35, 41);

        /// <summary>
        /// Constructs a new player
        /// </summary>
        /// <param name="frames">The sprite frames associated with the player</param>
        public Player(IEnumerable<Sprite> frames, uint x, uint y)
        {
            this.frames = frames.ToArray();
            animationState = PlayerState.Idle;
            Position.X = x + 10;
            Position.Y = y + 21;
            StartingPosition = Position;
        }

        /// <summary>
        /// Updates the player, applying movement and physics
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();

            if (gameState == 0)
            {

                // Vertical movement
                switch (verticalState)
                {
                    case VerticalMovementState.OnGround:
                        if (keyboard.IsKeyDown(Keys.Space))
                        {
                            verticalState = VerticalMovementState.Jumping;
                            jumpTimer = new TimeSpan(0);
                        }
                        break;
                    case VerticalMovementState.Jumping:
                        jumpTimer += gameTime.ElapsedGameTime;
                        // Simple jumping with platformer physics
                        Position.Y -= (250 / (float)jumpTimer.TotalMilliseconds);
                        if (jumpTimer.TotalMilliseconds >= JUMP_TIME) verticalState = VerticalMovementState.Falling;
                        if (playerBounce == 1)
                        {
                            jumpTimer = new TimeSpan(0);
                            Position.Y -= (250 / (float)jumpTimer.TotalMilliseconds);
                            playerBounce = 0;
                        }
                        if (Position.X - 16 < 0)
                        {
                            Position.X = 16;
                        }
                        if (Position.X + 20 > 2100)
                        {
                            Position.X = 2100;
                        }
                        break;
                    case VerticalMovementState.Falling:
                        Position.Y += speed;                                                                
                        if (Position.X - 16 < 0)
                        {
                            Position.X = 16;
                        }
                        if (Position.X + 20 > 2100)
                        {
                            Position.X = 2100;
                        }
                        
                        break;

                }

                        // Horizontal movement
                        if (keyboard.IsKeyDown(Keys.Left))
                        {
                            if (verticalState == VerticalMovementState.Jumping || verticalState == VerticalMovementState.Falling)
                                animationState = PlayerState.JumpingLeft;
                            else animationState = PlayerState.WalkingLeft;
                            Position.X -= speed;
                            /*
                            if (Position.X - 16 < 0)
                            {
                                Position.X = 16;
                            }*/
                        }
                        else if (keyboard.IsKeyDown(Keys.Right))
                        {
                            if (verticalState == VerticalMovementState.Jumping || verticalState == VerticalMovementState.Falling)
                                animationState = PlayerState.JumpingRight;
                            else animationState = PlayerState.WalkingRight;
                            Position.X += speed;
                            if (Position.X - 16 < 0)
                            {
                                Position.X = 16;
                            }
                            if (Position.X + 20 > 2100)
                            {
                                Position.X = 2100;
                            }
                        }
                        else
                        {
                            animationState = PlayerState.Idle;
                        }

                
            }
            if (Position.X >= 2090 && Position.Y <= 200) //if player gets to the end of the level, they win!
            {
                gameState = 1;
                speed = 0;
            }
            if(Position.Y > 1870)
            {
                gameState = 3;
            }

            // Apply animations
            switch (animationState)
            {
                case PlayerState.Idle:
                    currentFrame = 0;
                    animationTimer = new TimeSpan(0);
                    break;

                case PlayerState.JumpingLeft:
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    currentFrame = 7;
                    break;

                case PlayerState.JumpingRight:
                     spriteEffects = SpriteEffects.None;
                    currentFrame = 7;
                    break;

                case PlayerState.WalkingLeft:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    // Walking frames are 9 & 10
                    if(animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 9;
                    break;

                case PlayerState.WalkingRight:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.None;
                    // Walking frames are 9 & 10
                    if (animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 9;
                    break;

            }
        }

        public void CheckForPlatformCollision(IEnumerable<IBoundable> platforms)
        {
            Debug.WriteLine($"Checking collisions against {platforms.Count()} platforms");
            if (verticalState != VerticalMovementState.Jumping)
            {
                verticalState = VerticalMovementState.Falling;
                foreach (IBoundable platform in platforms)
                {
                    if (Bounds.CollidesWith(platform.Bounds))
                    {
                        Position.Y = platform.Bounds.Y - 1;
                        verticalState = VerticalMovementState.OnGround;
                    }
                }
            }
        }

        /// <summary>
        /// Render the player sprite.  Should be invoked between 
        /// SpriteBatch.Begin() and SpriteBatch.End()
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to use</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
#if VISUAL_DEBUG 
            VisualDebugging.DrawRectangle(spriteBatch, Bounds, Color.Red);
#endif
            frames[currentFrame].Draw(spriteBatch, Position, color, 0, origin, 2, spriteEffects, 1);
        }

    }
}
