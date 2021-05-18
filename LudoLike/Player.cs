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
        private string _color;
        private List<Piece> _pieces;

        public void ChangeScore(int amount)
        {
            _score += amount;
        }
    }
}
