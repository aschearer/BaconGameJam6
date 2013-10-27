using System;
using System.Linq;

using BaconGameJam6.Models.Games;

using UnityEngine;

using BaconGameJam6.Models.Boards;
using BaconGameJam6.Models.Blocks;
using BaconGameJam6.Models.Player;
using System.Collections.Generic;

public class GameLoop : MonoBehaviour
{
    public GameObject Board;
    public List<GameObject> Blocks;
    public GameObject Ship;
    public GameObject Missile;
    public float Scale;

    private Game game;

    private GameObject[] boardsViews;

    private Dictionary<int, GameObject> blockViews;

    void Start()
    {
        var players = new PlayerId[] { PlayerId.One, PlayerId.Two };
        this.game = new Game(players);

        this.boardsViews = new GameObject[this.game.Simulations.Length];
        this.blockViews = new Dictionary<int, GameObject>();

        int startingX = -8;
        for (int i = 0; i < this.game.Simulations.Length; i++)
        {
            this.boardsViews[i] = Instantiate(Board, new Vector3(startingX + 8 * i, 0, 0), Quaternion.identity) as GameObject;
            foreach (BoardPiece boardPiece in this.game.Simulations[i].Board)
            {
                this.blockViews[boardPiece.Id] = this.CreateViewFor(boardPiece, this.boardsViews[i]);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.game.OnFire(PlayerId.One);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.game.OnMoveLeft(PlayerId.One);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.game.OnMoveRight(PlayerId.One);
        }

        this.game.Update(TimeSpan.FromSeconds(Time.deltaTime));

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
                blockView.transform.localPosition = new Vector3(boardPiece.X, boardPiece.Y, boardPiece.Z);
                blockView.transform.localEulerAngles = new Vector3(boardPiece.Rotation, 90 + boardPiece.Rotation, boardPiece.Rotation);

                var color = blockView.renderer.material.color;
                color.a = boardPiece.Opacity;
                blockView.renderer.material.color = color;

                seenBlocks[boardPiece.Id] = true;
            }
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
}
