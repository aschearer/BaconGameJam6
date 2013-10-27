namespace BaconGameJam6.Models.Games
{
    using System;
    using System.Collections.Generic;

    using BaconGameJam6.Models.Simulations;
    using BaconGameJam6.Models.States;

    public class Starting : IState
    {
        private Game game;
        
        public Starting(Game game)
        {
            this.game = game;
        }

        public bool IsComplete { get; private set; }

        public void Update(TimeSpan elapsedTime)
        {
            if (!this.game.IsAnimating)
            {
                this.IsComplete = true;
            }
        }
    }
}