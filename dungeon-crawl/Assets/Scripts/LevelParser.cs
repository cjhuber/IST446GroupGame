using System;
using UnityEngine;
using System.Collections;
using SimpleJSON;

public class LevelParser : MonoBehaviour
{

	public GameObject wall;
	public GameObject floor;



    public TextAsset mapData;
	// Use this for initialization
	private string mapFromServer;
	void Start ()
	{
	    string temp = mapData.text;
		StartCoroutine(RetrieveMapFromServer());
	}

	IEnumerator RetrieveMapFromServer()
	{
		// Use POST request for testing
		WWWForm form = new WWWForm();
		form.AddField ("player_1", "123");
		form.AddField ("player_2", "456");

		WWW www = new WWW("http://localhost:3000/rooms", form);
		yield return www;
		if (www.error == null) {
			Debug.Log ("Request ok");
			Debug.Log (www.text);
			mapFromServer = www.text;
			ParseMap(mapFromServer);
		} else {
			Debug.Log ("Request error" + www.error);
		}
		//mapFromServer = www.text;
		//Debug.Log(mapFromServer);
	}

	// Update is called once per frame
	void Update () {
	
	}

    void ParseMap(String map)
	{
		var tileWidth = wall.GetComponent<SpriteRenderer>().bounds.size.x;
		var tileHeight = wall.GetComponent<SpriteRenderer>().bounds.size.y;
		Debug.Log ("Parsing map");
        var room = JSONNode.Parse (mapFromServer);
		Debug.Log ("Done parsing map");
		Debug.Log (room["map"]["properties"].ToString());
		int mapWidth = room["map"]["properties"]["width"].AsInt;
		int mapHeight = room["map"]["properties"]["width"].AsInt;
		Debug.Log ("Map Width:" + mapWidth);
		Debug.Log ("Map Height:" + mapHeight);
		Debug.Log ("Tile size: " + tileWidth + "x" + tileHeight);

		for (int x = 0; x < mapWidth; x++) {
			for (int y = 0; y < mapHeight; y++) {
				int type = room["map"]["terrain"][x][y].AsInt;
				if (type == 0) {
					Instantiate (floor, (new Vector3(x, y, 0)), Quaternion.identity);
				} else if (type == 1) {
					
					Instantiate (wall, (new Vector3(x, y, 0)), Quaternion.identity);
				}
			}
		}
    }
}
