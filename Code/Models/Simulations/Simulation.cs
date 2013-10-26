namespace BaconGameJam6.Models.Simulations
{
    using System;

    using BaconGameJam6.Models.Blocks;
    using BaconGameJam6.Models.Boards;
    using BaconGameJam6.Models.Player;

    public class Simulation
    {
        private readonly Board board;

        private readonly Ship ship;

        public Simulation()
        {
            this.board = new Board(5, 10);
            this.ship = new Ship(this.board, 3, 9, PlayerId.One);
            this.board.Add(this.ship);
            for (int col = 0; col < this.board.NumberOfColumns; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    this.board.Add(new Block(this.board, col, row, BlockType.Blue));
                }
            }
        }

        public Board Board
        {
            get
            {
                return this.board;
            }
        }

        public void OnMoveLeft()
        {
            if (this.ship.Column > 0)
            {
                this.ship.Column--;
            }
        }

        public void OnMoveRight()
        {
            if (this.ship.Column < this.board.NumberOfColumns - 1)
            {
                this.ship.Column++;
            }
        }

        public void OnFire()
        {
            this.ship.FireMainWeapon();
        }

        public void Update(TimeSpan elapsedTime)
        {
            foreach (var piece in this.board)
            {
                piece.Update(elapsedTime);
            }
        }
    }
}