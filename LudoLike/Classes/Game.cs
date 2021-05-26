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
        private int turn = 0;

        public Game()
        {
            _board = new LudoBoard();
            _players = new List<Player>();
            //AddTiles(42); //Placeholder amount
            //AddPlayers(playerAmount);
        }

        public void AddPlayers(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                switch (i)
                {
                    case 0:
                        _players.Add(new Player(PlayerColors.red, _board.NestTiles["Red"])); // assumes first four tiles are Home/Nests tiles
                        break;
                    case 1:
                        _players.Add(new Player(PlayerColors.blue, _board.NestTiles["Blue"])); // assumes first four tiles are Home/Nests tiles
                        break;
                    case 2:
                        _players.Add(new Player(PlayerColors.yellow, _board.NestTiles["Yellow"])); // assumes first four tiles are Home/Nests tiles
                        break;
                    case 3:
                        _players.Add(new Player(PlayerColors.green, _board.NestTiles["Green"])); // assumes first four tiles are Home/Nests tiles
                        break;

                }
            }
        }

        //need to be used before AddPlayers()
        //public void AddTiles(int amount)
        //{
        //    for (int i = 0; i < amount; i++)
        //    {
        //        _board.Add(new Tile(i));
        //    }
        //}

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
                if (i != turn)
                {
                    //creates i list of tiles that two pieces share
                    List<Vector2> same = list[turn].Intersect(list[i]).ToList();
                    //checks if current player is in same position as another
                    if (same.Count != 0)
                    {
                        //looks for piece of another player to send back to home/nest
                        foreach (Piece piece in _players[i].pieces)
                        {
                            if (piece.position == same[0])
                            {
                                //moves piece to nest/home
                                piece.position = piece.StartPosition; // might wanna use the move method of piece when implemented
                                _players[turn].ChangeScore(100); //100 points is placeholder
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
        }

        private void DrawScore(CanvasAnimatedDrawEventArgs drawArgs)
        {
            //todo: Initialize ScoreBox and formatting _once_ instead of on every update.
            var textFormat = new CanvasTextFormat()
            {
                FontFamily = "Helvetica",
                FontSize = 30,
                FontWeight = Windows.UI.Text.FontWeights.Bold
            };
            Rect ScoreBox = new Rect
            {
                //Arbitrary values. Todo: Scale with window size.
                X = 30,
                Y = 30,
                Width = 175,
                Height = 6 * textFormat.FontSize
            };

            drawArgs.DrawingSession.FillRoundedRectangle(
                ScoreBox, 10, 10, Windows.UI.Colors.DarkGray); //10, 10 are x- and y-radii for rounded corners
            for (int n = 0; n < _players.Count; ++n)
            {
                drawArgs.DrawingSession.DrawText(
                    $"{_players[n].PlayerColor}: {_players[n].Score}",
                    (float)ScoreBox.X + 30f,
                    (float)ScoreBox.Y + textFormat.FontSize * (n + 1),
                    _players[n].UIcolor);
            }
        }

        public void NextTurn()
        {
            turn++;

            if (turn == 4)
            {
                turn = 0;
            }
            //method to change color and visuals/ maybe
        }

        public void CreateStaticTiles()
        {
            foreach (KeyValuePair<string, List<Vector2>> staticTiles in _board.StaticTiles)
            {
                foreach (Vector2 vector in staticTiles.Value)
                {
                    Tiles.Add(new Tile(_board.TileGrid[vector], Tile.TileImages[staticTiles.Key]));
                }
            }
        }
        public void CreateDynamicTiles()
        {
            foreach (Vector2 vector in _board.DynamicTiles)
            {
                Tiles.Add(new Tile(_board.TileGrid[vector], Tile.TileImages["Regular"]));
            }
        }

        //public void TileToNormal(int index)
        //{
        //    _board[index] = new Tile(index);
        //    _players[turn].pieces[**piece above**].position = _board[index]; //100 is a placeholder //idk how to get right piece
        //}

        //for testing purposes
        //public bool Test()
        //{
        //    AddTiles(20);
        //    AddPlayers(4);
        //    //10
        //    _players[0].pieces[0].position = _board[10];
        //    _players[1].pieces[0].position = _board[10];

        //    CheckTiles();

        //    if (_players[1].pieces[0].position == _board[10])
        //    {
        //        return false;
        //    }

        //    return true;
        //}
    }


}
