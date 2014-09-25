﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroGUI : MonoBehaviour {

	public GameObject[] unlitBlocks;
	public GameObject[] litBlocks;

	HighScoreXML highScores;

	public TextMesh dailyScores, weeklyScores, monthlyScores; 

	public TextMesh timerText;

	public enum PlayerColor {
		Green, Purple, Red, Yellow
	}

	public void TurnOnColor(string color) {
		int colorNum = -1;
		switch(color) {
		case "Green":
			colorNum = (int)PlayerColor.Green;
			break;
		case "Purple":
			colorNum = (int)PlayerColor.Purple;
			break;
		case "Red":
			colorNum = (int)PlayerColor.Red;
			break;
		case "Yellow":
			colorNum = (int)PlayerColor.Yellow;
			break;
		default:
			print("Bad color");
			return;
//			break;
		
		}
		if(unlitBlocks[colorNum])
			unlitBlocks[colorNum].SetActive(false);
		if(litBlocks[colorNum])
			litBlocks[colorNum].SetActive(true);
	}

	void Start () {
		highScores = HighScoreManager.s_instance.highScores;

		FormatScores(highScores.dailyScores, dailyScores);

		FormatScores(highScores.weeklyScores, weeklyScores);

		FormatScores(highScores.monthlyScores, monthlyScores);
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
			if(scores[i].time.Minute % 10 == 0) {
				textMesh.text += "0";
				print ("here");
			}

			textMesh.text += "\n";
		}
	}

	void Update () {

	}
}