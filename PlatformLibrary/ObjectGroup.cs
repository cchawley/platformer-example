using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformLibrary
{
    public class ObjectGroup
    {
        public string Name;
        public GroupObject[] Objects;
        public int TileCount => Objects.Length;
        public ObjectGroup(string name, GroupObject[] objects)
        {
            Name = name;
            Objects = objects;
        }
    }
}
