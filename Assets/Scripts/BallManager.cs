using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallManager : MonoBehaviour {

	List<GameObject> shotEnemies;
	public GameObject[] ballPrefabs;
	public float shootStrength = 80f;

	PlayerManager playerManager;

	void Start () {
		shotEnemies = new List<GameObject> ();
		playerManager = GameObject.FindObjectOfType<PlayerManager>();
	}

	void Update () {
		if(Input.GetButtonDown("Fire1")) {
			RaycastHit hit;
			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
				if(hit.collider.tag == "Enemy" && !shotEnemies.Contains(hit.collider.gameObject)) {
					shotEnemies.Add(hit.collider.gameObject);
					StartCoroutine("Shoot1", hit.collider.transform);
				} else {
					StartCoroutine("Shoot2", hit.point);
				}
			}
		}
	}

	public void Shoot(Vector2 pos, string color) {

	}

	IEnumerator Shoot1(Transform target) {
		GameObject ball = (GameObject)Instantiate (ballPrefabs [Random.Range (0, ballPrefabs.Length)], Camera.main.transform.position, Quaternion.identity);
		ball.collider.enabled = false;	//turn collider off so that it doesn't get interrupted while flying towards target
		float shootHeight = 1f;
		float speed = 0.5f;

		Vector3 startPos = ball.transform.position - Vector3.up * 2f;
		float timer = 0.0f;
		
		while (timer <= 1.0f) {
			float height = Mathf.Sin(Mathf.PI * timer) * shootHeight;
			ball.transform.position = Vector3.Lerp(startPos, target.position, timer) + Vector3.up * height; 
			
			timer += Time.deltaTime / speed;
			yield return null;
		}
		ball.collider.enabled = true;	//turn collider on so that it can collide with floor
		target.SendMessage ("Hit");
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

	void Shoot2(Vector3 target) {
		GameObject ball = (GameObject)Instantiate (ballPrefabs [Random.Range (0, ballPrefabs.Length)], Camera.main.transform.position, Quaternion.identity);

		ball.transform.position = ball.transform.position - Vector3.up * 2f;
		Vector3 shootDir = target - ball.transform.position;

		ball.rigidbody.AddForce(shootDir * shootStrength);// = Vector3.Lerp(startPos, target, timer) + Vector3.up * height; 
			
		ball.rigidbody.useGravity = true;
		Destroy (ball, 10f);
	}
}