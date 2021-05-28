using LudoLike.Classes;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;

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
        public List<Piece> _pieces;

        public static List<MediaSource> PieceMovingSounds = new List<MediaSource>(); 

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

            _pieces = new List<Piece>();

            for (int i = 0; i < 4; i++)
            {
                _pieces.Add(new Piece(startPositions[i], color));
            }
        }

        public void ChangeScore(int amount)
        {
            Score += amount;
        }

        public void DrawPieces(CanvasAnimatedDrawEventArgs drawArgs)
        {
            foreach (Piece p in _pieces)
            {
                p.Draw(drawArgs, LudoBoard.TileGridPositions[p.position]);
            }
        }

        public void MovePiece(int diceRoll)
        {
            int nextPosition;
            var path = LudoBoard.PlayerPaths[(int)PlayerColor];
            for (int i = 0; i < diceRoll; i++)
            {
                nextPosition = 0;
                foreach (Vector2 tile in path)
                {
                    if (_pieces[0].position == tile)
                    {
                        nextPosition = path.IndexOf(tile);
                    }
                }
                try
                {
                    _pieces[0].Move(path[nextPosition + 1]);
                }
                catch (ArgumentOutOfRangeException)
                {
                    _pieces[0].Move(path[44]);
                }
            }

            SoundMixer.PlaySound(PieceMovingSounds[new Random().Next(PieceMovingSounds.Count)]);
            // Detta måste anpassas till den nya tilegridlogiken
            //_piece.Move(100f, 100f);
        }

        public List<Vector2> ReturnPiecePostitions() // return list of tiles
        {
            List<Vector2> list = new List<Vector2>();
            foreach (Piece piece in _pieces)
            {
                list.Add(piece.position);
            }

            return list;
        }
    }
}
