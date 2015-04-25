using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathMenuLoad : MonoBehaviour {

	public Text scoreText;

	// Use this for initialization
	void Start () {
		scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
		scoreText.text = "Score: " + PlayerPrefs.GetString ("score");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
