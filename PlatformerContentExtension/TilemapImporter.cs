using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = PlatformerContentExtension.TilemapContent;

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

    [ContentImporter(".tmx", DisplayName = "TMX Importer - Tiled", DefaultProcessor = "TilemapProcessor")]
    public class TilemapImporter : ContentImporter<TInput>
    {

        public override TInput Import(string filename, ContentImporterContext context)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename);

            // The tilemap should be the tilemap tag
            XmlNode map = document.SelectSingleNode("//map");

            // The attributes on the tilemap 
            uint mapWidth = uint.Parse(map.Attributes["width"].Value);
            uint mapHeight = uint.Parse(map.Attributes["height"].Value);
            uint tileWidth = uint.Parse(map.Attributes["tilewidth"].Value);
            uint tileHeight = uint.Parse(map.Attributes["tileheight"].Value);

            // Construct the TilemapContent
            var output = new TilemapContent()
            {
                MapWidth = mapWidth,
                MapHeight = mapHeight,
                TileWidth =  tileWidth,
                TileHeight = tileHeight,
            };

            // A tilemap will have one or more tilesets 
            XmlNodeList tilesets = map.SelectNodes("//tileset");
            foreach(XmlNode tileset in tilesets)
            {
                output.Tilesets.Add(new TilemapTileset()
                {
                    FirstGID = int.Parse(tileset.Attributes["firstgid"].Value),
                    Source = tileset.Attributes["source"].Value
                });
            }

            // A tilemap will have one or more layers
            XmlNodeList layers = map.SelectNodes("//layer");
            foreach(XmlNode layer in layers)
            {
                var id = uint.Parse(layer.Attributes["id"].Value);
                var name = layer.Attributes["name"].Value;
                var width = uint.Parse(layer.Attributes["width"].Value);
                var height = uint.Parse(layer.Attributes["height"].Value);

                // A tilemap layer will have a data element
                XmlNode data = layer.SelectSingleNode("//data");
                if (data.Attributes["encoding"].Value != "csv") throw new NotSupportedException("Only csv encoding is supported");
                var dataString = data.InnerText;

                output.Layers.Add(new TilemapLayerContent()
                {
                    Id = id,
                    Name = name,
                    Width = width,
                    Height = height,
                    DataString = dataString
                });
            }

            //used to get nodes of objects
            XmlNodeList objectGroups = map.SelectNodes("//objectgroup");
            foreach (XmlNode objectGroup in objectGroups)
            {
                //get the id and name of the objectgroup
                var id = uint.Parse(objectGroup.Attributes["id"].Value);
                var name = objectGroup.Attributes["name"].Value;

                //Create new TilemapObjectGroupContent object
                TileMapObjects objectGroupContent = new TileMapObjects();
                objectGroupContent.Id = id;
                objectGroupContent.Name = name;

                //Get all of the object children of the objectgroup
                XmlNodeList groupObjects = objectGroup.ChildNodes;
                foreach (XmlNode groupObject in groupObjects)
                {
                    //Bring in all of the info for the object
                    var sheetIndex = int.Parse(groupObject.Attributes["gid"].Value);
                    var x = uint.Parse(groupObject.Attributes["x"].Value);
                    var y = uint.Parse(groupObject.Attributes["y"].Value);
                    var width = uint.Parse(groupObject.Attributes["width"].Value);
                    var height = uint.Parse(groupObject.Attributes["height"].Value);

                    //Add the object to its objectgroup
                    objectGroupContent.Objects.Add(new ObjectGroup()
                    {
                        SheetIndex = sheetIndex,
                        X = x,
                        Y = y,
                        Width = width,
                        Height = height
                    });
                }
                //Add the objectgroup to the TileMapContent
                output.AllObjects.Add(objectGroupContent);
            }

            return output;
        }
    }
}
