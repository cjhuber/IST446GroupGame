using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Level : MonoBehaviour {

	public GameObject wall;
	public GameObject ground;
	public GameObject player;
	public GameObject enemy;
	public GameObject door;

	private string rawMapData;
	private Vector3 playerSpawn;
	private Vector3 exitPosition;

	// Use this for initialization
	void Start () {
		StartCoroutine(GetMap());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator GetMap() {
		// Use POST request for testing
		WWWForm form = new WWWForm();
		// Generate random players for testing
		form.AddField ("player_1", Mathf.Round(Random.value * 1000).ToString());
		form.AddField ("player_2", Mathf.Round(Random.value * 1000).ToString());

		WWW www = new WWW("http://107.170.10.115:3000/rooms", form);
		// Wait for request to finish
		yield return www;
		if (www.error == null) {
			Debug.Log ("Request ok");
			Debug.Log (www.text);
			rawMapData = www.text;
			ParseMapData();
		} else {
			Debug.Log ("Request error" + www.error);
		}
	}

	private void ParseMapData() {
		
		Debug.Log ("Parsing map");

		var tileWidth = wall.GetComponent<SpriteRenderer>().bounds.size.x;
		var tileHeight = wall.GetComponent<SpriteRenderer>().bounds.size.y;
		var room = JSONNode.Parse (rawMapData);
		int mapWidth = room["map"]["properties"]["width"].AsInt;
		int mapHeight = room["map"]["properties"]["width"].AsInt;
		Debug.Log ("Map Width:" + mapWidth);
		Debug.Log ("Map Height:" + mapHeight);
		Debug.Log ("Tile size: " + tileWidth + "x" + tileHeight);

		// Parse terrain/tiles
		for (int x = 0; x < mapWidth; x++) {
			for (int y = 0; y < mapHeight; y++) {
				int type = room["map"]["terrain"][x][y].AsInt;
				if (type == 0) {
					Instantiate (ground, new Vector3(y, mapWidth-x, 0), Quaternion.identity);
				} else if (type == 1) {
					Instantiate (wall, new Vector3(y, mapWidth-x, 0), Quaternion.identity);
				}
			}
		}

		foreach (JSONNode enemyobj in room["map"]["enemies"].AsArray) {
			Debug.Log ("Render emeny at: " + enemyobj["position"]["y"].AsFloat + "," + enemyobj["position"]["x"].AsFloat);
			Instantiate (enemy, new Vector3(enemyobj["position"]["y"].AsFloat, mapWidth - enemyobj["position"]["x"].AsFloat, -1), Quaternion.identity);
		}

		playerSpawn = new Vector3(room["map"]["playerSpawn"]["y"].AsFloat, mapWidth - room["map"]["playerSpawn"]["x"].AsFloat, -2);
		Debug.Log (playerSpawn);
		Instantiate(player, playerSpawn, Quaternion.identity);

		exitPosition = new Vector3(room["map"]["exitPosition"]["y"].AsFloat, mapWidth - room["map"]["exitPosition"]["x"].AsFloat, -1);
		Instantiate(door, exitPosition, Quaternion.identity);
	}
}
