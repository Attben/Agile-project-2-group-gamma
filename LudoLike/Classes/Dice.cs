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
    /// <summary>
    /// Represents a Dice and its functions inside the game.
    /// </summary>
    class Dice
    {
        private int _animationTimer = 0;
        private readonly int _min, _max; //Range of possible values when rolling this die.
        private CanvasBitmap CurrentDieImage;
        private readonly Random _prng;
        private readonly float _diceWidth = 200;
        private readonly float _diceHeight = 200;
        public Rect DiceHolder;
        private Rect _glowHolder;
        public static List<CanvasBitmap> GlowEffects = new List<CanvasBitmap>();

        //Possible improvement: Support rendering of arbitrary values (currently only works with 1-6).
        public static readonly CanvasBitmap[] DiceImages = new CanvasBitmap[6];
        public static CanvasBitmap SpinningDieImage;
        public static CanvasBitmap StandardDieImage;

        public Dice(int min = 0, int max = 6)
        {
            _min = min;
            _max = max;
            _prng = new Random();
            CurrentDieImage = Dice.StandardDieImage;
        }

        /// <summary>
        /// Draws the Die and its glowing effect.
        /// </summary>
        /// <param name="drawArgs"></param>
        /// <param name="playerTurn"></param>
        public void Draw(CanvasAnimatedDrawEventArgs drawArgs, int playerTurn)
        {
            DiceHolder = new Rect(Scaling.bWidth - Scaling.Xpos(_diceWidth * 2), 
                                   Scaling.bHeight - Scaling.Ypos(_diceHeight * 2),
                                   Scaling.Xpos(200), Scaling.Ypos(200));
            _glowHolder = new Rect(Scaling.bWidth - Scaling.Xpos(_diceWidth * 2) - Scaling.Xpos(_diceWidth / 2) - Scaling.Xpos(AnimationHandler.GlowHolderAddedSize / 2), 
                                   Scaling.bHeight - Scaling.Ypos(_diceHeight * 2) - Scaling.Ypos(_diceHeight / 2) - Scaling.Ypos(AnimationHandler.GlowHolderAddedSize / 2), 
                                   Scaling.Xpos(_diceHeight * 2 + AnimationHandler.GlowHolderAddedSize), 
                                   Scaling.Ypos(_diceHeight * 2 + AnimationHandler.GlowHolderAddedSize));
            if (_animationTimer == 0)
            {
                drawArgs.DrawingSession.DrawImage(GlowEffects[playerTurn], _glowHolder);
                drawArgs.DrawingSession.DrawImage(CurrentDieImage, DiceHolder);
            }
            else
            {
                drawArgs.DrawingSession.DrawImage(GlowEffects[playerTurn], _glowHolder);
                drawArgs.DrawingSession.DrawImage(Dice.SpinningDieImage, DiceHolder);
                --_animationTimer;
            }
        }

        /// <summary>
        /// Rolls in range of the dice min - max range. Updates the dice picture according the the cast.
        /// </summary>
        /// <returns>The cast value.</returns>
        public int Roll()
        {
            CurrentDieImage = Dice.SpinningDieImage;
            _animationTimer = 30;
            int result = _prng.Next(_min, _max);
            CurrentDieImage = Dice.DiceImages[result];

            return result;
        }

        /// <summary>
        /// Sets the current die image to standard.
        /// </summary>
        public void SetStandardDieImage()
        {
            CurrentDieImage = StandardDieImage;
        }
    }
}
