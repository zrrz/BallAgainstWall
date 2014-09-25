using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

//	PlayerManager playerManager;
	public bool hasCollided = false;
	public GameObject hitParticle;

	float impactEndTime = 0;
	Rigidbody impactTarget = null;
	Vector3 impact;

	AudioSource audioSource;

//	class BallHitData {
//		GameObject gameObject;
//
//	}

	void Start() {
//		playerManager = GameObject.FindObjectOfType<PlayerManager>();
		audioSource = GetComponent<AudioSource>();
	}

	void Update() {
		if (Time.time<impactEndTime)
		{
			if(rigidbody)
				impactTarget.AddForce(impact,ForceMode.VelocityChange);
		}
	}

	void OnCollisionEnter(Collision col) {
		if(!hasCollided) {
			hasCollided = true;

			if(col.transform.tag == "Enemy") {
				col.transform.SendMessageUpwards("Hit", gameObject, SendMessageOptions.DontRequireReceiver);

				//set the impact target to whatever the ray hit
				impactTarget = col.rigidbody;
				
				//impact direction also according to the ray
				impact = rigidbody.velocity * 0.5f;
				
				//the impact will be reapplied for the next 250ms
				//to make the connected objects follow even though the simulated body joints
				//might stretch
				impactEndTime=Time.time+0.10f;

				audioSource.Play();
				Destroy(Instantiate(hitParticle, col.contacts[0].point, Quaternion.identity), 4f); //TODO Eric: change to a better method
			}

			rigidbody.velocity = Vector3.zero;
			rigidbody.AddForce((transform.position - col.contacts[0].point).normalized * 200f);
			rigidbody.useGravity = true;
			Destroy (gameObject, 10f);
		}
	}

	public void PlayAudio() {
		audioSource.Play();
	}
}
