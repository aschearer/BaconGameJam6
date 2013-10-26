namespace BaconGameJam6.Models.Boards
{
    using System;

    using BaconGameJam6.Models.Player;

    public abstract class BoardPiece : IEquatable<Ship>
    {
        private static int idGenerator;

        private readonly int id;

        protected BoardPiece(int col, int row)
        {
            this.id = ++idGenerator;
            this.X = this.Column = col;
            this.Y = this.Row = row;
            this.Z = 0;
        }

        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }
        public int Column { get; set; }
        public int Row { get; set; }

        public bool Equals(Ship other)
        {
            return this.id == other.id;
        }

        public override int GetHashCode()
        {
            return this.id;
        }

        public override bool Equals(object obj)
        {
            var boardPiece = obj as BoardPiece;
            return boardPiece != null && this.Equals(boardPiece);
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", this.GetType().Name, this.id);
        }
    }
}