using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace LudoLike
{
    /// <summary>
    /// Represents a Ludo piece on the board.
    /// </summary>
    public class Piece
    {
        public Vector2 StartPosition;
        public Vector2 position;
        public CanvasBitmap _pieceImage;
        public bool HoverOver = false;

        public static CanvasBitmap Red;
        public static CanvasBitmap Blue;
        public static CanvasBitmap Yellow;
        public static CanvasBitmap Green;

        public Piece(Vector2 startPostition, PlayerColors colors)
        {
            StartPosition = startPostition;
            position = startPostition;
            PieceColor(colors);
        }

        public void Move(Vector2 newTarget)
        {
            Thread.Sleep(272);
            position = newTarget;
        }

        public void PieceColor(PlayerColors color)
        {
            switch (color)
            {
                case PlayerColors.Red:
                    _pieceImage = Red;
                    break;
                case PlayerColors.Blue:
                    _pieceImage = Blue;
                    break;
                case PlayerColors.Yellow:
                    _pieceImage = Yellow;
                    break;
                case PlayerColors.Green:
                    _pieceImage = Green;
                    break;
            }
        }

        public void Draw(CanvasAnimatedDrawEventArgs drawArgs, Rect targetRectangle)
        { 
            drawArgs.DrawingSession.DrawImage(_pieceImage, targetRectangle);

            //drawArgs.DrawingSession.DrawImage(PlayableEffect, targetRectangle);
        }

        /// <summary>
        /// Draws the piece aswell as a hovereffect on the underlying grid.
        /// </summary>
        /// <param name="drawArgs"></param>
        /// <param name="targetRectangle"></param>
        /// <param name="effectColor"></param>
        /// <param name="opacity"></param>
        public void Draw(CanvasAnimatedDrawEventArgs drawArgs, Rect targetRectangle, Windows.UI.Color effectColor, float opacity)
        {
            ColorSourceEffect effect = new ColorSourceEffect()
            {
                Color = effectColor
            };
            drawArgs.DrawingSession.DrawImage(effect, (float)targetRectangle.X, (float)targetRectangle.Y, targetRectangle, opacity);
            drawArgs.DrawingSession.DrawImage(_pieceImage, targetRectangle);
        }

    }
}
