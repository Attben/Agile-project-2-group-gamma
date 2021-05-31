using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.Text;
using Windows.Foundation;
using Windows.Media.Core;
using LudoLike.Classes;

namespace LudoLike
{
    /// <summary>
    /// Represents an instance of a Ludo game.
    /// </summary>
    public class Game
    {
        private List<Player> _players;
        public LudoBoard Board;
        public List<Tile> Tiles;
        public int CurrentPlayerTurn { get; private set; }

        //Audio
        public static MediaSource BackgroundMusic;

        //Used for displaying the current score
        private readonly CanvasTextFormat _textFormat;
        private Rect _scoreBox;

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
                        _players.Add(new Player(PlayerColors.Red, LudoBoard.NestTilesPositions["Red"])); // assumes first four tiles are Home/Nests tiles
                        break;
                    case 1:
                        _players.Add(new Player(PlayerColors.Blue, LudoBoard.NestTilesPositions["Blue"])); // assumes first four tiles are Home/Nests tiles
                        break;
                    case 2:
                        _players.Add(new Player(PlayerColors.Yellow, LudoBoard.NestTilesPositions["Yellow"])); // assumes first four tiles are Home/Nests tiles
                        break;
                    case 3:
                        _players.Add(new Player(PlayerColors.Green, LudoBoard.NestTilesPositions["Green"])); // assumes first four tiles are Home/Nests tiles
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
                            if (piece.position == same[0] && piece.position != LudoBoard.RedPath[44])
                            {
                                //moves piece to nest/home
                                piece.position = piece.StartPosition; // might wanna use the move method of piece when implemented
                                _players[CurrentPlayerTurn].ChangeScore(100); //100 points is placeholder
                                _players[i].ChangeScore(-100);
                                SoundMixer.PlaySound(Player.PieceCollisionSounds[new Random().Next(Player.PieceCollisionSounds.Count)]);
                            }
                        }
                    }
                }
            }
        }

        public void CheckSpecialTile()
        {
            for (int i = 0; i < Tiles.Count(); i++)
            {
                if (_players[CurrentPlayerTurn].ReturnPiecePostitions()[0] == Tiles[i].GridPosition)
                {
                    if (Tiles[i].GetType() != (typeof(StaticTile)))
                    {
                        if (Tiles[i].TileEvent(_players[CurrentPlayerTurn])) 
                        {
                            StaticTile newTile = new StaticTile(Tiles[i].TargetRectangle, "Regular", Tiles[i].GridPosition);
                            Tiles[i] = newTile;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
        // TODO: Test this method and integrate the functionality to the game
        public void NextTurn()
        {
            CurrentPlayerTurn++;

            if (CurrentPlayerTurn == 4)
            {
                CurrentPlayerTurn = 0;
            }
            //method to change color and visuals/ maybe
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
                            if (!usedVectors.Contains(targetVector))
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
        /// Passes the control to the next player.
        /// </summary>
        /// <param name="diceRoll"></param>
        public void TakeTurn(int diceRoll)
        {
            _players[CurrentPlayerTurn].MovePiece(diceRoll);

            CheckTilesForCollisions();
            CheckSpecialTile();
            //Pass control to the next player
            CurrentPlayerTurn = ++CurrentPlayerTurn % _players.Count;
        }

        /// <summary>
        /// Draws the current score along with each players pieces on the board.
        /// </summary>
        /// <param name="drawArgs"></param>
        public void DrawMainContent(CanvasAnimatedDrawEventArgs drawArgs)
        {
            Board.Draw(drawArgs);
            DrawScore(drawArgs);
            DrawTiles(drawArgs);
            foreach (Player player in _players)
            {
                player.DrawPieces(drawArgs);
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
    // Might have to make another drawmethod for drawing minigame 
    }
}
