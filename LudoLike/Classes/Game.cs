using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.Text;
using Windows.Foundation;
using Windows.Media.Core;
using LudoLike.Classes;
using Microsoft.Graphics.Canvas.Effects;

namespace LudoLike
{
    /// <summary>
    /// Represents an instance of a Ludo game.
    /// </summary>
    public class Game
    {
        public readonly List<Player> Players;
        public LudoBoard Board;
        public List<Tile> Tiles;
        public int CurrentPlayerTurn { get; private set; }
        public int RemainingPieces;
        //Audio
        public static MediaSource BackgroundMusic;

        //Used for displaying the current history and score
        private readonly CanvasTextFormat _textFormat;
        private Rect _scoreBox;
        private Rect _turnGraphTarget;
        private readonly TurnHistoryHandler _turnHistory;

        // Track the current diceroll for checking move availability
        public static int? CurrentDiceRoll;
        public Game()
        {
            Board = new LudoBoard();
            Players = new List<Player>();
            Tiles = new List<Tile>();

            _textFormat = new CanvasTextFormat()
            {
                FontFamily = "Helvetica",
                FontSize = 30,
                FontWeight = Windows.UI.Text.FontWeights.Bold
            };
            _scoreBox = new Rect
            {
                //Arbitrary values. Todo: Scale with window size.
                X = 30,
                Y = 30,
                Width = 175,
                Height = 6 * _textFormat.FontSize
            };
            _turnGraphTarget = new Rect
            {
                X = 1200,
                Y = 30,
                Width = 350,
                Height = 148
            };
            _turnHistory = new TurnHistoryHandler(
                _textFormat,
                new Rect
                {
                    //TODO: Refactor ugly magic constants.
                    X = 30,
                    Y = 240,
                    Width = 175,
                    Height = 22 * _textFormat.FontSize
                });
        }

        /// <summary>
        /// Adds players to the game according to the chosen slider value.
        /// </summary>
        /// <param name="players"></param>
        public void AddPlayers(int players, int piecesPerPlayer)
        {
            for (int i = 0; i < players; i++)
            {
                PlayerColors color = (PlayerColors)i;
                Players.Add(new Player(color, LudoBoard.NestTilesPositions[color.ToString()], piecesPerPlayer, _turnHistory)); // assumes first four tiles are Home/Nests tiles
            }

            RemainingPieces = Players.Count * piecesPerPlayer;
        }

        public void AddRollToHistory(int roll)
        {
            _turnHistory.Add(Players[CurrentPlayerTurn], $"︵🎲 {CurrentDiceRoll}");
        }

