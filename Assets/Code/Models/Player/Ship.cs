namespace BaconGameJam6.Models.Player
{
    using System;
    using System.Collections.Generic;

    using Assets.Code.Models.Player;

    using BaconGameJam6.Models.Blocks;
    using BaconGameJam6.Models.Boards;
    using BaconGameJam6.Models.States;

    public enum PlayerId
    {
        One,
        Two,
        Three,
        Four
    }

    public class Ship : BoardPiece
    {
        private const float Speed = 8f;

        public event EventHandler<MatchEventArgs> Match;
        public event EventHandler<MatchEventArgs> BlockDestroyed;

        private readonly List<Block> outstandingBlocks;

        public Ship(Board board, int col, int row, PlayerId playerId)
            : base(board, col, row)
        {
            this.PlayerId = playerId;
            this.outstandingBlocks = new List<Block>();
            this.CanFire = true;
            this.CanMove = true;
            this.StartingColumn = col;
        }

        public PlayerId PlayerId { get; private set; }

        public bool CanFire { get; private set; }

        public bool CanMove { get; set; }

        public void FireMainWeapon()
        {
            //Utilities.PlaySound("FireWeapon0");
            //this.Board.Add(new Missile(this.Board, this, (int)Math.Round(this.X), this.Row));
            this.CanFire = false;
        }

        public int StartingColumn { get; private set; }

        public void ReloadWeapon()
        {
            this.CanFire = true;
        }

        public void ResetOutstandingBlocks()
        {
            this.outstandingBlocks.Clear();
            UpdateLights(false);
        }

        public void Bump()
        {
            this.CanMove = false;
            this.AddState(new Bumping(this));
        }

        public void RecordHit(Block block)
        {
            this.outstandingBlocks.Add(block);
            UpdateLights(false);

            if (this.outstandingBlocks.Count == 3 || this.outstandingBlocks.Count == 2 && block.BlockType != this.outstandingBlocks[0].BlockType)
            {
                MatchEventArgs args = new MatchEventArgs(this.outstandingBlocks.ToArray(), false);
                if (this.Match != null)
                {
                    this.Match(this, args);
                }

                this.outstandingBlocks.Clear();
                if (!args.IsMatch)
                {
                    this.outstandingBlocks.Add(block);
                }

                UpdateLights(true);
            }
        }

        protected override IEnumerable<IState> OnColumnChanged(int oldColumn, int newColumn)
        {
            Utilities.PlaySound("Move0");
            this.CanMove = false;
            yield return new Sliding(this);
        }

        protected override IEnumerable<IState> OnDestroy()
        {
            yield return new Destroying(this, this.Board);
        }

        private void UpdateLights(bool animate)
        {
            if (this.BlockDestroyed != null)
            {
                this.BlockDestroyed(this, new MatchEventArgs(this.outstandingBlocks.ToArray(), animate));
            }

            if (!animate && this.outstandingBlocks.Count > 0)
            {
                bool mismatch = this.outstandingBlocks.Count > 1
                    && this.outstandingBlocks[this.outstandingBlocks.Count - 1].BlockType
                        != this.outstandingBlocks[this.outstandingBlocks.Count - 2].BlockType;
                if (mismatch)
                {
                    Utilities.PlaySound("MatchError0");
                }
                else if (this.outstandingBlocks.Count < 3)
                {
                    Utilities.PlaySound("Piece1Hit0");
                }
                else
                {
                    Utilities.PlaySound("MatchSuccess0");
                }
            }
        }
    }
}