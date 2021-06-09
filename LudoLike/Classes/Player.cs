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
        public List<Piece> _pieces;

        public Piece ChosenPiece;

        public static List<MediaSource> PieceCollisionSounds = new List<MediaSource>();
        public static List<MediaSource> PieceMovingSounds = new List<MediaSource>();



        public Player(PlayerColors color, List<Vector2> startPositions)
        {
            Score = 0;
            PlayerColor = color;

            switch (color)
            {
                case PlayerColors.Red:
                    UIcolor = Windows.UI.Colors.Red;
                    break;
                case PlayerColors.Blue:
                    UIcolor = Windows.UI.Colors.Blue;
                    break;
                case PlayerColors.Yellow:
                    UIcolor = Windows.UI.Colors.Yellow;
                    break;
                case PlayerColors.Green:
                    UIcolor = Windows.UI.Colors.LawnGreen;
                    break;
            }

            _pieces = new List<Piece>();

            for (int i = 0; i < 4; i++)
            {
                _pieces.Add(new Piece(startPositions[i], color));
            }
        }

        public void ChangeScore(int amount)
        {
            Score += amount;
        }

        public void DrawPieces(CanvasAnimatedDrawEventArgs drawArgs)
        {
            foreach (Piece p in _pieces)
            {
                p.Draw(drawArgs, LudoBoard.TileGridPositions[p.position]);
            }
        }
        


        /// <summary>
        /// Specifically used draw function used for the current player. Allows for clicking playable pieces and showing their destination tiles.
        /// </summary>
        /// <param name="drawArgs"></param>
        public void DrawCurrentPlayerPieces(CanvasAnimatedDrawEventArgs drawArgs)
        {
            foreach (Piece p in _pieces)
            {
                if (Game.CurrentDiceRoll.HasValue)
                {
                    //Check if current Piece is in the players nest
                    if (LudoBoard.NestTilesPositions[PlayerColor.ToString()].Contains(p.position))
                    {
                        HandleClickingOnDifferentPieces(drawArgs, p);
                    }
                    else if (LudoBoard.StaticTilesPositions["Middle"].Contains(p.position))
                    {
                        p.RemoveClicked();
                        p.Draw(drawArgs, LudoBoard.TileGridPositions[p.position], Windows.UI.Colors.Black, "In front", ChosenPiece);
                    }
                    else if (GameBoard.UserClickedBoard && GameBoard.ClickedTileVector == p.position)
                    {
                        if (ChosenPiece != null)
                        {
                            if (!ReferenceEquals(p, ChosenPiece) && p.position == ChosenPiece.position)
                            {
                                p.RemoveClicked();
                                p.Draw(drawArgs, LudoBoard.TileGridPositions[p.position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                            }
                            else
                            {
                                p.SetClicked();
                                ChosenPiece = p;
                                p.Draw(drawArgs, LudoBoard.TileGridPositions[p.position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                            }
                        }
                        else
                        {
                            p.SetClicked();
                            ChosenPiece = p;
                            p.Draw(drawArgs, LudoBoard.TileGridPositions[p.position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                        }
                    }
                    else    // The piece is available for moving
                    {
                        p.RemoveClicked();
                        p.Draw(drawArgs, LudoBoard.TileGridPositions[p.position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                    }
                }
                else
                {
                    p.RemoveClicked();
                    p.Draw(drawArgs, LudoBoard.TileGridPositions[p.position]);
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
                if (GameBoard.ClickedTileVector.HasValue && GameBoard.ClickedTileVector != p.position)      // We have clicked somewhere but not on piece tile
                {
                    if ((Game.CurrentDiceRoll.Value == 1 || Game.CurrentDiceRoll.Value == 6))
                    {
                        p.RemoveClicked();
                        p.Draw(drawArgs, LudoBoard.TileGridPositions[p.position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                    }
                    else
                    {
                        p.RemoveClicked();
                        p.Draw(drawArgs, LudoBoard.TileGridPositions[p.position], Windows.UI.Colors.Black, "In front", ChosenPiece);
                    }
                }
                else if (GameBoard.ClickedTileVector.HasValue && GameBoard.ClickedTileVector == p.position)     // We have clicked on the piece tile
                {
                    if (Game.CurrentDiceRoll.Value == 1 || Game.CurrentDiceRoll.Value == 6)
                    {
                        if (ChosenPiece != null)
                        {
                            if (!ReferenceEquals(p, ChosenPiece) && p.position == ChosenPiece.position)
                            {
                                p.RemoveClicked();
                                p.Draw(drawArgs, LudoBoard.TileGridPositions[p.position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                            }
                            else
                            {
                                p.SetClicked();
                                ChosenPiece = p;
                                p.Draw(drawArgs, LudoBoard.TileGridPositions[p.position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                            }
                        }
                        else
                        {
                            p.SetClicked();
                            ChosenPiece = p;
                            p.Draw(drawArgs, LudoBoard.TileGridPositions[p.position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                        }
                    }
                    else
                    {
                        p.RemoveClicked();
                        p.Draw(drawArgs, LudoBoard.TileGridPositions[p.position], Windows.UI.Colors.Black, "In front", ChosenPiece);
                    }
                }
                else                                                                                            // We have not clicked Anywhere
                {
                    if (Game.CurrentDiceRoll.Value == 1 || Game.CurrentDiceRoll.Value == 6)
                    {
                        p.RemoveClicked();
                        p.Draw(drawArgs, LudoBoard.TileGridPositions[p.position], Windows.UI.Colors.ForestGreen, "Behind", ChosenPiece);
                    }
                    else
                    {
                        p.RemoveClicked();
                        p.Draw(drawArgs, LudoBoard.TileGridPositions[p.position], Windows.UI.Colors.Black, "In front", ChosenPiece);
                    }
                }
            }
            else                                                                                                // The dice has not yet been cast
            {
                p.RemoveClicked();
                p.Draw(drawArgs, LudoBoard.TileGridPositions[p.position]);
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
                foreach (Piece piece in _pieces)
                {
                    if (!LudoBoard.NestTilesPositions[PlayerColor.ToString()].Contains(piece.position) &&  // If there is any piece out on the board - return true
                        !LudoBoard.StaticTilesPositions["Middle"].Contains(piece.position))
                    {
                        return true;
                    }
                    
                }
            }
            else  // If we roll 1 or 6
            {
                foreach (Piece piece in _pieces)
                {
                    if (!LudoBoard.StaticTilesPositions["Middle"].Contains(piece.position))  // If any piece is not in the end - return true
                    {
                        return true;
                    }
                }
            }
            return false;
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
                    if (piece.position == tile)
                    {
                        nextPosition = path.IndexOf(tile) + 1;
                    }
                }
                try
                {
                    piece.Move(path[nextPosition]);
                    SoundMixer.PlayRandomSound(PieceMovingSounds);
                }
                catch (ArgumentOutOfRangeException)
                {
                    piece.Move(path[44]);
                    break;
                }
            }
            // Detta måste anpassas till den nya tilegridlogiken
            //_piece.Move(100f, 100f);
        }

        public List<Vector2> ReturnPiecePostitions() // return list of tiles
        {
            List<Vector2> list = new List<Vector2>();
            foreach (Piece piece in _pieces)
            {
                list.Add(piece.position);
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
