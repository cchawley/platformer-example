using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformLibrary
{
    /// <summary>
    /// A class representing a single tile from a tilesheet
    /// </summary>
    public struct Tile 
    {
        #region Fields

        // The tile's source rectangle
        private Rectangle source;

        // The tile's texture
        private Texture2D texture;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the tile's width
        /// </summary>
        public int Width => source.Width;

        /// <summary>
        /// Gets the tile's height
        /// </summary>
        public int Height => source.Height;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructs a new tile
        /// </summary>
        /// <param name="source"></param>
        /// <param name="texture"></param>
        public Tile(Rectangle source, Texture2D texture)
        {
            this.texture = texture;
            this.source = source;
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Draws the tile using the provided SpriteBatch 
        /// This method should should be called between 
        /// SpriteBatch.Begin() and SpriteBatch.End()
        /// </summary>
        /// <param name="SpriteBatch">The SpriteBatch</param>
        /// <param name="destinationRectangle">The rectangle to draw the tile into</param>
        /// <param name="color">The color</param>
        /// <param name="rotation">Rotation about the origin (in radians)</param>
        /// <param name="origin">A vector2 to the origin</param>
        /// <param name="effects">The SpriteEffects</param>
        /// <param name="layerDepth">The sorting layer of the tile</param>
        public void Draw(SpriteBatch SpriteBatch, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            
            
                SpriteBatch.Draw(texture, destinationRectangle, source, color, rotation, origin, effects, layerDepth);
            
            
        }

        /// <summary>
        /// Draws the tile using the provided SpriteBatch 
        /// This method should should be called between 
        /// SpriteBatch.Begin() and SpriteBatch.End()
        /// </summary>
        /// <param name="SpriteBatch">The SpriteBatch</param>
        /// <param name="destinationRectangle">The rectangle to draw the tile into</param>
        /// <param name="color">The color</param>
        public void Draw(SpriteBatch SpriteBatch, Rectangle destinationRectangle, Color color)
        {
            
            
                SpriteBatch.Draw(texture, destinationRectangle, source, color);
            
        }

        /// <summary>
        /// Draws the tile using the provided SpriteBatch 
        /// This method should should be called between 
        /// SpriteBatch.Begin() and SpriteBatch.End()
        /// </summary>
        /// <param name="SpriteBatch">The SpriteBatch</param>
        /// <param name="position">A Vector2 for position</param>
        /// <param name="color">The color</param>
        public void Draw(SpriteBatch SpriteBatch, Vector2 position, Color color)
        {
          
            
                SpriteBatch.Draw(texture, position, source, color);
            
        }

        /// <summary>
        /// Draws the tile using the provided SpriteBatch 
        /// This method should should be called between 
        /// SpriteBatch.Begin() and SpriteBatch.End()
        /// </summary>
        /// <param name="SpriteBatch">The SpriteBatch</param>
        /// <param name="position">A Vector2 for position</param>
        /// <param name="color">The color</param>
        /// <param name="rotation">Rotation about the origin (in radians)</param>
        /// <param name="origin">A vector2 to the origin</param>
        /// <param name="scale">The scale of the tile centered on the origin</param>
        /// <param name="effects">The SpriteEffects</param>
        /// <param name="layerDepth">The sorting layer of the tile</param>
        public void Draw(SpriteBatch SpriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            
            
                SpriteBatch.Draw(texture, position, source, color, rotation, origin, scale, effects, layerDepth);
            
        }

        #endregion
    }
}