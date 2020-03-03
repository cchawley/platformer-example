using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = PlatformerContentExtension.TilesetContent;

namespace PlatformerContentExtension
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentImporter attribute to specify the correct file
    /// extension, display name, and default processor for this importer.
    /// </summary>

    [ContentImporter(".tsx", DisplayName = "TSX Importer - Tiled", DefaultProcessor = "TilesetProcessor - Tiled")]
    public class TilesetImporter : ContentImporter<TInput>
    {

        public override TInput Import(string filename, ContentImporterContext context)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename);

            // The tileset should be the tileset tag
            XmlNode tileset = document.SelectSingleNode("//tileset");

            // The attributes on the tileset are the properties of our spritesheet
            string name = tileset.Attributes["name"].Value;
            int tileWidth = int.Parse(tileset.Attributes["tilewidth"].Value);
            int tileHeight = int.Parse(tileset.Attributes["tileheight"].Value);
            int spacing = int.Parse(tileset.Attributes["spacing"].Value);
            int margin = int.Parse(tileset.Attributes["margin"].Value);
            int tileCount = int.Parse(tileset.Attributes["tilecount"].Value);
            int columns = int.Parse(tileset.Attributes["columns"].Value);

            // A tileset will contain an image element that serves as the source of the tiles
            XmlNodeList images = tileset.SelectNodes("//image");
            var imageFilename = images[0].Attributes["source"].Value;
            var imageColorKey = images[0].Attributes["trans"].Value;

            // TODO: Import any custom tile properties
            
            // Create and return the TileContent
            return new TilesetContent()
            {
                Name = name,
                TileWidth = tileWidth,
                TileHeight = tileHeight,
                Spacing = spacing,
                Margin = margin,
                TileCount = tileCount,
                Columns = columns,
                ImageFilename = imageFilename,
                ImageColorKey = imageColorKey
            };
        }

    }

}
