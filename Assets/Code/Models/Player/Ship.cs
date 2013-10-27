namespace BaconGameJam6.Models.Player
{
    using System;
    using System.Collections.Generic;

    using BaconGameJam6.Models.Blocks;
    using BaconGameJam6.Models.Boards;

    using UnityEngine;

    public enum PlayerId
    {
        One,
        Two
    }

    public enum ShipMovement
    {
        None,
        Left,
        Right
    }

    public class Ship : BoardPiece
    {
        private const float Speed = 8f;

        public event EventHandler<MatchEventArgs> Match;

        private readonly List<Block> outstandingBlocks; 

        public Ship(Board board, int col, int row, PlayerId playerId)
            : base(board, col, row)
        {
            this.PlayerId = playerId;
            this.outstandingBlocks = new List<Block>();
            this.CanFire = true;
        }

        public PlayerId PlayerId { get; private set; }

        public ShipMovement Movement { get; set; }

        public bool IsMoving
        {
            get
            {
                return this.Movement != ShipMovement.None;
            }
        }

        public bool CanFire { get; private set; }

        public void FireMainWeapon()
        {
            this.Board.Add(new Missile(this.Board, this, (int)Math.Round(this.X), this.Row));
            this.CanFire = false;
        }

        public void ReloadWeapon()
        {
            this.CanFire = true;
        }

        protected override void OnUpdate(TimeSpan elapsedTimeSpan)
        {
            switch (this.Movement)
            {
                case ShipMovement.None:
                    break;
                case ShipMovement.Left:
                    this.X = Math.Max(0, Math.Min(this.Board.NumberOfColumns - 1,  this.X - Ship.Speed * (float)elapsedTimeSpan.TotalSeconds));
                    this.Column = (int)this.X;

                    break;
                case ShipMovement.Right:
                    this.X = Math.Max(0, Math.Min(this.Board.NumberOfColumns - 1, this.X + Ship.Speed * (float)elapsedTimeSpan.TotalSeconds));
                    this.Column = (int)this.X;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void RecordHit(Block block)
        {
            this.outstandingBlocks.Add(block);
            if (this.outstandingBlocks.Count == 3 || this.outstandingBlocks.Count == 2 && block.BlockType != this.outstandingBlocks[0].BlockType)
            {
                this.Match(this, new MatchEventArgs(this.outstandingBlocks.ToArray()));
                this.outstandingBlocks.Clear();
            }
        }
    }
}