using UnityEngine;
using System.Collections;


public class Bullet : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start() {
		player = GameObject.FindWithTag("player_2");
	}
	
	// Update is called once per frame
	void Update() {

	}


	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log (other.name);

		if (other.name != "Background" && other.name != "Enemy(Clone)") {
			if (!other.name.Contains("boundary")) {
				//Debug.Log (this.tag + " colliding with " + other.tag);
			}
			Destroy(this.gameObject);

			if(other.name =="Player(Clone)"){
				other.GetComponent<MPlayer>().TakeDamage();
			}

		}
		/*

		if (other.name == "Player") {
			var player = other.GetComponent<Player>();
			player.TakeDamage();
		}
		
		if (this.tag == "player_bullet" && other.tag == "enemy") {
			Scene game = FindObjectOfType<Scene>();
			Vector3 enemyDeathPos = other.transform.position;
			if (Random.Range (0,100) < 10) {
				game.SpawnHealth(enemyDeathPos);
			}
			game.KillEnemy(enemyDeathPos);
			Destroy(other.gameObject);
			
		}
		*/
	}
}
