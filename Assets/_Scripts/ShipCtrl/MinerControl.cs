using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerControl : MonoBehaviour {
	public WeaponType weapon;

	public float hitpoint;
	public float shield;

	// These are defined through UI and will not be changed throughout the game
	// Thus, serializeField and not public
	[SerializeField] float movespeed;
	[SerializeField] float turnrate;

	[SerializeField] GameObject target = null;

	[SerializeField] string state = "N/A";
	private string prevState = "N/A";

	AsteroidFieldCtrl asteroidField;
	GameObject station;

	[SerializeField] float stopDistance = 2.0f;

	private UnityEngine.AI.NavMeshAgent navAgent;

	float time = 0f;
	float delay = 0.3f;

	float stateTime = 0f;
	float stateDelay = 2.0f;

	// Use this for initialization
	void Start () {
		state = "Find";
		navAgent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		navAgent.stoppingDistance = stopDistance;

		asteroidField = GameManager.instance.AsteroidSpawnField.GetComponent<AsteroidFieldCtrl> ();
		station = GameManager.instance.station;
	}
	
	void Update() {
		time += Time.deltaTime;
		if (time > delay) {
			PerformState ();
			time = 0f;
		}
	}

	void PerformState() {
		if (state == "Find") {
			// Get an asteroid from the asteroid field
			if (target != null) {
				prevState = "Find";
				state = "Mine";
				stateTime = 0f;
				stateDelay = asteroidField.miningTime;
			} else {
				target = asteroidField.getAsteroid ();
			}
		}
		if (state == "Mine") {
			navAgent.destination = target.transform.position;
			if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
				// Mine animation & particles
				stateTime += delay;
				if (stateTime > stateDelay) {
					prevState = "Mine";
					state = "Back";
					stateTime = 0f;
					stateDelay = asteroidField.transportingTime;
					navAgent.stoppingDistance = 4.0f;
					asteroidField.removeAsteroid (target);
				}

			}


		}
		if (state == "Run") {
			// Go back to prev state after some time of running away
			navAgent.destination = station.transform.position;
			stateTime += delay;
			if (stateTime > stateDelay) {
				state = prevState;
			}
		}
		if (state == "Back") {
			navAgent.destination = station.transform.position;
			if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
				// Transport minerals to station animation & particles

				stateTime += delay;
				if (stateTime > stateDelay) {
					prevState = "Back";
					state = "Find";
					navAgent.stoppingDistance = 2.0f;
				}
			}
		}
	}
}
