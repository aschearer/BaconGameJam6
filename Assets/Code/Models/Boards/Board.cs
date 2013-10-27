﻿
namespace BaconGameJam6.Models.Boards
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using BaconGameJam6.Models.Blocks;
    using BaconGameJam6.Models.Player;

    public class Board : IEnumerable<BoardPiece>
    {
        public readonly int NumberOfColumns;

        public readonly int NumberOfRows;

        private readonly List<BoardPiece> pieces;

        private readonly BlockFactory blockFactory;

        private int rowsAdded;

        public Board(int numberOfColumns, int numberOfRows)
        {
            this.NumberOfColumns = numberOfColumns;
            this.NumberOfRows = numberOfRows;
            this.pieces = new List<BoardPiece>();
            this.blockFactory = new BlockFactory(this);
        }

        private int Level
        {
            get
            {
                return Math.Min(8, 1 + (this.rowsAdded / 5));
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
            this.PushBlocksDown();
            for (int col = 0; col < this.NumberOfColumns; col++)
            {
                var block = this.blockFactory.CreateBlock(col, -1, this.Level);
                block.Row = 0;
                this.Add(block);
            }
        }

        private void PushBlocksDown()
        {
            foreach (var boardPiece in pieces)
            {
                var block = boardPiece as Block;
                if (block != null && block.IsActive)
                {
                    block.Row++;
                }
            }
        }
    }
}
