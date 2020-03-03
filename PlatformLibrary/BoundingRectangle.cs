using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PlatformLibrary
{
    public struct BoundingRectangle : IBoundable
    {
        /// <summary>
        /// The x coordinate of the upper-left corner of the rectangle
        /// </summary>
        public float X;

        /// <summary>
        /// The y cooridnate of the upper-left corner of the rectangle
        /// </summary>
        public float Y;

        /// <summary>
        /// The width of the rectangle
        /// </summary>
        public float Width;

        /// <summary>
        /// The height of the rectangle
        /// </summary>
        public float Height;

        /// <summary>
        /// Gets this bounding rectangle
        /// </summary>
        public BoundingRectangle Bounds => this;

        /// <summary>
        /// Constructs a new bounding rectangle.  This class is similar to a rectangle,
        /// but uses floats
        /// </summary>
        /// <param name="x">The x coordinate of the upper-left corner</param>
        /// <param name="y">The y cooridnate of the upper-left corner</param>
        /// <param name="width">The width of the rectangle</param>
        /// <param name="height">The height of the rectangle</param>
        public BoundingRectangle(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Constructs a new bounding rectangle.  This class is similar to a rectangle,
        /// but uses floats
        /// </summary>
        /// <param name="position">The upper-left corner's position</param>
        /// <param name="width">The width of the rectangle</param>
        /// <param name="height">The height of the rectangle</param>
        public BoundingRectangle(Vector2 position, float width, float height)
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
        }


        /// <summary>
        /// Detects collisions between this and another bounding rectangle
        /// </summary>
        /// <param name="other">The other bounding rectangle</param>
        /// <returns>True if a collision is detected, false otherwise</returns>
        public bool CollidesWith(BoundingRectangle other) 
        {
            return !(this.X > other.X + other.Width
                  || this.X + this.Width < other.X
                  || this.Y > other.Y + other.Height
                  || this.Y + this.Height < other.Y);
        }

        /// <summary>
        /// Cast operator for casting into a Rectangle.  
        /// Useful for sprite rendering when the sprite's bounds 
        /// and the size of the sprite are the same
        /// </summary>
        /// <param name="br">The bounding rectangle to cast</param>
        public static implicit operator Rectangle(BoundingRectangle br)
        {
            return new Rectangle(
                (int)br.X,
                (int)br.Y,
                (int)br.Width,
                (int)br.Height);
        }
    }   

}
