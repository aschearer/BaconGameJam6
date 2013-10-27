namespace BaconGameJam6.Models.Player
{
    using System;

    using BaconGameJam6.Models.States;
    using BaconGameJam6.Models.Tweens;

    public class Sliding : IState
    {
        private readonly ITween tween;

        public Sliding(Ship block)
        {
            this.tween = TweenFactory.Tween(block.X, block.Column, TimeSpan.FromSeconds(0.1));
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