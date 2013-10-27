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
        public event EventHandler<MatchEventArgs> Match;

        private readonly List<Block> outstandingBlocks; 

        public Ship(Board board, int col, int row, PlayerId playerId)
            : base(board, col, row)
        {
            this.PlayerId = playerId;
            this.outstandingBlocks = new List<Block>();
        }

        public PlayerId PlayerId { get; private set; }

        public void FireMainWeapon()
        {
            this.Board.Add(new Missile(this.Board, this, this.Column, this.Row));
        }

        protected override IEnumerable<IState> OnColumnChanged(int oldColumn, int newColumn)
        {
            yield return new Sliding(this);
        }

        public void RecordHit(Block block)
        {
            this.outstandingBlocks.Add(block);
            if (this.outstandingBlocks.Count == 3)
            {
                this.Match(this, new MatchEventArgs(this.outstandingBlocks.ToArray()));
                this.outstandingBlocks.Clear();
            }
        }
    }
}