        /// <summary>
        /// Finds out whether any Tiles hold Pieces from multiple opposing Players and determines if any of them should be bumped back to the nest.
        /// </summary>
        public void CheckTilesForCollisions()
        {
            List<List<Vector2>> list = new List<List<Vector2>>();
            for (int i = 0; i < Players.Count; i++)
            {
                list.Add(Players[i].ReturnPiecePostitions());
            }
            
            for (int i = 0; i < Players.Count; i++)     // Take this turns player pieces locations and comapre to others
            {
                if (i != CurrentPlayerTurn)
                {
                    List<Vector2> same = list[CurrentPlayerTurn].Intersect(list[i]).ToList();   // Creates i list of tiles that two pieces share
                    if (same.Count != 0)    // Checks if current player is in same position as another
                    {
                        foreach (Piece piece in Players[i].Pieces)     // Looks for piece of another player to send back to home/nest
                        {
                            if (piece.Position == same[0] && piece.Position != LudoBoard.StaticTilesPositions["Middle"][0])
                            {
                                piece.Position = piece.StartPosition;   // Moves piece to nest/home
                                Players[CurrentPlayerTurn].ChangeScore(100);    // 100 points is placeholder
                                Players[i].ChangeScore(-100);
                                SoundMixer.PlayRandomSound(Player.PieceCollisionSounds);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the current tile that the newly moved piece is on is a special tile.
        /// </summary>
        /// <param name="piece"></param>
        public void CheckSpecialTile(Piece piece)
        {
            //TODO: Refactor Tile logic to avoid inefficiently checking *every single tile* on *every single move*.
            for (int i = 0; i < Tiles.Count(); i++)
            {
                if (piece.Position == Tiles[i].GridPosition)
                {
                    Tiles[i].TileEvent(Players[CurrentPlayerTurn]);
                    break;
                }
            }
        }

        /// <summary>
        /// Creates static tiles on the board.
        /// </summary>
        public void CreateStaticTiles()
        {
            foreach (KeyValuePair<string, List<Vector2>> staticTiles in LudoBoard.StaticTilesPositions)
            {
                foreach (Vector2 vector in staticTiles.Value)
                {
                    Tiles.Add(new StaticTile(LudoBoard.TileGridPositions[vector], staticTiles.Key, vector));
                }
            }
        }

        /// <summary>
        /// Creates dynamic tiles on the board via randomization.
        /// </summary>
        public void CreateDynamicTiles()
        {
            Random rng = new Random();
            List<Vector2> usedVectors = new List<Vector2>();

            foreach (Vector2 vector in LudoBoard.DynamicTilesPositions)
            {
                if (!usedVectors.Contains(vector))
                {
                    double tileType = rng.NextDouble();
                    if (tileType < 0.10 && LudoBoard.DynamicTilesPositions.IndexOf(vector) < (LudoBoard.DynamicTilesPositions.Count() - 1))
                    {
                        while (true)
                        {
                            int targetPosition = rng.Next(LudoBoard.DynamicTilesPositions.IndexOf(vector), LudoBoard.DynamicTilesPositions.Count());
                            Vector2 targetVector = LudoBoard.DynamicTilesPositions[targetPosition];
                            if (!usedVectors.Contains(targetVector) && targetVector != vector)
                            {
                                Tiles.Add(new TeleportTile(LudoBoard.TileGridPositions[vector], targetVector, vector));
                                Tiles.Add(new TeleportTile(LudoBoard.TileGridPositions[targetVector], vector, targetVector));
                                usedVectors.Add(targetVector);
                                break;
                            }
                        }
                    }
                    else if (tileType < 0.2)
                    {
                        Tiles.Add(new MinigameTile(LudoBoard.TileGridPositions[vector], new Minigame(), vector));
                    }
                    else if (tileType < 0.5)
                    {
                        Tiles.Add(new ScoreTile(LudoBoard.TileGridPositions[vector], 100, vector));
                    }
                    else
                    {
                        Tiles.Add(new StaticTile(LudoBoard.TileGridPositions[vector], "Regular", vector));
                    }
                    usedVectors.Add(vector);
                }
            }
        }

        /// <summary>
        /// Updates the tiles target rectangles to new scaling of window.
        /// </summary>
        private void UpdateTilePositions()
        {
            foreach (Tile tile in Tiles)
            {
                tile.TargetRectangle = LudoBoard.TileGridPositions[tile.GridPosition];
            }
        }

        /// <summary>
        /// Moves a piece, triggers collisions and events at the destination, 
        /// then passes the control to the next player.
        /// </summary>
        /// <param name="diceRoll"></param>
        public void TakeTurn(Player player)
        {
            Player currentPlayer = Players[CurrentPlayerTurn];
            currentPlayer.MovePiece(CurrentDiceRoll.Value, player.ChosenPiece);
            CheckSpecialTile(player.ChosenPiece);
            CheckTilesForCollisions();
            currentPlayer.ResetTurnChoice();
            CheckPiecesAtGoal();

            if (CurrentDiceRoll.Value != 6)     // Player gets another turn when rolling a 6.
            {
                NextPlayerTurn();
            }
            CurrentDiceRoll = null;     // Reset the die
        }

        /// <summary>
        /// Changes the player turn to the one next in line.
        /// </summary>
        public void NextPlayerTurn()
        {
            CurrentPlayerTurn = ++CurrentPlayerTurn % Players.Count;

            if (RemainingPieces == 0)
            {
                return;
            }
            else if (Players[CurrentPlayerTurn].Pieces.Count == 0)
            {
                NextPlayerTurn();
            }
        }

        /// <summary>
        /// Draws the current score along with each players pieces on the board.
        /// </summary>
        /// <param name="drawArgs"></param>
        public void DrawMainContent(CanvasAnimatedDrawEventArgs drawArgs)
        {
            Board.Draw(drawArgs);
            _turnHistory.Draw(drawArgs);
            DrawScore(drawArgs);
            DrawTiles(drawArgs);
            DrawTurnGraphic(drawArgs);
            foreach (Player player in Players)
            {
                if (Players.IndexOf(player) == CurrentPlayerTurn)
                {
                    player.DrawCurrentPlayerPieces(drawArgs);
                }
                else
                {
                    player.DrawPieces(drawArgs);
                }
            }
        }

        /// <summary>
        /// Draws the score board on the given canvas.
        /// </summary>
        /// <param name="drawArgs"></param>
        private void DrawScore(CanvasAnimatedDrawEventArgs drawArgs)
        {
            drawArgs.DrawingSession.FillRoundedRectangle(
                _scoreBox, 10, 10, Windows.UI.Colors.DarkGray);     // 10, 10 are x- and y-radii for rounded corners
            for (int n = 0; n < Players.Count; ++n)
            {
                drawArgs.DrawingSession.DrawText(
                    $"{Players[n].PlayerColor}: {Players[n].Score}",
                    (float)_scoreBox.X + 30f,
                    (float)_scoreBox.Y + _textFormat.FontSize * (n + 1),
                    Players[n].UIcolor);
            }
        }

        /// <summary>
        /// Draws the tiles on the given canvas.
        /// </summary>
        /// <param name="drawArgs"></param>
        private void DrawTiles(CanvasAnimatedDrawEventArgs drawArgs)
        {
            UpdateTilePositions();
            foreach (Tile tile in Tiles)
            {
                tile.Draw(drawArgs);
            }
        }

        /// <summary>
        /// Draws the turn square in the top right corner.
        /// </summary>
        /// <param name="drawArgs"></param>
        private void DrawTurnGraphic(CanvasAnimatedDrawEventArgs drawArgs)
        {
            _turnGraphTarget.X = Scaling.bWidth / 1.3;
            _turnGraphTarget.Y = Scaling.bHeight / 13;
            _turnGraphTarget.Width = Scaling.bWidth / 5.5;
            _turnGraphTarget.Height = Scaling.bHeight / 7.3;
            drawArgs.DrawingSession.DrawImage(Players[CurrentPlayerTurn].TurnGraphic, _turnGraphTarget);
        }

        /// <summary>
        /// Checks if any of the pieces is at the goal and gives points depending on how early it is.
        /// </summary>
        public void CheckPiecesAtGoal()
        {
            Vector2 goal = new Vector2(5, 5);
            int delpiece = 10;

            for (int i = 0; i < Players[CurrentPlayerTurn].Pieces.Count; i++)
            {
                if (Players[CurrentPlayerTurn].Pieces[i].Position == goal)
                {
                    Players[CurrentPlayerTurn].ChangeScore(RemainingPieces * 100);
                    RemainingPieces--;
                    delpiece = i;   // Removes piece from list
                }
            }
            if (delpiece != 10)
            {
                Players[CurrentPlayerTurn].Pieces.RemoveAt(delpiece);
            }
        }
    }
}
