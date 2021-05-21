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
        public Colors color;
        public List<Piece> pieces;

        public Player(Colors color, Tile board)
        {
            _score = 0;
            this.color = color;
            pieces = new List<Piece>();

            for (int i = 0; i < 4; i++)
            {
                pieces.Add(new Piece(board));
            }
        }

        public void ChangeScore(int amount)
        {
            _score += amount;
        }

        public List<int> returnPiecePostitions() // return list of tiles
        {
            List<int> list = new List<int>();
            foreach (Piece piece in pieces)
            {
                list.Add(piece.position.index);
            }

            return list;
        }
    }
}
