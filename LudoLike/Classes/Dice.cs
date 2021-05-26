using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace LudoLike
{
    class Dice
    {
        private int _animationTimer = 0;
        private int _min, _max; //Range of possible values when rolling this die.
        private CanvasBitmap CurrentDieImage;
        private Random _prng;
        private float _diceWidth = 200;
        private float _diceHeight = 200;
        private Rect _diceHolder;

        //Possible improvement: Support rendering of arbitrary values (currently only works with 1-6).
        public static readonly CanvasBitmap[] DiceImages = new CanvasBitmap[6];
        public static CanvasBitmap SpinningDieImage;

        public Dice(int min = 0, int max = 6)
        {
            _min = min;
            _max = max;
            _prng = new Random();
            CurrentDieImage = Dice.DiceImages[2];
        }

        public void Draw(CanvasAnimatedDrawEventArgs drawArgs)
        {
            _diceHolder = new Rect(Scaling.bWidth - Scaling.Xpos(_diceWidth * 2), Scaling.bHeight - Scaling.Ypos(_diceHeight * 4), Scaling.Xpos(200), Scaling.Ypos(200));
            if (_animationTimer == 0)
            {
                drawArgs.DrawingSession.DrawImage(CurrentDieImage, _diceHolder);
            }
            else
            {
                drawArgs.DrawingSession.DrawImage(Dice.SpinningDieImage, _diceHolder);
                --_animationTimer;
            }
        }

        public int Roll()
        {
            CurrentDieImage = Dice.SpinningDieImage;
            _animationTimer = 30;
            int result = _prng.Next(_min, _max);
            CurrentDieImage = Dice.DiceImages[result];

            return result;
        }
    }
}
