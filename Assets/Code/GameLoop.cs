using UnityEngine;
using System.Collections;
using BaconGameJam6.Models.Simulations;
using BaconGameJam6.Models.Boards;
using BaconGameJam6.Models.Blocks;
using BaconGameJam6.Models.Player;
using System.Collections.Generic;

public class GameLoop : MonoBehaviour
{
    public GameObject Board;
    public List<GameObject> Blocks;
    public GameObject Ship;

	private Simulation simulation;
	
	void Start () {
		this.simulation = new Simulation(PlayerId.One);
		
		Board board = this.simulation.Board;
		GameObject boardView = Instantiate(Board, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		
		foreach (BoardPiece boardPiece in board)
		{
			GameObject objectToClone = null;
			if (boardPiece is Ship)
			{
				objectToClone = Ship;
			}
			else
			{
				Block block = boardPiece as Block;
				objectToClone = Blocks[(int)block.BlockType];
			}

			GameObject boardPieceView = Instantiate(objectToClone) as GameObject;
			boardPieceView.transform.localPosition = new Vector3(boardPiece.X, boardPiece.Y, boardPiece.Z);
			boardPieceView.transform.parent = boardView.transform;
		}		
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			this.simulation.OnFire();
		}
		
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			this.simulation.OnMoveLeft();
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			this.simulation.OnMoveRight();
		}
		
		this.simulation.Update(System.TimeSpan.FromSeconds(Time.deltaTime));
	}
}
