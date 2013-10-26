namespace BaconGameJam6.Models.States
{
    using System;

    public interface IState
    {
        bool IsComplete { get; }

        void Update(TimeSpan elapsedTime);
    }
}