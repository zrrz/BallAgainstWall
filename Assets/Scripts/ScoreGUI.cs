using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreGUI : MonoBehaviour {

	public GameObject redBox, yellowBox, greenBox, purpleBox;

	public GameObject dodgeBall;

	public float growTime = 2f;

	public float maxHeight = 5f;

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
//		SortByScore(playerData);

		PlayerManager.PlayerData highest = playerData[0];

		if(playerData.Count > 1) {
			for(int i = 1; i < playerData.Count; i++) {
				if(playerData[i].score > highest.score)
					highest = playerData[i];
			}
		}

		foreach(PlayerManager.PlayerData player in playerData) {
			if(player.color == "Red") {
				redBox.SetActive(true);
				StartCoroutine(GrowScoreBox(new GrowData(redBox, player.score, highest.score, 1)));
			} else if(player.color == "Purple") {
				redBox.SetActive(true);
				StartCoroutine(GrowScoreBox(new GrowData(purpleBox, player.score, highest.score, 1)));
			} else if(player.color == "Green") {
				redBox.SetActive(true);
				StartCoroutine(GrowScoreBox(new GrowData(greenBox, player.score, highest.score, 1)));
			} else if(player.color == "Yellow") {
				redBox.SetActive(true);
				StartCoroutine(GrowScoreBox(new GrowData(yellowBox, player.score, highest.score, 1)));
			}
		}
	}

	IEnumerator GrowScoreBox(GrowData data) {
		Vector3 endSize = new Vector3(data.box.transform.localScale.x, (data.score/data.highScore) * maxHeight, data.box.transform.localScale.z);
		print (endSize);
		float timer = 0f;
		bool growing = true;

		while(growing) {
			data.box.transform.localScale = Vector3.Lerp(data.box.transform.localScale, endSize, timer);
			if(timer > 1f)
				growing = false;
			timer += Time.deltaTime/growTime;
			yield return null;
		}
		if(data.place == 1)
			Instantiate(dodgeBall, data.box.transform.position, Quaternion.identity);
		GameObject text = (GameObject)Instantiate(textPrefab, data.box.transform.position, Quaternion.identity);
		text.GetComponent<TextMesh>().text = data.score.ToString();
	}

//	void SortByScore(List<PlayerManager.PlayerData> players) {
//		players.Sort(delegate(PlayerManager.PlayerData x, PlayerManager.PlayerData y) {
//			if(x.score > y.score)
//				return x;
//			return y;
//		});
//	}
}
