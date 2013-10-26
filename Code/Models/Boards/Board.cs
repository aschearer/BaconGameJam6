
namespace BaconGameJam6.Models.Boards
{
    using System.Collections;
    using System.Collections.Generic;

    using BaconGameJam6.Models.Blocks;

    public class Board : IEnumerable<BoardPiece>
    {
        public readonly int NumberOfColumns;

        public readonly int NumberOfRows;

        private readonly List<BoardPiece> pieces;

        public Board(int numberOfColumns, int numberOfRows)
        {
            this.NumberOfColumns = numberOfColumns;
            this.NumberOfRows = numberOfRows;
            this.pieces = new List<BoardPiece>();
        }

        public void Add(BoardPiece piece)
        {
            this.pieces.Add(piece);
        }

        public void Remove(BoardPiece piece)
        {
            this.pieces.Remove(piece);
        }

        public IEnumerator<BoardPiece> GetEnumerator()
        {
            return this.pieces.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void PushBlocksDown()
        {
            foreach (var boardPiece in pieces)
            {
                var block = boardPiece as Block;
                if (block != null)
                {
                    block.Row++;
                }
            }
        }

        public void AddNewRow()
        {
            for (int col = 0; col < this.NumberOfColumns; col++)
            {
                var block = new Block(this, col, -1, BlockType.Blue);
                block.Row = 0;
                this.Add(block);
            }
        }
    }
}
