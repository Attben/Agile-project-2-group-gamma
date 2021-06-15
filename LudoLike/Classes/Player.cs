using Microsoft.Graphics.Canvas;
using LudoLike.Classes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Core;

namespace LudoLike
{
    public enum PlayerColors
    {
        Red, Blue, Yellow, Green
    }

    /// <summary>
    /// Represents a player in the Ludo game.
    /// </summary>
    public class Player
    {
        public static Dictionary<PlayerColors, Windows.UI.Color> WindowsPlayerColors = new Dictionary<PlayerColors, Windows.UI.Color>()
        {
            {PlayerColors.Red, Windows.UI.Colors.Red },
            {PlayerColors.Blue, Windows.UI.Colors.Blue },
            {PlayerColors.Yellow, Windows.UI.Colors.Yellow },
            {PlayerColors.Green, Windows.UI.Colors.LawnGreen }
        };
        public int Score { get; private set; }
        public PlayerColors PlayerColor;
        public Windows.UI.Color UIcolor;
        public List<Piece> Pieces;
        public CanvasBitmap TurnGraphic;

        public Piece ChosenPiece;

        public static List<MediaSource> PieceCollisionSounds = new List<MediaSource>();
        public static List<MediaSource> PieceMovingSounds = new List<MediaSource>();

        public static CanvasBitmap RedTurn;
        public static CanvasBitmap BlueTurn;
        public static CanvasBitmap GreenTurn;
        public static CanvasBitmap YellowTurn;


        private readonly TurnHistoryHandler _turnHistory;


        public Player(PlayerColors color, List<Vector2> startPositions, int pieceCount, TurnHistoryHandler turnHistory)
        {
            Score = 0;
            PlayerColor = color;
            _turnHistory = turnHistory;

            switch (color)
            {
                case PlayerColors.Red:
                    UIcolor = Windows.UI.Colors.Red;
                    TurnGraphic = RedTurn;
                    break;
                case PlayerColors.Green:
                    UIcolor = Windows.UI.Colors.LawnGreen;
                    TurnGraphic = GreenTurn;
                    break;
                case PlayerColors.Blue:
                    UIcolor = Windows.UI.Colors.Blue;
                    TurnGraphic = BlueTurn;
                    break;
                case PlayerColors.Yellow:
                    UIcolor = Windows.UI.Colors.Yellow;
                    TurnGraphic = YellowTurn;
                    break;
            }

            Pieces = new List<Piece>();

            for (int i = 0; i < pieceCount; i++)
            {
                Pieces.Add(new Piece(startPositions[i], color));
            }
        }

        /// <summary>
        /// Add a move to the turn history.
        /// This method is mostly a workaround to avoid being forced to add a TurnHistoryHandler
        /// to all of the constructors in the various Tile classes.
        /// </summary>
        /// <param name="move">A string describing the move that was made.</param>
        public void AddMoveToTurnHistory(string move)
        {
            _turnHistory.Add(this, move);
        }

        /// <summary>
        /// Updates the score of the player.
        /// </summary>
        /// <param name="amount"></param>
        public void ChangeScore(int amount)
        {
            Score += amount;
            AddMoveToTurnHistory($"💲{amount}");
        }

        /// <summary>
        /// Draws the active pieces of the player.
        /// </summary>
        /// <param name="drawArgs"></param>
        public void DrawPieces(CanvasAnimatedDrawEventArgs drawArgs)
        {
            foreach (Piece p in Pieces)
            {
                p.Draw(drawArgs, LudoBoard.TileGridPositions[p.Position]);
            }
        }
        
