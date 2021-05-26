using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoLike
{
    class Piece
    {
        public Tile position;
        private CanvasBitmap _pieceImage;

        public static CanvasBitmap Red;
        public static CanvasBitmap Blue;
        public static CanvasBitmap Yellow;
        public static CanvasBitmap Green;

        private float xpos = 20.0f;
        private float ypos = 20.0f;


        public Piece(Tile startPostition, PlayerColors colors)
        {
            position = startPostition;
            PieceColor(colors);
        }

        public void Move(float targetX, float targetY)
        {
            float xDiff = targetX - xpos;
            float yDiff = targetY - ypos;
            for (float i = xDiff; i > 0.0f; i--)
            {
                xpos += 1f;
                ypos += 1f;
            }
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

        public void Draw(CanvasAnimatedDrawEventArgs drawArgs)
        {
            drawArgs.DrawingSession.DrawImage(_pieceImage, xpos, ypos); //placeholder position value

        }
    }
}
