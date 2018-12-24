using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIntrctrl : MonoBehaviour {
	void OnTriggerExit(Collider other) {
		if (other.tag == "IntrctObj") {
			other.gameObject.GetComponent<IntrctCtrl> ().StopInteract();
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "IntrctObj") {
			IntrctCtrl control = other.gameObject.GetComponent<IntrctCtrl> ();
			string intrctButton = control.IntrctButton;
			if (Input.GetButton(intrctButton)) {
				other.gameObject.GetComponent<IntrctCtrl> ().Interact();
			} else {
				other.gameObject.GetComponent<IntrctCtrl> ().StopInteract();
			}
		}
	}
}
