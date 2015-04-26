using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Door : MonoBehaviour {
	private float score;
	public MPlayer Player;
	
	void Start()
	{
		Player = MPlayer.FindObjectOfType<MPlayer>(); 
	}


	public void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("Door colliding with " + other.name);
		if (other.tag == "player_2") {
			Debug.Log ("player at door");
			score = Player.score;
			PlayerPrefs.SetInt("score", (int)score);
			Application.LoadLevel("Finish");
		}
	}
}
