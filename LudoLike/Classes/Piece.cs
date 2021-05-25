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
    public class Piece
    {
        public Vector2 startPosition;
        public Vector2 position;
        private CanvasBitmap _pieceImage;

        public static CanvasBitmap Red;
        public static CanvasBitmap Blue;
        public static CanvasBitmap Yellow;
        public static CanvasBitmap Green;


        public Piece(Vector2 startPostition, PlayerColors colors)
        {
            startPosition = startPostition;
            position = startPosition;
            PieceColor(colors);
        }

        public void Move()
        {
            //NYI
        }

        public void PieceColor(PlayerColors color)
        {
            switch (color)
            {
                case 0:
                    _pieceImage = Red;
                    break;
                case (PlayerColors)1:
                    _pieceImage = Green;
                    break;
                case (PlayerColors)2:
                    _pieceImage = Blue;
                    break;
                case (PlayerColors)3:
                    _pieceImage = Yellow;
                    break;
            }
        }

        public void Draw(CanvasAnimatedDrawEventArgs drawArgs, Rect targetRectangle)
        {
            drawArgs.DrawingSession.DrawImage(_pieceImage, targetRectangle); //placeholder position value

        }
    }
}
