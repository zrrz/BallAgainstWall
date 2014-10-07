using UnityEngine;
using System.Collections;

public class PlayMovie : MonoBehaviour {

	MovieTexture tex;

	void Start () {
		tex = (MovieTexture)renderer.material.mainTexture;
		tex.loop = true;
		tex.Play();
	}

	void Update () {
//		if(!tex.isPlaying) {
//			tex.Play();
//		}
	}
}
