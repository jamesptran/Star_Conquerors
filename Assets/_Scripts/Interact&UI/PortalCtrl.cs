using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {
	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "PlayerShip") {
			Debug.Log ("Ship touched portal");
		}
	
	}
}
