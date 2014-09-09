using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallManager : MonoBehaviour {

	List<GameObject> shotEnemies;
	public GameObject[] ballPrefabs;
	public float shootStrength = 80f;

	PlayerManager playerManager;

	Dictionary<string, GameObject> ballPrefabDict;

	void Start () {
		shotEnemies = new List<GameObject> ();
		playerManager = GameObject.FindObjectOfType<PlayerManager>();

		char[] delimChar = new char[1] {'_'};

		ballPrefabDict = new Dictionary<string, GameObject> ();
		for(int i = 0; i < ballPrefabs.Length; i++) {
			ballPrefabDict.Add(ballPrefabs[i].name.Split(delimChar)[0], ballPrefabs[i]);
		}
	}
	
	// Fuck it.
	class ShootData1 {
		public ShootData1(Transform p_dest, string p_color) {dest = p_dest; color = p_color;}
		public Transform dest;
		public string color;
	}

	class ShootData2 {
		public ShootData2(Vector3 p_dest, string p_color) {dest = p_dest; color = p_color;}
		public Vector3 dest;
		public string color;
	}

	void Update () {
//		if(Input.GetButtonDown("Fire1")) {
//			RaycastHit hit;
//			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
//				if(hit.collider.tag == "Enemy" && !shotEnemies.Contains(hit.collider.gameObject)) {
//					shotEnemies.Add(hit.collider.gameObject);
//					StartCoroutine("Shoot1", hit.collider.transform);
//				} else {
//					StartCoroutine("Shoot2", hit.point);
//				}
//			}
//		}
	}

	public void Shoot(Vector2 pos, string color) {
		RaycastHit hit;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(pos), out hit)) {
			if(hit.collider.tag == "Enemy" && !shotEnemies.Contains(hit.collider.gameObject)) {
				shotEnemies.Add(hit.collider.gameObject);
				StartCoroutine("Shoot1", new ShootData1(hit.collider.transform, color));
			} else {
				StartCoroutine("Shoot2", new ShootData2(hit.point, color));
			}
		}
	}

	IEnumerator Shoot1(ShootData1 shootData) {
		GameObject ball = (GameObject)Instantiate (ballPrefabDict[shootData.color], Camera.main.transform.position, Quaternion.identity);
		ball.collider.enabled = false;	//turn collider off so that it doesn't get interrupted while flying towards target
		float shootHeight = 0.2f;
		float speed = 0.9f;

		Vector3 startPos = ball.transform.position - Vector3.up;
		float timer = 0.0f;
		
		while (timer <= 1.0f) {
			float height = Mathf.Sin(Mathf.PI * timer) * shootHeight;
			if(ball == null) {
				StopCoroutine("Shoot1");
				yield return null;
			}
			ball.transform.position = Vector3.Lerp(startPos, shootData.dest.position, timer) + Vector3.up * height; 
			
			timer += Time.deltaTime / speed;
			yield return null;
		}
		ball.collider.enabled = true;	//turn collider on so that it can collide with floor
		shootData.dest.SendMessage ("Hit");
		ball.GetComponent<Ball>().hasCollided = true;

		if(ball.name.Contains("Red")) {
			playerManager.AddPoints("Red", 1);
		} 
		else if(ball.name.Contains("Yellow")) {
			playerManager.AddPoints("Yellow", 1);
		} 
		else if(ball.name.Contains("Green")) {
			playerManager.AddPoints("Green", 1);
		}

		ball.rigidbody.AddForce (Random.onUnitSphere * 800f);
		ball.rigidbody.useGravity = true;
		Destroy (ball, 10f);
	}

	void Shoot2(ShootData2 shootData) {
		GameObject ball = (GameObject)Instantiate (ballPrefabDict[shootData.color], Camera.main.transform.position, Quaternion.identity);

		ball.transform.position = ball.transform.position - Vector3.up * 2f;
		Vector3 shootDir = shootData.dest - ball.transform.position;

		ball.rigidbody.AddForce(shootDir * shootStrength);// = Vector3.Lerp(startPos, target, timer) + Vector3.up * height; 
			
		ball.rigidbody.useGravity = true;
		Destroy (ball, 10f);
	}
}