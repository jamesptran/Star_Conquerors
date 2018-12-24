using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponType : MonoBehaviour {
	public float damage;
	public float attackspeed;
	public float range;
	public float projectileSpeed;
	public string weaponname;
	public ShipType ship;
	public bool fromAlly = true;

	// Projectile has to have contactController and ProjectileMove components
	private float fireRate = 0.5F;
	private float nextFire = 0.5F;
	public GameObject newProjectile;

	// Use this for initialization of weapon's stats based on weapon name
	// Also initialize projectile ally vs enemy
	void Start () {
		damage = 1;
		attackspeed = 200;
		range = 10;
		projectileSpeed = 30;

		if (ship.tag == "AllyShip") {
			fromAlly = true;
		}
		if (ship.tag == "EnemyShip") {
			fromAlly = false;
		}
	}

	// Update is called once per frame
	void Update() {
		fireRate = 100 / attackspeed;
		if (ship.tag == "PlayerShip") {
			if (Input.GetButton("Fire1") && Time.time > nextFire) {
				FireAtSpawn ();
			}
		} else {
			if (ship.GetComponent<ShipType> ().state == "Attacking" && Time.time > nextFire) {
				FireAtSpawn ();
			}
		}
	}

	void FireAtSpawn() {
		nextFire = Time.time + fireRate;

		var contactController = newProjectile.GetComponent<ContactController> ();
		var moveProjetile = newProjectile.GetComponent<MoveProjectile> ();

		contactController.damage = damage;
		contactController.fromAlly = fromAlly;

		moveProjetile.speed = projectileSpeed;
		moveProjetile.projectileRange = range;
		moveProjetile.initialPosition = ship.transform.position;

		Instantiate(newProjectile, transform.position, transform.rotation);
	}
}
