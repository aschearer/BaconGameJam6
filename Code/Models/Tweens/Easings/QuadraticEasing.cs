namespace BaconGameJam6.Models.Tweens.Easings
{
    using System;

    public class QuadraticEasing : IEasing
    {
        #region Public Methods and Operators

        public float EaseIn(float startingValue, float targetValue, TimeSpan targetRunTime, TimeSpan elapsedTime)
        {
            float time = (float)(elapsedTime.TotalSeconds / targetRunTime.TotalSeconds);
            float delta = targetValue - startingValue;
            return (delta * time * time) + startingValue;
        }

        public float EaseOut(float startingValue, float targetValue, TimeSpan targetRunTime, TimeSpan elapsedTime)
        {
            float time = (float)(elapsedTime.TotalSeconds / targetRunTime.TotalSeconds);
            float delta = targetValue - startingValue;
            return (-delta * time * (time - 2f)) + startingValue;
        }

        #endregion
    }
}