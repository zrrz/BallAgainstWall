using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallManager : MonoBehaviour {
	
	public GameObject[] ballPrefabs;
	public float shootStrength = 1000f;
	public GameObject hitParticle;

	float redBallTimer, greenBallTimer, yellowBallTimer, purpleBallTimer;
	bool redOnCd, greenOnCd, yellowOnCd, purpleOnCd = false;
	float ballShootCooldown = 0.5f;

	Dictionary<string, GameObject> ballPrefabDict;

	void Start () {
		char[] delimChar = new char[1] {'_'};

		ballPrefabDict = new Dictionary<string, GameObject> ();
		for(int i = 0; i < ballPrefabs.Length; i++) {
			ballPrefabDict.Add(ballPrefabs[i].name.Split(delimChar)[0], ballPrefabs[i]);
			StaticPool.InitObj(ballPrefabs[i]);
		}
	}

	class ShootData {
		public ShootData(Vector2 p_start, Vector3 p_dest, PlayerColor p_color) {
			start = p_start; dest = p_dest; color = p_color;
		}
		public ShootData() { }
		public Vector2 start;
		public Vector3 dest;
		public PlayerColor color;
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

	public void Shoot(Vector2 pos, PlayerColor color) {
		switch( color ) 
		{
		case PlayerColor.Red:
			if( redOnCd )
				return;
			else
				redOnCd = true;
			break;

		case PlayerColor.Yellow:
			if( yellowOnCd )
				return;
			else
				yellowOnCd = true;
			break;

		case PlayerColor.Green:
			if( greenOnCd ) 
				return;
			else
				greenOnCd = true;
			break;
		case PlayerColor.Purple:
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
//			StartCoroutine("Shoot", new ShootData(pos, hit.point, color));
			Shoot(new ShootData(pos, hit.point, color));
		}
	}
	

	void Shoot(ShootData shootData) {
		GameObject ball = StaticPool.GetObj(ballPrefabDict[shootData.color.ToString()]);
		ball.GetComponent<Ball>().Reset();

		ball.transform.position = Camera.main.ScreenToWorldPoint(shootData.start);
		ball.rigidbody.velocity = Vector3.zero;

		Vector3 shootDir = shootData.dest - ball.transform.position;
		shootDir.Normalize();

		ball.rigidbody.AddForce(shootDir * shootStrength);
			
		ball.rigidbody.useGravity = true;
//		Destroy (ball, 10f);
	}
}
