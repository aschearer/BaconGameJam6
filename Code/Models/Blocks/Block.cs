namespace BaconGameJam6.Models.Blocks
{
    using System;
    using System.Collections.Generic;

    using BaconGameJam6.Models.Boards;
    using BaconGameJam6.Models.States;
    using BaconGameJam6.Models.Tweens;

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

    public class Block : BoardPiece
    {
        public Block(Board board, int col, int row, BlockType blockType)
            : base(board, col, row)
        {
            this.BlockType = blockType;
        }

        public BlockType BlockType { get; private set; }

        protected override IEnumerable<IState> OnRowChanged(int oldRow, int newRow)
        {
            yield return new Falling(this);
        }

        protected override IEnumerable<IState> OnDestroy()
        {
            yield return new Flying(this, this.Board);
        }
    }

    public class Flying : IState
    {
        private const float Torque = 4;

        private const float YAcceleration = 10;

        private const float StartingYVelocity = -10;

        private const float XVelocity = 2;

        private const float ZVelocity = -4;

        private readonly Block block;

        private readonly Board board;

        private readonly float xVelocity;

        private float yVelocity;

        private TimeSpan timeRemaining;

        private ITween opacityTween;

        public Flying(Block block, Board board)
        {
            this.block = block;
            this.board = board;
            this.xVelocity = Flying.XVelocity;
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
            this.block.Opacity = this.opacityTween.Value;
            this.block.X += this.xVelocity * (float)elapsedTime.TotalSeconds;
            this.block.Y += this.yVelocity * (float)elapsedTime.TotalSeconds;
            this.block.Z += Flying.ZVelocity * (float)elapsedTime.TotalSeconds;
            this.yVelocity += Flying.YAcceleration;

            if (this.IsComplete)
            {
                this.board.Remove(this.block);
            }
        }
    }
}