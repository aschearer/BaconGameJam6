namespace BaconGameJam6.Models.Blocks
{
    using System;

    public enum PlayerId
    {
        One,
        Two
    }

    public class Ship : IEquatable<Ship>
    {
        private static int idGenerator = 0;
        private readonly int id;

        public Ship(int col, int row, PlayerId playerId)
        {
            this.id = ++idGenerator;
            this.X = this.Column = col;
            this.Y = this.Row = row;
            this.PlayerId = playerId;
        }

        public PlayerId PlayerId { get; private set; }
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
            var ship = obj as Ship;
            return ship != null && this.Equals(ship);
        }

        public override string ToString()
        {
            return string.Format("{0} Ship-{1}", this.PlayerId, this.id);
        }
    }
}