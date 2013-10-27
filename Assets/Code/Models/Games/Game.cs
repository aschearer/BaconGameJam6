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
        private bool isActive;

        public Game()
        {
            this.states = new Queue<IState>();
        }
        
        public void Start(int players)
        {
            this.Simulations = new Simulation[players];
            for (int i = 0; i < players; i++)
            {
                this.Simulations[i] = new Simulation((PlayerId)i, i);
                this.Simulations[i].Defeated += this.OnPlayerDefeated;
                this.Simulations[i].SuccessfulMatch += this.OnSuccessfulMatch;
                this.Simulations[i].EmptiedBoard += this.OnBoardEmptied;
            }
   
            this.isActive = true;
            this.states.Enqueue(new Starting(this));
        }
        
        public void Restart()
        {
            Utilities.PlaySound("Eruption0");
            this.states.Enqueue(new Starting(this));
        }
        
        public void Stop()
        {
            if (this.Simulations != null)
            {
                foreach (Simulation simulation in this.Simulations)
                {
                    simulation.Stop();
                    simulation.Defeated -= this.OnPlayerDefeated;
                    simulation.SuccessfulMatch -= this.OnSuccessfulMatch;
                    simulation.EmptiedBoard -= this.OnBoardEmptied;
                }
                this.Simulations = null;
            }

            this.isActive = false;
        }

        public Simulation[] Simulations { get; private set; }
        
        public bool CanUpdate
        {
            get
            {
                return this.isActive;
            }
        }
        
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
            // Figure out the sender's position in the simulations
            var matchingSimulation = sender as Simulation;
            var iSimulation = 0;
            bool found = false;
            for (; iSimulation < this.Simulations.Length; ++iSimulation)
            {
                if (this.Simulations[iSimulation] == matchingSimulation)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                return;
            }
            
            // Attack the next simulation that isn't defeated
            var iSimulationToAttack = (iSimulation + 1) % this.Simulations.Length;
            for (; iSimulationToAttack != iSimulation; iSimulationToAttack = (iSimulationToAttack + 1) % this.Simulations.Length)
            {
                if (!this.Simulations[iSimulationToAttack].IsDefeated)
                {
                    this.Simulations[iSimulationToAttack].OnAddRow();
                    break;
                }
            }
        }

        private void OnBoardEmptied(object sender, EventArgs e)
        {
            var matchingSimulation = sender as Simulation;
            foreach (var simulation in this.Simulations)
            {
                if (simulation != matchingSimulation)
                {
                    simulation.Slam();
                }
            }
        }

        private void OnPlayerDefeated(object sender, EventArgs e)
        {
            if (this.Simulations.Count(simulation => simulation.IsDefeated) >= this.Simulations.Length - 1)
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