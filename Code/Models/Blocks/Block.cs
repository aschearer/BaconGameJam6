namespace BaconGameJam6.Models.Blocks
{
    using BaconGameJam6.Models.Boards;

    public enum BlockType
    {
        Red,
        Green,
        Blue,
        Yellow,
        Black,
        Orange,
        Purple,
        White,
    }

    public class Block : BoardPiece
    {
        public Block(int col, int row, BlockType blockType)
            : base(col, row)
        {
            this.BlockType = blockType;
        }

        public BlockType BlockType { get; private set; }
    }
}