using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformerExample
{
    /// <summary>
    /// A class representing a texture to render with a SpriteBatch
    /// </summary>
    public class StaticSprite : ISprite
    {
        /// <summary>
        /// The sprite's position in the game world
        /// </summary>
        public Vector2 position = Vector2.Zero;

        /// <summary>
        /// The texture this sprite uses
        /// </summary>
        Texture2D texture;

        /// <summary>
        /// Creates a new static sprite
        /// </summary>
        /// <param name="texture">the texture to use</param>     
        public StaticSprite(Texture2D texture)
        {
            this.texture = texture;
        }

        /// <summary>
        /// Creates a new static sprite
        /// </summary>
        /// <param name="texture">the texture to use</param>
        /// <param name="position">the upper-left hand corner of the sprite</param>
        public StaticSprite(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        /// <summary>
        /// Draws the sprite using the provided SpriteBatch.  This
        /// method should be invoked between SpriteBatch.Begin() 
        /// and SpriteBatch.End() calls.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
