using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

using TInput = PlatformerContentExtension.TilemapContent;
using TOutput = PlatformerContentExtension.TilemapContent;

namespace PlatformerContentExtension
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to tileset data 
    /// </summary>
    [ContentProcessor(DisplayName = "Tilemap Processor - Tiled")]
    public class TilemapProcessor : ContentProcessor<TInput, TOutput>
    {
        /// <summary>
        /// Processes the raw .tsx XML and creates a TilesetContent
        /// for use in an XNA framework game
        /// </summary>
        /// <param name="input">The XML string</param>
        /// <param name="context">The pipeline context</param>
        /// <returns>A TilesetContent instance corresponding to the tsx input</returns>
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            // Process the Tilesets
            for(int i =0; i < input.Tilesets.Count; i++)
            {
                var tileset = input.Tilesets[i];
                ExternalReference<TilesetContent> externalRef = new ExternalReference<TilesetContent>(tileset.Source);
                tileset.Reference = context.BuildAsset<TilesetContent, TilesetContent>(externalRef, "TilesetProcessor");
            }

            //correction for orgin position
            foreach (TileMapObjects objectGroup in input.AllObjects)
            {
                foreach (ObjectGroup groupObject in objectGroup.Objects)
                {
                    groupObject.Y -= input.TileHeight;
                }
            }

            foreach (TilemapLayerContent layer in input.Layers)
            {
                List<uint> dataIds = new List<uint>();
                foreach (var id in layer.DataString.Split(',')) 
                {
                    dataIds.Add(uint.Parse(id));
                };
                layer.Data = dataIds.ToArray();
            }

            // The tileset has been processed
            return input;
        }
    }
}