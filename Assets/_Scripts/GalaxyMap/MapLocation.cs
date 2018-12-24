using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocation : MonoBehaviour {
	[SerializeField] Texture2D cursor;

	void Start() {
	}

	void OnMouseOver() {
		Cursor.SetCursor (cursor, Vector2.zero, CursorMode.Auto);
	}

	void OnMouseExit() {
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}
}
