using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipMovementField : MonoBehaviour {
	UnityEngine.AI.NavMeshAgent playerNavAgent;
	public GameObject target = null;


	// Use this for initialization
	void Start () {
		playerNavAgent = GameObject.FindGameObjectWithTag ("PlayerShip").GetComponent<UnityEngine.AI.NavMeshAgent> ();
		playerNavAgent.stoppingDistance = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1")) { 
			RaycastHit hit; 
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				playerNavAgent.destination = hit.point;
				if (hit.collider.gameObject == this.gameObject || hit.collider.gameObject.tag == "PlayerShip" ) {
					target = null;
				} else {
					target = hit.collider.gameObject;
				}
			}
		}

		if (target != null) {
			playerNavAgent.destination = target.transform.position;
			if (Vector3.Distance(target.transform.position, playerNavAgent.gameObject.transform.position) <= 3.0f) {
				// Check to see if target is a MapLocation or not before transitioning to visit scene
				// If enemy, transition to battle scene
				SceneManager.LoadScene("Main", LoadSceneMode.Single);
			}
		}
	}
}
