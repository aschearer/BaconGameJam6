using System;
using System.Linq;

using BaconGameJam6.Models.Games;

using UnityEngine;

using BaconGameJam6.Models.Boards;
using BaconGameJam6.Models.Blocks;
using BaconGameJam6.Models.Player;
using System.Collections.Generic;
using BaconGameJam6.Models.Simulations;

public class GameLoop : MonoBehaviour
{
    public GameObject Board;
    public List<GameObject> Blocks;
    public GameObject Ship;
    public GameObject Missile;

    private Game game;

    private GameObject[] boardsViews;

    private Dictionary<int, GameObject> blockViews;
    private List<BlinkingLight> blinkingLights;
    
    void Start()
    {
        var players = new PlayerId[] { PlayerId.One, PlayerId.Two };
        this.game = new Game(players);
        this.game.Start();

        this.boardsViews = new GameObject[this.game.Simulations.Length];
        this.blockViews = new Dictionary<int, GameObject>();
        this.blinkingLights = new List<BlinkingLight>();

        int startingX = -8;
        for (int i = 0; i < this.game.Simulations.Length; i++)
        {
            this.boardsViews[i] = Instantiate(Board, new Vector3(startingX + 8 * i, 0, 0), Quaternion.identity) as GameObject;
            this.game.Simulations[i].BlockDestroyed += SetLights;
            foreach (BoardPiece boardPiece in this.game.Simulations[i].Board)
            {
                this.blockViews[boardPiece.Id] = this.CreateViewFor(boardPiece, this.boardsViews[i]);
            }
        }
    }

    void Update()
    {
        float elapsedTime = Time.deltaTime;
        
        this.ProcessInput();

        this.game.Update(TimeSpan.FromSeconds(elapsedTime));

        var seenBlocks = new Dictionary<int, bool>();
        for (int i = 0; i < this.game.Simulations.Length; i++)
        {
            var simulation = this.game.Simulations[i];
            foreach (var boardPiece in simulation.Board)
            {
                if (!this.blockViews.ContainsKey(boardPiece.Id))
                {
                    this.blockViews[boardPiece.Id] = this.CreateViewFor(boardPiece, this.boardsViews[i]);
                }

                var blockView = this.blockViews[boardPiece.Id];
                blockView.SetActive(boardPiece.Y > -1);
                blockView.transform.localPosition = new Vector3(boardPiece.X, boardPiece.Y, boardPiece.Z);
                blockView.transform.localEulerAngles = new Vector3(boardPiece.Rotation, 90 + boardPiece.Rotation, boardPiece.Rotation);

                var color = blockView.renderer.material.color;
                color.a = boardPiece.Opacity;
                blockView.renderer.material.color = color;

                seenBlocks[boardPiece.Id] = true;
            }

            this.boardsViews[i].transform.localPosition = new Vector3(-8 + 8 * i + simulation.Board.XOffset, simulation.Board.YOffset, 0);
        }

        int[] ids = this.blockViews.Keys.ToArray();
        foreach (var id in ids)
        {
            if (!seenBlocks.ContainsKey(id))
            {
                DestroyObject(this.blockViews[id]);
                this.blockViews.Remove(id);
            }
        }
        
        for (var i = this.blinkingLights.Count; i > 0; --i)
        {
            BlinkingLight light = this.blinkingLights[i - 1];
            light.Update(elapsedTime);
            if (light.IsCompleted)
            {
                this.blinkingLights.RemoveAt(i - 1);
            }
        }
    }

    private void ProcessInput()
    {
        if (!this.game.InputEnabled)
        {
            return;
        }

        for (int i = 0; i < this.game.Simulations.Length; i++)
        {
            int playerId = i + 1;
            if (Input.GetAxis("Fire" + playerId) > 0)
            {
                this.game.OnFire(this.game.Simulations[i].PlayerId);
            }
            else
            {
                this.game.OnReload(this.game.Simulations[i].PlayerId);
            }

            var horizontal = Input.GetAxis("Horizontal" + playerId);
            if (Math.Abs(horizontal) < 0.000005)
            {
                this.game.OnStopMoving(this.game.Simulations[i].PlayerId);
            }
            else if (horizontal < 0)
            {
                this.game.OnMoveLeft(this.game.Simulations[i].PlayerId);
            }
            else if (horizontal > 0)
            {
                this.game.OnMoveRight(this.game.Simulations[i].PlayerId);
            }
        }
    }

    private GameObject CreateViewFor(BoardPiece boardPiece, GameObject parent)
    {
        GameObject objectToClone = null;
        if (boardPiece is Ship)
        {
            objectToClone = this.Ship;
        }
        else if (boardPiece is Missile)
        {
            objectToClone = this.Missile;
        }
        else
        {
            Block block = boardPiece as Block;
            objectToClone = this.Blocks[(int)block.BlockType];
        }

        GameObject boardPieceView = Instantiate(objectToClone) as GameObject;
        boardPieceView.transform.localPosition = new Vector3(boardPiece.X, boardPiece.Y, boardPiece.Z);
        boardPieceView.transform.parent = parent.transform;
        return boardPieceView;
    }

    private void SetLights(object sender, MatchEventArgs args)
    {
        Simulation simulation = sender as Simulation;
                
        Transform[] allChildrenOfBoard = this.boardsViews[simulation.SimulationIndex].GetComponentsInChildren<Transform>();
        int lightBulbIndex = 0;
        foreach (Transform transform in allChildrenOfBoard)
        {
            if (transform.tag.Equals("LightBulb"))
            {
                Color newColor = new Color(0.2f, 0.2f, 0.2f, 1f);
                
                if (lightBulbIndex < args.Blocks.Length)
                {
                    BlockType blockType = args.Blocks[lightBulbIndex].BlockType;

                    switch (blockType)
                    {
                        case BlockType.Black:
                            newColor = Color.black;
                            break;
                        case BlockType.Blue:
                            newColor = Color.blue;
                            break;
                        case BlockType.Green:
                            newColor = Color.green;
                            break;
                        case BlockType.Orange:
                            newColor = new Color(1f, 0.5f, 0f, 1f);
                            break;
                        case BlockType.Purple:
                            newColor = new Color(1f, 0f, 1f, 1f);
                            break;
                        case BlockType.Red:
                            newColor = Color.red;
                            break;
                        case BlockType.White:
                            newColor = Color.white;
                            break;
                        case BlockType.Yellow:
                            newColor = Color.yellow;
                            break;
                    }
                }

                this.blinkingLights.RemoveAll(light => (light.Transform == transform));
                
                if (args.Animate)
                {
                    if (transform.renderer.material.color != newColor)
                    {
                        this.blinkingLights.Add(new BlinkingLight(transform, transform.renderer.material.color, newColor, lightBulbIndex, args.IsMatch));
                    }
                }
                else
                {                    
                    transform.renderer.material.color = newColor;
                }

                ++lightBulbIndex;
            }
        }
    }
}
