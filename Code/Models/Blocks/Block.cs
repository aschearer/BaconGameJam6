namespace BaconGameJam6.Models.Blocks
{
    using System;

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

    public class Block : IEquatable<Block>
    {
        private static int idGenerator = 0;
        private readonly int id;

        public Block(int col, int row, BlockType blockType)
        {
            this.id = ++idGenerator;
            this.X = this.Column = col;
            this.Y = this.Row = row;
            this.BlockType = blockType;
        }

        public BlockType BlockType { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public int Column { get; set; }

        public int Row { get; set; }

        public bool Equals(Block other)
        {
            return this.id == other.id;
        }

        public override int GetHashCode()
        {
            return this.id;
        }

        public override bool Equals(object obj)
        {
            var block = obj as Block;
            return block != null && this.Equals(block);
        }

        public override string ToString()
        {
            return string.Format("{0} Block-{1}", this.BlockType, this.id);
        }
    }
}