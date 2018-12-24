using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactController : MonoBehaviour {

	public GameObject explosion;
	public GameObject playerExplosion;
	public bool fromAlly = true;

	// If isProjectile then check damage
	// Damage is from WeaponType
	public float damage = 0;
	ShipType otherShip;
	float otherShield;
	float otherHitpoint;

	void OnTriggerEnter(Collider other) {
		bool eval = (fromAlly && (other.gameObject.tag == "EnemyShip"))
			||
			(!fromAlly && (other.gameObject.tag == "AllyShip" || other.gameObject.tag == "PlayerShip"));
		
		if (eval) {
			otherShip = other.gameObject.GetComponent<ShipType> ();
			otherShield = otherShip.shield;
			otherHitpoint = otherShip.hitpoint;

			if (otherShield > 0) {
				otherShield -= damage;
			} else {
				otherHitpoint -= damage;
			}

			if (otherShield < 0) {
				otherShield = 0;
			}

			otherShip.shield = otherShield;
			otherShip.hitpoint = otherHitpoint;

			if (otherHitpoint <= 0) {
				GameManager.instance.UpdateShipList ();
				GameManager.instance.death.Add (other.gameObject);
			}
			Destroy (gameObject);	
		}

	}
}
