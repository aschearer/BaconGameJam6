
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

        private readonly BlockFactory blockFactory;

        public Board(int numberOfColumns, int numberOfRows)
        {
            this.NumberOfColumns = numberOfColumns;
            this.NumberOfRows = numberOfRows;
            this.pieces = new List<BoardPiece>();
            this.blockFactory = new BlockFactory(this);
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

        public void Fill()
        {
            for (int col = 0; col < this.NumberOfColumns; col++)
            {
                for (int row = 0; row < 4; row++)
                {
                    int offset = col % 2 == 0 ? 4 : 5;
                    var block = this.blockFactory.CreateBlock(col, row - offset);
                    block.Row = row;
                    this.Add(block);
                }
            }
        }

        public void AddNewRow()
        {
            this.PushBlocksDown();
            for (int col = 0; col < this.NumberOfColumns; col++)
            {
                var block = this.blockFactory.CreateBlock(col, -1);
                block.Row = 0;
                this.Add(block);
            }
        }

        private void PushBlocksDown()
        {
            foreach (var boardPiece in pieces)
            {
                var block = boardPiece as Block;
                if (block != null && block.IsActive)
                {
                    block.Row++;
                }
            }
        }
    }
}
