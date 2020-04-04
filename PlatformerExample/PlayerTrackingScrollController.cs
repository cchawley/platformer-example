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
    /// A parallax scroll controller that tracks a player's position.
    /// </summary>
    public class PlayerTrackingScrollController : IScrollController
    {
        /// <summary>
        /// The Player to track
        /// </summary>
        Player player;

        /// <summary>
        /// How much the parallax layer should scroll relative to the player position
        /// Should probably be a number between 0 and 1, corresponding to 0% to 100%.
        /// </summary>
        public float ScrollRatio = 1.0f;

        /// <summary>
        /// The offset between the scrolling layer and the player. Space between the players posistion and the edge of the screen. Helicopter at <200, 200>, so using 200
        /// </summary>
        public float Offset = 200;

        /// <summary>
        /// Gets the transformation matrix to use with the layer
        /// </summary>
        public Matrix Transform
        {
            get
            {
                float x = ScrollRatio * (Offset - player.Position.X);
                return Matrix.CreateTranslation(x, 0, 0);
            }
        }

        // <summary>
        /// Updates the controller (a no-op in this case)
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) { }

        /// <summary>
        /// Constructs a new PlayerTrackingScrollController
        /// </summary>
        /// <param name="player">The player to track</param>
        /// <param name="ratio">The scroll ratio for the controlled layer</param>
        public PlayerTrackingScrollController(Player player, float ratio)
        {
            this.player = player;
            this.ScrollRatio = ratio;
        }
    }
}
