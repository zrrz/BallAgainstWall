using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreGUI : MonoBehaviour {

	public GameObject redBox, yellowBox, greenBox, blueBox;
	public GameObject redContainer, yellowContainer, greenContainer, blueContainer;

	public GameObject dodgeBall;

	public float growTime = 2f;

	public float maxHeight = 4f;

	public GameObject textPrefab;

	public float fadeInSpeed = 1f;

	GameObject endScoreZone;

	void Start () {
		endScoreZone = GameObject.Find("EndScoreZone");
		endScoreZone.SetActive(false);
	}

	void Update () {
	
	}

	class GrowData {
		public GrowData(GameObject p_box, TextMesh p_scoreText, float p_score, float p_highScore, int p_place) {
			box = p_box; scoreText = p_scoreText; score = p_score; highScore = p_highScore; place = p_place;
		}
		public GrowData(){}
		public GameObject box;
		public TextMesh scoreText;
		public float score;
		public float highScore;
		public int place;
	}

	public void Activate() {
		List<PlayerManager.PlayerData> playerData = GameObject.Find("GameManager").GetComponent<PlayerManager>().playerData;
		SortByScore(playerData);

		GameObject.Find("InGameScoreZone").gameObject.SetActive(false);
		endScoreZone.SetActive(true);

		for(int i = 0; i < playerData.Count; i++) {
			if(playerData[i].color == PlayerColor.Red) {
				redBox.SetActive(true);
				redContainer.SetActive(true);
			} else if(playerData[i].color == PlayerColor.Blue) {
				blueBox.SetActive(true);
				blueContainer.SetActive(true);
			} else if(playerData[i].color == PlayerColor.Green) {
				greenBox.SetActive(true);
				greenContainer.SetActive(true);
			} else if(playerData[i].color == PlayerColor.Yellow) {
				yellowBox.SetActive(true);
				yellowContainer.SetActive(true);
			}
		}

		StartCoroutine("FadeIn", playerData);
	}

	IEnumerator FadeIn(List<PlayerManager.PlayerData> playerData) {
		GameObject scoreZone = endScoreZone;
		Renderer[] renderers = scoreZone.GetComponentsInChildren<Renderer>();

		List<float> endAlphas = new List<float>();

		Color t_color = Color.white;

		for(int i = 0; i < renderers.Length; i++) {
			t_color = renderers[i].material.color;
			endAlphas.Add(t_color.a);
			t_color.a = 0f;
			renderers[i].material.color = t_color;
		}

		float timer = 0f;
		while(timer < 1f) {
			for(int i = 0; i < renderers.Length; i++) {
				t_color.a = Mathf.Lerp(0f, endAlphas[i], timer);
				renderers[i].material.color = t_color;
			}
			timer += Time.deltaTime/fadeInSpeed;
			yield return null;
		}

		for(int i = 0; i < playerData.Count; i++) {
			if(playerData[i].color == PlayerColor.Red) {
				StartCoroutine(GrowScoreBox(new GrowData(redBox, redContainer.GetComponentInChildren<TextMesh>(), playerData[i].score, playerData[0].score, i + 1)));
			} else if(playerData[i].color == PlayerColor.Blue) {
				StartCoroutine(GrowScoreBox(new GrowData(blueBox, blueContainer.GetComponentInChildren<TextMesh>(), playerData[i].score, playerData[0].score, i + 1)));
			} else if(playerData[i].color == PlayerColor.Green) {
				StartCoroutine(GrowScoreBox(new GrowData(greenBox, greenContainer.GetComponentInChildren<TextMesh>(), playerData[i].score, playerData[0].score, i + 1)));
			} else if(playerData[i].color == PlayerColor.Yellow) {
				StartCoroutine(GrowScoreBox(new GrowData(yellowBox, yellowContainer.GetComponentInChildren<TextMesh>(), playerData[i].score, playerData[0].score, i + 1)));
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

		while(growing) {
			data.box.transform.localScale = Vector3.Lerp(data.box.transform.localScale, endSize, timer);
			data.scoreText.text = (Mathf.Ceil(Mathf.Lerp(float.Parse(data.scoreText.text), data.score, timer))).ToString();
			if(timer > 1f) {
				growing = false;
				data.scoreText.text = data.score.ToString();
			}
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
