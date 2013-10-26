namespace BaconGameJam6.Models.Player
{
    using System;

    using BaconGameJam6.Models.States;

    public class Exploding : IState
    {
        private readonly Missile missile;

        private TimeSpan remainingTime;

        public Exploding(Missile missile)
        {
            this.missile = missile;
            this.remainingTime = TimeSpan.FromSeconds(0.2);
        }

        public bool IsComplete
        {
            get
            {
                return this.remainingTime > TimeSpan.Zero;
            }
        }

        public void Update(TimeSpan elapsedTime)
        {
            this.remainingTime -= elapsedTime;
            if (this.IsComplete)
            {
                this.missile.Destroy();
            }
        }
    }
}