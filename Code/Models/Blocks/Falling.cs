namespace BaconGameJam6.Models.Blocks
{
    using System;

    using BaconGameJam6.Models.States;

    public class Falling : IState
    {
        public Falling(Block block)
        {
            throw new System.NotImplementedException();
        }

        public bool IsComplete { get; private set; }

        public void Update(TimeSpan elapsedTime)
        {
            throw new NotImplementedException();
        }
    }
}