using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FleetControl : MonoBehaviour {
	private List<GameObject> allies;
	private List<GameObject> targetCommand;

	[SerializeField] Toggle ToggleAll;
	[SerializeField] Toggle ToggleBombers;
	[SerializeField] Toggle ToggleInterceptors;
	[SerializeField] Toggle ToggleShielders;


	[SerializeField] RectTransform CommandUI;
	bool selectB = false;
	bool selectI = false;
	bool selectS = false;

	void SelectAll() {
		if (selectB && selectI && selectS) {
			selectB = false;
			selectI = false;
			selectS = false;
		} else {
			selectB = true;
			selectI = true;
			selectS = true;
		}
		UpdateToggle ();
	}

	void SelectBombers() {
		if (selectB) {
			selectB = false;
		} else {
			selectB = true;
		}
		UpdateToggle ();
	}

	void SelectInterceptors() {
		if (selectI) {
			selectI = false;
		} else {
			selectI = true;
		}
		UpdateToggle ();
	}

	void SelectShielders() {
		if (selectS) {
			selectS = false;
		} else {
			selectS = true;
		}
		UpdateToggle ();
	}

	void UpdateToggle() {
		if (selectB && selectI && selectS) {
			ToggleAll.isOn = true;
		}

		ToggleBombers.isOn = selectB;
		ToggleInterceptors.isOn = selectI;
		ToggleShielders.isOn = selectS;

		targetCommand.Clear ();


	}

	// Use this for initialization
	void Start () {
		allies = GameManager.instance.allies;
	}

	// Get input buttons for commands, H for hold, C for charge and F for follow
	void Update() {
		
	}
	
	// Update is called once per frame
	public void Charge () {
		for (int i = 0; i < allies.Count; i++) {
			allies[i].GetComponent<ShipType>().command = "Charge";
		}
	}

	public void Retreat() {
		for (int i = 0; i < allies.Count; i++) {
			allies[i].GetComponent<ShipType>().command = "Retreat";
		}
	}

	public void Follow() {
		for (int i = 0; i < allies.Count; i++) {
			allies[i].GetComponent<ShipType>().command = "Follow";
		}
	}



	public void FollowSpecific() {
		for (int i = 0; i < targetCommand.Count; i++) {
			targetCommand[i].GetComponent<ShipType>().command = "Follow";
		}

		selectI = false;
		selectB = false;
		selectS = false;
	}

	public void Hold() {
		// In the future, allows for a flag to place hold position
		for (int i = 0; i < targetCommand.Count; i++) {
			targetCommand[i].GetComponent<ShipType>().command = "Hold";
			targetCommand [i].GetComponent<ShipType> ().holdPosition = GameManager.instance.player.transform.position;
		}

		selectI = false;
		selectB = false;
		selectS = false;
	}

	public void ChargeSpecific() {
		for (int i = 0; i < targetCommand.Count; i++) {
			targetCommand[i].GetComponent<ShipType>().command = "Charge";
		}

		CommandUI.gameObject.SetActive(false);
		selectI = false;
		selectB = false;
		selectS = false;
	}
}
