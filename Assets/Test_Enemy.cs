using UnityEngine;
using System.Collections;

public class Test_Enemy : MonoBehaviour {

	public float jumpRate = 2f;
	public float hopHeight = 2.25f;

	Animator animator;

	class HopData {
		public HopData(Vector3 p_dest, float p_time) {dest = p_dest; time = p_time;}
		public Vector3 dest;
		public float time;
	}
	
	void Start () {
		animator = transform.GetChild(0).GetComponent<Animator> ();
		StartCoroutine("Move");
	}

	void Update () {
	
	}

	IEnumerator Move() {
		Vector3 pos = transform.position;
		while(true) {
//			yield return StartCoroutine ("Hop", new HopData (pos, Random.Range(1.5f, 1.7f)));
			yield return StartCoroutine ("Hop", new HopData (pos, Random.Range(0.5f, 0.7f)));
		}

	}

	IEnumerator Hop(HopData data) {
		animator.transform.localPosition = Vector3.zero;
		animator.SetBool("Jump", true);
		animator.SetInteger("RandomJump", Random.Range(1, 10));

		Vector3 startPos = transform.position;
		float timer = 0.0f;
		
//		yield return null;
		
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
		
		float temp_hopHeight = hopHeight * Random.Range(0.8f, 1.2f);
		
		while (timer <= 1.0f) {
			if(timer > 0.01f)
				animator.Play(animator.GetCurrentAnimatorStateInfo(0).nameHash, 0, timer);

			Debug.DrawRay(animator.transform.position, Vector3.forward, Color.red, 60f);

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
}
