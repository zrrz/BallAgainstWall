using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public bool hasCollided = false;
	public GameObject hitParticle;

	float impactEndTime = 0;
	Rigidbody impactTarget = null;
	Vector3 impact;

	AudioSource audioSource;
	
	float lifeEndTime = 0;

	public PlayerColor color;

	void Start() {
		audioSource = GetComponent<AudioSource>();
		rigidbody.AddTorque(Random.onUnitSphere * 10);
		Reset();
	}

	void Update() {
		if (Time.time < impactEndTime)
		{
			if(impactTarget.rigidbody != null)
				impactTarget.AddForce(impact, ForceMode.VelocityChange);
		}
		if(Time.time > lifeEndTime) {
			transform.position = Vector3.one * 1000f;
			gameObject.SetActive(false);
		}
	}

	public void Reset() {
		lifeEndTime = Time.time + 10f;
		hasCollided = false;
		transform.rotation = Random.rotation;
	}

	void OnCollisionEnter(Collision col) {
		if(!hasCollided) {
			hasCollided = true;

			if(col.transform.tag == "Enemy") {
				PlayerManager.IncreaseHits(color);

				col.transform.SendMessageUpwards("Hit", gameObject, SendMessageOptions.DontRequireReceiver);

				//set the impact target to whatever the ray hit
				impactTarget = col.rigidbody;
				
				//impact direction also according to the ray
				impact = rigidbody.velocity * 0.5f;
				
				//the impact will be reapplied for the next 100ms
				//to make the connected objects follow even though the simulated body joints
				//might stretch
				impactEndTime = Time.time+0.10f;

				audioSource.Play();
				Destroy(Instantiate(hitParticle, col.contacts[0].point, Quaternion.identity), 4f); //TODO Eric: change to a better method

				if(color == PlayerColor.Red ) {
					FloatingTextManager.instance.CreateFloatingText( col.transform.position, 1, Color.red );
				}
				else if(color == PlayerColor.Green) {
					FloatingTextManager.instance.CreateFloatingText( col.transform.position, 1, Color.green );
				}
				else if(color == PlayerColor.Yellow) {
					FloatingTextManager.instance.CreateFloatingText( col.transform.position, 1, Color.yellow );
				}
				else if(color == PlayerColor.Blue) {
					FloatingTextManager.instance.CreateFloatingText( col.transform.position, 1, Color.blue );
				}
			}

			rigidbody.velocity = Vector3.zero;
			rigidbody.AddForce((transform.position - col.contacts[0].point).normalized * 200f);
			rigidbody.useGravity = true;
		}
	}

	public void PlayAudio() {
		audioSource.Play();
	}
}
