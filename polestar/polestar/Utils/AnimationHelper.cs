using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace polestar.Utils
{
    public static class AnimationHelper
    {
        public static void FadeTo(UIElement element, double toOpacity, int durationMs = 300, IEasingFunction? easing = null)
        {
            DoubleAnimation anim = new DoubleAnimation
            {
                To = toOpacity,
                Duration = TimeSpan.FromMilliseconds(durationMs),
                EasingFunction = easing ?? new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            element.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        public static void RotateTo(UIElement element, double toAngle, int durationMs = 600)
        {
            if (element.RenderTransform is not RotateTransform rt)
            {
                rt = new RotateTransform(0);
                element.RenderTransformOrigin = new Point(0.5, 0.5);
                element.RenderTransform = rt;
            }

            DoubleAnimation anim = new DoubleAnimation
            {
                To = toAngle,
                Duration = TimeSpan.FromMilliseconds(durationMs),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };
            rt.BeginAnimation(RotateTransform.AngleProperty, anim);
        }

        public static void ScaleAndFade(UIElement element, double toScale, double toOpacity, int durationMs = 600)
        {
            if (element.RenderTransform is not ScaleTransform st)
            {
                st = new ScaleTransform(1.0, 1.0);
                element.RenderTransformOrigin = new Point(0.5, 0.5);
                element.RenderTransform = st;
            }

            DoubleAnimation scaleAnim = new DoubleAnimation
            {
                To = toScale,
                Duration = TimeSpan.FromMilliseconds(durationMs),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };

            DoubleAnimation fadeAnim = new DoubleAnimation
            {
                To = toOpacity,
                Duration = TimeSpan.FromMilliseconds(durationMs),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };

            st.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            st.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);
            element.BeginAnimation(UIElement.OpacityProperty, fadeAnim);
        }
    }
}
