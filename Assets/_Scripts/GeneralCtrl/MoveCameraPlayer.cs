using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraPlayer : MonoBehaviour {
	public GameObject player;       //Public variable to store a reference to the player game object
	private Vector3 offset;         //Private variable to store the offset distance between the player and camera

	// Use this for initialization
	void Start () 
	{
		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		offset = transform.position - player.transform.position;
	}

	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		transform.position = player.transform.position + offset;

		Vector3 currentAngle = transform.rotation.eulerAngles;
		Vector3 updatedEuler = new Vector3 (
			80.0f, 
			player.transform.rotation.eulerAngles.y, 
			0.0f
		);

		transform.rotation = Quaternion.Euler(updatedEuler);
	}
}
