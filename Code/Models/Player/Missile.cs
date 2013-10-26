namespace BaconGameJam6.Models.Player
{
    using System;

    using BaconGameJam6.Models.Boards;

    public class Missile : BoardPiece
    {
        private const float Velocity = 0.1f;

        public Missile(Board board, int col, int row)
            : base(board, col, row)
        {
        }

        protected override void OnUpdate(TimeSpan elapsedTimeSpan)
        {
            this.Y -= Missile.Velocity;
            this.Row = (int)this.Y;
        }

        public void Destroy()
        {
            this.Board.Remove(this);
        }
    }
}