using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Numerics;

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
                        _players.Add(new Player(PlayerColors.red, _board.RedPath[0])); // assumes first four tiles are Home/Nests tiles
                        break;
                    case 1:
                        _players.Add(new Player(PlayerColors.blue, _board.BluePath[0])); // assumes first four tiles are Home/Nests tiles
                        break;
                    case 2:
                        _players.Add(new Player(PlayerColors.yellow, _board.YellowPath[0])); // assumes first four tiles are Home/Nests tiles
                        break;
                    case 3:
                        _players.Add(new Player(PlayerColors.green, _board.GreenPath[0])); // assumes first four tiles are Home/Nests tiles
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

            for (int i = 0; i < 4; i++)
            {
                if (i != turn)
                {
                    List<Vector2> same = list[turn].Intersect(list[i]).ToList();

                    if (same.Count != 0)
                    {
                        foreach (Piece piece in _players[i].pieces)
                        {
                            if (piece.position == same[0])
                            {
                                piece.position = piece.startPosition;
                            }
                        }
                    }
                }
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
