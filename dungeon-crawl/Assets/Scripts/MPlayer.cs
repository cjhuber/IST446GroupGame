using UnityEngine;
using System.Collections;

public class MPlayer : MonoBehaviour {

	public GameObject spotLight;
	public GameObject light;
	public Rigidbody2D rigidBody;
	// Use this for initialization
	void Start () {
		this.rigidBody = this.GetComponent<Rigidbody2D>();
		// When player is created, automatically move camera to player's position
		Camera.main.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z);
		light = Instantiate(spotLight, new Vector3(this.transform.position.x, this.transform.position.y, -2), Quaternion.identity) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		// movement for debugging
		Vector3 moveV = Vector3.zero;
		if (Input.GetKey (KeyCode.W)) {
			moveV += new Vector3(0, 1, 0);
		}
		if (Input.GetKey (KeyCode.A)) {
			moveV += new Vector3(-1, 0, 0);
		}
		if (Input.GetKey (KeyCode.D)) {
			moveV += new Vector3(1, 0, 0);
		}
		if (Input.GetKey (KeyCode.S)) {
			moveV += new Vector3(0, -1, 0);
		}
		//this.transform.position += moveV;
		this.rigidBody.velocity = moveV * 5.5f;
		//this.transform.position += new Vector3(horizontal, vertical, 0).normalized * Time.deltaTime * 5;
		Camera.main.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10);
		this.light.transform.position = this.transform.position;
		
	}
}
