﻿using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;
using UnitySampleAssets.Utility;
using UnityEngine.UI;

public class MPlayer : MonoBehaviour {

	public GameObject spotLight;
	public GameObject light;
	public GameObject bullet;
	public GameObject firePosition;
	public GameObject healthText;

	public Rigidbody2D rigidBody;
	public float SPEED = 6f;
	public float TOTAL_HEALTH = 5f;
	public float INITIAL_SCORE = 0f;
	public float health;
	public float score;
	private float BULLET_SPEED = 20.0f;
	private float lastShot = 0.0f;
	
	private CharacterController mainController;

	// Use this for initialization
	void Start () {
		healthText = GameObject.FindWithTag("health_text");
		mainController = GetComponent<CharacterController>();
		this.rigidBody = this.GetComponent<Rigidbody2D>();
		// When player is created, automatically move camera to player's position
		Camera.main.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z);
		light = Instantiate(spotLight, new Vector3(this.transform.position.x, this.transform.position.y, -2), Quaternion.identity) as GameObject;
		health = TOTAL_HEALTH;
		score = INITIAL_SCORE;
		healthText.GetComponent<Text>().text = health.ToString();
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
		
		if (CrossPlatformInputManager.GetButtonDown("Jump")){

			// Instantiate a new bullet and give it the same rotation as the player and starting position
			// as the empty child firePosition game object
			GameObject clone = Instantiate(bullet, firePosition.transform.position, Quaternion.identity) as GameObject;
			clone.transform.rotation = transform.rotation;
			
			// Calculate velocity vector by using the current rotation of the player
			Quaternion rotDir = Quaternion.AngleAxis(clone.transform.rotation.eulerAngles.z, Vector3.right);
			Vector3 ldir = rotDir * Vector3.forward;
			
			clone.GetComponent<Rigidbody2D>().velocity = new Vector2(
				ldir.normalized.y * BULLET_SPEED,
				ldir.normalized.z * BULLET_SPEED);
			
			lastShot = 0;
		}
		
		lastShot+=Time.deltaTime;
	}

	public void TakeDamage(){
		health--;
		
		healthText.GetComponent<Text>().text = health.ToString();

		Debug.Log (health);
		if (health == 0) {
			var mpController = GameObject.Find ("MPController");
			var mp = mpController.GetComponent<MultiplayerController>();
			mp.takeTurn();
			Destroy(this.gameObject);
			PlayerPrefs.SetInt("score", (int)score);
			Application.LoadLevel("Death");
		}
	}
}
