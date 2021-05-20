using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoLike
{
    class Dice
    {
        public int AnimationTimer { get; set; } = 0;
        public static readonly CanvasBitmap[] DiceImages = new CanvasBitmap[6];
        public static CanvasBitmap SpinningDieImage;

    }
}
