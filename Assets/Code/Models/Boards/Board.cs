
namespace BaconGameJam6.Models.Boards
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using BaconGameJam6.Models.Blocks;
    using BaconGameJam6.Models.Player;

    using UnityEngine;

    using Random = System.Random;

    public class Board : IEnumerable<BoardPiece>
    {
        private static readonly Random Random = new Random();

        public readonly int NumberOfColumns;

        public readonly int NumberOfRows;

        private readonly List<BoardPiece> pieces;

        private readonly BlockFactory blockFactory;

        private int rowsAdded;

        private TimeSpan shakeTimer = TimeSpan.Zero;

        public Board(int numberOfColumns, int numberOfRows)
        {
            this.NumberOfColumns = numberOfColumns;
            this.NumberOfRows = numberOfRows;
            this.pieces = new List<BoardPiece>();
            this.blockFactory = new BlockFactory(this);
        }

        public float XOffset { get; private set; }
        public float YOffset { get; private set; }

        public int NumberOfBlocks
        {
            get
            {
                return this.pieces.Count(piece => piece is Block);
            }
        }

        private int Level
        {
            get
            {
                return Math.Min(8, 1 + (this.rowsAdded / 5));
            }
        }

        public int Count
        {
            get
            {
                return this.pieces.Count;
            }
        }

        public bool IsAnimating
        {
            get
            {
                return this.pieces.Any(piece => piece.IsStateful);
            }
        }
        
        public void Clear()
        {
            this.rowsAdded = 0;
            this.pieces.ForEach(piece =>
            {
                if (!(piece is Ship))
                {
                    piece.Destroy();
                }
            });
        }
        
        public void Add(BoardPiece piece)
        {
            this.pieces.Add(piece);
        }

        public void Remove(BoardPiece piece)
        {
            this.pieces.Remove(piece);
        }

        public IEnumerator<BoardPiece> GetEnumerator()
        {
            return this.pieces.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Update(TimeSpan elapsedTime)
        {
            if (this.shakeTimer > TimeSpan.Zero)
            {
                this.shakeTimer -= elapsedTime;
                if (this.shakeTimer > TimeSpan.Zero)
                {
                    this.XOffset = (float)(0.1 - 0.2 * Board.Random.NextDouble());
                    this.YOffset = (float)(0.1 - 0.2 * Board.Random.NextDouble());
                }
                else
                {
                    this.XOffset = this.YOffset = 0;
                }
            }
        }

        public void Fill()
        {
            this.pieces.ForEach(piece =>
            {
                if (piece is Ship)
                {
                    Ship ship = piece as Ship;
                    ship.ReloadWeapon();
                    ship.ResetOutstandingBlocks();
                    ship.Column = ship.StartingColumn;
                }
            });

            for (int col = 0; col < this.NumberOfColumns; col++)
            {
                for (int row = 0; row < 4; row++)
                {
                    int offset = col % 2 == 0 ? 4 : 5;
                    var block = this.blockFactory.CreateBlock(col, row - offset, this.Level);
                    block.Row = row;
                    this.Add(block);
                }
            }
        }

        public void AddNewRow()
        {
            this.rowsAdded++;
            this.shakeTimer = TimeSpan.FromSeconds(0.25);
            this.PushBlocksDown();
            for (int col = 0; col < this.NumberOfColumns; col++)
            {
                var block = this.blockFactory.CreateBlock(col, -1, this.Level);
                block.Row = 0;
                this.Add(block);
            }
        }

        public void SlamNewRows()
        {
            int bottomMostRow = 4;
            if (this.NumberOfBlocks > 0)
            {
                bottomMostRow = this.pieces.Where(piece => piece is Block && piece.IsActive).Max(piece => piece.Row);
            }

            int numberOfRowsToAdd = Math.Min(4, this.NumberOfRows - bottomMostRow);
            Debug.Log(numberOfRowsToAdd);

            this.rowsAdded += numberOfRowsToAdd;

            this.PushBlocksDown(numberOfRowsToAdd);

            this.shakeTimer = TimeSpan.FromSeconds(0.5);
            for (int col = 0; col < this.NumberOfColumns; col++)
            {
                for (int row = 0; row < numberOfRowsToAdd; row++)
                {
                    int offset = numberOfRowsToAdd;
                    var block = this.blockFactory.CreateBlock(col, row - offset, this.Level);
                    block.Row = row;
                    this.Add(block);
                }
            }
        }

        private void PushBlocksDown(int howFar = 1)
        {
            foreach (var boardPiece in pieces)
            {
                var block = boardPiece as Block;
                if (block != null && block.IsActive)
                {
                    block.Row += howFar;
                }
            }
        }
    }
}
