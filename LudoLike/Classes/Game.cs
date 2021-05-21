using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LudoLike
{
    public class Game
    {
        private List<Player> _players;
        private List<Tile> _board;
        private int turn = 0;

        public Game()
        {
            _players = new List<Player>();
            _board = new List<Tile>();
            AddTiles(42); //Placeholder amount
        }

        public void AddPlayers(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _players.Add(new Player((Colors)i, _board[i])); // assumes first four tiles are Home/Nests tiles
            }
        }

        //need to be used before AddPlayers()
        public void AddTiles(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _board.Add(new Tile(i));
            }
        }

        //does not adapt to only 2 or 3 players 
        public void CheckTiles()
        {
            //get list of tiles
            List<int>[] list = {
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
                    List<int> same = list[turn].Intersect(list[i]).ToList();

                    if (same.Count != 0)
                    {
                        foreach (Piece piece in _players[i].pieces)
                        {
                            if (piece.position.index == same[0])
                            {
                                piece.position = _board[(int)_players[i].color];
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

        public void TileToNormal(int index)
        {
            _board[index] = new Tile(index);
        }

        //for testing purposes
        public bool Test()
        {
            AddTiles(20);
            AddPlayers(4);
            //10
            _players[0].pieces[0].position = _board[10];
            _players[1].pieces[0].position = _board[10];

            CheckTiles();

            if (_players[1].pieces[0].position == _board[10])
            {
                return false;
            }

            return true;
        }
    }
}
