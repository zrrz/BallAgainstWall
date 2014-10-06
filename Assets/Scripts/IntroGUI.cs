using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroGUI : MonoBehaviour {

	public GameObject[] unlitBlocks;
	public GameObject[] litBlocks;

	HighScoreXML highScores;

	public TextMesh dailyScores, weeklyScores, monthlyScores; 

	public TextMesh timerText;

	float timer = 0f;

	enum State {
		Splash, Directions, Score
	}

	State state;

	GameObject splash, score, directions;

	public float splashDuration = 10f, directionDuration = 3f, scoreDuration = 6f;

	public void TurnOnColor(PlayerColor color) {
//		if(unlitBlocks[(int)color])
//			unlitBlocks[(int)color].SetActive(false);
//		if(litBlocks[(int)color])
//			litBlocks[(int)color].SetActive(true);
	}

	void Start () {
		splash = GameObject.Find("Splash");
		score = GameObject.Find("Score");
		directions = GameObject.Find("Directions");

		highScores = HighScoreManager.s_instance.highScores;

		FormatScores(highScores.dailyScores, dailyScores);
		FormatScores(highScores.weeklyScores, weeklyScores);
		FormatScores(highScores.monthlyScores, monthlyScores);

		dailyScores.renderer.sortingOrder = 5;
		weeklyScores.renderer.sortingOrder = 5;
		monthlyScores.renderer.sortingOrder = 5;

		GameObject.Find("Daily Title").renderer.sortingOrder = 5;
		GameObject.Find("Weekly Title").renderer.sortingOrder = 5;
		GameObject.Find("Monthly Title").renderer.sortingOrder = 5;

		score.SetActive(false);

		state = State.Splash;
		directions.SetActive(false);
	}

	void Update() {
		timer += Time.deltaTime;

		switch(state) {
		case State.Splash:
			if(timer > splashDuration) {
				timer = 0f;
				state = State.Score;
				splash.SetActive(false);
				score.SetActive(true);
			}
			break;
		case State.Score:
			if(timer > scoreDuration) {
				timer = 0f;
				state = State.Directions;
				score.SetActive(false);
				directions.SetActive(true);
			}
			break;
		case State.Directions:
			if(timer > directionDuration) {
				timer = 0f;
				state = State.Splash;
				directions.SetActive(false);
				splash.SetActive(true);
			}
			break;
		default:
			break;
		}
	}

	void FormatScores(List<Score> scores, TextMesh textMesh) {
		textMesh.text = "";
		for(int i = 0; i < scores.Count; i++) {
			textMesh.text += scores[i].score.ToString();
			if(scores[i].score/10 == 0) {
				textMesh.text += "  ";
			}

			textMesh.text += " - " + scores[i].time.Day + "/" + scores[i].time.Month + "/";
			textMesh.text += (scores[i].time.Year - scores[i].time.Year/100 * 100) + " ";
			textMesh.text += scores[i].time.Hour + ":";

			if(scores[i].time.Minute < 10)
				textMesh.text += "0";
			textMesh.text += scores[i].time.Minute;
//			if(scores[i].time.Minute % 10 == 0) {
//				textMesh.text += "0";
//			}

			textMesh.text += "\n";
		}
	}
}