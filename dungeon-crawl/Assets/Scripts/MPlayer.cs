using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;
using UnitySampleAssets.Utility;

public class MPlayer : MonoBehaviour {

	public GameObject spotLight;
	public GameObject light;
	public Rigidbody2D rigidBody;
	public float SPEED = 6f;
	
	private CharacterController mainController;

	// Use this for initialization
	void Start () {
		mainController = GetComponent<CharacterController>();
		this.rigidBody = this.GetComponent<Rigidbody2D>();
		// When player is created, automatically move camera to player's position
		Camera.main.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z);
		light = Instantiate(spotLight, new Vector3(this.transform.position.x, this.transform.position.y, -2), Quaternion.identity) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");
		float vertical = CrossPlatformInputManager.GetAxisRaw("Vertical");
		Vector2 move = new Vector2(horizontal, vertical);
		if ((horizontal != 0) && (vertical != 0))
		{
			Vector3 rotate = new Vector3(0,0,Mathf.Atan2(-horizontal,vertical)*Mathf.Rad2Deg);
			this.GetComponent<Rigidbody2D>().transform.localRotation = Quaternion.Euler (rotate);
		}

		
		this.GetComponent<Rigidbody2D>().velocity = move*SPEED;
		//this.transform.position += new Vector3(horizontal, vertical, 0).normalized * Time.deltaTime * 5;
		Camera.main.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10);
		this.light.transform.position = this.transform.position;
		
	}
}
