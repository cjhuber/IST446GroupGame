using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// When player is created, automatically move camera to player's position
		Camera.main.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
