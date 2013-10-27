﻿namespace BaconGameJam6.Models.Player
{
    using System;
    using System.Linq;

    using BaconGameJam6.Models.Blocks;
    using BaconGameJam6.Models.Boards;

    public class Missile : BoardPiece
    {
        private readonly Ship ship;

        private const float Velocity = 1f;

        public Missile(Board board, Ship ship, int col, int row)
            : base(board, col, row)
        {
            this.X = ship.X;
            this.ship = ship;
        }

        protected override void OnUpdate(TimeSpan elapsedTimeSpan)
        {
            if (this.Y < 1)
            {
                this.Destroy();
                return;
            }

            this.Y -= Missile.Velocity;
            this.Row = (int)this.Y;
            var block = (Block)this.Board.FirstOrDefault(
                piece => piece is Block && piece.Column == this.Column && piece.Row == this.Row && piece.IsActive);

            if (block != null)
            {
                this.Destroy();
                this.ship.RecordHit(block);
                block.Destroy();
            }
        }
    }
}