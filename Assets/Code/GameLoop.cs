using UnityEngine;
using System.Collections;
using BaconGameJam6.Models.Simulations;
using BaconGameJam6.Models.Blocks;

public class GameLoop : MonoBehaviour {
	
	private Simulation simulation;
	
	void Start () {
		this.simulation = new Simulation();
	}
	
	void Update () {
	}
	
}
