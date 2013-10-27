using System;
using System.Linq;

using BaconGameJam6.Models.Games;

using UnityEngine;

using BaconGameJam6.Models.Boards;
using BaconGameJam6.Models.Blocks;
using BaconGameJam6.Models.Player;
using System.Collections.Generic;
using BaconGameJam6.Models.Simulations;
using BaconGameJam6;

public enum GameLoopState
{
    MainMenu,
    Playing,
}

public class GameLoopStateEventArgs : EventArgs
{
    public GameLoopState GameLoopState { get; private set; }

    public GameLoopStateEventArgs(GameLoopState gameLoopState)
    {
        this.GameLoopState = gameLoopState;
    }
}

public enum PlayerStateEvent
{
    Joined,
    Left,
}

public class PlayerStateEventArgs : EventArgs
{
    public PlayerStateEvent PlayerStateEvent { get; private set; }
    public PlayerId PlayerId { get; private set; }

    public PlayerStateEventArgs(PlayerStateEvent playerStateEvent, PlayerId playerId)
    {
        this.PlayerStateEvent = playerStateEvent;
        this.PlayerId = playerId;
    }
}

public class BoardTransform
{
    public Vector3 position;
    public Vector3 scale;
    public BoardTransform(Vector3 position, Vector3 scale)
    {
        this.position = position;
        this.scale = scale;
    }
}

public class BoardTransformSet
{
    public BoardTransform[] Transforms;
    public BoardTransformSet(params float[] args)
    {
        var count = args.Length / 2;
        this.Transforms = new BoardTransform[count];
        for (var i = 0; i < count; ++i)
        {
            float xOffset = args[i * 2];
            float scale = args[i * 2 + 1];
            this.Transforms[i] = new BoardTransform(new Vector3(xOffset, 0, 0), new Vector3(scale, scale, scale));
        }
    }
}

public class GameLoop : MonoBehaviour
{
    public event EventHandler<GameLoopStateEventArgs> GameLoopStateChanged;
    public event EventHandler<PlayerStateEventArgs> PlayerStateChanged;

    public GameObject Board;
    public List<GameObject> Blocks;
    public GameObject Ship;
    public GameObject Missile;

    private Game game;

    private GameObject[] boardsViews;
 
    private static readonly BoardTransformSet[] boardTransformSets =
    {
        new BoardTransformSet(-4, 1f),
        new BoardTransformSet(-7, 1f, 1, 1f),
        new BoardTransformSet(-12, 1f, -4, 1f, 4, 1f),
        new BoardTransformSet(-10, 1f, -4, 1f, 2, 1f, 8, 1f),
    };
    
    private Dictionary<int, GameObject> blockViews;
    private List<BlinkingLight> blinkingLights;

    void Start()
    {
        this.blinkingLights = new List<BlinkingLight>();

        this.game = new Game();
    }
    
    private GameLoopState gameLoopState;
    public GameLoopState GameLoopState
    {
        get
        {
            return this.gameLoopState;
        }
        
        private set
        {
            this.gameLoopState = value;
            if (this.GameLoopStateChanged != null)
            {
                this.GameLoopStateChanged.Invoke(this, new GameLoopStateEventArgs(value));
            }
        }
    }
 
    public void StartNewGame(int playerCount)
    {
        this.GameLoopState = GameLoopState.Playing;
        
        this.game.Start(playerCount);
        this.boardsViews = new GameObject[this.game.Simulations.Length];
        this.blockViews = new Dictionary<int, GameObject>();
                    
        BoardTransformSet boardTransformSet = boardTransformSets[this.game.Simulations.Length - 1];
        
        for (int i = 0; i < this.game.Simulations.Length; i++)
        {
            BoardTransform boardTransform = boardTransformSet.Transforms[i];
            this.boardsViews[i] = Instantiate(Board, boardTransform.position, Quaternion.identity) as GameObject;
            this.boardsViews[i].transform.localScale = boardTransform.scale;
            this.game.Simulations[i].Board.TargetPosition = boardTransform.position;
            this.game.Simulations[i].BlockDestroyed += SetLights;
            foreach (BoardPiece boardPiece in this.game.Simulations[i].Board)
            {
                this.blockViews[boardPiece.Id] = this.CreateViewFor(boardPiece, this.boardsViews[i]);
            }
        }
    }
    
    public void EndGame()
    {
        if (this.game.Simulations != null)
        {
            for (int i = 0; i < this.game.Simulations.Length; i++)
            {
                GameObject.Destroy(this.boardsViews[i]);
                this.boardsViews[i] = null;
                this.game.Simulations[i].BlockDestroyed -= SetLights;
            }
        }
        this.game.Stop();

        this.GameLoopState = GameLoopState.MainMenu;
    }

