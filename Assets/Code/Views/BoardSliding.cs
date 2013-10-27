namespace BaconGameJam6.Views
{
    using UnityEngine;
    using System;

    using BaconGameJam6.Models.Boards;
    using BaconGameJam6.Models.States;
    using BaconGameJam6.Models.Tweens;

    public class BoardSliding : IState
    {
        private readonly Board board;
        private readonly float targetX;

        private readonly ITween tween;

        public BoardSliding(Board board, float targetX)
        {
            this.board = board;
            
            this.tween = TweenFactory.Tween(board.TargetPosition.x, targetX, TimeSpan.FromSeconds(0.1));
        }

        public bool IsComplete
        {
            get
            {
                return this.tween.IsFinished;
            }
        }

        public void Update(TimeSpan elapsedTime)
        {
            this.tween.Update(elapsedTime);
            board.TargetPosition = new Vector3(this.tween.Value, board.TargetPosition.y, board.TargetPosition.z);
        }
    }
}