using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	[System.NonSerialized]
	public int curColumn;

	int curRow;

	public float jumpRate = 2f;

	public float hopHeight = 2.25f;

	SpawnFloor floor;

	class HopData {
		public HopData(Vector3 p_dest, float p_time) {dest = p_dest; time = p_time;}
		public Vector3 dest;
		public float time;
	}

	void Start () {
		floor = GameObject.Find ("Floor").GetComponent<SpawnFloor> ();
		curRow = floor.rows - 1;

		StartCoroutine ("Move");
	}

	void Update () {

	}

	void Hit() {
		Destroy(gameObject);
	}

	IEnumerator Move() {
		float timer = 0f;
		while(curRow >= 0) {
			float t_time = Time.time;

			yield return StartCoroutine ("Hop", new HopData (floor.tiles[curColumn, curRow].transform.position, 1f));

			timer += Time.time - t_time;

			if(timer > jumpRate) {
				timer = 0f;
				int rand = Random.Range(0, 3);
				switch(rand) {
				case 0:
					curRow--; 			// Forward
					break;
				case 1:
					if(curColumn > 0)
						curColumn--; 	// Left
					break;
				case 2:
					if(curColumn < floor.columns - 1)
						curColumn++; 	// Right
					break;
				default:
					print ("wtf");
					break;
				}
			}
		}
		print ("Ouch!");
		Destroy (gameObject);
	}

	IEnumerator Hop(HopData data) {
		Vector3 startPos = transform.position;
		float timer = 0.0f;
		
		while (timer <= 1.0f) {
			float height = Mathf.Sin(Mathf.PI * timer) * hopHeight;
			transform.position = Vector3.Lerp(startPos, data.dest, timer) + Vector3.up * height; 
			
			timer += Time.deltaTime / data.time;
			yield return null;
		}
	}
}
