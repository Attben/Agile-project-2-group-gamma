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
        public Vector2 Position;
        public CanvasBitmap PieceImage;
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
            Position = startPostition;
            SetPieceColor(colors);
        }
        /// <summary>
        /// Updates piece position when moving.
        /// </summary>
        /// <param name="newTarget"></param>
        public void UpdatePosition(Vector2 newTarget)
        {
            Thread.Sleep(272);
            Position = newTarget;
        }
        /// <summary>
        /// Loads the correct image based on what colour the piece has.
        /// </summary>
        /// <param name="color"></param>
        public void SetPieceColor(PlayerColors color)
        {
            PieceColor = color;
            switch (color)
            {
                case PlayerColors.Red:
                    PieceImage = Red;
                    break;
                case PlayerColors.Blue:
                    PieceImage = Blue;
                    break;
                case PlayerColors.Yellow:
                    PieceImage = Yellow;
                    break;
                case PlayerColors.Green:
                    PieceImage = Green;
                    break;
            }
        }
        /// <summary>
        /// Draws the piece
        /// </summary>
        /// <param name="drawArgs"></param>
        /// <param name="targetRectangle"></param>
        public void Draw(CanvasAnimatedDrawEventArgs drawArgs, Rect targetRectangle)
        { 
            drawArgs.DrawingSession.DrawImage(PieceImage, targetRectangle);
        }

        /// <summary>
        /// Draws the piece aswell as a hovereffect on the underlying grid.
        /// </summary>
        /// <param name="drawArgs"></param>
        /// <param name="targetRectangle"></param>
        /// <param name="effectColor"></param>
        /// <param name="effectPlacement"></param>
        /// <param name="opacity"></param>
        public void Draw(CanvasAnimatedDrawEventArgs drawArgs, Rect targetRectangle, Color effectColor, string effectPlacement, Piece chosenPiece)
        {
            switch (effectPlacement)
            {
                case "Behind":
                    if (Clicked && ReferenceEquals(this, chosenPiece))
                    {
                        AnimationHandler.DrawBlinkAnimation(drawArgs, targetRectangle, effectColor, EffectSize.Medium);
                        AnimationHandler.DrawBlinkAnimation(drawArgs, LudoBoard.TileGridPositions[AllowedDestinationTileVector.Value], Colors.Yellow, EffectSize.Medium);
                    }
                    else
                    {
                        AnimationHandler.DrawBlinkAnimation(drawArgs, targetRectangle, effectColor, EffectSize.Small);
                    }
                    drawArgs.DrawingSession.DrawImage(PieceImage, targetRectangle);
                    break;
                case "In front":
                    drawArgs.DrawingSession.DrawImage(PieceImage, targetRectangle);
                    AnimationHandler.DrawBlinkAnimation(drawArgs, targetRectangle, effectColor, EffectSize.Small);
                    break;

                default:
                    break;
            }
        }
        /// <summary>
        /// Selects a piece, so that the player can choose to move it.
        /// </summary>
        public void SetClicked()
        {
            Clicked = true;
            try
            {
                if (Position != StartPosition)
                {
                    int pathPosition = LudoBoard.PlayerPaths[(int)PieceColor].IndexOf(Position);
                    // Take off one value from the dice cast
                    AllowedDestinationTileVector = LudoBoard.PlayerPaths[(int)PieceColor][pathPosition + Game.CurrentDiceRoll.Value];
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
        /// <summary>
        /// Makes it so that a piece is no longer selected.
        /// </summary>
        public void RemoveClicked()
        {
            Clicked = false;
            AllowedDestinationTileVector = null;
        }
    }
}
