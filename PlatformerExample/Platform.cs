using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformLibrary;

namespace PlatformerExample
{
    /// <summary>
    /// A class representing a platform
    /// </summary>
    public class Platform : IBoundable
    {
        /// <summary>
        /// The platform's bounds
        /// </summary>
        BoundingRectangle bounds;

        /// <summary>
        /// The platform's sprite
        /// </summary>
        Sprite sprite;

        /// <summary>
        /// The number of times the sprite is repeated (tiled) in the platform
        /// </summary>
        int tileCount;

        /// <summary>
        /// The bounding rectangle of the 
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// Constructs a new platform
        /// </summary>
        /// <param name="bounds">The platform's bounds</param>
        /// <param name="sprite">The platform's sprite</param>
        public Platform(BoundingRectangle bounds, Sprite sprite)
        {
            this.bounds = bounds;
            this.sprite = sprite;
            tileCount = (int)bounds.Width / sprite.Width;
        }

        /// <summary>
        /// Draws the platform
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch to render to</param>
        public void Draw(SpriteBatch spriteBatch)
        {
#if VISUAL_DEBUG
            VisualDebugging.DrawRectangle(spriteBatch, bounds, Color.Green);
#endif
            for (int i = 0; i < tileCount; i++)
            {
                sprite.Draw(spriteBatch, new Vector2(bounds.X + i * sprite.Width, bounds.Y), Color.Purple);
            }

        }
    }
}
