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
        public readonly List<Player> _players;
        public LudoBoard Board;
        public List<Tile> Tiles;
        public int CurrentPlayerTurn { get; private set; }
        //Audio
        public static MediaSource BackgroundMusic;

        //Used for displaying the current history and score
        private readonly CanvasTextFormat _textFormat;
        private Rect _scoreBox;
        private readonly TurnHistoryHandler _turnHistory;

        // Track the current diceroll for checking move availability
        public static int? CurrentDiceRoll;
        public Game()
        {
            Board = new LudoBoard();
            _players = new List<Player>();
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
        /// <param name="amount"></param>
        public void AddPlayers(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                switch (i)
                {
                    case 0:
                        _players.Add(new Player(PlayerColors.Red, LudoBoard.NestTilesPositions["Red"], _turnHistory)); // assumes first four tiles are Home/Nests tiles
                        break;
                    case 1:
                        _players.Add(new Player(PlayerColors.Blue, LudoBoard.NestTilesPositions["Blue"], _turnHistory)); // assumes first four tiles are Home/Nests tiles
                        break;
                    case 2:
                        _players.Add(new Player(PlayerColors.Yellow, LudoBoard.NestTilesPositions["Yellow"], _turnHistory)); // assumes first four tiles are Home/Nests tiles
                        break;
                    case 3:
                        _players.Add(new Player(PlayerColors.Green, LudoBoard.NestTilesPositions["Green"], _turnHistory)); // assumes first four tiles are Home/Nests tiles
                        break;

                }
            }
        }

        //does not adapt to only 2 or 3 players
        // TODO: Test this method and integrate the functionality to to the game
        public void CheckTilesForCollisions()
        {
            //get list of tiles

            List<List<Vector2>> list = new List<List<Vector2>>();
            for (int i = 0; i < _players.Count; i++)
            {
                list.Add(_players[i].ReturnPiecePostitions());
            }
            //compare lists
            //take this turns player pieces locations and comapre to others
            //List<Tile> same = list[0].Union(list[1]).ToList(); - checks if same vlaues exists in both lists, returns said values into same list // loop to check each piece to send to nest/home

            for (int i = 0; i < _players.Count; i++)
            {
                if (i != CurrentPlayerTurn)
                {
                    //creates i list of tiles that two pieces share
                    List<Vector2> same = list[CurrentPlayerTurn].Intersect(list[i]).ToList();
                    //checks if current player is in same position as another
                    if (same.Count != 0)
                    {
                        //looks for piece of another player to send back to home/nest
                        foreach (Piece piece in _players[i]._pieces)
                        {
                            if (piece.position == same[0] && piece.position != LudoBoard.StaticTilesPositions["Middle"][0])
                            {
                                //moves piece to nest/home
                                piece.position = piece.StartPosition; // might wanna use the move method of piece when implemented
                                _players[CurrentPlayerTurn].ChangeScore(100); //100 points is placeholder
                                _players[i].ChangeScore(-100);
                                SoundMixer.PlayRandomSound(Player.PieceCollisionSounds);
                            }
                        }
                    }
                }
            }
        }

        public void CheckSpecialTile(Piece piece)
        {
            //TODO: Refactor Tile logic to avoid inefficiently checking *every single tile* on *every single move*.
            for (int i = 0; i < Tiles.Count(); i++)
            {
                if (piece.position == Tiles[i].GridPosition)
                {
                    Tiles[i].TileEvent(_players[CurrentPlayerTurn]);
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
            foreach(Tile tile in Tiles)
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
            Player currentPlayer = _players[CurrentPlayerTurn];
            currentPlayer.MovePiece(CurrentDiceRoll.Value, player.ChosenPiece);
            CheckSpecialTile(player.ChosenPiece);
            CheckTilesForCollisions();
            currentPlayer.ResetTurnChoice();

            if (CurrentDiceRoll.Value != 6) //Player gets another turn when rolling a 6.
            {
                NextPlayerTurn();
            }
            _turnHistory.Add(currentPlayer, $"︵🎲 {diceRoll}");
            CurrentDiceRoll = null;     // Reset the die
        }
            

        public void NextPlayerTurn()
        {
            CurrentPlayerTurn = ++CurrentPlayerTurn % _players.Count;
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
            
            foreach (Player player in _players)
            {
                if (_players.IndexOf(player) == CurrentPlayerTurn)
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
        /// 
        private void DrawScore(CanvasAnimatedDrawEventArgs drawArgs)
        {
            drawArgs.DrawingSession.FillRoundedRectangle(
                _scoreBox, 10, 10, Windows.UI.Colors.DarkGray); //10, 10 are x- and y-radii for rounded corners
            for (int n = 0; n < _players.Count; ++n)
            {
                drawArgs.DrawingSession.DrawText(
                    $"{_players[n].PlayerColor}: {_players[n].Score}",
                    (float)_scoreBox.X + 30f,
                    (float)_scoreBox.Y + _textFormat.FontSize * (n + 1),
                    _players[n].UIcolor);
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


        void Stealpoints(int player1, int player2, int points)//steals from player1 and gives to plaýer2
        {
            _players[player1].ChangeScore(-points);
            _players[player2].ChangeScore(points);
        }
    }
}
