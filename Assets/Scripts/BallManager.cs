using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallManager : MonoBehaviour {

//	List<GameObject> shotEnemies;
	public GameObject[] ballPrefabs;
	public float shootStrength = 1000f;
	public GameObject hitParticle;

	float redBallTimer, greenBallTimer, yellowBallTimer, purpleBallTimer;
	bool redOnCd, greenOnCd, yellowOnCd, purpleOnCd = false;
	float ballShootCooldown = 0.5f;


//	PlayerManager playerManager;

	Dictionary<string, GameObject> ballPrefabDict;

	void Start () {
//		shotEnemies = new List<GameObject> ();
//		playerManager = GameObject.FindObjectOfType<PlayerManager>();

		char[] delimChar = new char[1] {'_'};

		ballPrefabDict = new Dictionary<string, GameObject> ();
		for(int i = 0; i < ballPrefabs.Length; i++) {
			ballPrefabDict.Add(ballPrefabs[i].name.Split(delimChar)[0], ballPrefabs[i]);
		}
	}
	
//	// Fuck it.
//	class ShootData1 {
//		public ShootData1(Transform p_dest, string p_color) {dest = p_dest; color = p_color;}
//		public Transform dest;
//		public string color;
//	}

	class ShootData2 {
		public ShootData2(Vector2 p_start, Vector3 p_dest, string p_color) {start = p_start; dest = p_dest; color = p_color;}
		public Vector2 start;
		public Vector3 dest;
		public string color;
	}

	void Update () {
		if( redOnCd ) {
			if( redBallTimer > ballShootCooldown ) {
				redOnCd = false;
				redBallTimer = 0f;
			}
			redBallTimer += Time.deltaTime;
		}
		if( yellowOnCd ) {
			if( yellowBallTimer > ballShootCooldown ) {
				yellowOnCd = false;
				yellowBallTimer = 0f;
			}
			yellowBallTimer += Time.deltaTime;
		}
		if( greenOnCd ) {
			if( greenBallTimer > ballShootCooldown ) {
				greenOnCd = false;
				greenBallTimer = 0f;
			}
			greenBallTimer += Time.deltaTime;
		}
		if( purpleOnCd ) {
			if( purpleBallTimer > ballShootCooldown ) {
				purpleOnCd = false;
				purpleBallTimer = 0f;
			}
			purpleBallTimer += Time.deltaTime;
		}
	}

	public void Shoot(Vector2 pos, string color) {
		switch( color ) 
		{
		case "Red":
			if( redOnCd )
				return;
			else
				redOnCd = true;
			break;

		case "Yellow":
			if( yellowOnCd )
				return;
			else
				yellowOnCd = true;
			break;

		case "Green":
			if( greenOnCd ) 
				return;
			else
				greenOnCd = true;
			break;
		case "Purple":
			if( purpleOnCd )
				return;
			else
				purpleOnCd = true;
			break;
		}

		if(!Camera.main)
			return;
		RaycastHit hit;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(pos), out hit)) {
			StartCoroutine("Shoot2", new ShootData2(pos, hit.point, color));
		}
	}
	

	void Shoot2(ShootData2 shootData) {
		GameObject ball = (GameObject)Instantiate (ballPrefabDict[shootData.color], Camera.main.ScreenToWorldPoint(shootData.start), Quaternion.identity);

		//ball.transform.position = ball.transform.position - Vector3.up ;
		Vector3 shootDir = shootData.dest - ball.transform.position;
		shootDir.Normalize();

		ball.rigidbody.AddForce(shootDir * shootStrength);// = Vector3.Lerp(startPos, target, timer) + Vector3.up * height; 
			
		ball.rigidbody.useGravity = true;
		Destroy (ball, 10f);
	}
}
