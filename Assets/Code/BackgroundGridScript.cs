using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundGridScript : MonoBehaviour
{
    public GameObject cubeModel;

    private const int GridWidth = 100;
    private const int GridHeight = 50;
    private const float InitialZPosition = -30;
    private const float MaximumZOffset = 6.28f;
    private const float MinimumSpeed = 2;
    private const float MaximumSpeed = 4;
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
                GameObject backgroundCube = Instantiate(this.cubeModel, new Vector3(x, y, InitialZPosition), Quaternion.identity) as GameObject;

                switch (Random.Range(0, 2))
                {
                    case 0:
                        backgroundCube.renderer.material.color = Color.Lerp(Color.red, Color.green, Random.Range(0f, 1f));
                        break;
                    case 1:
                        backgroundCube.renderer.material.color = Color.Lerp(Color.green, Color.blue, Random.Range(0f, 1f));
                        break;
                    case 2:
                        backgroundCube.renderer.material.color = Color.Lerp(Color.blue, Color.red, Random.Range(0f, 1f));
                        break;
                }

                this.allCubeModels.Add(new BackgroundCubeData()
                {
                    BackgroundCube = backgroundCube,
                    ZOffset = Random.Range(0f, MaximumZOffset),
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
            backgroundCubeData.BackgroundCube.transform.position.Set(
                backgroundCubeData.BackgroundCube.transform.position.x,
                backgroundCubeData.BackgroundCube.transform.position.y,
                InitialZPosition + MaximumZOffset * Mathf.Cos(Time.timeSinceLevelLoad * backgroundCubeData.Speed + backgroundCubeData.ZOffset));
        }
    }
}
