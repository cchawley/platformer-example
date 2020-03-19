using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using TWrite = PlatformerContentExtension.TilemapContent;

namespace PlatformerContentExtension
{
    /// <summary>
    /// A ContentTypeWriter for the TiledSpriteSheetContent type
    /// </summary>
    [ContentTypeWriter]
    public class TilemapWriter : ContentTypeWriter<TWrite>
    {
        
        /// <summary>
        /// Write the binary (xnb) file corresponding to the supplied 
        /// TilesetContent that will be imported into our game
        /// as a Tileset
        /// </summary>
        /// <param name="output">The ContentWriter that writes the binary output</param>
        /// <param name="value">The TilesetContent we are writing</param>
        protected override void Write(ContentWriter output, TWrite value)
        {
            // We only need to write the data that is needed in-game.  

            // Write the map width & height 
            output.Write(value.MapWidth);
            output.Write(value.MapHeight);

            // Write the tile width & height
            output.Write(value.TileWidth);
            output.Write(value.TileHeight);

            // Write the layer count 
            output.Write(value.Layers.Count);

            // Write the layer data
            foreach (var layer in value.Layers) 
            {
                // Write the layer single-value data
                output.Write(layer.Data.Length);
                foreach(uint id in layer.Data)
                {
                    output.Write(id);
                }
            }

            // Write the tileset data 
            output.Write(value.Tilesets.Count);
            foreach(var tileset in value.Tilesets)
            {
                output.Write(tileset.FirstGID);
                output.WriteExternalReference(tileset.Reference);
            }

            //Used to write out information regarding the objects
            output.Write(value.AllObjects.Count);
            foreach (var group in value.AllObjects)
            {             
                output.Write(group.Name);
                output.Write(group.Objects.Count);
                foreach (var Object in group.Objects)
                {                 
                    output.Write(Object.SheetIndex);
                    output.Write(Object.X);
                    output.Write(Object.Y);
                    output.Write(Object.Width);
                    output.Write(Object.Height);
                }
            }
        }

        /// <summary>
        /// Gets the reader needed to read the binary content written by this writer
        /// </summary>
        /// <param name="targetPlatform"></param>
        /// <returns></returns>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "PlatformLibrary.TilemapReader, PlatformLibrary";
        }
    }
}
