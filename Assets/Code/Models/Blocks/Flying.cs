namespace BaconGameJam6.Models.Blocks
{
    using System;

    using BaconGameJam6.Models.Boards;
    using BaconGameJam6.Models.States;
    using BaconGameJam6.Models.Tweens;

    public class Flying : IState
    {
        private static readonly Random Random = new Random();

        private const float Torque = 60;

        private const float YAcceleration = 0.5f;

        private const float StartingYVelocity = -6;

        private const float XVelocity = 2;

        private const float ZVelocity = 4;

        private readonly BoardPiece boardPiece;

        private readonly Board board;

        private readonly float torque;

        private readonly float xVelocity;

        private readonly ITween opacityTween;

        private float yVelocity;

        private TimeSpan timeRemaining;

        public Flying(BoardPiece boardPiece, Board board)
        {
            this.boardPiece = boardPiece;
            this.board = board;
            this.xVelocity = Random.NextDouble() > 0.5 ? Flying.XVelocity : -Flying.XVelocity;
            this.torque = Random.NextDouble() > 0.5 ? Flying.Torque : -Flying.Torque;
            this.yVelocity = Flying.StartingYVelocity;
            this.timeRemaining = TimeSpan.FromSeconds(0.5);
            this.opacityTween = TweenFactory.Tween(1, 0, this.timeRemaining);
        }

        public bool IsComplete
        {
            get
            {
                return this.timeRemaining <= TimeSpan.Zero;
            }
        }

        public void Update(TimeSpan elapsedTime)
        {
            this.timeRemaining -= elapsedTime;
            this.opacityTween.Update(elapsedTime);
            this.boardPiece.Opacity = this.opacityTween.Value;
            this.boardPiece.X += this.xVelocity * (float)elapsedTime.TotalSeconds;
            this.boardPiece.Y += this.yVelocity * (float)elapsedTime.TotalSeconds;
            this.boardPiece.Z += Flying.ZVelocity * (float)elapsedTime.TotalSeconds;
            this.boardPiece.Rotation += this.torque * (float)elapsedTime.TotalSeconds;
            this.yVelocity += Flying.YAcceleration;

            if (this.IsComplete)
            {
                this.board.Remove(this.boardPiece);
            }
        }
    }
}