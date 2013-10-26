namespace BaconGameJam6.Models.Tweens
{
    public struct Repeat
    {
        #region Static Fields

        public static readonly Repeat Forever = new Repeat(-1);

        public static readonly Repeat Never = new Repeat(0);

        public static readonly Repeat Once = new Repeat(1);

        public static readonly Repeat Twice = new Repeat(2);

        #endregion

        #region Fields

        public readonly int NumberOfRepetitions;

        #endregion

        #region Constructors and Destructors

        public Repeat(int numberOfRepetitions)
        {
            this.NumberOfRepetitions = numberOfRepetitions;
        }

        #endregion

        #region Public Methods and Operators

        public static bool operator ==(Repeat one, Repeat two)
        {
            return one.Equals(two);
        }

        public static bool operator !=(Repeat one, Repeat two)
        {
            return !(one == two);
        }

        public bool Equals(Repeat other)
        {
            return this.NumberOfRepetitions == other.NumberOfRepetitions;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Repeat && this.Equals((Repeat)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return this.NumberOfRepetitions;
            }
        }

        #endregion
    }
}