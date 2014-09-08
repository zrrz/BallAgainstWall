using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private static GameManager _instance;

	enum GameMode {Intro, Main, Scoreboard}

	public GameObject enemy;

	public float spawnRate = 3f;

	SpawnFloor floor;

	public bool randomSpawnTime = false;
	public float randomRange = 1f;

	GameMode mode = GameMode.Intro;
	bool gameStarted = false;
	bool event1 = false;
	bool event2 = false;

	public float joinTimer = 5f;
	public float gameTimer = 180f;
	public float scoreboardTimer = 15f;

	[System.NonSerialized]
	public float timer = 0f;

	GUIStyle guiStyle;
	public Font guiFont;
	public float guiLeft = 0.2f;
	public float guiTop = 0.8f;


	BallManager ballManager;
	PlayerManager playerManager;

	#region Singleton Initialization
	public static GameManager instance {
		get { 
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<GameManager>();
			
			return _instance;
		}
	}
	
	void Awake() {
		if(_instance == null) {
			//If I am the fist instance, make me the first Singleton
			_instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			//If a Singleton already exists and you find another reference in scene, destroy it
			if(_instance != this)
				Destroy(gameObject);
		}
	}
	#endregion

	void Start() {
		ballManager = GetComponent<BallManager> ();
		playerManager = GetComponent<PlayerManager> ();

		guiStyle = new GUIStyle();
	}

	void OnLevelWasLoaded(int level) {
		if(level == 1) {
			floor = GameObject.Find ("Floor").GetComponent<SpawnFloor> ();
			StartCoroutine ("SpawnEnemy");
		}
	}

	void Update() {
		switch(mode)
		{
		case GameMode.Intro:
			if(gameStarted) {
				if(timer > 0f) {
					timer -= Time.deltaTime;
					if(timer <= 0f) {
						timer = gameTimer;
						mode = GameMode.Main;
						Application.LoadLevel("Main");
						return;
					}
				}
			}
			break;
		case GameMode.Main:
			//Incrememts of thirds. i.e. In a 3 minute game, this event will happen after the first minute.
			if(timer <= gameTimer*(2f/3f) && !event1) {
				event1 = true;
			} else if(timer <= gameTimer/3f && !event2) {
				event2 = true;
			} else if(timer <= 0) {
				ballManager.StopAllCoroutines();
				StopAllCoroutines();
				timer = scoreboardTimer;
				mode = GameMode.Scoreboard;
				Application.LoadLevel("Scoreboard");
				return;
			}

			timer -= Time.deltaTime;
			break;
		case GameMode.Scoreboard:
			if(timer <= 0) {
				timer = 0f;
				gameStarted = false;
				playerManager.playerData.Clear();
				mode = GameMode.Intro;

				Application.LoadLevel("Intro");
				return;
			}
			timer -= Time.deltaTime;
			break;
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
			GameObject t_obj = (GameObject)Instantiate (enemy, floor.tiles[column, 0].transform.position, Quaternion.identity);
			t_obj.GetComponent<Enemy>().curColumn = column;
			if(randomSpawnTime)
				yield return new WaitForSeconds(spawnRate + Random.Range(0f, randomRange));
			else
				yield return new WaitForSeconds(spawnRate);
		}
	}

	void OnGUI() {
		//guiStyle.font = guiFont;
//		guiStyle.normal.textColor = Color.white;

		switch(mode) {
		case GameMode.Intro:
			if(timer > 0f) {
				string joinedPlayers = "";
				for(int i = 0; i < playerManager.playerData.Count; i++) {
					joinedPlayers += "\n" + playerManager.playerData[i].color + " has joined!";
				}
				GUI.Label(new Rect(Screen.width/2f - 100f, Screen.height/2f - 100f, 200f, 200f), "Game starts in: " + (int)timer + joinedPlayers/*, guiStyle*/);
			}
			break;
		case GameMode.Main:
			break;
		case GameMode.Scoreboard:
			Rect windowRect = new Rect((float)Screen.width*guiLeft, (float)Screen.height-((float)Screen.height*guiTop), 
			                           (float)Screen.width*(1f-(guiLeft*2f)), (float)Screen.height*(1f-guiTop));
			windowRect = GUILayout.Window(0, windowRect, ScoreboardWindow, "Scoreboard"/*, guiStyle*/);
			break;
		}
	}

	void ScoreboardWindow(int windowID) {

		GUILayout.BeginVertical();

		//GUILayout.Label("");

		foreach(PlayerManager.PlayerData player in playerManager.playerData) {
			GUILayout.BeginHorizontal();

			GUILayout.Label(player.color+" Player", /*guiStyle,*/ GUILayout.Width((float)Screen.width*(guiLeft*2.5f)));
			GUILayout.Label(player.score.ToString()/*, guiStyle*/);

			GUILayout.EndHorizontal();
		}

		GUILayout.EndVertical();
	}


	public void AdjustGameSetting(string setting, float value) {
		switch(setting) 
		{
		default:
			break;
		}
	}

	public void AdjustGameSetting(string setting, bool value) {
		switch(setting)
		{
		default:
			break;
		}
	}
}