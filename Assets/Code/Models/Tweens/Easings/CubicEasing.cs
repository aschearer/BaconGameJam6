namespace BaconGameJam6.Models.Tweens.Easings
{
    using System;

    public class CubicEasing : IEasing
    {
        #region Public Methods and Operators

        public float EaseIn(float startingValue, float targetValue, TimeSpan targetRunTime, TimeSpan elapsedTime)
        {
            float time = (float)(elapsedTime.TotalSeconds / targetRunTime.TotalSeconds);
            float delta = targetValue - startingValue;
            return (delta * time * time * time) + startingValue;
        }

        public float EaseOut(float startingValue, float targetValue, TimeSpan targetRunTime, TimeSpan elapsedTime)
        {
            float time = (float)(elapsedTime.TotalSeconds / targetRunTime.TotalSeconds);
            time -= 1;
            float delta = targetValue - startingValue;
            return (delta * ((time * time * time) + 1)) + startingValue;
        }

        #endregion
    }
}