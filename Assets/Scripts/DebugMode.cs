using UnityEngine;
using System.Collections;

public class DebugMode : MonoBehaviour {

	public static bool GRAVITY = true;
	public static bool FORWARDMODE = false;
	public static bool CENTERSPAWN = false;
	public static float FORCECHANGE = 0f; 

	void Update () {
		if(Input.GetKeyDown(KeyCode.F1)) {
			GRAVITY = !GRAVITY;
		}
		if(Input.GetKeyDown(KeyCode.F2)) {
			FORWARDMODE = !FORWARDMODE;
		}
		if(Input.GetKeyDown(KeyCode.F3)) {
			CENTERSPAWN = !CENTERSPAWN;
		}
		if(Input.GetKey(KeyCode.LeftShift)) {
			if(Input.GetKeyDown(KeyCode.LeftBracket))
				FORCECHANGE -= 100f;
			if(Input.GetKeyDown(KeyCode.RightBracket))
				FORCECHANGE += 100f;
		}
	}

	void OnGUI() {
		GUI.Box(new Rect(0f, 0f, 250f, 70f), "F1: Gravity - " + GRAVITY + "\nF2: ForwardMode - " + FORWARDMODE + "\nF3: CenterSpawn - " + CENTERSPAWN + "\nLShift+Brackets:Change Force - " + FORCECHANGE);
	}
}
