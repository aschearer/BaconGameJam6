
namespace BaconGameJam6.Models.Boards
{
    using System.Collections;
    using System.Collections.Generic;

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
    }
}
