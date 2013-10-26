namespace BaconGameJam6.Models.Player
{
    using BaconGameJam6.Models.Boards;
    using BaconGameJam6.Models.States;

    public enum PlayerId
    {
        One,
        Two
    }

    public class Ship : BoardPiece
    {
        public Ship(int col, int row, PlayerId playerId)
            : base(col, row)
        {
            this.PlayerId = playerId;
        }

        public PlayerId PlayerId { get; private set; }

        public void FireMainWeapon()
        {
        }

        protected override IState OnColumnChanged(int oldColumn, int newColumn)
        {
            return new Sliding(this);
        }
    }
}