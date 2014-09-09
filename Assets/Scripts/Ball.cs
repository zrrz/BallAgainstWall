using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	PlayerManager playerManager;
	public bool hasCollided = false;

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
			}

			rigidbody.velocity = Vector3.zero;
			//rigidbody.AddForce (Random.onUnitSphere * 800f);
			rigidbody.AddForce((transform.position - col.contacts[0].point).normalized * 200f);
			rigidbody.useGravity = true;
			Destroy (gameObject, 10f);
		}
	}
}
