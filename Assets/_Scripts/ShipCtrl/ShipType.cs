using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipType : MonoBehaviour {
	public string shipClass;
	public int shipRank;

	public WeaponType weapon;
	public float hitpoint;
	public float shield;

	// These are defined through UI and will not be changed throughout the game
	// Thus, serializeField and not Public
	[SerializeField] float movespeed;
	[SerializeField] float turnrate;
	[SerializeField] float followDistance;
	[SerializeField] float holdDistance;


	// Write functions that attack closest enemy or follow the current command

	// State can be Attacking, Hold, Find, Follow
	// Attacking = having a target to attack
	// Hold = stand still and waiting for an enemy to come into range
	// Move = Move to a position then hold
	// Find = finding the closest target to follow then attack

	public GameObject target;

	public string state = "N/A";
	private string prevState = "N/A";
	public string command = "N/A";

	public Vector3 holdPosition;
	private UnityEngine.AI.NavMeshAgent navAgent;

	// Store variables for getting obj within hitCollider
	Vector3 center;
	float radius;
	Collider[] hitColliders;

	// To store and calculate when timer goes above delay
	// This is to do ExecuteCommand to put the ship back to
	// the initial state of the command to make sure ship is
	// doing the command when some condition is satisfied
	// ie. keep following the player when player gets further away
	// and stop following when gets near enough.
	float time = 0f;
	float delay = 0.3f;

	// Store oposition list of ship Obj
	GameObject[] shipList;

	// Store attack range (which is different based on commands
	// since you want to start attacking sooner as you hold
	float range;
	void Start() {
		//weapon = transform.GetChild (0).gameObject.GetComponent<WeaponType> ();
		if (tag != "PlayerShip") {
			range = 1.05f * weapon.range;
			navAgent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
			navAgent.stoppingDistance = range;
			navAgent.speed = movespeed;
		}
	}


	void Update() {
		time += Time.deltaTime;
		if (time > delay) {
			ExecuteCommand ();
			PerformState ();
			time = 0f;
		}
	}

	// ExecuteCommand will always be followed with a PerformState
	// ExecuteCommand will change the AI state based on current command
	void ExecuteCommand() {
		if (isAlly()) {
			holdDistance = GameManager.instance.holdDistance;
		} else {
			holdDistance = GameManager.instance.holdDistanceEnemy;
		}
		followDistance = GameManager.instance.followDistance;

		if (command == "Follow") {
			// Change state to follow if player moves too far
			if (Vector3.Distance (GameManager.instance.player.transform.position, transform.position) > followDistance) {
				state = "Follow";
			}
			range = 1.2f * weapon.range;
		}

		// Charge command equates to finding target to fight immediately
		// thus changing the state to Find.
		if (command == "Charge") {
			if (state != "Attacking") {
				state = "Find";
			}
			range = 1.05f * weapon.range;
		}

		// Retreat command equates to running to the nearest exit point
		// thus changing the state to Run.
		if (command == "Retreat") {
			state = "Run";
		}

		// Since hold command ALWAYS comes with a position to hold
		// ships are controlled to move to hold position before holding
		if (command == "Hold") {
			if (Vector3.Distance( holdPosition, transform.position ) > holdDistance) {
				state = "Move";
			}
			range = 1.2f * weapon.range;
		}
	}

	bool isAlly() {
		return (tag == "AllyShip" || tag == "PlayerShip");
	}

	bool checkAlly(GameObject ship) {
		return (ship.tag == "PlayerShip" || ship.tag == "AllyShip");
	}

	bool checkEnemy(GameObject ship) {
		return (ship.tag == "EnemyShip");
	}

	GameObject getPlayer() {
		return GameManager.instance.player;
	}

	GameObject[] getAlliesAndPlayer() {
		return (GameManager.instance.alliesAndPlayer.ToArray ());
	}

	GameObject[] getEnemies() {
		return (GameManager.instance.enemies.ToArray ());
	}

	GameObject[] getOposition() {
		if (isAlly ()) {
			return getEnemies ();
		} else {
			return getAlliesAndPlayer ();
		}
	}

	// Set the current target to the closest enemy within range
	void getClosestEnemyWithinRange() {
		// Check any enemy within weapon range
		// Increase check radius by 5% of range for the behavior of 
		// start attacking before enemy in range
		center = transform.position;
		radius = range;

		hitColliders = Physics.OverlapSphere(center, radius);

		float prevDistance;

		for (int i = 0; i < hitColliders.Length; i++) {
			var currTarg = hitColliders [i].gameObject;
			if ((checkAlly(currTarg) && !isAlly()) || (checkEnemy(currTarg) && isAlly())) {
				var currDistance = Vector3.Distance (transform.position, currTarg.transform.position);


				if (target != null) {
					prevDistance = Vector3.Distance (transform.position, target.transform.position);
				} else {
					prevDistance = range;
				}

				if (currDistance < prevDistance) {
					target = currTarg;
				}
			}
		}
	}

	// Set the current target to the closest enemy
	void getClosestEnemy() {
		shipList = getOposition ();

		float dist = 0;
		if (shipList.Length > 0) {
			dist = Vector3.Distance (shipList [0].transform.position, transform.position);
			target = shipList [0];
		} else {
			target = null;
		}


		foreach (GameObject ship in shipList) {
			float currDist = Vector3.Distance (ship.transform.position, transform.position);
			if (dist > currDist) {
				target = ship;
				dist = currDist;
			}
		}
	}

	// Note: Follow command is only applicable to player and ally ships will follow player
	// Follow command is NOT supposed to be given to the enemies ship

	void PerformState() {
		if (state == "Attacking") {
			// Continuously update closest enemy within range until no target is left in range
			if (target != null) {
				getClosestEnemyWithinRange();

				navAgent.destination = target.transform.position;

				if (Vector3.Distance (navAgent.destination, transform.position) > range) {
					navAgent.SetDestination (transform.position);
					target = null;
					state = prevState;
				}
			} else {
				state = prevState;
			}
		}

		if (state == "Find") {
			// Get the closest enemy to target variable then set navAgent destination as the target
			getClosestEnemy ();

			if (target != null) {
				if (Vector3.Distance (target.transform.position, transform.position) <= range) {
					prevState = "Find";
					state = "Attacking";
				}
				navAgent.isStopped = false;
				navAgent.destination = target.transform.position;
				navAgent.stoppingDistance = range;
			}
		}

		if (state == "Follow") {
			navAgent.isStopped = false;
			navAgent.destination = getPlayer ().transform.position;

			navAgent.stoppingDistance = followDistance;

			// Change to hold when standing within stopping distance from the player
			if (Vector3.Distance (getPlayer ().transform.position, transform.position) <= navAgent.stoppingDistance) {
				state = "Hold";
			}
		} else {
			if (tag != "PlayerShip") {
				navAgent.stoppingDistance = range;
			}
		}

		if (state == "Run") {
			
		}

		if (state == "Hold") {
			navAgent.isStopped = true;

			getClosestEnemyWithinRange ();

			// If found a target within attack range then start attacking
			if (target != null) {
				prevState = "Hold";
				state = "Attacking";
			}
		}

		if (state == "Move") {
			navAgent.destination = holdPosition;
			navAgent.isStopped = false;
			navAgent.stoppingDistance = holdDistance;

			if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
				state = "Hold";
			}
		}
	}

}
