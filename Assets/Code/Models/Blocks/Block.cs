namespace BaconGameJam6.Models.Blocks
{
    using System.Collections.Generic;

    using BaconGameJam6.Models.Boards;
    using BaconGameJam6.Models.States;

    public enum BlockType
    {
        Red,
        Blue,
        Yellow,
        Green,
        Purple,
        White,
        Black,
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

        protected override IEnumerable<IState> OnDestroy()
        {
            yield return new Flying(this, this.Board);
        }
    }
}