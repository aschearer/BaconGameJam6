using UnityEngine;
using BaconGameJam6.Models.Blocks;

namespace BaconGameJam6
{
    internal static class Utilities
    {
        internal static Color BlockTypeToColor(BlockType blockType)
        {
            switch (blockType)
            {
                case BlockType.Black:
                    return Color.black;
                case BlockType.Blue:
                    return Color.blue;
                case BlockType.Green:
                    return Color.green;
                case BlockType.Purple:
                    return new Color(1f, 0f, 1f, 1f);
                case BlockType.Red:
                    return Color.red;
                case BlockType.White:
                    return Color.white;
                case BlockType.Yellow:
                    return Color.yellow;
            }

            return Color.black;
        }
    }
}
