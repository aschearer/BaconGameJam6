namespace BaconGameJam6.Models.Tweens.Easings
{
    using System;

    public interface IEasing
    {
        #region Public Methods and Operators

        float EaseIn(float startingValue, float targetValue, TimeSpan targetRunTime, TimeSpan elapsedTime);

        float EaseOut(float startingValue, float targetValue, TimeSpan targetRunTime, TimeSpan elapsedTime);

        #endregion
    }
}