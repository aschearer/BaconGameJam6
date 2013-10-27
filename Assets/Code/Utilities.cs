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
                    return new Color(0, 255f / 255f, 231f / 255f);
                case BlockType.Green:
                    return new Color(0, 218f / 255f, 60f / 255f);
                case BlockType.Purple:
                    return new Color(255, 0f, 255f / 255f);
                case BlockType.Red:
                    return new Color(223f / 255f, 21f / 255f, 26f / 255f);
                case BlockType.White:
                    return Color.white;
                case BlockType.Yellow:
                    return new Color(244f / 255f, 243f / 255f, 40f / 255f);
            }

            return Color.black;
        }

        internal static void PlaySound(string soundName)
        {
            GameObject.Find(soundName).GetComponent<AudioSource>().Play();
        }
    }
}
