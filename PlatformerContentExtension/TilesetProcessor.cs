using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

using TInput = PlatformerContentExtension.TilesetContent;
using TOutput = PlatformerContentExtension.TilesetContent;

namespace PlatformerContentExtension
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to tileset data 
    /// </summary>
    [ContentProcessor(DisplayName = "Tileset Processor - Tiled")]
    public class TilesetProcessor : ContentProcessor<TInput, TOutput>
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
            // The image is a separate file - we need to create an external reference to represent it
            // Then we can load that file through the content pipeline and embed it into the TilesetContent
            ExternalReference<TextureContent> externalRef = new ExternalReference<TextureContent>(input.ImageFilename);
            OpaqueDataDictionary options = new OpaqueDataDictionary();
            if (input.ImageColorKey != null)
            {
                // The color key from the TSX file is in hexidecimal string format,
                // We need to convert it into an XNA Color
                int rgb = int.Parse(input.ImageColorKey, System.Globalization.NumberStyles.HexNumber);
                int r = (rgb & 0xff0000) >> 16;
                int g = (rgb & 0xff00) >> 8;
                int b = (rgb & 0xff);
                options.Add("ColorKeyColor", new Color(r, g, b));
            }
            input.Texture = context.BuildAndLoadAsset<TextureContent, TextureContent>(externalRef, "TextureProcessor", options, "TextureImporter");

            // Create the Tiles array
            input.Tiles = new TileContent[input.TileCount];

            // Run the logic to generate the individual tile source rectangles
            for (int i = 0; i < input.TileCount; i++)
            {
                var source = new Rectangle(
                        (i % input.Columns) * (input.TileWidth + input.Spacing) + input.Margin, // x coordinate 
                        (i / input.Columns) * (input.TileHeight + input.Spacing) + input.Margin, // y coordinate
                        input.TileWidth,
                        input.TileHeight
                        );
                input.Tiles[i] = new TileContent(source);
            }

            // The tileset has been processed
            return input;
        }
    }
}