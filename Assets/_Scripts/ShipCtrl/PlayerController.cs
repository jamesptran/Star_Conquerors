using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float moveSpeed;
	public float rotateSpeed;
	Rigidbody rigidBody;
	ShipType ship;
	float maxHP;
	float maxShield;
	float MaxHealthWidth;

	// Varibles to update the health and shield bar every frame
	float height;
	float width;
	float hpWidth;
	float shieldWidth;

	[SerializeField] RectTransform hitpointBar;
	[SerializeField] RectTransform shieldBar; 

	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
		rigidBody.drag = 5.0f;
		ship = GetComponent<ShipType> ();

		maxHP = ship.hitpoint;
		maxShield = ship.shield;
		MaxHealthWidth = hitpointBar.sizeDelta.x;
	}

	void Update() {
		// Update health and shield bar every frame
		height = hitpointBar.sizeDelta.y;
		width = MaxHealthWidth;

		hpWidth = width * (ship.hitpoint / maxHP);
		shieldWidth = width * (ship.shield / maxShield);

		hitpointBar.sizeDelta = new Vector2 (hpWidth, height);
		UpdatePosition (hitpointBar);

		shieldBar.sizeDelta = new Vector2 (shieldWidth, height);
		UpdatePosition (shieldBar);


	}

	void UpdatePosition(RectTransform bar) {
		bar.localPosition = new Vector3 (
			-(MaxHealthWidth - bar.sizeDelta.x) / 2.0f, 
			bar.localPosition.y, 
			bar.localPosition.z
		);
	}

	void FixedUpdate(){
		if (Input.GetKey ("w")) {
			rigidBody.AddForce (transform.forward * moveSpeed);
		}
		if (Input.GetKey ("a")) {			
			rigidBody.angularVelocity -= new Vector3 (0.0f, rotateSpeed, 0.0f);
		}
		if (Input.GetKey ("d")) {
			rigidBody.angularVelocity -= new Vector3 (0.0f, -rotateSpeed, 0.0f);
		}
		if (Input.GetKey ("s")) {
			rigidBody.AddForce (transform.forward * -moveSpeed/4);
		}
	}
}
