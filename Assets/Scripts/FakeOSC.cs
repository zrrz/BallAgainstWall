using UnityEngine;
using System.Collections;

public class FakeOSC : MonoBehaviour {
	
	GameManager gameManager;

	string[] colors;

	void Start () {
		gameManager = GetComponent<GameManager>();
	
		colors = new string[3] {"Red", "Green", "Yellow"};
	}

	void Update () {
		if(Input.GetButtonDown("Fire1")) {
			BallHit(Input.mousePosition, colors[0]);
		}
	}

	void BallHit(Vector2 pos, string color) {
		gameManager.BallHit(pos, color);
	}
}