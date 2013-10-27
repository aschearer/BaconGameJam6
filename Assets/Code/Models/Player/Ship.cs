namespace BaconGameJam6.Models.Player
{
    using System;
    using System.Collections.Generic;

    using BaconGameJam6.Models.Blocks;
    using BaconGameJam6.Models.Boards;
    using BaconGameJam6.Models.States;

    public enum PlayerId
    {
        One,
        Two
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
            this.CanMove = true;
        }

        public PlayerId PlayerId { get; private set; }

        public bool CanFire { get; private set; }

        public bool CanMove { get; set; }

        public void FireMainWeapon()
        {
            this.Board.Add(new Missile(this.Board, this, (int)Math.Round(this.X), this.Row));
            this.CanFire = false;
        }

        public void ReloadWeapon()
        {
            this.CanFire = true;
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

        protected override IEnumerable<IState> OnColumnChanged(int oldColumn, int newColumn)
        {
            this.CanMove = false;
            yield return new Sliding(this);
        }
    }
}