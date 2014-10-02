using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayerColor {
	Green, Purple, Red, Yellow
}

public class PlayerManager : MonoBehaviour {

	public class PlayerData {
		public PlayerData(PlayerColor p_color) {
			color = p_color;
		}
		public int score = 0;
		public int totalShots = 0;
		public int shotsHit = 0;
		public PlayerColor color = PlayerColor.Red;
	}

	public List<PlayerData> playerData;

	static PlayerManager s_instance;

	void Start () {
		playerData = new List<PlayerData> ();
		s_instance = this;
	}

	void Update () {

	}

	void OnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 100f, 0f, 100f, 200f));
		GUILayout.BeginVertical();
		foreach(PlayerData player in playerData){
			GUILayout.Label(player.color.ToString() + " " + player.shotsHit + "/" + player.totalShots);

		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	public static void IncreaseShots(PlayerColor color) {
		foreach(PlayerData player in s_instance.playerData){
			if(player.color == color) {
				player.totalShots ++;
				return;
			}
		}
	}

	public static void IncreaseHits(PlayerColor color) {
		foreach(PlayerData player in s_instance.playerData){
			if(player.color == color) {
				player.shotsHit++;
				return;
			}
		}
	}

	public void AddPlayer(PlayerColor color) {
		playerData.Add (new PlayerData (color));
	}

	public bool Added(PlayerColor color) {
		for(int i = 0; i < playerData.Count; i++) {
			if(playerData[i].color == color)
				return true;
		}
		return false;
	}

	public static void ReducePoints(int dmg) {
		foreach(PlayerData player in s_instance.playerData) {
			if(player.score < 0)
				player.score -= dmg;
		}
	}

	public static void AddPoints(PlayerColor color, int pts) {
		foreach(PlayerData player in s_instance.playerData){
			if(player.color == color) {
				player.score += pts;
				return;
			}
		}
	}
}