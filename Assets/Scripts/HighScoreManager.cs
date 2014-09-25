using UnityEngine;
using System;
using System.Collections;

public class HighScoreManager : MonoBehaviour {

	[System.NonSerialized]
	public HighScoreXML highScores;

//	public bool showScores = false;

	public static HighScoreManager s_instance;

	void Awake () {
		highScores = HighScoreXML.Load<HighScoreXML>();
		InvokeRepeating("UpdateScores", 0f, 300f);
		s_instance = this;
	}

	public static void AddScore(int score) {
		s_instance.highScores.AddScore(new Score(score, DateTime.Now));

	}

	void UpdateScores() {
		highScores.UpdateScores();
	}
//
//	void OnGUI() {
//		if(showScores) {
//			GUI.BeginGroup(new Rect(Screen.width * 0.25f, Screen.height * 0.15f, Screen.width/2f, Screen.height*0.75f));
//			GUILayout.BeginVertical();//.BeginGroup(new Rect(0f, 0f, 500f, 500f));
//
//			GUILayout.Box("Daily scores:");
//			for(int i = 0; i < highScores.dailyScores.Count; i++) {
//				GUILayout.Label(highScores.dailyScores[i].score + " - " + highScores.dailyScores[i].time);
//			}
//			GUILayout.Space(20f);
//
//			GUILayout.Box("Weekly scores:");
//			for(int i = 0; i < highScores.weeklyScores.Count; i++) {
//				GUILayout.Label(highScores.weeklyScores[i].score + " - " + highScores.weeklyScores[i].time);
//			}
//			GUILayout.Space(20f);
//
//			GUILayout.Box("Monthly scores:");
//			for(int i = 0; i < highScores.monthlyScores.Count; i++) {
//				GUILayout.Label(highScores.monthlyScores[i].score + " - " + highScores.monthlyScores[i].time);
//			}
//
//			GUILayout.EndVertical();
//			GUI.EndGroup();
//		}
//	}
}
