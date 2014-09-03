using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallManager : MonoBehaviour {

	List<GameObject> shotEnemies;
	public GameObject[] ballPrefabs;

	void Start () {
		shotEnemies = new List<GameObject> ();
	}

	void Update () {
		if(Input.GetButton("Fire1")) {
			RaycastHit hit;
			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
				if(hit.collider.tag == "Enemy" && !shotEnemies.Contains(hit.collider.gameObject)) {
					shotEnemies.Add(hit.collider.gameObject);
					StartCoroutine("Shoot1", hit.collider.transform);
				}
			}
		}
	}

	public void Shoot(Vector2 pos, string color) {

	}

	IEnumerator Shoot1(Transform target) {
		GameObject ball = (GameObject)Instantiate (ballPrefabs [Random.Range (0, ballPrefabs.Length)], Camera.main.transform.position, Quaternion.identity);
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
		target.SendMessage ("Hit");
		ball.rigidbody.AddForce (Random.onUnitSphere * 800f);
		ball.rigidbody.useGravity = true;
		Destroy (ball, 10f);
	}
}