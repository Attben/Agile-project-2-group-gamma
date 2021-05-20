using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoLike
{
    public enum Colors
    {
        red, green, blue, yellow
    }

    class Player
    {
        private int _score;
        private Colors _color;
        private List<Piece> _pieces;

        public Player(Colors color)
        {
            _score = 0;
            _color = color;
            _pieces = new List<Piece>();

            for (int i = 0; i < 4; i++)
            {
                _pieces.Add(new Piece());
            }
        }

        public void ChangeScore(int amount)
        {
            _score += amount;
        }
    }
}
