using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The biggest class that holds information about the current scene
// Hold info about: current location, who it belongs to, location of different buildings, ...
public class GameManager : MonoBehaviour {
	//Static instance of GameManager which allows it to be accessed by any other script.
	public static GameManager instance = null;              

	public List<GameObject> allies = new List<GameObject> ();
	public List<GameObject> alliesAndPlayer = new List<GameObject> ();
	public List<GameObject> enemies = new List<GameObject> ();
	public List<GameObject> death = new List<GameObject> ();

	public List<GameObject> alliedBombers;
	public List<GameObject> alliedShielders;
	public List<GameObject> alliedInterceptors;

	public List<GameObject> enemyBombers;
	public List<GameObject> enemyShielders;
	public List<GameObject> enemyInterceptors;

	public GameObject player;

	public GameObject station;
	public GameObject AsteroidSpawnField;
	public GameObject portal;
	public GameObject planet;

	// Info about scene
	public bool playerOwned;

	// These changes as number of targets in command changes
	public float followDistance;
	public float holdDistance;
	public float holdDistanceEnemy;

	void Awake() {
		if (instance == null) {
			instance = this;
		}

		foreach (GameObject ship in GameObject.FindGameObjectsWithTag ("AllyShip")) {
			allies.Add (ship);
			alliesAndPlayer.Add (ship);
		}
		foreach (GameObject ship in GameObject.FindGameObjectsWithTag ("EnemyShip")) {
			enemies.Add (ship);
		}

		player = GameObject.FindGameObjectWithTag ("PlayerShip");
		alliesAndPlayer.Add (player);

		followDistance = 5.0f;
		holdDistance = 5.0f;
		holdDistanceEnemy = 5.0f;
	}
		
	public void UpdateShipList() {
		foreach (GameObject ship in death) {
			allies.Remove (ship);
			alliesAndPlayer.Remove (ship);
			enemies.Remove (ship);
			Destroy (ship);
		}
		death.Clear ();
	}
}
