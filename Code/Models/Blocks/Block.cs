namespace BaconGameJam6.Models.Blocks
{
    using System.Collections.Generic;

    using BaconGameJam6.Models.Boards;
    using BaconGameJam6.Models.States;

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
        public Block(Board board, int col, int row, BlockType blockType)
            : base(board, col, row)
        {
            this.BlockType = blockType;
        }

        public BlockType BlockType { get; private set; }

        protected override IEnumerable<IState> OnRowChanged(int oldRow, int newRow)
        {
            yield return new Falling(this);
        }
    }
}