        /// <summary>
        /// Specifically used draw function used for the current player. Allows for clicking playable pieces and showing their destination tiles.
        /// </summary>
        /// <param name="drawArgs"></param>
        public void DrawCurrentPlayerPieces(CanvasAnimatedDrawEventArgs drawArgs)
        {
            foreach (Piece p in Pieces)
            {
                if (Game.CurrentDiceRoll.HasValue)
                {
                    if (LudoBoard.NestTilesPositions[PlayerColor.ToString()].Contains(p.Position))      // Check if current Piece is in the players nest
                    {
                        HandleClickingOnDifferentPieces(drawArgs, p);
                    }
                    else if (LudoBoard.StaticTilesPositions["Middle"].Contains(p.Position))     // If its in the middle
                    {
                        p.RemoveClicked();
                        p.Draw(drawArgs, LudoBoard.TileGridPositions[p.Position], Windows.UI.Colors.Black, "In front", ChosenPiece);
                    }
                    else if (GameBoard.UserClickedBoard && GameBoard.ClickedTileVector == p.Position)   // If the piece is clicked by the current player
                    {
                        if (ChosenPiece != null)
                        {
                            if (!ReferenceEquals(p, ChosenPiece) && p.Position == ChosenPiece.Position)     // If the position is clicked but its not the chosen piece 
                            {                                                                               // (two pieces on the same square)
                                p.RemoveClicked();
                                p.Draw(drawArgs, LudoBoard.TileGridPositions[p.Position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                            }
                            else                                                                        // The user selects the piece
                            {
                                p.SetClicked();
                                p.Draw(drawArgs, LudoBoard.TileGridPositions[p.Position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                            }
                        }
                        else                                                                            // If user clics an already selected piece 
                        {
                            p.SetClicked();
                            p.Draw(drawArgs, LudoBoard.TileGridPositions[p.Position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                        }
                    }
                    else                                                                        // Show that the piece is available for moving
                    {
                        p.RemoveClicked();
                        p.Draw(drawArgs, LudoBoard.TileGridPositions[p.Position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                    }
                }
                else                                                                            // Its a piece that is not the current players
                {
                    p.RemoveClicked();
                    p.Draw(drawArgs, LudoBoard.TileGridPositions[p.Position]);
                }
            }
        }
        /// <summary>
        /// Helper method for easier reding through the large clickinglogic
        /// </summary>
        /// <param name="drawArgs"></param>
        /// <param name="p"></param>
        private void HandleClickingOnDifferentPieces(CanvasAnimatedDrawEventArgs drawArgs, Piece p)
        {
            if (Game.CurrentDiceRoll.HasValue)
            {
                if (GameBoard.ClickedTileVector.HasValue && GameBoard.ClickedTileVector != p.Position)      // We have clicked somewhere but not on a piece tile
                {
                    if ((Game.CurrentDiceRoll.Value == 1 || Game.CurrentDiceRoll.Value == 6))       // If the dice is 1 or 6 the pieces in nest are available for moving
                    {
                        p.RemoveClicked();
                        p.Draw(drawArgs, LudoBoard.TileGridPositions[p.Position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                    }
                    else                                                                            // If the dice cast is not 1 or 6 show that pieces in the nest are not
                    {                                                                               // viable options to move
                        p.RemoveClicked();
                        p.Draw(drawArgs, LudoBoard.TileGridPositions[p.Position], Windows.UI.Colors.Black, "In front", ChosenPiece);
                    }
                }
                else if (GameBoard.ClickedTileVector.HasValue && GameBoard.ClickedTileVector == p.Position)     // The user has clicked on a tile which has a piece on it
                {
                    if (Game.CurrentDiceRoll.Value == 1 || Game.CurrentDiceRoll.Value == 6)
                    {
                        if (ChosenPiece != null)                                                    // If the user does have a currently selected piece
                        {
                            if (!ReferenceEquals(p, ChosenPiece) && p.Position == ChosenPiece.Position)     // If the clicked piece is on the same square as the selected
                            {                                                                               // piece but is not the chosen piece
                                p.RemoveClicked();
                                p.Draw(drawArgs, LudoBoard.TileGridPositions[p.Position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                            }
                            else                                                                            // This is the new selected piece
                            {
                                p.SetClicked();
                                p.Draw(drawArgs, LudoBoard.TileGridPositions[p.Position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                            }
                        }
                        else                                                                                // If the user has not selected any piece, this is now selected
                        {
                            p.SetClicked();
                            p.Draw(drawArgs, LudoBoard.TileGridPositions[p.Position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                        }
                    }
                    else                                                                                    // If the piece is not clicked and its not playable
                    {
                        p.RemoveClicked();
                        p.Draw(drawArgs, LudoBoard.TileGridPositions[p.Position], Windows.UI.Colors.Black, "In front", ChosenPiece);
                    }
                }
                else                                                                                            // User has not clicked anywhere yet
                {
                    if (Game.CurrentDiceRoll.Value == 1 || Game.CurrentDiceRoll.Value == 6)
                    {
                        p.RemoveClicked();
                        p.Draw(drawArgs, LudoBoard.TileGridPositions[p.Position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                    }
                    else
                    {
                        p.RemoveClicked();
                        p.Draw(drawArgs, LudoBoard.TileGridPositions[p.Position], Windows.UI.Colors.Black, "In front", ChosenPiece);
                    }
                }
            }
            else    // The dice has not yet been cast
            {
                p.RemoveClicked();
                p.Draw(drawArgs, LudoBoard.TileGridPositions[p.Position]);
            }
        }

        /// <summary>
        /// Checks if the player can make any moves on the given dice roll.
        /// </summary>
        /// <param name="DiceRoll"></param>
        /// <returns></returns>
        public bool CheckPossibilityToMove(int DiceRoll)
        {
            if (DiceRoll != 1 && DiceRoll != 6)  // If we roll 2, 3, 4 or 5
            {
                foreach (Piece piece in Pieces)
                {
                    if (!LudoBoard.NestTilesPositions[PlayerColor.ToString()].Contains(piece.Position) &&  // If there is any piece out on the board 
                        !LudoBoard.StaticTilesPositions["Middle"].Contains(piece.Position))                // and its not in the middle - return true
                    {
                        return true;
                    }
                    
                }
            }
            else  // If we roll 1 or 6
            {
                foreach (Piece piece in Pieces)
                {
                    if (!LudoBoard.StaticTilesPositions["Middle"].Contains(piece.Position))  // If any piece is not in the end - return true
                    {
                        return true;
                    }
                }
            }
            return false;   // Otherwise, the player cannot do anything
        }

        /// <summary>
        /// Steps the given piece * diceroll many times.
        /// </summary>
        /// <param name="diceRoll"></param>
        /// <param name="piece"></param>
        public void MovePiece(int diceRoll, Piece piece)
        {
            int nextPosition;
            var path = LudoBoard.PlayerPaths[(int)PlayerColor];
            for (int i = 0; i < diceRoll; i++)
            {
                nextPosition = 0;
                foreach (Vector2 tile in path)
                {
                    if (piece.Position == tile)
                    {
                        nextPosition = path.IndexOf(tile) + 1;
                    }
                }
                try
                {
                    piece.UpdatePosition(path[nextPosition]);
                    SoundMixer.PlayRandomSound(PieceMovingSounds);
                }
                catch (ArgumentOutOfRangeException)
                {
                    piece.UpdatePosition(path[44]);
                    break;
                }
            }
        }

        /// <summary>
        /// Returns the players pieces positions.
        /// </summary>
        /// <returns>A list of Vector2</returns>
        public List<Vector2> ReturnPiecePostitions() // return list of tiles
        {
            List<Vector2> list = new List<Vector2>();
            foreach (Piece piece in Pieces)
            {
                list.Add(piece.Position);
            }

            return list;
        }

        /// <summary>
        /// Resets the chosen piece (the clicked) to null.
        /// </summary>
        public void ResetTurnChoice()
        {
            ChosenPiece = null;
        }
    }
}
