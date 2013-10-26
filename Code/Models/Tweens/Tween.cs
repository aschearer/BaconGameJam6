namespace BaconGameJam6.Models.Tweens
{
    using System;

    public class Tween : ITween
    {
        #region Fields

        private readonly EasingFunction normalEasingFunction;

        private readonly TimeSpan targetRunTime;

        private EasingFunction currentEasingFunction;

        private int currentIteration;

        private TimeSpan delayTimer;

        private TimeSpan elapsedTime;

        private bool isFirstRun;

        private float start;

        private float target;

        private float value;

        #endregion

        #region Constructors and Destructors

        public Tween(float start, float target, TimeSpan targetRunTime, EasingFunction normalEasingFunction)
        {
            this.target = target;
            this.targetRunTime = targetRunTime;
            this.start = this.Value = start;
            this.normalEasingFunction = normalEasingFunction;
            this.currentEasingFunction = this.normalEasingFunction;
            this.elapsedTime = TimeSpan.Zero;
            this.DelayType = DelayType.Always;
            this.isFirstRun = true;
        }

        #endregion

        #region Delegates

        public delegate float EasingFunction(
            float startingValue, float targetValue, TimeSpan targetRunTime, TimeSpan elapsedTime);

        #endregion

        #region Public Properties

        public int CurrentIteration
        {
            get
            {
                return this.currentIteration;
            }
        }

        public TimeSpan Delay { get; set; }

        public DelayType DelayType { get; set; }

        public Action<ITween, float> FinishCallback { get; set; }

        public bool IsFinished { get; private set; }

        public bool IsPaused { get; set; }

        public TimeSpan RemainingDelay
        {
            get
            {
                return this.Delay - this.delayTimer;
            }
        }

        public Repeat Repeats { get; set; }

        public Action<ITween, float> RepeatsCallback { get; set; }

        public EasingFunction ReverseEasing { get; set; }

        public TimeSpan Runtime
        {
            get
            {
                return this.targetRunTime;
            }
        }

        public Action<ITween, float> UpdateCallback { get; set; }

        public float Value
        {
            get
            {
                return this.value;
            }

            private set
            {
                if (Math.Abs(this.value - value) > 0.001f)
                {
                    this.value = value;
                    if (this.UpdateCallback != null)
                    {
                        this.UpdateCallback(this, this.Value);
                    }
                }
            }
        }

        public bool YoYos { get; set; }

        #endregion

        #region Public Methods and Operators

        public ITween Copy()
        {
            var copy = new Tween(this.start, this.target, this.targetRunTime, this.normalEasingFunction);
            copy.elapsedTime = this.elapsedTime;
            copy.Delay = this.Delay;
            copy.DelayType = this.DelayType;
            copy.delayTimer = this.delayTimer;
            copy.Repeats = this.Repeats;
            copy.YoYos = this.YoYos;
            copy.isFirstRun = this.isFirstRun;

            return copy;
        }

        public void Restart()
        {
            this.IsPaused = false;
            this.IsFinished = false;
            this.elapsedTime = TimeSpan.Zero;
            this.delayTimer = TimeSpan.Zero;
            this.currentIteration = 0;
            this.Value = this.start;
            this.currentEasingFunction = this.normalEasingFunction;
        }

        public void Update(TimeSpan elapsedGameTime)
        {
            if (this.IsPaused)
            {
                return;
            }

            if (this.isFirstRun)
            {
                this.isFirstRun = false;
                if (this.DelayType == DelayType.OnlyOnRepeat || this.DelayType == DelayType.OnlyOnYoYo)
                {
                    this.delayTimer = this.Delay;
                }
            }

            TimeSpan delta = (this.delayTimer + elapsedGameTime) - this.Delay;
            if (delta < TimeSpan.Zero)
            {
                this.delayTimer += elapsedGameTime;
                return;
            }
            else
            {
                this.delayTimer = this.Delay;
            }

            this.elapsedTime += delta;
            if (this.elapsedTime.CompareTo(this.targetRunTime) < 0)
            {
                this.Value = this.currentEasingFunction(this.start, this.target, this.targetRunTime, this.elapsedTime);
            }
            else if (!this.IsFinished)
            {
                bool shouldRepeat = this.Repeats == Repeat.Forever
                                    || this.Repeats.NumberOfRepetitions > this.currentIteration;
                if (shouldRepeat)
                {
                    if (this.RepeatsCallback != null)
                    {
                        this.RepeatsCallback(this, this.target);
                    }

                    this.currentIteration++;
                    if (this.YoYos)
                    {
                        if (this.DelayType == DelayType.OnlyOnYoYo)
                        {
                            this.delayTimer = TimeSpan.Zero;
                        }

                        this.Reverse();
                    }
                    else
                    {
                        if (this.DelayType == DelayType.Always || this.DelayType == DelayType.OnlyOnRepeat)
                        {
                            this.delayTimer = TimeSpan.Zero;
                        }

                        while (this.elapsedTime >= this.targetRunTime)
                        {
                            this.elapsedTime = this.elapsedTime.Subtract(this.targetRunTime);
                        }

                        this.Value = this.currentEasingFunction(
                            this.start, this.target, this.targetRunTime, this.elapsedTime);
                    }
                }
                else
                {
                    this.Value = this.target;
                    this.IsFinished = true;
                    if (this.FinishCallback != null)
                    {
                        this.FinishCallback(this, this.Value);
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Flips start and end values and adjusts elapsed time to reflect changes.
        /// </summary>
        private void Reverse()
        {
            float originalStart = this.start;
            this.start = this.target;
            this.target = originalStart;
            this.elapsedTime = this.targetRunTime.Subtract(this.elapsedTime);
            if (this.currentEasingFunction == this.normalEasingFunction && this.ReverseEasing != null)
            {
                this.currentEasingFunction = this.ReverseEasing;
            }
            else
            {
                this.currentEasingFunction = this.normalEasingFunction;
            }
        }

        #endregion
    }
}