namespace BaconGameJam6.Models.Player
{
    using System;
    using System.Diagnostics;

    using BaconGameJam6.Models.Blocks;

    public class MatchEventArgs : EventArgs
    {
        private readonly Block[] blocks;

        public MatchEventArgs(Block[] blocks, bool animate)
        {
            Debug.Assert(blocks.Length == 3);
            this.blocks = blocks;
            this.IsMatch = true;
            for (int i = 0; i < this.blocks.Length - 1; i++)
            {
                if (this.blocks[i].BlockType != this.blocks[i + 1].BlockType)
                {
                    this.IsMatch = false;
                    break;
                }
            }
            this.Animate = animate;
        }

        public Block[] Blocks
        {
            get
            {
                return this.blocks;
            }
        }
  
        public bool Animate { get; private set; }
        public bool IsMatch { get; private set; }
    }
}