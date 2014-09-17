using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	[System.NonSerialized]
	public int curColumn;
	[System.NonSerialized]
	public int omittedColumns = 1;

	int curRow;

	public float jumpRate = 2f;
	public float hopHeight = 2.25f;

	SpawnFloor floor;

	Animator animator;

	public List<Texture> textures;
	public List<Renderer> renderers;

	public float moveForwardChancePct = 50f;

	bool hit = false;

	[System.NonSerialized]
	public string hitBy = "";

	class HopData {
		public HopData(Vector3 p_dest, float p_time) {dest = p_dest; time = p_time;}
		public Vector3 dest;
		public float time;
	}

	void Awake () {
		int rand = Random.Range(0, textures.Count - 1);
		foreach(Renderer rend in renderers) {
			rend.material.mainTexture = textures[rand];
		}

		moveForwardChancePct = Mathf.Clamp(moveForwardChancePct, 0f, 100f);


		floor = GameObject.Find ("Floor").GetComponent<SpawnFloor> ();
		curRow = 0;

		animator = transform.GetChild(0).GetComponent<Animator> ();

		//StartCoroutine ("Move");
	}

	void Update () {

	}

	void OnCollisionEnter(Collision col) {
		if(GameManager.instance.enemiesKnockback) {
			if(col.gameObject.tag == "Enemy") {
				hitBy = col.transform.GetComponentInParent<Enemy>().hitBy;
				Hit (hitBy);
			//	col.gameObject.SendMessageUpwards("Hit", hitBy, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public void StartMove() {
		StartCoroutine("Move");
	}

	void Hit(string p_hitBy) {
		if(!hit) {
//			collider.enabled = false;
			hitBy = p_hitBy;

			if(p_hitBy.Contains("Red")) {
				PlayerManager.AddPoints("Red", 1);
			} 
			else if(p_hitBy.Contains("Yellow")) {
				PlayerManager.AddPoints("Yellow", 1);
			} 
			else if(p_hitBy.Contains("Green")) {
				PlayerManager.AddPoints("Green", 1);
			} else {
				print ("wtf");
			}

			hit = true;
			StopCoroutine ("Move");
			StopCoroutine ("Hop");
			//collider.enabled = false;
			transform.GetChild (0).gameObject.SetActive (false);
			transform.GetChild (1).gameObject.SetActive (true);
			transform.GetComponentInChildren<Rigidbody> ().AddForce (Vector3.forward * 1000f);
			Destroy(gameObject, 1f);
		}
	}

	IEnumerator Move() {
		float timer = 0f;
		while(curRow <= floor.rows - 1) {
			float t_time = Time.time;

			yield return StartCoroutine ("Hop", new HopData (floor.tiles[curColumn, curRow].transform.position, 2f));
			//yield return StartCoroutine ("Hop", new HopData (floor.tiles[curColumn, curRow].transform.position, 1.78f));

			timer += Time.time - t_time;

			if(timer > jumpRate) {
				timer = 0f;
				float rand = Random.Range(0f, 100f);
				if(rand < moveForwardChancePct) {
					curRow++; 			// Forward
				} else if(rand < moveForwardChancePct + ((100f - moveForwardChancePct)/2f)) {
					if(curColumn > 0)
						curColumn--; 	// Left
				} else {
					if(curColumn < floor.columns - 1)
						curColumn++; 	// Right
				}
			}
		}
		print ("Ouch!");
		PlayerManager.ReducePoints(1);
		Destroy (gameObject);
	}

	IEnumerator Hop(HopData data) {
		animator.SetBool ("Land", false);
		animator.SetBool("Jump", true);
		floor.tiles [curColumn, curRow].GetComponent<Animator> ().SetTrigger ("Bounce");
		Vector3 startPos = transform.position;
		float timer = 0.0f;
		
		while (timer <= 1.0f) {
			float height = Mathf.Sin(Mathf.PI * timer) * hopHeight;
			transform.position = Vector3.Lerp(startPos, data.dest, timer) + Vector3.up * height; 
			
			timer += Time.deltaTime / data.time;
			yield return null;
			animator.SetBool ("Jump", false);
			if(timer > .90f)
				animator.SetBool ("Land", true);
		}
		animator.SetBool ("Land", true);
	}

	public void GoToQueuePos( Waypoint dest ) {
		StartCoroutine( "MoveToQueuePos", dest );
	}

	IEnumerator MoveToQueuePos( Waypoint dest ) {
		float timer = 0f;
		float lerpTime = 1.0f;
		Vector3 startPos = transform.position;

		while( timer <= 1.0f ) {
			transform.position = Vector3.Lerp( startPos, dest.transform.position, timer );
			timer += Time.deltaTime / lerpTime;
			yield return null;
		}

		dest.m_occupant = gameObject;
		dest.m_reserved = false;
	}
}
