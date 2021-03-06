﻿namespace BaconGameJam6.Models.Simulations
{
    using System;
    using System.Linq;

    using BaconGameJam6.Models.Blocks;
    using BaconGameJam6.Models.Boards;
    using BaconGameJam6.Models.Player;

    using UnityEngine;

    public class Simulation
    {
        public event EventHandler<EventArgs> SuccessfulMatch;
        public event EventHandler<EventArgs> EmptiedBoard;
        public event EventHandler<EventArgs> Defeated;
        public event EventHandler<MatchEventArgs> BlockDestroyed;

        private readonly Board board;

        private readonly Ship ship;

        private readonly int simulationIndex;

        private bool hasStarted;

        public Simulation(PlayerId playerId, int simulationIndex)
        {
            this.PlayerId = playerId;
            this.board = new Board(5, 14);
            this.ship = new Ship(this.board, 2, this.board.NumberOfRows - 1, playerId);
            this.simulationIndex = simulationIndex;
        }

        public Board Board
        {
            get
            {
                return this.board;
            }
        }

        public int SimulationIndex
        {
            get
            {
                return this.simulationIndex;
            }
        }
  
        public bool HasPlayer { get; private set; }
        public bool IsActive
        {
            get
            {
                return this.hasStarted && !this.IsDefeated && this.HasPlayer;
            }
        }
        public bool IsDefeated { get; private set; }
        public bool IsPaused { get; set; }
        public PlayerId PlayerId { get; private set; }
        
        public void EnablePlayer()
        {
            this.HasPlayer = true;
        }
        
        public void DisablePlayer()
        {
            this.HasPlayer = false;
        }

        public void Start()
        {
            Utilities.PlaySound("BoardStart0");
            this.ship.Reset(2, this.board.NumberOfRows - 1);
            this.IsDefeated = false;
            this.board.Add(this.ship);
            this.board.Fill();
            this.hasStarted = true;
            this.ship.BlockDestroyed += this.OnBlockDestroyed;
            this.ship.Match += this.OnMatch;
            this.board.RowAdded += this.OnRowAdded;
        }

        public void Stop()
        {
            if (this.hasStarted)
            {
                this.hasStarted = false;
                this.ship.ResetOutstandingBlocks();
                this.ship.BlockDestroyed -= this.OnBlockDestroyed;
                this.ship.Match -= this.OnMatch;
                this.board.RowAdded -= this.OnRowAdded;
            }
        }

        public void OnMoveLeft()
        {
            if (this.ship.CanMove)
            {
                if (this.ship.Column > 0)
                {
                    this.ship.Column--;
                }
                else
                {
                    this.ship.Bump();
                }
            }
        }

        public void OnMoveRight()
        {
            if (this.ship.CanMove)
            {
                if (this.ship.Column < this.board.NumberOfColumns - 1)
                {
                    this.ship.Column++;
                }
                else
                {
                    this.ship.Bump();
                }
            }
        }

        public void OnStopMoving()
        {
            this.ship.CanMove = true;
        }

        public void OnFire()
        {
            if (this.ship.CanFire)
            {
                this.ship.FireMainWeapon();
            }
        }

        public void OnAddRow()
        {
            this.board.AddNewRow();
        }

        public void Update(TimeSpan elapsedTime)
        {
            if (this.hasStarted && this.board.NumberOfBlocks == 0)
            {
                this.EmptiedBoard(this, new EventArgs());
                this.board.SlamNewRows();
            }

            this.board.Update(elapsedTime);
            var pieces = this.board.ToArray();
            foreach (var piece in pieces)
            {
                piece.Update(elapsedTime);
            }
        }

        private void OnMatch(object sender, MatchEventArgs e)
        {
            if (!e.IsMatch)
            {
                this.OnAddRow();
            }
            else
            {
                this.SuccessfulMatch(this, new EventArgs());
            }
        }

        private void OnBlockDestroyed(object sender, MatchEventArgs e)
        {
            if (this.BlockDestroyed != null)
            {
                this.BlockDestroyed(this, e);
            }
        }

        public void OnReload()
        {
            this.ship.ReloadWeapon();
        }

        public void Slam()
        {
            Utilities.PlaySound("Slam0");
            this.board.SlamNewRows();
            if (this.board.Any(
                piece => piece is Block && piece.IsActive && piece.Row == (this.board.NumberOfRows - 1)))
            {
                this.OnDefeat();
            }
        }

        private void OnRowAdded(object sender, EventArgs e)
        {
            if (this.board.Any(
                piece => piece is Block && piece.IsActive && piece.Row == (this.board.NumberOfRows - 1)))
            {
                this.OnDefeat();
            }
        }

        private void OnDefeat()
        {
            this.IsDefeated = true;
            this.ship.Destroy();
            this.Defeated(this, new EventArgs());
            this.board.Shake();
            Utilities.PlaySound("Defeated0");
        }
    }
}