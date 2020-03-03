using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformLibrary
{
    /// <summary>
    /// An interface defining game objects with bounds
    /// </summary>
    public interface IBoundable
    {
        BoundingRectangle Bounds { get; }
    }
}
