namespace BaconGameJam6.Models.Player
{
    using System;

    using BaconGameJam6.Models.States;

    public class Sliding : IState
    {
        public Sliding(Ship ship)
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