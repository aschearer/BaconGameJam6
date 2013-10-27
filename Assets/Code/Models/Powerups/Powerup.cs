namespace Assets.Code.Models.Powerups
{
    using BaconGameJam6.Models.Boards;

    public enum PowerUpType
    {
    }

    public class Powerup : BoardPiece
    {
        private readonly PowerUpType powerUpType;

        public Powerup(Board board, int col, int row, PowerUpType powerUpType)
            : base(board, col, row)
        {
            this.powerUpType = powerUpType;
        }
    }
}