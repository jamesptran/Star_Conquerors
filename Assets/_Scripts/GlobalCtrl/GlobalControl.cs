using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour {
	public static GlobalControl Instance;

	public float HP;
	public float Shield;
	public float XP;

	// Array for keeping different levels units
	public int BombersParty;
	public int BombersTotal;
	public int ShieldersParty;
	public int ShieldersTotal;
	public int InterceptorsParty;
	public int InterceptorsTotal;

	public int MinersTotal;

	public int currentIC = 0;


	// Rank from 1-7, changes appearance and power
	public int BombersRank;
	public int ShieldersRank;
	public int InterceptorsRank;

	public int MinersRank;

	void Awake () {
		if (Instance == null) {
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if (Instance != this) {
			Destroy (gameObject);
		}
		MinersTotal = 5;
	}
}
