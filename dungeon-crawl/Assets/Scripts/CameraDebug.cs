﻿using UnityEngine;
using System.Collections;

public class CameraDebug : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		this.transform.position += new Vector3(horizontal, vertical, 0);

		if (Input.GetKey(KeyCode.Z)) {
			this.GetComponent<Camera>().fieldOfView += 5;
		} else if (Input.GetKey(KeyCode.X)) {
			this.GetComponent<Camera>().fieldOfView -= 5;
		}
	}
}
