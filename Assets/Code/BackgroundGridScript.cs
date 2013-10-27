using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundGridScript : MonoBehaviour
{
    public GameObject cubeModel;

    private const int GridXStart = -40;
    private const int GridWidth = 80;
    private const int GridYStart = -10;
    private const int GridHeight = 40;
    private const float InitialZPosition = -3;
    private const float MaximumZOffset = 0.2f;
    private const float MinimumSpeed = 0.2f;
    private const float MaximumSpeed = 1f;
    private List<BackgroundCubeData> allCubeModels;

    private struct BackgroundCubeData
    {
        public GameObject BackgroundCube;
        public float ZOffset;
        public float Speed;
    }

    // Use this for initialization
    private void Start()
    {
        this.allCubeModels = new List<BackgroundCubeData>();

        // Create a grid of background cubes.
        for (int y = GridYStart; y < GridHeight + GridYStart; ++y)
        {
            for (int x = GridXStart; x < GridWidth + GridXStart; ++x)
            {
                GameObject backgroundCube = Instantiate(this.cubeModel, new Vector3(x * 0.5f, y * 0.5f, InitialZPosition), Quaternion.identity) as GameObject;
                backgroundCube.renderer.material.color = Color.Lerp(Color.Lerp(Color.white, Color.black, Random.Range(0.33f, 0.66f)), Color.blue, 0.1f);

                this.allCubeModels.Add(new BackgroundCubeData()
                {
                    BackgroundCube = backgroundCube,
                    ZOffset = Random.Range(0f, 6.28f),
                    Speed = Random.Range(MinimumSpeed, MaximumSpeed)
                });
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        foreach (BackgroundCubeData backgroundCubeData in this.allCubeModels)
        {
            backgroundCubeData.BackgroundCube.transform.position = new Vector3(
                backgroundCubeData.BackgroundCube.transform.position.x,
                backgroundCubeData.BackgroundCube.transform.position.y,
                InitialZPosition + MaximumZOffset * Mathf.Cos(Time.timeSinceLevelLoad * backgroundCubeData.Speed + backgroundCubeData.ZOffset));
        }
    }
}
