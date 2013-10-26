namespace BaconGameJam6.Models.Tweens.Easings
{
    using System;

    public class LinearEasing : IEasing
    {
        #region Public Methods and Operators

        public float EaseIn(float startingValue, float targetValue, TimeSpan targetRunTime, TimeSpan elapsedTime)
        {
            float delta = targetValue - startingValue;
            return (float)(((delta * elapsedTime.TotalSeconds) / targetRunTime.TotalSeconds) + startingValue);
        }

        public float EaseOut(float startingValue, float targetValue, TimeSpan targetRunTime, TimeSpan elapsedTime)
        {
            float delta = targetValue - startingValue;
            return (float)(((delta * elapsedTime.TotalSeconds) / targetRunTime.TotalSeconds) + startingValue);
        }

        #endregion
    }
}