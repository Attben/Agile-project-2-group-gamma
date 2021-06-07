using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace LudoLike.Classes
{
    /// <summary>
    /// Class to keep track of and display information about 
    /// the actions players take during their turns.
    /// </summary>
    public class TurnHistoryHandler
    {
        private readonly int _maxHistoryLength = 20;
        private CanvasTextFormat _textFormat;
        private Queue<Tuple<Player, string>> _actions;
        private Rect _guiBox;

        public TurnHistoryHandler(CanvasTextFormat textFormat, Rect guiBox)
        {
            _actions = new Queue<Tuple<Player, string>>();
            _textFormat = textFormat;
            _guiBox = guiBox;
        }

        public void Add(Player player, string action)
        {
            if(_actions.Count >= _maxHistoryLength)
            {
                _actions.Dequeue();
            }

            _actions.Enqueue(new Tuple<Player, string>(player, action));
        }

        public void Draw(CanvasAnimatedDrawEventArgs drawArgs)
        {
            drawArgs.DrawingSession.FillRoundedRectangle(
                _guiBox, 10, 10, Windows.UI.Colors.DarkGray); //10, 10 are x- and y-radii for rounded corners
            for (int n = 0; n < _actions.Count; ++n)
            {
                int reverseIndex = _actions.Count - 1 - n;
                drawArgs.DrawingSession.DrawText(
                    _actions.ElementAt(reverseIndex).Item2, //Item2 is a string describing what happened in the turn.
                    (float)_guiBox.X + 30f,
                    (float)_guiBox.Y + _textFormat.FontSize * (n + 1),
                    _actions.ElementAt(reverseIndex).Item1.UIcolor); //Item1 is the player who performed the action.
            }
        }
    }
}
