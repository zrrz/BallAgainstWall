using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreGUI : MonoBehaviour {

	public GameObject redBox, yellowBox, greenBox, purpleBox;

	public GameObject dodgeBall;

	public float growTime = 2f;

	public float maxHeight = 4f;

	public GameObject textPrefab;

	void Start () {
	
	}

	void Update () {
	
	}

	class GrowData {
		public GrowData(GameObject p_box, float p_score, float p_highScore, int p_place) {
			box = p_box; score = p_score; highScore = p_highScore; place = p_place;
		}
		public GrowData(){}
		public GameObject box;
		public float score;
		public float highScore;
		public int place;
	}

	public void Activate() {
		List<PlayerManager.PlayerData> playerData = GameObject.Find("GameManager").GetComponent<PlayerManager>().playerData;
		SortByScore(playerData);

		for(int i = 0; i < playerData.Count; i++) {
			if(playerData[i].color == PlayerColor.Red) {
				redBox.SetActive(true);
				StartCoroutine(GrowScoreBox(new GrowData(redBox, playerData[i].score, playerData[0].score, i + 1)));
			} else if(playerData[i].color == PlayerColor.Purple) {
				purpleBox.SetActive(true);
				StartCoroutine(GrowScoreBox(new GrowData(purpleBox, playerData[i].score, playerData[0].score, i + 1)));
			} else if(playerData[i].color == PlayerColor.Green) {
				greenBox.SetActive(true);
				StartCoroutine(GrowScoreBox(new GrowData(greenBox, playerData[i].score, playerData[0].score, i + 1)));
			} else if(playerData[i].color == PlayerColor.Yellow) {
				yellowBox.SetActive(true);
				StartCoroutine(GrowScoreBox(new GrowData(yellowBox, playerData[i].score, playerData[0].score, i + 1)));
			}
		}
	}

	IEnumerator GrowScoreBox(GrowData data) {
		float startY = data.box.transform.localScale.y;
		if(data.highScore == 0)
			data.highScore++;
		Vector3 endSize = new Vector3(data.box.transform.localScale.x, ((data.score/data.highScore) * maxHeight) + startY, data.box.transform.localScale.z);
		float timer = 0f;
		bool growing = true;

		GameObject text = (GameObject)Instantiate(textPrefab, data.box.transform.position + (Vector3.up*0.3f), Quaternion.identity);
		text.GetComponent<TextMesh>().text = data.score.ToString();

		while(growing) {
			data.box.transform.localScale = Vector3.Lerp(data.box.transform.localScale, endSize, timer);
			if(timer > 1f)
				growing = false;
			timer += Time.deltaTime/growTime;
			yield return null;
		}
//		if(data.place == 1)
//			Instantiate(dodgeBall, data.box.transform.position, Quaternion.identity);

	}

	void SortByScore(List<PlayerManager.PlayerData> players) {
		players.Sort(delegate(PlayerManager.PlayerData x, PlayerManager.PlayerData y) {
			if(x.score > y.score)
				return -1;
			else if(x.score < y.score)
				return 1;
			return 0;
		});
	}
}
