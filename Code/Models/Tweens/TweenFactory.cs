namespace BaconGameJam6.Models.Tweens
{
    using System;

    using BaconGameJam6.Models.Tweens.Easings;

    public static class TweenFactory
    {
        #region Public Methods and Operators

        public static ITween Tween(float start, float target, TimeSpan targetRunTime)
        {
            return TweenFactory.Tween(start, target, targetRunTime, new LinearEasing().EaseIn);
        }

        public static ITween Tween(
            float start, 
            float target, 
            TimeSpan targetRunTime, 
            Tween.EasingFunction easingFunction)
        {
            return new Tween(start, target, targetRunTime, easingFunction);
        }

        #endregion
    }
}