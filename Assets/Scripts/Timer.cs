using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	GameManager manager;
	TextMesh textMesh;

	// Use this for initialization
	void OnLevelWasLoaded(int level) {
		manager = GameManager.instance;

		if(manager == null)
			Destroy(this);

		textMesh = GetComponent<TextMesh>();
		transform.LookAt (Camera.main.transform.position + Camera.main.transform.forward * 1000f);
	}
	
	// Update is called once per frame
	void Update () {
		if(manager) {
			string minutes = ((int)(manager.timer / 60)).ToString();
			string seconds = ((int)(manager.timer % 60f)).ToString();
		
			if(seconds.Length == 1) {
				seconds = "0"+seconds;
			}

			textMesh.text = minutes + ":" + seconds;
		}
	}
}
