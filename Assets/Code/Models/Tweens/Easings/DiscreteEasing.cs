namespace BaconGameJam6.Models.Tweens.Easings
{
    using System;

    public class DiscreteEasing : IEasing
    {
        #region Public Methods and Operators

        public float EaseIn(float startingValue, float targetValue, TimeSpan targetRunTime, TimeSpan elapsedTime)
        {
            int delta = (int)(targetValue - startingValue);
            float percentComplete = (float)(elapsedTime.TotalSeconds / targetRunTime.TotalSeconds);
            return (float)(startingValue + Math.Floor(delta * percentComplete));
        }

        public float EaseOut(float startingValue, float targetValue, TimeSpan targetRunTime, TimeSpan elapsedTime)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}