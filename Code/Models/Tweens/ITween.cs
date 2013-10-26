namespace BaconGameJam6.Models.Tweens
{
    using System;

    /// <summary>
    ///     A utility class which makes it easy to interpolate between two values.
    /// </summary>
    /// <remarks>
    ///     This class is used to create simple animations programatically. For instance
    ///     to fade a screen into view or scale a button on click.
    /// </remarks>
    public interface ITween
    {
        #region Public Properties

        int CurrentIteration { get; }

        TimeSpan Delay { get; set; }

        DelayType DelayType { get; set; }

        Action<ITween, float> FinishCallback { get; set; }

        bool IsFinished { get; }

        bool IsPaused { get; set; }

        TimeSpan RemainingDelay { get; }

        Repeat Repeats { get; set; }

        Action<ITween, float> RepeatsCallback { get; set; }

        Tween.EasingFunction ReverseEasing { get; set; }

        TimeSpan Runtime { get; }

        Action<ITween, float> UpdateCallback { get; set; }

        float Value { get; }

        bool YoYos { get; set; }

        #endregion

        #region Public Methods and Operators

        ITween Copy();

        void Restart();

        void Update(TimeSpan elapsedGameTime);

        #endregion
    }
}