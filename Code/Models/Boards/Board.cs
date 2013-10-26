
namespace BaconGameJam6.Models.Boards
{
    using System.Collections;
    using System.Collections.Generic;

    using BaconGameJam6.Models.Blocks;

    public class Board : IEnumerable<Block>
    {
        public readonly int NumberOfColumns;

        public readonly int NumberOfRows;

        private readonly List<Block> blocks;

        private readonly Ship ship;

        public Board(int numberOfColumns, int numberOfRows, PlayerId playerId)
        {
            this.NumberOfColumns = numberOfColumns;
            this.NumberOfRows = numberOfRows;
            this.blocks = new List<Block>();
            this.ship = new Ship(numberOfColumns / 2, numberOfRows - 1, playerId);
        }

        public void Add(Block block)
        {
            this.blocks.Add(block);
        }

        public void Remove(Block block)
        {
            this.blocks.Remove(block);
        }

        public IEnumerator<Block> GetEnumerator()
        {
            return this.blocks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
