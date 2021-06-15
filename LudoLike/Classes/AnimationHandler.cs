using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace LudoLike
{
    public enum EffectSize
    {
        Small, Medium, Big
    }

    /// <summary>
    /// Class for managing different animations.
    /// </summary>
    public class AnimationHandler
    {
        // Blinking effects on pieces variables
        public static float effectOpacity = 0;
        static float _opacityCoreValue = 5000;
        static bool effectOpacityUp = true;

        // Growing gloweffect on dice variables
        public static float GlowHolderAddedSize = 0;
        static bool GlowHolderAddedSizeUp = false;


        /// <summary>
        /// Takes a rectangle and creates a blinking animation with the chosen color.
        /// </summary>
        /// <param name="drawArgs"></param>
        /// <param name="targetRect"></param>
        /// <param name="color"></param>
        /// <param name="effectSize"></param>
        public static void DrawBlinkAnimation(CanvasAnimatedDrawEventArgs drawArgs, Rect targetRect, Color color, EffectSize effectSize)
        {
            Rect newTargetRect;
            switch (effectSize)
            {
                case EffectSize.Small:
                    newTargetRect = targetRect;
                    break;
                case EffectSize.Medium:
                    newTargetRect = new Rect(targetRect.X - 10, targetRect.Y - 10, targetRect.Width + 20, targetRect.Height + 20);
                    break;
                case EffectSize.Big:
                    newTargetRect = new Rect(targetRect.X - 20, targetRect.Y - 20, targetRect.Width + 40, targetRect.Height + 40);
                    break;
                default:
                    break;
            }

            ColorSourceEffect effect = new ColorSourceEffect()
            {
                Color = color
            };

            drawArgs.DrawingSession.DrawImage(effect,
                                             (float)newTargetRect.X,
                                             (float)newTargetRect.Y,
                                             newTargetRect,
                                             effectOpacity);
        }

        /// <summary>
        /// Updates the effectopacity to create a blinking effect. This is bound to the canvas update.
        /// </summary>
        static public void UpdateEffectOpacity()
        {
            switch (_opacityCoreValue)
            {
                case 8000:
                    effectOpacityUp = false;
                    break;
                case 5000:
                    effectOpacityUp = true;
                    break;
                default:
                    break;
            }

            if (effectOpacityUp)
            {
                _opacityCoreValue += 100f;
            }
            else
            {
                _opacityCoreValue -= 100f;
            }
            effectOpacity = _opacityCoreValue / 10000;
        }

        /// <summary>
        /// Updates the added size to the glow effect.
        /// </summary>
        static public void UpdateGlowHolderAddedSize()
        {
            switch (GlowHolderAddedSize)
            {
                case 100:
                    GlowHolderAddedSizeUp = false;
                    break;
                case 0:
                    GlowHolderAddedSizeUp = true;
                    break;
                default:
                    break;
            }

            if (GlowHolderAddedSizeUp)
            {
                GlowHolderAddedSize += 1f;
            }
            else
            {
                GlowHolderAddedSize -= 1f;
            }
        }
    }
}
