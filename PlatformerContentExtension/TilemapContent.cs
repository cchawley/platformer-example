using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace PlatformerContentExtension
{
    public class TilemapTileset
    {
        public int FirstGID;

        public string Source;

        public ExternalReference<TilesetContent> Reference;
    }

    public class TilemapContent
    {
        public uint MapWidth { get; set; }

        public uint MapHeight { get; set; }

        public uint TileWidth { get; set; }

        public uint TileHeight { get; set; }

        public List<TilemapTileset> Tilesets = new List<TilemapTileset>();

        public List<TilemapLayerContent> Layers = new List<TilemapLayerContent>();

        public List<TileMapObjects> AllObjects = new List<TileMapObjects>();
    }
}
