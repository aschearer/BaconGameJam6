using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundGridScript : MonoBehaviour
{
    public GameObject cubeModel;

    private const int GridWidth = 100;
    private const int GridHeight = 60;
    private const float InitialZPosition = -10;
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
        for (int y = -GridHeight / 2; y < GridHeight / 2; ++y)
        {
            for (int x = -GridWidth / 2; x < GridWidth / 2; ++x)
            {
                GameObject backgroundCube = Instantiate(this.cubeModel, new Vector3(x / 2f, y / 2f, InitialZPosition), Quaternion.identity) as GameObject;
                backgroundCube.renderer.material.color = Color.Lerp(Color.white, Color.black, Random.Range(0.33f, 0.66f));

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
