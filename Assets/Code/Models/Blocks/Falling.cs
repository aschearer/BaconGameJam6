namespace BaconGameJam6.Models.Blocks
{
    using System;

    using BaconGameJam6.Models.States;
    using BaconGameJam6.Models.Tweens;

    public class Falling : IState
    {
        private readonly Block block;

        private readonly ITween tween;

        public Falling(Block block)
        {
            this.block = block;
            this.tween = TweenFactory.Tween(block.Y, block.Row, TimeSpan.FromSeconds(0.1));
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
            this.block.Y = this.tween.Value;
        }
    }
}