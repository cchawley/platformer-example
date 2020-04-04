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
    /// Interface representing a sprite to be drawn with a SpriteBatch
    /// </summary>
    public interface ISprite
    {
        /// <summary>
        /// Draws the ISprite.  This method should be invoked between calls to 
        /// SpriteBatch.Begin() and SpriteBatch.End() with the supplied SpriteBatch
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        /// <param name="gameTime">The GameTime object</param>
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);

    }
}
