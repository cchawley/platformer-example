using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerExample
{
    /// <summary>
    /// A controller that scrolls a parallax layer at a set speed
    /// </summary>
    public class AutoScrollController : IScrollController
    {
        /// <summary>
        /// The time that has elapsed
        /// </summary>
        float elapsedTime = 0;

        /// <summary>
        /// The speed at which the layer should scroll
        /// </summary>
        public float Speed = 10f;

        /// <summary>
        /// Gets the current tansformation matrix
        /// </summary>
        public Matrix Transform
        {
            get
            {
                return Matrix.CreateTranslation(-elapsedTime * Speed, 0, 0);
            }
        }

        /// <summary>
        /// Updates the controller
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        public void Update(GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}

