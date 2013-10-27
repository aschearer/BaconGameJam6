namespace BaconGameJam6.Models.Games
{
    using System;
    using System.Collections.Generic;

    using BaconGameJam6.Models.Simulations;
    using BaconGameJam6.Models.States;

    public class Ending : IState
    {
        public Ending(IEnumerable<Simulation> simulations)
        {
        }

        public bool IsComplete { get; private set; }

        public void Update(TimeSpan elapsedTime)
        {
        }
    }
}