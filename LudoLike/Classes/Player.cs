using LudoLike.Classes;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Core;

namespace LudoLike
{
    public enum PlayerColors
    {
        Red, Blue, Yellow, Green
    }

    /// <summary>
    /// Represents a player in the Ludo game.
    /// </summary>
    public class Player
    {
        public static Dictionary<PlayerColors, Windows.UI.Color> WindowsPlayerColors = new Dictionary<PlayerColors, Windows.UI.Color>()
        {
            {PlayerColors.Red, Windows.UI.Colors.Red },
            {PlayerColors.Blue, Windows.UI.Colors.Blue },
            {PlayerColors.Yellow, Windows.UI.Colors.Yellow },
            {PlayerColors.Green, Windows.UI.Colors.Green }
        };
        public int Score { get; private set; }
        public PlayerColors PlayerColor;
        public Windows.UI.Color UIcolor;
        public List<Piece> _pieces;

        public static List<MediaSource> PieceCollisionSounds = new List<MediaSource>();
        public static List<MediaSource> PieceMovingSounds = new List<MediaSource>();

        private readonly TurnHistoryHandler _turnHistory;
        

        public Player(PlayerColors color, List<Vector2> startPositions, TurnHistoryHandler turnHistory)
        {
            Score = 0;
            PlayerColor = color;
            _turnHistory = turnHistory;

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

        /// <summary>
        /// Add a move to the turn history.
        /// This method is mostly a workaround to avoid being forced to add a TurnHistoryHandler
        /// to all of the constructors in the various Tile classes.
        /// </summary>
        /// <param name="move">A string describing the move that was made.</param>
        public void AddMoveToTurnHistory(string move)
        {
            _turnHistory.Add(this, move);
        }

        public void ChangeScore(int amount)
        {
            Score += amount;
            AddMoveToTurnHistory($"💲{amount}");
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
                        nextPosition = path.IndexOf(tile) + 1;
                    }
                }
                try
                {
                    _pieces[0].Move(path[nextPosition]);
                    SoundMixer.PlayRandomSound(PieceMovingSounds);
                }
                catch (ArgumentOutOfRangeException)
                {
                    _pieces[0].Move(path[44]);
                    break;
                }
            }
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
