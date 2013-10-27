namespace BaconGameJam6.Models.Boards
{
    using System;
    using System.Collections.Generic;

    using BaconGameJam6.Models.States;

    public abstract class BoardPiece : IEquatable<BoardPiece>
    {
        private static int idGenerator;

        public readonly int Id;

        protected readonly Board Board;

        private readonly Queue<IState> states; 

        private int column;

        private int row;

        protected BoardPiece(Board board, int col, int row)
        {
            this.Board = board;
            this.Id = ++idGenerator;
            this.states = new Queue<IState>();
            this.X = this.column = col;
            this.Y = this.row = row;
            this.Z = 0;
            this.Opacity = 1;
            this.IsActive = true;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Opacity { get; set; }
        public float Rotation { get; set; }
        public bool IsActive { get; set; }

        public int Column
        {
            get
            {
                return this.column;
            }
            set
            {
                if (this.column != value)
                {
                    int oldColumn = this.column;
                    this.column = value;
                    foreach (var state in this.OnColumnChanged(oldColumn, this.column))
                    {
                        this.states.Enqueue(state);
                    }
                }
            }
        }

        public int Row
        {
            get
            {
                return this.row;
            }
            set
            {
                if (this.row != value)
                {
                    int oldRow = this.row;
                    this.row = value;
                    foreach (var state in this.OnRowChanged(oldRow, this.row))
                    {
                        this.states.Enqueue(state);
                    }
                }
            }
        }

        public bool IsStateful
        {
            get
            {
                return this.states.Count > 0;
            }
        }

        public void Update(TimeSpan elapsedTime)
        {
            this.OnUpdate(elapsedTime);
            if (this.states.Count > 0)
            {
                this.states.Peek().Update(elapsedTime);
                if (this.states.Peek().IsComplete)
                {
                    this.states.Dequeue();
                }
            }
        }

        public void Destroy()
        {
            this.IsActive = false;
            foreach (var state in this.OnDestroy())
            {
                this.states.Enqueue(state);
            }
        }

        public bool Equals(BoardPiece other)
        {
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }

        public override bool Equals(object obj)
        {
            var boardPiece = obj as BoardPiece;
            return boardPiece != null && this.Equals(boardPiece);
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", this.GetType().Name, this.Id);
        }

        protected void AddState(IState state)
        {
            this.states.Enqueue(state);
        }

        protected virtual void OnUpdate(TimeSpan elapsedTimeSpan)
        {
        }

        protected virtual IEnumerable<IState> OnDestroy()
        {
            this.Board.Remove(this);
            return new IState[0];
        }

        protected virtual IEnumerable<IState> OnRowChanged(int oldRow, int newRow)
        {
            return new IState[0];
        }

        protected virtual IEnumerable<IState> OnColumnChanged(int oldColumn, int newColumn)
        {
            return new IState[0];
        }
    }
}