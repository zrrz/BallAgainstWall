using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private static GameManager _instance;

	enum GameMode {Intro, Main, Scoreboard}

	public GameObject enemy;
	public Transform enemySpawnPoint;
	public float spawnRate = 3f;

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

	public bool enemiesKnockback = false;

	BallManager ballManager;
	PlayerManager playerManager;
	QueueManager queueManager;

	public TextMesh scoreText;

	GUIText redGameScore, yellowGameScore, greenGameScore; //In-game score gui

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
			queueManager = GameObject.Find( "QueueManager" ).GetComponent<QueueManager>();
			//enemySpawnPoint = GameObject.Find( "EnemySpawnPoint" ).transform;
			redGameScore = GameObject.Find("RedScore").GetComponent<GUIText>();
			yellowGameScore = GameObject.Find("YellowScore").GetComponent<GUIText>();
			greenGameScore = GameObject.Find("GreenScore").GetComponent<GUIText>();

			StartCoroutine ("SpawnEnemy");
			StartCoroutine( "StartEnemyMove" );
			GameObject.Find("GameCamera").camera.enabled = true;
			GameObject.Find("ScoreCamera").camera.enabled = false;
			GameObject.Find("Timer").SetActive(true);

			scoreText = GameObject.Find("ScoreText").GetComponent<TextMesh>();
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
						ChangeScene( "Main" );
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
				//Application.LoadLevel("Scoreboard");
				redGameScore.enabled = false;
				yellowGameScore.enabled = false;
				greenGameScore.enabled = false;
				GameObject.Find("GameCamera").camera.enabled = false;
				GameObject.Find("ScoreCamera").camera.enabled = true;
				GameObject.Find("Timer").SetActive(false);

				scoreText.text = "";
				for(int i = 0; i < playerManager.playerData.Count; i++) {
					scoreText.text += Tab(playerManager.playerData[i].color + ":", 20) + playerManager.playerData[i].score + "\n";
					HighScoreManager.AddScore(playerManager.playerData[i].score);
				}

				return;
			}

			for(int i = 0; i < playerManager.playerData.Count; i++) {
				string tempScoreStr = playerManager.playerData[i].score.ToString();
				
				if(tempScoreStr.Length < 2)
					tempScoreStr = " " + tempScoreStr;
				
				switch( playerManager.playerData[i].color ) {
				case "Red":
					redGameScore.text = "Red:" + tempScoreStr;
					break;
				case "Yellow":
					yellowGameScore.text = "Yellow:" + tempScoreStr;
					break;
				case "Green":
					greenGameScore.text = "Green:" + tempScoreStr;
					break;
				}
			}

			timer -= Time.deltaTime;
			break;
		case GameMode.Scoreboard:
			if(timer <= 0) {
				ChangeScene( "Intro" );
				return;
			}
			timer -= Time.deltaTime;
			break;
		}
	}

	// Why the shit doesnt this work?
	string Tab(string str, int numSpaces) {
		int size = str.Length;
		if(numSpaces > str.Length)
			for(int i = 0; i < numSpaces - size; i++)
				str += " ";
		return str;
	}

	public void BallHit(Vector2 pos, string color) {
		if(!playerManager.Added(color)) {
			playerManager.AddPlayer(color);
		}
		if(!gameStarted) {
			gameStarted = true;
			timer = joinTimer;
		}

		Debug.Log(pos);
			
		pos.x *= Screen.width;
		pos.y = 1 - pos.y;
		pos.y *= Screen.height;
		ballManager.Shoot(pos, color);
	}

	IEnumerator SpawnEnemy() {
		while(true) {
			queueManager.SpawnNewEnemy( enemy );
			yield return new WaitForSeconds( 0.25f );
		}
	}

	IEnumerator StartEnemyMove() {
		while( true ) {
			queueManager.StartNextInQueue();
			yield return new WaitForSeconds( 1.5f );
		}
	}

	void ChangeScene( string scene ) {
		switch( scene )
		{
		case "Intro":
			timer = 0f;
			gameStarted = false;
			playerManager.playerData.Clear();
			mode = GameMode.Intro;			
			Application.LoadLevel("Intro");
			break;
		case "Main":
			timer = gameTimer;
			mode = GameMode.Main;
			Application.LoadLevel("Main");
			break;
		}
	}

	void OnGUI() {
		guiStyle.font = guiFont;
		guiStyle.normal.textColor = Color.yellow;

		switch(mode) {
		case GameMode.Intro:
			if(timer > 0f) {
				string joinedPlayers = "";
				for(int i = 0; i < playerManager.playerData.Count; i++) {
					joinedPlayers += "\n" + playerManager.playerData[i].color + " has joined!";
				}
				GUI.Label(new Rect(Screen.width/2f - 100f, Screen.height/2f - 100f, 200f, 200f), "Game starts in: " + Mathf.CeilToInt(timer) + joinedPlayers, guiStyle);
			}
			break;
		case GameMode.Main:
			break;
		case GameMode.Scoreboard:
//			Rect windowRect = new Rect((float)Screen.width*guiLeft, (float)Screen.height-((float)Screen.height*guiTop), 
//			                           (float)Screen.width*(1f-(guiLeft*2f)), (float)Screen.height*(1f-guiTop));
//			windowRect = GUILayout.Window(0, windowRect, ScoreboardWindow, "Scoreboard"/*, guiStyle*/);
			break;
		}
	}

	void ScoreboardWindow(int windowID) {
		GUILayout.BeginVertical();

		foreach(PlayerManager.PlayerData player in playerManager.playerData) {
			GUILayout.BeginHorizontal();

			GUILayout.Label(player.color+" Player", /*guiStyle,*/ GUILayout.Width((float)Screen.width*(guiLeft*2.5f)));
			GUILayout.Label(player.score.ToString()/*, guiStyle*/);

			GUILayout.EndHorizontal();
		}

		GUILayout.EndVertical();
	}

	public void OSCMessageReceived(OSC.NET.OSCMessage message){
		if(message.Address == "/shoot"){
			ArrayList args = message.Values;
			float x = (float)(args[0]);
			float y = (float)(args[1]);
			Vector2 pos = new Vector2(x,y);
			BallHit(pos, "Red");  
		}
//    	if(message.Address == "/endGame"){
//      		AdjustGameSetting("Quit Game", true);
//      		timer = 0;
//    	} else if(message.Address == "/timeChange"){
//      		ArrayList args = message.Values;
//	      	print(args[0]);
//	     	string recieved = args[0].ToString();
//	     	float time;
//	     	float.TryParse(recieved, out time);
//	      	AdjustGameSetting("Game Time", time);
//    	}
	}


	public void AdjustGameSetting(string setting, float value) {
		switch(setting) 
		{
		case "Game Time":
			gameTimer = value;
			break;
		default:
			break;
		}
	}

	public void AdjustGameSetting(string setting, bool value) {
		switch(setting)
		{
		case "Quit Game":
			ChangeScene( "Intro" );
			break;
		default:
			break;
		}
	}
}
