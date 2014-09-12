using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

	public class PlayerData {
		public PlayerData(string p_color) {
			active = true;
			color = p_color;
		}
		public bool active = false;
		public int score = 0;
		public string color = "";
	}

	public List<PlayerData> playerData;

	static PlayerManager s_instance;

	void Start () {
		playerData = new List<PlayerData> ();
		s_instance = this;
	}

	void Update () {

	}

	public void AddPlayer(string color) {
		playerData.Add (new PlayerData (color));
	}

	public bool Added(string color) {
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

	public static void AddPoints(string p_color, int pts) {
		foreach(PlayerData player in s_instance.playerData){
			if(player.color == p_color) {
				player.score += pts;
				return;
			}
		}
	}
}