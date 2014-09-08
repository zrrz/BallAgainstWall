using UnityEngine;
using System.Collections;

public class SpawnFloor : MonoBehaviour {

	public GameObject trampolines;
	public float width = 3f, height = 3f;

	public int rows = 6, columns = 6;

	//[System.NonSerialized]
	public GameObject[,] tiles;

	void Awake () {
		tiles = new GameObject[columns, rows];
		for(int i = 0; i < columns; i++) {
			for(int j = 0; j < rows; j++) {
				tiles[i,j] = trampolines.transform.FindChild(i + " " + j).gameObject;

			//	float xPos = -(width * ((float)columns)/2f) + width * i + width/2f;
			//	float yPos = -(height * ((float)rows)/2f) + height * j + height/2f;
			//	tiles[i,j] = (GameObject)Instantiate(tilePrefab, new Vector3(xPos, 0f, yPos), Quaternion.identity);
			//	tiles[i,j].transform.localScale = new Vector3(width, 0.25f, height);
			//	tiles[i,j].transform.parent = transform;
			}
		}
	}
}