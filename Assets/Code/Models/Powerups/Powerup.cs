namespace Assets.Code.Models.Powerups
{
    using System;
    using System.Collections.Generic;

    using BaconGameJam6.Models.Blocks;
    using BaconGameJam6.Models.Boards;
    using BaconGameJam6.Models.States;

    using UnityEngine;

    public enum PowerUpType
    {
        Bomb,
    }

    public class PowerUp : Block
    {
        private readonly PowerUpType powerUpType;

        public PowerUp(Board board, int col, int row, PowerUpType powerUpType)
            : base(board, col, row, BlockType.Black)
        {
            this.powerUpType = powerUpType;
        }

        protected override IEnumerable<IState> OnDestroy()
        {
            switch (this.powerUpType)
            {
                case PowerUpType.Bomb:
                    for (int col = this.Column - 1; col < this.Column + 2; col++)
                    {
                        for (int row = this.Row - 1; row < this.Row + 2; row++)
                        {
                            if (col < 0 || 
                                col >= this.Board.NumberOfColumns - 1 ||
                                row < 0 ||
                                row >= this.Board.NumberOfRows - 1 ||
                                (col == this.Column && row == this.Row))
                            {
                                continue;
                            }

                            var block = this.Board.GetBlockAt(col, row);
                            if (block != null)
                            {
                                block.Destroy();
                            }
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            yield return new Flying(this, this.Board);
        }
    }
}