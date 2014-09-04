using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject enemy;

	public float spawnRate = 3f;

	SpawnFloor floor;

	public bool randomSpawnTime = false;
	public float randomRange = 1f;

	bool gameStarted = false;

	public float joinTimer = 5f;
	float timer = 0f;

	BallManager ballManager;
	PlayerManager playerManager;

	void Start() {
		GameObject.DontDestroyOnLoad(gameObject);
		ballManager = GetComponent<BallManager> ();
		playerManager = GetComponent<PlayerManager> ();
	}

	void OnLevelWasLoaded(int level) {
		if(level == 1) {
			floor = GameObject.Find ("Floor").GetComponent<SpawnFloor> ();
			StartCoroutine ("SpawnEnemy");
		}
	}

	void Update() {
		if(gameStarted) {
			if(timer > 0f) {
				timer -= Time.deltaTime;
				if(timer < 0f)
					Application.LoadLevel("Main");
			}
		}
	}

	public void BallHit(Vector2 pos, string color) {
		if(!playerManager.Added(color)) {
			playerManager.AddPlayer(color);
		}
		if(!gameStarted) {
			gameStarted = true;
			timer = joinTimer;
		} else if(timer <= 0) {
			ballManager.Shoot(pos, color);
		}
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

	void OnGUI() {
		if(timer > 0f) {
			string joinedPlayers = "";
			for(int i = 0; i < playerManager.playerData.Count; i++) {
				joinedPlayers += "\n" + playerManager.playerData[i].color + " has joined!";
			}
			GUI.Label(new Rect(Screen.width/2f - 100f, Screen.height/2f - 100f, 200f, 200f), "Game starts in: " + (int)timer + joinedPlayers);

		}
	}
}