    void Update()
    {
        if (!this.game.CanUpdate)
        {
            return;
        }
        
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

                bool isShip = blockView.tag.Equals("Ship");
                float xPadding = isShip ? 90 : 0;
                float yPadding = isShip ? 0 : 90;
                
                if (isShip)
                {
                    this.SetCursorVisiblity((Ship)boardPiece, blockView, this.boardsViews[i]);
                    blockView.transform.localEulerAngles = new Vector3(xPadding + boardPiece.Rotation, yPadding, 0);
                }
                else
                {
                    blockView.transform.localEulerAngles = new Vector3(xPadding + boardPiece.Rotation, yPadding + boardPiece.Rotation, boardPiece.Rotation);
                }

                var color = blockView.renderer.material.color;
                color.a = boardPiece.Opacity;
                blockView.renderer.material.color = color;

                seenBlocks[boardPiece.Id] = true;
            }

            this.boardsViews[i].transform.localPosition = new Vector3(simulation.Board.TargetPosition.x + simulation.Board.XOffset, simulation.Board.YOffset, 0);
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
  
        var startingSimulations = this.game.Simulations.Length;
        for (int i = startingSimulations; i > 0; --i)
        {
            var simulation = this.game.Simulations[i - 1];
            int playerId = i;
            if (Input.GetAxis("Fire" + playerId) > 0)
            {
                this.game.OnFire(simulation.PlayerId);
            }
            else
            {
                this.game.OnReload(simulation.PlayerId);
            }

            var horizontal = Input.GetAxis("Horizontal" + playerId);
            if (Math.Abs(horizontal) < 0.000005)
            {
                this.game.OnStopMoving(simulation.PlayerId);
            }
            else if (horizontal < 0)
            {
                this.game.OnMoveLeft(simulation.PlayerId);
            }
            else if (horizontal > 0)
            {
                this.game.OnMoveRight(simulation.PlayerId);
            }

            if (Input.GetAxis("Back" + playerId) > 0)
            {
                //RemovePlayer(playerId);
            }
        }
        
        for (int i = startingSimulations; i < boardTransformSets.Length; i++)
        {
            int playerId = i + 1;
            if ((Input.GetAxis("Fire" + playerId) > 0) || Input.GetAxis("Start" + playerId) > 0)
            {
                //AddPlayer(playerId);
            }
        }
                
        if (this.game.Simulations.Length == 0)
        {
            EndGame ();
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
        GameObject board = this.boardsViews[simulation.SimulationIndex];
        Transform[] allChildrenOfBoard = board.GetComponentsInChildren<Transform>();
        int lightBulbIndex = 0;
        foreach (Transform transform in allChildrenOfBoard)
        {
            if (transform.tag.Equals("LightBulb"))
            {
                Color newColor = new Color(0.2f, 0.2f, 0.2f, 1f);

                if (lightBulbIndex < args.Blocks.Length)
                {
                    BlockType blockType = args.Blocks[lightBulbIndex].BlockType;
                    newColor = Utilities.BlockTypeToColor(blockType);
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
            else if (transform.tag.Equals("Ship"))
            {
                Transform[] allChildrenOfShip = transform.GetComponentsInChildren<Transform>();

                foreach (Transform shipChild in allChildrenOfShip)
                {
                    if (shipChild.tag.Equals("AimingLight"))
                    {
                        // Set the ship's light to the last block's color, unless the array is empty, in which case set the ship's light color to white.
                        Color shipLightColor = new Color(1f, 1f, 1f, 1f);

                        if (args.Blocks.Length > 0)
                        {
                            shipLightColor = Utilities.BlockTypeToColor(args.Blocks[args.Blocks.Length - 1].BlockType);
                        }

                        shipChild.renderer.material.color = shipLightColor;
                    }
                }
            }
            else if (transform.tag.Equals("AimingLight"))
            {
                // Set the ship's light to the last block's color, unless the array is empty, in which case set the ship's light color to white.
                Color shipLightColor = new Color(1f, 1f, 1f, 1f);

                if (args.Blocks.Length > 0)
                {
                    shipLightColor = Utilities.BlockTypeToColor(args.Blocks[args.Blocks.Length - 1].BlockType);
                }

                transform.renderer.material.color = shipLightColor;
            }
        }
    }

    private void SetCursorVisiblity(Ship ship, GameObject shipView, GameObject boardView)
    {
        shipView.transform.Find("Plane").gameObject.SetActive(ship.IsActive);
        boardView.transform.Find("BottomPlayerLayer").gameObject.SetActive(ship.IsActive);
    }
}
