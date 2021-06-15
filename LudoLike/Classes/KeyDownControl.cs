using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace LudoLike
{
    /// <summary>
    /// A control necessary for being able to press keys on a canvas.
    /// </summary>
    public class KeyDownControl : Control
    {
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            base.OnKeyDown(e);
        }
    }
}
