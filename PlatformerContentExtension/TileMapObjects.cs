using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerContentExtension
{
    public class ObjectGroup
    {
        public int SheetIndex { get; set; }

        public uint X { get; set; }

        public uint Y { get; set; }

        public uint Width { get; set; }

        public uint Height { get; set; }
    }

    public class TileMapObjects
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public uint Width { get; set; }

        public List<ObjectGroup> Objects = new List<ObjectGroup>();
    }
}
