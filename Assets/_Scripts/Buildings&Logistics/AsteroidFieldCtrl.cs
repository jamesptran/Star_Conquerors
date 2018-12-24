using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFieldCtrl : MonoBehaviour {
	private List<GameObject> assignedAsteroids = new List<GameObject>();
	private List<GameObject> unassignedAsteroids = new List<GameObject>();

	public static AsteroidFieldCtrl Instance;

	float time = 0f;
	[SerializeField] float delay = 0.0f;
	[SerializeField] int maxAsteroid = 15;
	[SerializeField] float fieldSize = 20.0f;
	[SerializeField] float asteroidDistance = 10.0f;
	[SerializeField] GameObject asteroidPrefab;

	public float miningTime = 3.0f;
	public float transportingTime = 3.0f;

	void Awake() {
		if (Instance == null) {
			Instance = this;
		}
	}

	void Start() {
		
	}

	// Update is called once per frame
	void Update () {
		// Periodically spawn an asteroid and add it to unassigned asteroid list
		// If asteroid is near each other then find new position

		time += Time.deltaTime;
		bool hasSpawned = false;
		int count = 0;
		if (time > delay) {
			while (!hasSpawned) {
				hasSpawned = spawnAsteroid ();
				count++;
				if (count > 10) {
					break;
				}
			}
				
			time = 0f;
		}
	}

	public GameObject getAsteroid() {
		// Return an asteroid from unassigned asteroid list
		// If asteroid list is empty, spawn one and assign it
		// Move the asteroid from unassigned list to assigned list
		if (unassignedAsteroids.Count > 0) {
			int index = Random.Range (0, unassignedAsteroids.Count - 1);
			assignedAsteroids.Add (unassignedAsteroids[index] );
			unassignedAsteroids.RemoveAt (index);

			return assignedAsteroids [assignedAsteroids.Count - 1];
		} else
			return null;

	}

	public void removeAsteroid(GameObject asteroid) {
		assignedAsteroids.Remove (asteroid);
		Destroy (asteroid);

	}

	float x;
	float z;
	Vector3 asteroidPosition;
	int i = 0;
	bool spawnAsteroid() {
		if (maxAsteroid > (assignedAsteroids.Count + unassignedAsteroids.Count)) {
			x = (Random.value * fieldSize - fieldSize/2.0f);
			z = (Random.value * fieldSize - fieldSize/2.0f);
//			x = 0;
//			z = 0;


			asteroidPosition = new Vector3 (x, 0, z);

			for (i = 0; i < assignedAsteroids.Count; i++) {
				if (isNear (asteroidPosition + this.transform.position, assignedAsteroids [i].transform.position)) {
					return false;
				}
			}

			for (i = 0; i < unassignedAsteroids.Count; i++) {
//				Debug.Log (asteroidPosition + this.transform.position);
//				Debug.Log (unassignedAsteroids [i].transform.position);
//				Debug.Log (this.transform.position);
//				Debug.Log (isNear (asteroidPosition + this.transform.position, unassignedAsteroids [i].transform.position));

				if (isNear (asteroidPosition + this.transform.position, unassignedAsteroids [i].transform.position)) {
					return false;
				}
			}

			var newObj = Instantiate(asteroidPrefab, this.transform.position + asteroidPosition, transform.rotation, this.transform);
			unassignedAsteroids.Add (newObj);

			return true;
		} else 
			return true;
	}

	bool isNear(Vector3 asteroid1, Vector3 asteroid2) {
		return (Vector3.Distance (asteroid1, asteroid2) < asteroidDistance);
	}

}
