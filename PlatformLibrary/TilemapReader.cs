using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TRead = PlatformLibrary.Tilemap;

namespace PlatformLibrary
{
    public class TilemapReader : ContentTypeReader<TRead>
    {
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            // Read in the content properties in the exact same 
            // order they were written in the corresponding writer
            
            // readthe map width & height 
            var mapWidth = input.ReadUInt32();
            var mapHeight = input.ReadUInt32();

            // Read the tile width & height
            var tileWidth = input.ReadUInt32();
            var tileHeight = input.ReadUInt32();

            // Read the layer count and create collections to hold layers
            var layerCount = input.ReadInt32();
            var layers = new TilemapLayer[layerCount];
            

            // Read the layer data
            for (var i = 0; i < layerCount; i++)
            {
                //var layerId = input.ReadUInt32();
                //var layerName = input.ReadString();
                var tileCount = input.ReadUInt32();
                var layerData = new uint[tileCount];
                for (uint j = 0; j < tileCount; j++)
                {
                    layerData[j] = input.ReadUInt32();
                }
                layers[i] = new TilemapLayer(layerData);
                //layersById[layerId] = layers[i];
                //layersByName[layerName] = layers[i];
            }

            // Read the tileset data
            var tiles = new List<Tile>() { new Tile() };
            var tilesetCount = input.ReadInt32();
            for(var i = 0; i < tilesetCount; i++)
            {
                var nextGUID = input.ReadInt32();
                if (nextGUID != tiles.Count) throw new Exception("Tileset GUIDs do not match");
                var tileset = input.ReadExternalReference<Tileset>();
                // Create the tiles from the tileset 
                for (int j = 0; j < tileset.Count; j++)
                {
                    tiles.Add(tileset[j]);
                }
            }

            // Read the ObjectGroup data
            var objectGroups = new List<ObjectGroup>();
            var objectGroupsCount = input.ReadUInt32();
            for (var i = 0; i < objectGroupsCount; i++)
            {
                var name = input.ReadString();
                var groupObjects = new List<GroupObject>();
                var groupObjectsCount = input.ReadUInt32();
                for (var j = 0; j < groupObjectsCount; j++)
                {
                    var sheetIndex = input.ReadInt32();
                    var x = input.ReadUInt32();
                    var y = input.ReadUInt32();
                    var width = input.ReadUInt32();
                    var height = input.ReadUInt32();
                                                       
                    groupObjects.Add(new GroupObject(sheetIndex, x, y, width, height));
                }
                objectGroups.Add(new ObjectGroup(name, groupObjects.ToArray()));
            }

            // Construct and return the tilemap
            return new Tilemap(mapWidth, mapHeight, tileWidth, tileHeight, layers, tiles.ToArray(), objectGroups.ToArray());
        }
    }
}
