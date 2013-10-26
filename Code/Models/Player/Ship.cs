namespace BaconGameJam6.Models.Player
{
    using System.Collections.Generic;

    using BaconGameJam6.Models.Boards;
    using BaconGameJam6.Models.States;

    public enum PlayerId
    {
        One,
        Two
    }

    public class Ship : BoardPiece
    {
        public Ship(Board board, int col, int row, PlayerId playerId)
            : base(board, col, row)
        {
            this.PlayerId = playerId;
        }

        public PlayerId PlayerId { get; private set; }

        public void FireMainWeapon()
        {
            var missile = new Missile(this.Board, this.Column, this.Row);
            missile.Row = -1;
            this.Board.Add(missile);
        }

        protected override IEnumerable<IState> OnColumnChanged(int oldColumn, int newColumn)
        {
            yield return new Sliding(this);
        }
    }
}