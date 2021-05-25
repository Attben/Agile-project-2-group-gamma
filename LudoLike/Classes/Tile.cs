using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoLike
{
    class Tile
    {
        public int index;

        public Tile(int index)
        {
            this.index = index;
        }

        public virtual void TileEvent()
        {
            //Intentionally left blank in base class.
        }

        public void Draw(CanvasAnimatedDrawEventArgs drawArgs)
        {

        }
    }
}
