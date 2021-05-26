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
        public Vector2 StartPosition;
        public Vector2 position;
        public CanvasBitmap _pieceImage;

        public static CanvasBitmap Red;
        public static CanvasBitmap Blue;
        public static CanvasBitmap Yellow;
        public static CanvasBitmap Green;

        //private Vector2 _pos;

        public Piece(Vector2 startPostition, PlayerColors colors)
        {
            StartPosition = startPostition;
            position = startPostition;
            PieceColor(colors);
        }

        public void Move(Vector2 newTarget)
        {
            position = newTarget;
        }

        public void PieceColor(PlayerColors color)
        {
            switch (color)
            {
                case PlayerColors.red:
                    _pieceImage = Red;
                    break;
                case PlayerColors.blue:
                    _pieceImage = Blue;
                    break;
                case PlayerColors.yellow:
                    _pieceImage = Yellow;
                    break;
                case PlayerColors.green:
                    _pieceImage = Green;
                    break;
            }
        }

        public void Draw(CanvasAnimatedDrawEventArgs drawArgs, Rect targetRectangle)
        {
            //Rect newRect = new Rect(targetRectangle.X, targetRectangle.Y, targetRectangle.Width, targetRectangle.Height);
            //position.X = (float)targetRectangle.X;
            //position.Y = (float)targetRectangle.Y;
            drawArgs.DrawingSession.DrawImage(this._pieceImage, targetRectangle); //placeholder position value

        }
    }
}
