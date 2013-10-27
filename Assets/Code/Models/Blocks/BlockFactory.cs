namespace BaconGameJam6.Models.Blocks
{
    using System;

    using Assets.Code.Models.Powerups;

    using BaconGameJam6.Models.Boards;

    public class BlockFactory
    {
        private readonly Board board;

        private static readonly Random RandomGenerator = new Random();

        public BlockFactory(Board board)
        {
            this.board = board;
        }

        public Block CreateBlock(int col, int row, int level)
        {
            var blockType = (BlockType)RandomGenerator.Next(2 + level);
            if (blockType == BlockType.Black)
            {
                return new PowerUp(this.board, col, row, PowerUpType.Bomb);
            }

            return new Block(this.board, col, row, blockType);
        }
    }
}