﻿namespace BaconGameJam6.Models.Simulations
{
    using System;
    using System.Linq;

    using BaconGameJam6.Models.Blocks;
    using BaconGameJam6.Models.Boards;
    using BaconGameJam6.Models.Player;

    public class Simulation
    {
        public event EventHandler<EventArgs> SuccessfulMatch;
        public event EventHandler<EventArgs> Defeated; 

        private readonly Board board;

        private readonly Ship ship;

        public Simulation()
        {
            this.board = new Board(5, 10);
            this.ship = new Ship(this.board, 3, 9, PlayerId.One);
            this.ship.Match += this.OnMatch;
            this.board.Add(this.ship);
            for (int col = 0; col < this.board.NumberOfColumns; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    this.board.Add(new Block(this.board, col, row, BlockType.Blue));
                }
            }
        }

        public Board Board
        {
            get
            {
                return this.board;
            }
        }

        public void OnMoveLeft()
        {
            if (this.ship.Column > 0)
            {
                this.ship.Column--;
            }
        }

        public void OnMoveRight()
        {
            if (this.ship.Column < this.board.NumberOfColumns - 1)
            {
                this.ship.Column++;
            }
        }

        public void OnFire()
        {
            this.ship.FireMainWeapon();
        }

        public void OnAddRow()
        {
            this.board.PushBlocksDown();
            this.board.AddNewRow();

            if (this.board.Any(piece => piece is Block && piece.Row == this.board.NumberOfRows - 1))
            {
                this.Defeated(this, new EventArgs());
            }
        }

        public void Update(TimeSpan elapsedTime)
        {
            foreach (var piece in this.board)
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
    }
}