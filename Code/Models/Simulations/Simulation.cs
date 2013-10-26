namespace BaconGameJam6.Models.Simulations
{
    using BaconGameJam6.Models.Blocks;
    using BaconGameJam6.Models.Boards;

    public class Simulation
    {
        private readonly Board board;

        public Simulation()
        {
            this.board = new Board(5, 10);
            for (int col = 0; col < this.board.NumberOfColumns; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    this.board.Add(new Block(col, row, BlockType.Blue));
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
    }
}