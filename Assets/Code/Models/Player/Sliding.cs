namespace BaconGameJam6.Models.Player
{
    using System;

    using BaconGameJam6.Models.States;
    using BaconGameJam6.Models.Tweens;
    using BaconGameJam6.Models.Tweens.Easings;

    public class Sliding : IState
    {
        private readonly Ship ship;

        private readonly ITween tween;

        public Sliding(Ship ship)
        {
            this.ship = ship;
            this.tween = TweenFactory.Tween(ship.X, ship.Column, TimeSpan.FromSeconds(0.1), new CubicEasing().EaseIn);
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
            this.ship.X = this.tween.Value;
        }
    }
}