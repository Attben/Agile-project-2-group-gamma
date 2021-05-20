using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoLike
{
    public class Game
    {
        private List<Player> _players;
        private List<Tile> _board;

        public Game(int numberOfPlayers)
        {
            _players = new List<Player>(numberOfPlayers);
            AddPlayers(numberOfPlayers);
            _board = new List<Tile>();

        public void AddPlayers(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _players.Add(new Player((Colors)i));
            }
        }
    }
}
