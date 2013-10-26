using UnityEngine;
using System.Collections;
using BaconGameJam6.Models.Simulations;
using BaconGameJam6.Models.Boards;
using BaconGameJam6.Models.Blocks;
using System.Collections.Generic;

public class GameLoop : MonoBehaviour {
	
	public GameObject Board;
	public List<GameObject> Blocks;
	public GameObject Ship;

	private Simulation simulation;
	
	void Start () {
		this.simulation = new Simulation();
		
		Board board = this.simulation.Board;
		GameObject boardView = Instantiate(Board, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		
		foreach (Block block in board)
		{
			GameObject blockView = Instantiate(Blocks[(int)block.BlockType], new Vector3(block.X, block.Y, block.Z), Quaternion.identity) as GameObject;
			blockView.transform.parent = boardView.transform;
		}
		
		// $TODO: get ship
		GameObject shipView = Instantiate(Ship, new Vector3(3, 9, 0), Quaternion.identity) as GameObject;
		shipView.transform.parent = boardView.transform;
	}
	
	void Update () {
	}
	
}
