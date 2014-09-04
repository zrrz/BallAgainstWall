using UnityEngine;
using System.Collections;

public class FakeOSC : MonoBehaviour {
	
	GameManager gameManager;

	string[] colors;

	int curColor = 0;

	void Start () {
		gameManager = GetComponent<GameManager>();
	
		colors = new string[3] {"Red", "Green", "Yellow"};
	}

	void Update () {
		if(Input.GetButtonDown("Fire1")) {
			BallHit(Input.mousePosition, colors[curColor]);
		}

		if(Input.GetKeyDown(KeyCode.Q)) {
			curColor--;
			if(curColor < 0)
				curColor = colors.Length - 1;
		}

		if(Input.GetKeyDown(KeyCode.E)) {
			curColor++;
			if(curColor > colors.Length - 1)
				curColor = 0;
		}
	}

	void BallHit(Vector2 pos, string color) {
		gameManager.BallHit(pos, color);
	}

	void OnGUI() {
		GUI.Box (new Rect (0f, 0f, 140f, 40f), "Ball Color: " + colors [curColor] + "\n Q/E to toggle colors");
	}
}