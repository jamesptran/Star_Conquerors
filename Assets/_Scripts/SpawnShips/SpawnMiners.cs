using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMiners : MonoBehaviour {
	int totalMiners = 0;
	[SerializeField] GameObject minerPrefab;
	float time = 0f;
	float delay = 1f;

	void Start() {
		totalMiners = GlobalControl.Instance.MinersTotal;
	}


	void Update() {
		time += Time.deltaTime;
		if (time > delay) {
			time = 0f;
			if (totalMiners > 0) {
				Instantiate(minerPrefab, transform.position, transform.rotation, this.transform);
				totalMiners -= 1;
			}
		}
	}
}
