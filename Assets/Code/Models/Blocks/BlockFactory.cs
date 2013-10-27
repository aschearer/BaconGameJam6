namespace BaconGameJam6.Models.Blocks
{
    using System;

    using BaconGameJam6.Models.Boards;

    public class BlockFactory
    {
        private readonly Board board;

        private readonly Random random;

        public BlockFactory(Board board)
        {
            this.board = board;
            this.random = new Random();
        }

        public Block CreateBlock(int col, int row)
        {
            var blockType = (BlockType)this.random.Next(3);
            return new Block(this.board, col, row, blockType);
        }
    }
}