using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixed2DPlane : MonoBehaviour {
	public float tilt = 12.0f;
	public float turnspeed = 1.0f;

	private Rigidbody rigidBody;

	void Start () {
		if (gameObject.tag == "PlayerShip") {
			rigidBody = GetComponent<Rigidbody> ();
		}
	}

	private Vector3 lastFacing;
	private float currentRotationY;

	void Update () {
		// This bit is to keep position inside 2D plane (with y == 0.0f)
		transform.position = new Vector3 (transform.position.x, 0.0f, transform.position.z);

		Vector3 updatedEuler;

		if (gameObject.tag == "PlayerShip") {
			// This bit is for tilting when player turns since player uses angularVelocity
			updatedEuler = new Vector3 (0.0f, transform.rotation.eulerAngles.y, rigidBody.angularVelocity.y * -tilt);
			transform.rotation = Quaternion.Euler(updatedEuler);
		}



		if (gameObject.tag == "EnemyShip" || gameObject.tag == "AllyShip") {
			var navMesh = GetComponent<UnityEngine.AI.NavMeshAgent> ();
			Quaternion newRotation = transform.rotation;

			if (navMesh.remainingDistance <= navMesh.stoppingDistance || navMesh.isStopped) {

				// RotateTowards is for rotating towards the navMeshAgent target destination with a turnspeed
				// turnspeed is maxDeltaAngle

				// **** Need to find a correlation between turnspeed, navMeshAgent turn speed
				// taking into account ship's defined turn rate
				var rotation = Quaternion.LookRotation(navMesh.destination - transform.position);
				newRotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * turnspeed);
			}

			var diffRotationY = (newRotation.eulerAngles.y - currentRotationY) / Time.deltaTime;
			diffRotationY = Mathf.Clamp (diffRotationY, -120.0f, 120.0f);
			currentRotationY = newRotation.eulerAngles.y;

			// This updated euler is for tilting when diffRotationY is higher or lower than 0
			// It is clamped to avoid overtilt
			updatedEuler = new Vector3 (0.0f, newRotation.eulerAngles.y, -tilt * diffRotationY);

			newRotation = Quaternion.Euler(updatedEuler);

			transform.rotation = newRotation;
		}
	}
}
