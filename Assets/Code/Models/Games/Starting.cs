namespace BaconGameJam6.Models.Games
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BaconGameJam6.Models.Simulations;
    using BaconGameJam6.Models.States;

    public class Starting : IState
    {
        private Game game;
        private bool hasFilled = false;
        
        public Starting(Game game)
        {
            this.game = game;
            foreach (var simulation in game.Simulations)
            {
                simulation.Board.Clear();
            }
        }

        public bool IsComplete { get; private set; }

        public void Update(TimeSpan elapsedTime)
        {
            if (!this.game.IsAnimating)
            {
                if (!this.hasFilled)
                {
                    foreach (var simulation in game.Simulations)
                    {
                        simulation.Start();
                    }

                    this.hasFilled = true;
                }
                else
                {
                    this.IsComplete = true;
                }
            }
        }
    }
}