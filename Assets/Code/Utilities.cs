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
                case BlockType.Orange:
                    return new Color(1f, 0.5f, 0f, 1f);
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

        internal static void PlaySound(string soundName)
        {
            PlaySound(soundName, 1);
        }

        internal static void PlaySound(string soundName, int randomCeiling)
        {
            GameObject gameObject = GameObject.Find(soundName + Random.Range(0, randomCeiling));

            ////if (gameObject != null)
            {
                gameObject.GetComponentInChildren<AudioSource>().Play();
            }
        }
    }
}
