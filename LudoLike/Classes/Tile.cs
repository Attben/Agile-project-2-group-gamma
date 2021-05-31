using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace LudoLike
{
    /// <summary>
    /// Represents a tile which the player pieces can move to.
    /// </summary>
    public class Tile
    {
        public static Dictionary<string, CanvasBitmap> TileImages = new Dictionary<string, CanvasBitmap>();
        public CanvasBitmap TileImage;
        public Rect TargetRectangle;
        public Vector2 GridPosition;

        /// <summary>
        /// Use this constructor when you want to override the regular TileImage.
        /// </summary>
        /// <param name="targetRectangle"></param>
        /// <param name="tileImage"></param>
        /// <param name="gridPosition"></param>
        public Tile(Rect targetRectangle, CanvasBitmap tileImage, Vector2 gridPosition)
        {
            TileImage = tileImage;
            TargetRectangle = targetRectangle;
            GridPosition = gridPosition;
        }

        public Tile(Rect targetRectangle, Vector2 gridPosition)
        {
            TargetRectangle = targetRectangle;
            GridPosition = gridPosition;
        }

        /// <summary>
        /// Activates the function of the tile. 
        /// </summary>
        /// <returns>
        /// true if the tile is supposed to disappear when activated.
        /// false if the tile is supposed to be kept on the board.
        /// </returns>
        public virtual bool TileEvent()
        {
            //Intentionally left blank in base class.
            return false;
        }

        /// <summary>
        /// Activates the function of the tile. 
        /// </summary>
        /// <returns>
        /// <code>true</code> if the tile is supposed to disappear when activated.
        /// <code>false</code> if the tile is supposed to be kept on the board.
        /// </returns>
        public virtual bool TileEvent(Player player)
        {
            //Intentionally left blank in base class.
            return false;
        }


        /// <summary>
        /// Draws the image representing the tile onto its target rectangle in the Ludo board grid.
        /// <para></para>
        /// Standard drawing image is the regular grey.
        /// </summary>
        /// <param name="drawArgs"></param>
        public virtual void Draw(CanvasAnimatedDrawEventArgs drawArgs)
        {
            drawArgs.DrawingSession.DrawImage(TileImages["Regular"], TargetRectangle);
        }
    }
}
