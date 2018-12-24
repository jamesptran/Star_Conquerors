using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveProjectile : MonoBehaviour {

	public float speed;
	public float projectileRange;

	public Vector3 initialPosition;

	// Move projectile in a straight line until the distance exceeds the weapon range
	// Speed is the movespeed of the projectile, projectileRange is the range

	// Use this for initialization
	void Start () {
		var rigidBody = GetComponent<Rigidbody> ();

		rigidBody.drag = 0;
		rigidBody.angularDrag = 0;

		rigidBody.velocity = transform.forward * speed;
	}

	void Update() {
		if (Vector3.Distance (transform.position, initialPosition) > projectileRange) {
			Destroy (gameObject);
		}
	}
}
