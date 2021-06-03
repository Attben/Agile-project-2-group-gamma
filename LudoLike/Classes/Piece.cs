using Microsoft.Graphics.Canvas;
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
    public class Piece
    {
        public Vector2 StartPosition;
        public Vector2 position;
        public CanvasBitmap _pieceImage;

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
            drawArgs.DrawingSession.DrawImage(this._pieceImage, targetRectangle);
        }
    }
}
