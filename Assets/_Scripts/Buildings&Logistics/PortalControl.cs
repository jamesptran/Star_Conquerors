using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalControl : MonoBehaviour {
	public void GalaxyMap() {
		SceneManager.LoadScene("GalaxyMap", LoadSceneMode.Single);
	}
}
