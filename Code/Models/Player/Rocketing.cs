namespace BaconGameJam6.Models.Player
{
    using System;

    using BaconGameJam6.Models.States;
    using BaconGameJam6.Models.Tweens;
    using BaconGameJam6.Models.Tweens.Easings;

    public class Rocketing : IState
    {
        private readonly ITween tween;

        public Rocketing(Missile block)
        {
            this.tween = TweenFactory.Tween(
                block.Y,
                block.Row,
                TimeSpan.FromSeconds(0.5),
                new QuadraticEasing().EaseOut);
        }

        public bool IsComplete
        {
            get
            {
                return this.tween.IsFinished;
            }
        }

        public void Update(TimeSpan elapsedTime)
        {
            this.tween.Update(elapsedTime);
        }
    }
}