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
using Windows.UI;

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

        public PlayerColors PieceColor;
        // This is for seeing what Piece is chosen
        public bool Clicked;
        public Vector2? AllowedDestinationTileVector;
        public Piece(Vector2 startPostition, PlayerColors colors)
        {
            StartPosition = startPostition;
            position = startPostition;
            SetPieceColor(colors);
        }

        public void Move(Vector2 newTarget)
        {
            Thread.Sleep(272);
            position = newTarget;
        }

        public void SetPieceColor(PlayerColors color)
        {
            PieceColor = color;
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
        }

        /// <summary>
        /// Draws the piece aswell as a hovereffect on the underlying grid.
        /// </summary>
        /// <param name="drawArgs"></param>
        /// <param name="targetRectangle"></param>
        /// <param name="effectColor"></param>
        /// <param name="effectPlacement"></param>
        /// <param name="opacity"></param>
        public void Draw(CanvasAnimatedDrawEventArgs drawArgs, Rect targetRectangle, Color effectColor, string effectPlacement)
        {
            switch (effectPlacement)
            {
                case "Behind":
                    if (Clicked)
                    {
                        AnimationHandler.DrawBlinkAnimation(drawArgs, targetRectangle, effectColor, EffectSize.Medium);
                        AnimationHandler.DrawBlinkAnimation(drawArgs, LudoBoard.TileGridPositions[AllowedDestinationTileVector.Value], Colors.Yellow, EffectSize.Medium);
                    }
                    else
                    {
                        AnimationHandler.DrawBlinkAnimation(drawArgs, targetRectangle, effectColor, EffectSize.Small);
                    }
                    drawArgs.DrawingSession.DrawImage(_pieceImage, targetRectangle);
                    break;
                case "In front":
                    drawArgs.DrawingSession.DrawImage(_pieceImage, targetRectangle);
                    AnimationHandler.DrawBlinkAnimation(drawArgs, targetRectangle, effectColor, EffectSize.Small);
                    break;

                default:
                    break;
            }
        }

        public void SetClicked()
        {
            Clicked = true;
            try
            {
                if (position != StartPosition)
                {
                    int pathPosition = LudoBoard.PlayerPaths[(int)PieceColor].IndexOf(position);
                    AllowedDestinationTileVector = LudoBoard.PlayerPaths[(int)PieceColor][pathPosition + Game.CurrentDiceRoll.Value];   // Take off one value from the dice cast
                }
                else
                {
                    AllowedDestinationTileVector = LudoBoard.PlayerPaths[(int)PieceColor][Game.CurrentDiceRoll.Value - 1];
                }
            }
            catch
            {
                AllowedDestinationTileVector = LudoBoard.PlayerPaths[(int)PieceColor][LudoBoard.PlayerPaths[(int)PieceColor].Count - 1];
            }
        }

        public void RemoveClicked()
        {
            Clicked = false;
            AllowedDestinationTileVector = null;
        }
    }
}
