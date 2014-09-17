using UnityEngine;
using System.Collections;

public class QueueManager : MonoBehaviour {
	
	public Waypoint m_queueFront;

	[System.NonSerialized]
	public Waypoint m_queueEnd;

	SpawnFloor floor;

	// Use this for initialization
	void Awake () {
		floor = GameObject.Find("Floor").GetComponent<SpawnFloor>();
		m_queueEnd = m_queueFront;

	}
	
	public void SpawnNewEnemy(GameObject enemyObject) {
		// If the next spawn point in line is not aleady taken
		if( m_queueEnd.m_occupant == null && m_queueEnd.m_reserved == false ) {
			GameObject enemy = (GameObject)Instantiate( enemyObject, m_queueEnd.transform.position, Quaternion.identity );
			m_queueEnd.m_occupant = enemy;

			// If we are not at the front of the queue, look at next waypoint
			FaceNextWaypoint( enemy.transform, m_queueEnd );

			// If we aren't at the end of the queue, move the spawnpoint one position back
			if( m_queueEnd.m_previous != null )
				m_queueEnd = m_queueEnd.m_previous;
		}
	}

	/// <summary>
	/// Makes the enemy at the front of the queue start its Move coroutine.
	/// </summary>
	public void StartNextInQueue() {
		if( m_queueFront.m_occupant != null ) {
			int column = Random.Range (0, floor.columns);
			Enemy t_obj = m_queueFront.m_occupant.GetComponent<Enemy>();
			t_obj.curColumn = column;
			t_obj.StartMove();
			m_queueFront.m_occupant = null;
		}

		MoveQueueForward( m_queueFront );
	}

	/// <summary>
	/// Moves the queue forward.
	/// </summary>
	/// <param name="wp">Point on queue to move foward from.</param>
	void MoveQueueForward( Waypoint wp ) {
		Waypoint m_tempWp = wp;

		// Go forward from wp position to find the next open position
		if( wp.m_occupant != null ) {
			// Keep going forward until we reach the last unpopulated waypoint
			while( m_tempWp.m_next != null  ) {
				if(  m_tempWp.m_next.m_reserved == false && m_tempWp.m_next.m_occupant == null )
					m_tempWp = m_tempWp.m_next;
				else
					break;
			}

			FaceNextWaypoint( wp.m_occupant.transform, wp );
			wp.m_occupant.GetComponent<Enemy>().GoToQueuePos( m_tempWp );
			m_tempWp.m_reserved = true;
			wp.m_occupant = null;

//			if( wp.m_previous != null ) {
//				m_queueEnd = wp.m_previous;
//				return;
//			}
		}

		// If we aren't at the end of the line, continue moving the next enemy forward
		if( wp.m_previous != null ) 
			MoveQueueForward( wp.m_previous );
		else {
			if( wp == m_queueEnd.m_next ) 
				m_queueEnd = wp.m_previous;
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawSphere( m_queueEnd.transform.position, 0.3f );
	}

	/// <summary>
	/// Has enemy face the next point in queue.
	/// </summary>
	/// <param name="enemy">Enemy transform to be rotated.</param>
	/// <param name="currWp">The current enemy's waypoint.</param>
	void FaceNextWaypoint( Transform enemy, Waypoint currWp ) {
		// I'm using m_previous for the way they look because the enemy model's world forward is facing backwards

		// If we are not at the front of the line look at the point behind you, otherwise look away from the camera
		if( currWp.m_next != null ) {
			enemy.transform.LookAt( new Vector3( currWp.m_next.transform.position.x, enemy.transform.position.y, currWp.m_next.transform.position.z ) );
			enemy.Rotate( new Vector3( 0f, 180f, 0f ) );
		} else {
			enemy.transform.rotation = Quaternion.Euler( ( Camera.main.transform.position - enemy.transform.position ).normalized  * -1f);
		}
	}
}
