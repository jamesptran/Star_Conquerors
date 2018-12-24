using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntrctCtrl : MonoBehaviour {

	[SerializeField]
	Transform IntrctPanel;
	[SerializeField]

	public string IntrctButton = "Interact";

	public void Interact() {
		IntrctPanel.gameObject.SetActive (true);
	}

	public void StopInteract() {
		IntrctPanel.gameObject.SetActive (false);
	}
}
