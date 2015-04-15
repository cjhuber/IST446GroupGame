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
		Camera c = GetComponent<Camera>();
		float y2 = c.pixelHeight;
		Vector3 startingPoint = c.ScreenToWorldPoint(new Vector3(0, c.pixelHeight, 5));
		Vector3 tileUnit = c.ScreenToWorldPoint(new Vector3(16, 16, 5));

		for (int x = 0; x < mapWidth; x++) {
			for (int y = 0; y < mapHeight; y++) {
				int type = room["map"]["terrain"][x][y].AsInt;
				if (type == 0) {
					Instantiate (floor, c.ScreenToWorldPoint (new Vector3(x * tileWidth, y * tileHeight, 0)), Quaternion.identity);
				} else if (type == 1) {
					
					Instantiate (wall, c.ScreenToWorldPoint (new Vector3(x * tileWidth, y * tileHeight, 0)), Quaternion.identity);
				}
			}
		}
//        String parsedMap = map.Replace("],[", "$");
//        parsedMap = parsedMap.Replace("[[", "");
//        parsedMap = parsedMap.Replace("]]", "");
//        char[] delimiter = {'$'};
//
//        string[] seperatedParsedMap = parsedMap.Split(delimiter);
//        Debug.Log("Map Height,  " + seperatedParsedMap.Length);
//		Camera c = GetComponent<Camera>();
//		float y = c.pixelHeight;
//		Vector3 startingPoint = c.ScreenToWorldPoint(new Vector3(0, c.pixelHeight, 5));
//		Vector3 tileUnit = c.ScreenToWorldPoint(new Vector3(16, 16, 5));
//        foreach (string s in seperatedParsedMap)
//        {
//            string temp = (s.Replace("\",\"", "")).Trim(new char[] {' ', '"', ','});
//			float x = 0;
//            foreach(char z in temp)
//			{
//				switch (z)
//				{
//				case '1':
//					Instantiate(wall, c.ScreenToWorldPoint(new Vector3(x, y, 5)), Quaternion.identity);
//					break;
//				case '0':
//					Instantiate(floor, c.ScreenToWorldPoint(new Vector3(x, y, 5)), Quaternion.identity);
//					break;
//				default:
//					break;
//				}
//				x += 16;
//			}
//
//			y -= 16;
//            Debug.Log(temp);  
//        }
    }
}
