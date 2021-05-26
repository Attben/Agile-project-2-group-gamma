using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LudoLike
{
    public enum PlayerColors
    {
        red, blue, yellow, green
    }

    public class Player
    {
        private int _score;
        public PlayerColors color;
        public List<Piece> pieces;

        public Player(PlayerColors color, List<Vector2> startPositions)
        {
            _score = 0;
            this.color = color;
            pieces = new List<Piece>();

            for (int i = 0; i < 4; i++)
            {
                pieces.Add(new Piece(startPositions[i], color));
            }
        }

        public void ChangeScore(int amount)
        {
            _score += amount;
        }

        public List<Vector2> ReturnPiecePostitions() // return list of tiles
        {
            List<Vector2> list = new List<Vector2>();
            foreach (Piece piece in pieces)
            {
                list.Add(piece.position);
            }

            return list;
        }
    }
}
