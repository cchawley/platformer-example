using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformLibrary;
using PlatformerExample;
//using MonoGameWindoStarter;

namespace MonoGameWindowsStarter
{
    enum GhostState
    {
        Idle,
        JumpingLeft,
        JumpingRight,
        WalkingLeft,
        WalkingRight,
        FallingLeft,
        FallingRight
    }

    public class GhostEnemy
    {
        // The speed of the walking animation
        const int FRAME_RATE = 500;

        Player player;

        // The ghost sprite frames
        Sprite[] frames;

        // The currently rendered frame
        int currentFrame = 0;

        // The ghost's animation state
        GhostState animationState = GhostState.WalkingLeft;

        // The ghost's speed
        int speed = 3;



        bool jumping = false;


        bool falling = false;

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


        public Vector2 Position = new Vector2(1500, 400);

        public BoundingRectangle Bounds => new BoundingRectangle(Position - 1.55f * BoxCalc, 31, 38); //edit to get box accurate around player

        /// <summary>
        /// Constructs a new ghost
        /// </summary>
        /// <param name="frames">The sprite frames associated with the player</param>
        public GhostEnemy(IEnumerable<Sprite> frames, Player player)
        {
            this.frames = frames.ToArray();
            animationState = GhostState.WalkingLeft;
            this.player = player;
        }

        /// <summary>
        /// Updates the ghost, applying movement and physics
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        public void Update(GameTime gameTime)
        {

            if (player.gameState == 0) // if the game is still going
            {
                if (animationState == GhostState.WalkingLeft)
                {
                    Position.X -= speed;

                    if (Position.X - 16 < 0)
                    {
                        Position.X = 16;
                        animationState = GhostState.WalkingRight;
                    }
                }
                else if (animationState == GhostState.WalkingRight)
                {
                    Position.X += speed;
                    origin = new Vector2(10, 20);
                    if (Position.X + 20 > 1600)
                    {
                        Position.X = 1580;
                        animationState = GhostState.WalkingLeft;
                    }
                }

                if (Bounds.CollidesWith(player.Bounds)) // check for collisions with the player
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


            }
            switch (animationState)
            {
                case GhostState.WalkingLeft:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.None;
                    // Walking frames are 0 & 1                                                                                                            
                    if (animationTimer.TotalMilliseconds > (FRAME_RATE * 2 - (FRAME_RATE * 0.05)))   //this slight adjustment fixes the issue of seeing a small blip of frame 11 pop up
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)animationTimer.TotalMilliseconds / FRAME_RATE;
                    break;
                case GhostState.WalkingRight:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    // Walking frames are 0 & 1                                                                                                            
                    if (animationTimer.TotalMilliseconds > (FRAME_RATE * 2 - (FRAME_RATE * 0.05)))   //this slight adjustment fixes the issue of seeing a small blip of frame 11 pop up
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)animationTimer.TotalMilliseconds / FRAME_RATE;
                    break;
            }

        }
        /*
        public void CheckForPlayerCollision(Player player)
        {
            if (Bounds.CollidesWith(player.Bounds)) // check for collisions with the player
            {
                if (player.Bounds.Y - player.Bounds.Height <= Bounds.Y)
                {
                    //logic player bouncing off head, but ghost wont be dying, can bounce off ghost heads
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
            }
        }
        */


        /// <summary>
        /// Render the ghost sprite.  Should be invoked between 
        /// SpriteBatch.Begin() and SpriteBatch.End()
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
#if Debug
            VisualDebugging.DrawRectangle(spriteBatch, Bounds, Color.Red);
#endif
            frames[currentFrame].Draw(spriteBatch, Position, color, 0, origin, 2, spriteEffects, 1);
        }
    }
}
