namespace BaconGameJam6.Models.Games
{
    using System;
    using System.Collections.Generic;

    using BaconGameJam6.Models.Simulations;
    using BaconGameJam6.Models.States;

    public class Ending : IState
    {
        private Game game;
        private double endingTime = 0.0;
        private static readonly double DelayBeforeReset = 1.0;

        public Ending(Game game)
        {
            this.game = game;
            foreach (var simulation in game.Simulations)
            {
                simulation.Stop();
            }
        }

        public bool IsComplete { get; private set; }

        public void Update(TimeSpan elapsedTime)
        {
            if (!this.game.IsAnimating)
            {
                this.endingTime += elapsedTime.TotalSeconds;
                if (this.endingTime >= DelayBeforeReset)
                {
                    this.IsComplete = true;
                    this.game.Restart();
                }
            }
        }
    }
}