using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Numerics;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.Text;
using Windows.Foundation;

namespace LudoLike
{
    public class Game
    {
        public List<Player> _players;
        public LudoBoard _board;
        public List<Tile> Tiles;
        public int CurrentPlayerTurn { get; private set; }

        //Used for displaying the current score
        private readonly CanvasTextFormat _textFormat;
        private Rect _scoreBox;

        public Game()
        {
            _board = new LudoBoard();
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

        public void AddPlayers(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                switch (i)
                {
                    case 0:
                        _players.Add(new Player(PlayerColors.Red, _board.NestTiles["Red"])); // assumes first four tiles are Home/Nests tiles
                        break;
                    case 1:
                        _players.Add(new Player(PlayerColors.Blue, _board.NestTiles["Blue"])); // assumes first four tiles are Home/Nests tiles
                        break;
                    case 2:
                        _players.Add(new Player(PlayerColors.Yellow, _board.NestTiles["Yellow"])); // assumes first four tiles are Home/Nests tiles
                        break;
                    case 3:
                        _players.Add(new Player(PlayerColors.Green, _board.NestTiles["Green"])); // assumes first four tiles are Home/Nests tiles
                        break;

                }
            }
        }

        //does not adapt to only 2 or 3 players 
        public void CheckTiles()
        {
            //get list of tiles
            List<Vector2>[] list = {
            _players[0].ReturnPiecePostitions(),
            _players[1].ReturnPiecePostitions(),
            _players[2].ReturnPiecePostitions(),
            _players[3].ReturnPiecePostitions()
            };

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
                            if (piece.position == same[0])
                            {
                                //moves piece to nest/home
                                piece.position = piece.StartPosition; // might wanna use the move method of piece when implemented
                                _players[CurrentPlayerTurn].ChangeScore(100); //100 points is placeholder
                                _players[i].ChangeScore(-100);
                            }
                        }
                    }
                }
            }
        }

        public void Draw(CanvasAnimatedDrawEventArgs drawArgs)
        {
            DrawScore(drawArgs);
            foreach (Player player in _players)
            {
                player.Draw(drawArgs);
            }
            drawArgs.DrawingSession.DrawText($"Current player: {(PlayerColors)CurrentPlayerTurn}", 
                500, 20,
                _players[CurrentPlayerTurn].UIcolor);
        }

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

        public void NextTurn()
        {
            CurrentPlayerTurn++;

            if (CurrentPlayerTurn == 4)
            {
                CurrentPlayerTurn = 0;
            }
            //method to change color and visuals/ maybe
        }

        public void CreateStaticTiles()
        {
            foreach (KeyValuePair<string, List<Vector2>> staticTiles in _board.StaticTiles)
            {
                foreach (Vector2 vector in staticTiles.Value)
                {
                    Tiles.Add(new StaticTile(_board.TileGrid[vector], staticTiles.Key, vector));
                }
            }
        }

        public void CreateDynamicTiles()
        {
            Random rng = new Random();
            foreach (Vector2 vector in _board.DynamicTiles)
            {
                double tileType = rng.NextDouble();
                if (tileType < 0.10)
                {
                    Tiles.Add(new TeleportTile(_board.TileGrid[vector], _board.TileGrid[vector], vector));
                }
                else if (tileType < 0.20)
                {
                    Tiles.Add(new MinigameTile(_board.TileGrid[vector], new Minigame(), vector));
                }
                else if (tileType < 0.5)
                {
                    Tiles.Add(new ScoreTile(_board.TileGrid[vector], 100, vector));
                }
                else
                {
                    Tiles.Add(new Tile(_board.TileGrid[vector], Tile.TileImages["Regular"], vector));
                }
            }
        }
        public void UpdateTilePositions()
        {
            foreach(Tile tile in Tiles)
            {
                tile.TargetRectangle = _board.TileGrid[tile.GridPosition];
            }
        }

        public void TakeTurn(int diceRoll)
        {
            _players[CurrentPlayerTurn].MovePiece(diceRoll);

            //Pass control to the next player
            CurrentPlayerTurn = ++CurrentPlayerTurn % _players.Count;
        }
    }
}
