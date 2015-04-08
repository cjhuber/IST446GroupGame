using System;
using UnityEngine;
using System.Collections;
using SimpleJSON;

public class LevelParser : MonoBehaviour
{

	public Transform wall;
	public Transform floor;



    public TextAsset mapData;
	// Use this for initialization
	private string mapFromServer;
	void Start ()
	{
	    string temp = mapData.text;
//		RetrieveMapFromServer();
        ParseMap(temp);
	}

	private void RetrieveMapFromServer()
	{
		WWW www = new WWW("http://localhost:8000/output.txt");

		mapFromServer = www.text;
	}

	// Update is called once per frame
	void Update () {
	
	}

    void ParseMap(String map)
    {
        String parsedMap = map.Replace("],[", "$");
        parsedMap = parsedMap.Replace("[[", "");
        parsedMap = parsedMap.Replace("]]", "");
        char[] delimiter = {'$'};

        string[] seperatedParsedMap = parsedMap.Split(delimiter);
        Debug.Log("Map Height,  " + seperatedParsedMap.Length);
		Camera c = GetComponent<Camera>();
		float y = c.pixelHeight;
		Vector3 startingPoint = c.ScreenToWorldPoint(new Vector3(0, c.pixelHeight, 5));
		Vector3 tileUnit = c.ScreenToWorldPoint(new Vector3(16, 16, 5));
        foreach (string s in seperatedParsedMap)
        {
            string temp = (s.Replace("\",\"", "")).Trim(new char[] {' ', '"', ','});
			float x = 0;
            foreach(char z in temp)
			{
				switch (z)
				{
				case '1':
					Instantiate(wall, c.ScreenToWorldPoint(new Vector3(x, y, 5)), Quaternion.identity);
					break;
				case '0':
					Instantiate(floor, c.ScreenToWorldPoint(new Vector3(x, y, 5)), Quaternion.identity);
					break;
				default:
					break;
				}
				x += 16;
			}

			y -= 16;
            Debug.Log(temp);  
        }
    }
}
