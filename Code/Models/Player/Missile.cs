namespace BaconGameJam6.Models.Player
{
    using System;
    using System.Linq;

    using BaconGameJam6.Models.Blocks;
    using BaconGameJam6.Models.Boards;

    public class Missile : BoardPiece
    {
        private readonly Ship ship;

        private const float Velocity = 0.1f;

        public Missile(Board board, Ship ship, int col, int row)
            : base(board, col, row)
        {
            this.ship = ship;
        }

        protected override void OnUpdate(TimeSpan elapsedTimeSpan)
        {
            this.Y -= Missile.Velocity;
            this.Row = (int)this.Y;
            var block = (Block)this.Board.FirstOrDefault(
                piece => piece is Block && piece.Column == this.Column && piece.Row == this.Row);

            if (block != null)
            {
                this.Destroy();
                this.ship.RecordHit(block);
                block.Destroy();
            }
        }
    }
}