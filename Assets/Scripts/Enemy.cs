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

	public List<Texture> maleTextures;
	public List<Texture> femaleTextures;

	public float moveForwardChancePct = 50f;

	bool hit;

	float lifeEndTime;

	[System.NonSerialized]
	public GameObject hitBy;

	class HopData {
		public HopData(Vector3 p_dest, float p_time) {dest = p_dest; time = p_time;}
		public Vector3 dest;
		public float time;
	}

	//void Awake () {
	void OnEnable() {
		moveForwardChancePct = Mathf.Clamp(moveForwardChancePct, 0f, 100f);

		floor = GameObject.Find ("Floor").GetComponent<SpawnFloor> ();

		Reset();
	}
	
	void SetKinematic(bool newValue) {
		Component[] components = animator.GetComponentsInChildren(typeof(Rigidbody));

		foreach (Component c in components) {
			(c as Rigidbody).isKinematic = newValue;
		}

		floor = GameObject.Find ("Floor").GetComponent<SpawnFloor> ();
	}

	void OnLevelWasLoaded(int level) {
		if(level == 0) {
			StopAllCoroutines();
			gameObject.SetActive(false);
		}
		if(level == 1)
			floor = GameObject.Find ("Floor").GetComponent<SpawnFloor> ();
	}

	public void Reset() {
		for(int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(false);
		}

		int randChar = Random.Range(0, 3);
		transform.GetChild(randChar).gameObject.SetActive(true);
		animator = transform.GetChild(randChar).GetComponent<Animator> ();
		if(randChar == 0) {
			int rand = Random.Range(0, maleTextures.Count - 1);
			animator.GetComponentInChildren<SkinnedMeshRenderer>().material.mainTexture = maleTextures[rand];
		} else if(randChar == 1) {
			int rand = Random.Range(0, femaleTextures.Count - 1);
			animator.GetComponentInChildren<SkinnedMeshRenderer>().material.mainTexture = femaleTextures[rand];
		} 
//		else {
//			print("No");
//		}

		SetKinematic(true);

		curRow = 0;

		hit = false;

		lifeEndTime = -1f;

		animator.enabled = true;

//		Component[] components = GetComponentsInChildren(typeof(Rigidbody));
		
//		foreach (Component c in components) {
//			(c as Rigidbody).velocity = Vector3.zero;
//		}
	}

	void Update() {
		if(lifeEndTime > 0f) {
			if(Time.time > lifeEndTime) {
				gameObject.SetActive(false);
			}
		}
	}

//	void OnCollisionEnter(Collision col) {
//		if(GameManager.instance.enemiesKnockback) {
//			if(col.gameObject.tag == "Enemy") {
//				hitBy = col.transform.GetComponentInParent<Enemy>().hitBy;
//				Hit (hitBy);
//			//	col.gameObject.SendMessageUpwards("Hit", hitBy, SendMessageOptions.DontRequireReceiver);
//			}
//		}
//	}

	void Hit(GameObject p_hitBy) {
		if(!hit) {
			hitBy = p_hitBy;

			if(p_hitBy.name.Contains("Red")) {
				PlayerManager.AddPoints(PlayerColor.Red, 1);
			} 
			else if(p_hitBy.name.Contains("Yellow")) {
				PlayerManager.AddPoints(PlayerColor.Yellow, 1);
			} 
			else if(p_hitBy.name.Contains("Green")) {
				PlayerManager.AddPoints(PlayerColor.Green, 1);
			} else if(p_hitBy.name.Contains("Blue")) {
				PlayerManager.AddPoints(PlayerColor.Blue, 1);
			} else {
				print ("wtf");
			}

			hit = true;
			StopCoroutine ("Move");
			StopCoroutine ("Hop");
//			collider.enabled = false;

			SetKinematic(false);
			animator.enabled = false;

			StopAllCoroutines();
			lifeEndTime = Time.time + 1f;
		}
	}

	IEnumerator Move() {
		float timer = 0f;
		while(curRow <= floor.rows - 1) {
			float t_time = Time.time;

			yield return StartCoroutine ("Hop", new HopData (floor.tiles[curColumn, curRow].transform.position, Random.Range(1.5f, 1.7f)));
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

		gameObject.SetActive(false);
		StopAllCoroutines();
	}

	IEnumerator Hop(HopData data) {
		animator.transform.localPosition = Vector3.zero;
		animator.SetBool("Jump", true);
		if(animator == transform.GetChild(2).GetComponent<Animator>())
			animator.SetInteger("RandomJump", 1);
		else
			animator.SetInteger("RandomJump", Random.Range(1, 10));

		ClosestTile(transform.position).GetComponent<Animator>().SetTrigger("Bounce");
		Vector3 startPos = transform.position;
		float timer = 0.0f;

		switch(animator.GetInteger("RandomJump")) {
			case 1:
				animator.Play(Animator.StringToHash("jump " + 1), 0, 0f);
				break;
			case 2:
				animator.Play(Animator.StringToHash("jump " + 2), 0, 0f);
				break;
			case 3:
				animator.Play(Animator.StringToHash("jump " + 3), 0, 0f);
				break;
			case 4:
				animator.Play(Animator.StringToHash("jump " + 4), 0, 0f);
				break;
			case 5:
				animator.Play(Animator.StringToHash("jump " + 5), 0, 0f);
				break;
			case 6:
				animator.Play(Animator.StringToHash("jump " + 6), 0, 0f);
				break;
			case 7:
				animator.Play(Animator.StringToHash("jump " + 7), 0, 0f);
				break;
			case 8:
				animator.Play(Animator.StringToHash("jump " + 8), 0, 0f);
				break;
			case 9:
				animator.Play(Animator.StringToHash("jump " + 9), 0, 0f);
				break;
			default:
				print ("wtf");
				break;
		}

		float temp_hopHeight = hopHeight * Random.Range(0.8f, 1.2f);

		while (timer <= 1.0f) {
			if(timer > 0.01f)
				animator.Play(animator.GetCurrentAnimatorStateInfo(0).nameHash, 0, timer);

			float height = Mathf.Sin(Mathf.PI * timer) * temp_hopHeight;
			transform.position = Vector3.Lerp(startPos, data.dest, timer) + Vector3.up * height; 
			
			timer += Time.deltaTime / data.time;
			yield return null;
		}
	}

	Transform ClosestTile(Vector3 pos) {
		Transform closestTile = floor.tiles[0, 0].transform;
		for(int i = 0; i < floor.columns; i++) {
			for(int j = 0; j < floor.rows; j++) {
				if(Vector3.Distance(floor.tiles[i, j].transform.position, pos) < Vector3.Distance(closestTile.position, pos)) {
					closestTile = floor.tiles[i, j].transform;
				}
			}
		}
		return closestTile;
	}

	public void StartMove() {
		StartCoroutine( "Move" );
	}

	public void GoToQueuePos( Waypoint wp ) {
		StartCoroutine( "MoveToPos", wp );
	}

	IEnumerator MoveToPos( Waypoint dest ) {
		animator.SetBool("Walk", true);
		animator.Play("Walk", 0, 0f);
		Vector3 startPos = transform.position;
		float moveTime = 0.5f;
		float timer = 0.0f;

		while( timer <= 1.0f ) {
			transform.position = Vector3.Lerp( startPos, dest.transform.position, timer );
			timer += Time.deltaTime / moveTime;
			yield return null;
		}

		dest.m_occupant = gameObject;
		dest.m_reserved = false;
		animator.SetBool("Walk", false);
		animator.CrossFade("Start", 0.2f, 0);
	}
}
