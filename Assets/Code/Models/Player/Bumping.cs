namespace BaconGameJam6.Models.Player
{
    using System;

    using BaconGameJam6.Models.States;
    using BaconGameJam6.Models.Tweens;

    public class Bumping : IState
    {
        private readonly Ship ship;

        private readonly ITween tween;

        public Bumping(Ship ship)
        {
            this.ship = ship;
            var targetX = ship.Column + (ship.Column == 0 ? -0.2f : 0.2f);
            this.tween = TweenFactory.Tween(ship.X, targetX, TimeSpan.FromSeconds(0.1));
            this.tween.YoYos = true;
            this.tween.Repeats = Repeat.Once;
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