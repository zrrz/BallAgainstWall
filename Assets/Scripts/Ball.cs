using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	PlayerManager playerManager;
	public bool hasCollided = false;
	public GameObject hitParticle;

	void Start() {
		playerManager = GameObject.FindObjectOfType<PlayerManager>();
	}

	void OnCollisionEnter(Collision col) {
		if(!hasCollided) {
			hasCollided = true;

			if(col.transform.tag == "Enemy") {
				col.transform.SendMessage("Hit");

				if(name.Contains("Red")) {
					playerManager.AddPoints("Red", 1);
				} 
				else if(name.Contains("Yellow")) {
					playerManager.AddPoints("Yellow", 1);
				} 
				else if(name.Contains("Green")) {
					playerManager.AddPoints("Green", 1);
				}

				Destroy(Instantiate(hitParticle, col.contacts[0].point, Quaternion.identity), 4f); //TODO Eric: change to a better method
			}

			rigidbody.velocity = Vector3.zero;
			rigidbody.AddForce((transform.position - col.contacts[0].point).normalized * 200f);
			rigidbody.useGravity = true;
			Destroy (gameObject, 10f);
		}
	}
}
