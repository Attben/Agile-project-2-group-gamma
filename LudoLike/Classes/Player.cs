using Microsoft.Graphics.Canvas.UI.Xaml;
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
        Red, Blue, Yellow, Green
    }

    public class Player
    {
        public int Score { get; private set; }
        public PlayerColors PlayerColor;
        public Windows.UI.Color UIcolor;
        public List<Piece> pieces;

        public Player(PlayerColors color, List<Vector2> startPositions)
        {
            Score = 0;
            this.PlayerColor = color;

            switch (color)
            {
                case PlayerColors.Red:
                    UIcolor = Windows.UI.Colors.Red;
                    break;
                case PlayerColors.Green:
                    UIcolor = Windows.UI.Colors.LawnGreen;
                    break;
                case PlayerColors.Blue:
                    UIcolor = Windows.UI.Colors.Blue;
                    break;
                case PlayerColors.Yellow:
                    UIcolor = Windows.UI.Colors.Yellow;
                    break;
            }

            pieces = new List<Piece>();

            for (int i = 0; i < 4; i++)
            {
                pieces.Add(new Piece(startPositions[i], color));
            }
        }

        public void ChangeScore(int amount)
        {
            Score += amount;
        }

        public void Draw(CanvasAnimatedDrawEventArgs drawArgs)
        {
            //TODO: Move player drawing here.
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
