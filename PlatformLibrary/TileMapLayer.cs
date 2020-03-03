using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformLibrary
{
    public class TilemapLayer
    {
        public uint[] Data;

        public int TileCount => Data.Length;

        public TilemapLayer(uint[] data)
        {
            Data = data;
        }


    }
}
