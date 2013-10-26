namespace BaconGameJam6.Models.Player
{
    using System.Collections.Generic;

    using BaconGameJam6.Models.Boards;
    using BaconGameJam6.Models.States;

    public class Missile : BoardPiece
    {
        public Missile(Board board, int col, int row)
            : base(board, col, row)
        {
        }

        protected override IEnumerable<IState> OnRowChanged(int oldRow, int newRow)
        {
            return new IState[]
            {
                new Rocketing(this),
                new Exploding(this),
            };
        }

        public void Destroy()
        {
            this.Board.Remove(this);
        }
    }
}