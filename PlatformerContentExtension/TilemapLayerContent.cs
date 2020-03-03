using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerContentExtension
{
    public class TilemapLayerContent
    {
        public uint Id { get; set; }
        
        public string Name { get; set; }

        public uint Width { get; set; }

        public uint Height { get; set; }

        public string DataString { get; set; }

        public uint[] Data { get; set; }
    }
}
