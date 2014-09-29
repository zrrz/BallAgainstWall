﻿using UnityEngine;
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

	bool hit;

	float lifeEndTime;

	[System.NonSerialized]
	public GameObject hitBy;

	class HopData {
		public HopData(Vector3 p_dest, float p_time) {dest = p_dest; time = p_time;}
		public Vector3 dest;
		public float time;
	}

	void Awake () {
		moveForwardChancePct = Mathf.Clamp(moveForwardChancePct, 0f, 100f);

		floor = GameObject.Find ("Floor").GetComponent<SpawnFloor> ();

		animator = transform.GetChild(0).GetComponent<Animator> ();

		Reset();
	}
	
	void SetKinematic(bool newValue) {
		Component[] components = GetComponentsInChildren(typeof(Rigidbody));

		foreach (Component c in components) {
			(c as Rigidbody).isKinematic = newValue;
		}
	}

	public void Reset() {
		SetKinematic(true);

		int rand = Random.Range(0, textures.Count - 1);
		foreach(Renderer rend in renderers) {
			rend.material.mainTexture = textures[rand];
		}

		curRow = 0;

		hit = false;

		lifeEndTime = -1f;

		animator.enabled = true;
	}

	void Update() {
		if(lifeEndTime > 0f) {
			if(Time.time > lifeEndTime) {
				gameObject.SetActive(false);
			}
		}

		Vector3 t_pos = transform.GetChild(0).localPosition;
		t_pos.x = t_pos.z = 0f;
		transform.GetChild(0).localPosition = t_pos;
		transform.rotation = Quaternion.identity;
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
			} else if(p_hitBy.name.Contains("Purple")) {
				PlayerManager.AddPoints(PlayerColor.Purple, 1);
			} else {
				print ("wtf");
			}

			hit = true;
			StopCoroutine ("Move");
			StopCoroutine ("Hop");
			collider.enabled = false;

			SetKinematic(false);
			animator.enabled = false;

			StopAllCoroutines();
			lifeEndTime = Time.time + 1f;
//			Destroy(gameObject, 1f);
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
//		Destroy (gameObject);
	}

	void LateUpdate() {
		Vector3 t_pos = transform.GetChild(0).localPosition;
		t_pos.x = 0f;
		transform.GetChild(0).localPosition = t_pos;
	}

	IEnumerator Hop(HopData data) {
//		animator.SetBool ("Land", false);
		animator.SetBool("Jump", true);
		animator.SetInteger("RandomJump", Random.Range(1, 10));
		//floor.tiles [curColumn, curRow].GetComponent<Animator> ().SetTrigger ("Bounce");
		ClosestTile(transform.position).GetComponent<Animator>().SetTrigger("Bounce");
		Vector3 startPos = transform.position;
		float timer = 0.0f;

		yield return null;

//		if(animator.GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base.Start") 
//		   || animator.GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base.Jump Hub")
//		   || animator.GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base.Walk")) {
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
//		}

		float temp_hopHeight = hopHeight * Random.Range(0.8f, 1.2f);

		while (timer <= 1.0f) {
			if(timer > 0.01f)
				animator.Play(animator.GetCurrentAnimatorStateInfo(0).nameHash, 0, timer);

			float height = Mathf.Sin(Mathf.PI * timer) * temp_hopHeight;
			transform.position = Vector3.Lerp(startPos, data.dest, timer) + Vector3.up * height; 
			
			timer += Time.deltaTime / data.time;
			yield return null;
//			animator.SetBool ("Jump", false);
//			if(timer > .90f)
//				animator.SetBool ("Land", true);
		}
//		animator.SetBool ("Land", true);
//		animator.SetBool ("Jump", false);
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
