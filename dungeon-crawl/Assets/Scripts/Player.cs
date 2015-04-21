using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

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
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		//this.transform.position += new Vector3(horizontal, vertical, 0);
		this.rigidBody.velocity = new Vector3(horizontal, vertical, 0) * 5f;
		Camera.main.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10);
		this.light.transform.position = this.transform.position;
    }
}
