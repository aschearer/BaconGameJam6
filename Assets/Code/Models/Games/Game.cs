namespace BaconGameJam6.Models.Games
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BaconGameJam6.Models.Player;
    using BaconGameJam6.Models.Simulations;
    using BaconGameJam6.Models.States;

    public class Game
    {
        private readonly Queue<IState> states;

        private bool isPaused;

        public Game(params PlayerId[] players)
        {
            this.states = new Queue<IState>();
            this.Simulations = new Simulation[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                this.Simulations[i] = new Simulation(players[i], i);
                this.Simulations[i].Defeated += this.OnPlayerDefeated;
                this.Simulations[i].SuccessfulMatch += this.OnSuccessfulMatch;
            }
        }

        public Simulation[] Simulations { get; private set; }
        
        public bool InputEnabled
        {
            get
            {
                return (this.states.Count == 0);
            }            
        }
        
        public bool IsAnimating
        {
            get
            {
                return this.Simulations.Any(simulation => simulation.Board.IsAnimating);
            }
        }

        public bool IsPaused
        {
            get
            {
                return this.isPaused;
            }

            set
            {
                if (this.isPaused != value)
                {
                    this.isPaused = value;
                    foreach (var simulation in this.Simulations)
                    {
                        simulation.IsPaused = value;
                    }
                }
            }
        }

        public void Start()
        {
            this.states.Enqueue(new Starting(this));
        }

        public void Update(TimeSpan elapsedTime)
        {
            if (this.IsPaused)
            {
                return;
            }

            foreach (var simulation in this.Simulations)
            {
                simulation.Update(elapsedTime);
            }

            if (this.states.Count > 0)
            {
                this.states.Peek().Update(elapsedTime);
                if (this.states.Peek().IsComplete)
                {
                    this.states.Dequeue();
                }
            }
        }

        private void OnSuccessfulMatch(object sender, EventArgs e)
        {
            var matchingSimulation = sender as Simulation;
            foreach (var simulation in this.Simulations)
            {
                if (simulation != matchingSimulation)
                {
                    simulation.OnAddRow();
                }
            }
        }

        private void OnPlayerDefeated(object sender, EventArgs e)
        {
            if (this.Simulations.Count(simulation => simulation.IsDefeated) == this.Simulations.Length - 1)
            {
                this.states.Enqueue(new Ending(this));
            }
        }

        public void OnFire(PlayerId playerId)
        {
            this.Simulations.First(simulation => simulation.PlayerId == playerId).OnFire();
        }

        public void OnMoveLeft(PlayerId playerId)
        {
            this.Simulations.First(simulation => simulation.PlayerId == playerId).OnMoveLeft();
        }

        public void OnMoveRight(PlayerId playerId)
        {
            this.Simulations.First(simulation => simulation.PlayerId == playerId).OnMoveRight();
        }

        public void OnStopMoving(PlayerId playerId)
        {
            this.Simulations.First(simulation => simulation.PlayerId == playerId).OnStopMoving();
        }

        public void OnReload(PlayerId playerId)
        {
            this.Simulations.First(simulation => simulation.PlayerId == playerId).OnReload();
        }
    }
}