using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformLibrary
{
    /// <summary>
    /// A class representing a tilemap created with the Tiled editor
    /// </summary>
    public class Tilemap
    {
        #region private fields

        // An array of all tiles in the tilemap
        Tile[] tiles;
        
        // An array of all tile layers in the tilemap
        TilemapLayer[] layers;

        // The width of the map, measured in tiles
        uint mapWidth;
        
        // The height of the map, measured in tiles
        uint mapHeight;

        // The width of the tiles in the map
        uint tileWidth;

        // The height of the tiles in the map
        uint tileHeight;

        
        ObjectGroup[] objectGroups;

        public ObjectGroup[] ObjectGroups
        {
            get { return objectGroups; }
        }

        #endregion

        #region initialization

        /// <summary>
        /// Constructs a new instance of a Tilemap
        /// </summary>
        /// <param name="mapWidth">The width of the map, in tiles</param>
        /// <param name="mapHeight">The height of the map, in tiles</param>
        /// <param name="tileWidth">The width of the tiles, in pixels</param>
        /// <param name="tileHeight">The heigh of the tiles, in pixels</param>
        /// <param name="layers">The layers of the map</param>
        /// <param name="tiles">The tiles of the map</param>
        public Tilemap(uint mapWidth, uint mapHeight, uint tileWidth, uint tileHeight, TilemapLayer[] layers, Tile[] tiles, ObjectGroup[] objectGroups)
        {
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.layers = layers;
            this.tiles = tiles;
            this.objectGroups = objectGroups;
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Draws the specified layer of the tilemap
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        /// <param name="layerIndex">The index of the layer to use</param>
        public void DrawLayer(SpriteBatch spriteBatch, int layerIndex)
        {

            var layer = layers[layerIndex];
            for (uint y = 0; y < mapHeight; y++)
            {
                for (uint x = 0; x < mapWidth; x++)
                {
                    uint dataIndex = y * mapWidth + x;
                    uint tileIndex = layer.Data[dataIndex];
                    if (tileIndex != 0 && tileIndex < tiles.Length)
                    {
                        Vector2 position = new Vector2(x * tileWidth, y * tileHeight);
                        tiles[tileIndex].Draw(spriteBatch, position, Color.White);
                    }
                }
            }
        }

        /// <summary>
        /// Draws the entire Tilemap
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var layer in layers)
            {
                for (uint y = 0; y < mapHeight; y++)
                {
                    for (uint x = 0; x < mapWidth; x++)
                    {
                        uint dataIndex = y * mapWidth + x;
                        uint tileIndex = layer.Data[dataIndex];
                        if (tileIndex != 0 && tileIndex < tiles.Length)
                        {
                            Tile tile = tiles[tileIndex];
                            Vector2 position = new Vector2(x * tileWidth, y * tileHeight);
                            tiles[tileIndex].Draw(spriteBatch, position, Color.White);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
