using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject enemy;

	public float spawnRate = 3f;

	SpawnFloor floor;

	public bool randomSpawnTime = false;
	public float randomRange = 1f;

	void Start () {
		floor = GameObject.Find ("Floor").GetComponent<SpawnFloor> ();

		StartCoroutine ("SpawnEnemy");
	}

	IEnumerator SpawnEnemy() {
		while(true) {
			int column = Random.Range (0, floor.columns);
			GameObject t_obj = (GameObject)Instantiate (enemy, floor.tiles[column, floor.rows - 1].transform.position, Quaternion.identity);
			t_obj.GetComponent<Enemy>().curColumn = column;
			if(randomSpawnTime)
				yield return new WaitForSeconds(spawnRate + Random.Range(0f, randomRange));
			else
				yield return new WaitForSeconds(spawnRate);
		}
	}
}