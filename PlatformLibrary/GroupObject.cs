using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformLibrary
{
    /// <summary>
    /// class for holding a object in a objectGroup
    /// </summary>
    public class GroupObject
    {
        public uint Width;
        public uint Height;    
        public uint X;
        public uint Y;
        public int SheetIndex;

        public GroupObject(int sheetIndex, uint x, uint y, uint width, uint height)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;           
            SheetIndex = sheetIndex;
        }
    }
}
