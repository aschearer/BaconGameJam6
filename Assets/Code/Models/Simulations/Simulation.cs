namespace BaconGameJam6.Models.Simulations
{
    using System;
    using System.Linq;

    using BaconGameJam6.Models.Blocks;
    using BaconGameJam6.Models.Boards;
    using BaconGameJam6.Models.Player;

    public class Simulation
    {
        public event EventHandler<EventArgs> SuccessfulMatch;
        public event EventHandler<EventArgs> Defeated; 

        private readonly Board board;

        private readonly Ship ship;

        public Simulation(PlayerId playerId)
        {
            this.PlayerId = playerId;
            this.board = new Board(5, 10);
            this.ship = new Ship(this.board, 2, 9, playerId);
            this.ship.Match += this.OnMatch;
            this.board.Add(this.ship);

            for (int row = 0; row < 4; row++)
            {
                this.board.AddNewRow();
            }
        }

        public Board Board
        {
            get
            {
                return this.board;
            }
        }

        public bool IsDefeated { get; private set; }
        public bool IsPaused { get; set; }
        public PlayerId PlayerId { get; private set; }

        public void OnMoveLeft()
        {
            this.ship.Movement = ShipMovement.Left;
        }

        public void OnMoveRight()
        {
            this.ship.Movement = ShipMovement.Right;
        }

        public void OnStopMoving()
        {
            this.ship.Movement = ShipMovement.None;
        }

        public void OnFire()
        {
            this.ship.FireMainWeapon();
        }

        public void OnAddRow()
        {
            this.board.AddNewRow();

            if (this.board.Any(
                piece => piece is Block && piece.IsActive && piece.Row == this.board.NumberOfRows - 1))
            {
                this.IsDefeated = true;
                this.Defeated(this, new EventArgs());
            }
        }

        public void Update(TimeSpan elapsedTime)
        {
            var pieces = this.board.ToArray();
            foreach (var piece in pieces)
            {
                piece.Update(elapsedTime);
            }
        }

        private void OnMatch(object sender, MatchEventArgs e)
        {
            if (!e.IsMatch)
            {
                this.OnAddRow();
            }
            else
            {
                this.SuccessfulMatch(this, new EventArgs());
            }
        }
    }
}