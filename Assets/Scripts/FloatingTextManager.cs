using UnityEngine;
using System.Collections;

public class FloatingTextManager : MonoBehaviour {

	public static FloatingTextManager instance;

	public TextMesh m_textMesh;

	private float m_fadeTime = 1.0f;
	private float m_yGain = 0.5f;	// meters per second

	void Awake () {
		instance = this;
	}

	// Use this for initialization
	void Start () {
		if( m_textMesh == null ) {
			Debug.LogError( "FloatingTextManager missing TextMesh object prefab" );
		}
	}

	public void CreateFloatingText( Vector3 pos, int points, Color color ) {
		TextMesh t_obj = (TextMesh)GameObject.Instantiate( m_textMesh, pos, Quaternion.identity );
		if(Camera.main)
			t_obj.transform.LookAt( Camera.main.transform );
		t_obj.transform.Rotate( new Vector3( 0f, 180f, 0f ) );
		t_obj.text = "+" + points.ToString();

//		Color clr = new Color();
//
//		switch( color )
//		{
//		case "Red":
//			clr = Color.red;
//			break;
//		case "Green":
//			clr = Color.green;
//			break;
//		case "Yellow":
//			clr = Color.yellow;
//			break;
//		case "Purple":
//			clr = Color.magenta;
//			break;
//		}

		t_obj.color = color;

		StartCoroutine( "FloatingText", t_obj );
	}

	IEnumerator FloatingText( TextMesh tMesh ) {
		Vector3 startingPos = tMesh.transform.position;
		Vector3 yOffset = Vector3.zero;
		float timer = 0f;

		while( timer < m_fadeTime ) {
			yOffset.y += m_yGain * Time.deltaTime;
			
			tMesh.transform.position = startingPos + yOffset;
			
			timer += Time.deltaTime;
			yield return null;
		}

		Destroy( tMesh.gameObject );
	}
}
