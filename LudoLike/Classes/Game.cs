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

        public Game(int playerAmount)
        {
            _players = new List<Player>();
            _board = new List<Tile>();
            AddTiles(42); //Placeholder amount
            AddPlayers(playerAmount);
        }

        public void AddPlayers(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _players.Add(new Player((PlayerColors)i, _board[i])); // assumes first four tiles are Home/Nests tiles
            }
        }

        //need to be used before AddPlayers()
        public void AddTiles(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _board.Add(new Tile());
            }
        }

        //does not adapt to only 2 or 3 players 
        public void CheckPieces()
        {
            //get list of tiles where pieces stand on
            /*List<Tile>[] list = {
            _players[0].ReturnPiecePostitions(),
            _players[1].ReturnPiecePostitions(),
            _players[2].ReturnPiecePostitions(),
            _players[3].ReturnPiecePostitions()
            };*/

            List<List<Tile>> list = new List<List<Tile>>();

            for (int i = 0; i < _players.Count; i++)
            {
                list.Add(_players[i].ReturnPiecePostitions());
            }

            //compare lists
            //take this turns player pieces locations and comapre to other players
            //List<Tile> same = list[0].Union(list[1]).ToList(); - checks if same vlaues exists in both lists, returns said values into same list // loop to check each piece to send to nest/home

            /*for (int i = 0; i < _players.Count; i++)
            {
                if (i != turn)
                {
                    //creates i list of tiles that two pieces share
                    List<int> same = list[turn].Intersect(list[i]).ToList();
                    //checks if current player is in same position as another
                    if (same.Count != 0)
                    {

                        //looks for piece of another player to send back to home/nest
                        foreach (Piece piece in _players[i].pieces)
                        {
                            if (piece.position.index == same[0])
                            {
                                //moves piece to nest/home
                                piece.position = _board[(int)_players[i].color]; // might wanna use the move method of piece when implemented
                                _players[turn].ChangeScore(100); //100 points is placeholder
                                _players[i].ChangeScore(-100);
                            }
                        }



                    }
                }
            }*/

            for (int i = 0; i < _players.Count; i++)
            {
                if (i != turn)
                {
                    //creates i list of tiles that two pieces share
                    List<Tile> same = list[turn].Intersect(list[i]).ToList();
                    //checks if current player is in same position as another
                    if (same.Count != 0)
                    {

                        //looks for piece of another player to send back to home/nest
                        foreach (Piece piece in _players[i].pieces)
                        {
                            if (piece.position == same[0])
                            {
                                //moves piece to nest/home
                                piece.position = _board[(int)_players[i].color]; // might wanna use the move method of piece when implemented
                                _players[turn].ChangeScore(100); //100 points is placeholder
                                _players[i].ChangeScore(-100);
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

        public void TileToNormal(int index)// idk maybe tile instead 
        {
            //exchanges the special tile to a normal one
            _board[index] = new Tile();
            //move piece to new tile
            _players[turn].pieces[100].position = _board[index]; //100 is a placeholder //idk how to get right piece
        }

        //for testing purposes
        public bool Test()
        {
            //tile 10 see if checkpieces() work
            _players[0].pieces[0].position = _board[10];
            _players[1].pieces[0].position = _board[10];
            _players[1].pieces[1].position = _board[10];
            _players[1].pieces[2].position = _board[10];

            CheckPieces();

            if (_players[1].pieces[0].position == _board[10])
            {
                return false;
            }

            return true;
        }
    }
}
