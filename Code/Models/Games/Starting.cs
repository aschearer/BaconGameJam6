namespace BaconGameJam6.Models.Games
{
    using System;
    using System.Collections.Generic;

    using BaconGameJam6.Models.Simulations;
    using BaconGameJam6.Models.States;

    public class Starting : IState
    {
        public Starting(IEnumerable<Simulation> simulations)
        {
            this.IsComplete = true;
        }

        public bool IsComplete { get; private set; }

        public void Update(TimeSpan elapsedTime)
        {
        }
    }
}