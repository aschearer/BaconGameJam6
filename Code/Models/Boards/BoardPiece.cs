namespace BaconGameJam6.Models.Boards
{
    using System;
    using System.Collections.Generic;

    using BaconGameJam6.Models.Player;
    using BaconGameJam6.Models.States;

    public abstract class BoardPiece : IEquatable<Ship>
    {
        private static int idGenerator;

        private readonly int id;
        private readonly Queue<IState> states; 

        private int column;

        private int row;

        protected BoardPiece(int col, int row)
        {
            this.id = ++idGenerator;
            this.states = new Queue<IState>();
            this.X = this.Column = col;
            this.Y = this.Row = row;
            this.Z = 0;
        }

        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

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
                    var state = this.OnColumnChanged(oldColumn, this.column);
                    if (state != null)
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
                    var state = this.OnRowChanged(oldRow, this.row);
                    if (state != null)
                    {
                        this.states.Enqueue(state);
                    }
                }
            }
        }

        public void Update(TimeSpan elapsedTime)
        {
            if (this.states.Count > 0)
            {
                this.states.Peek().Update(elapsedTime);
                if (this.states.Peek().IsComplete)
                {
                    this.states.Dequeue();
                }
            }
        }

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

        protected virtual IState OnRowChanged(int oldRow, int newRow)
        {
            return null;
        }

        protected virtual IState OnColumnChanged(int oldColumn, int newColumn)
        {
            return null;
        }
    }
}