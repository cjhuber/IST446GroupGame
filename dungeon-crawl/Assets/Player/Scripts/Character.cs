using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;
using UnitySampleAssets.Utility;

public class Character : MonoBehaviour {
    private float SPEED = 6f;
    private float BULLET_SPEED = 20.0f;
    private float FIRE_DELAY = 0.20f; // Time between each shot
    private float MUZZLE_FLASH_LIFE_TIME = 0.025f;
    private float RELOAD_TIME = 1f;
    public int MAX_BULLETS = 30;
    public int MAX_HEALTH = 100;
	public float smoothTime = 5f;
    private float lastShot = 0.0f;
    private bool isReloading = false;
    private CharacterController mainController;
    private Quaternion charTargRot;

    // Screen shake vars
    private Vector3 originalCameraPosition;
    private float shakeAmount = 0;

    public int currentBulletCount;
    public int health;
	
    public Camera mainCamera;
    public GameObject firePosition;
    public GameObject bullet;
    public GameObject muzzleFlash;
    public GameObject crosshair;

    // Use this for initialization
    void Start() {
    	mainController = GetComponent<CharacterController>();
        currentBulletCount = MAX_BULLETS;
        health = MAX_HEALTH;
    }
    

    // Update is called once per frame
    void Update() {
		float horizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");
		float vertical = CrossPlatformInputManager.GetAxisRaw("Vertical");
        Vector2 move = new Vector2(horizontal, -vertical);
		if ((horizontal != 0) && (vertical != 0))
		{
			Vector3 rotate = new Vector3(0,0,Mathf.Atan2(-horizontal,-vertical)*Mathf.Rad2Deg);
			this.GetComponent<Rigidbody2D>().transform.localRotation = Quaternion.Euler (rotate);
		}

		this.GetComponent<Rigidbody2D>().velocity = move*SPEED;

        // Handle reloading
        if (Input.GetKeyDown(KeyCode.R)) {  
            if (!isReloading) {
                StartCoroutine("Reload");
            }
        }

        if (shakeAmount > 0) {
            shakeAmount -= 0.2f;
        }
        // Handle firing bullets
        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) && lastShot >= FIRE_DELAY && currentBulletCount > 0 && !isReloading) {

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

            HandleEffects(firePosition.transform.position, clone.transform.rotation);
            currentBulletCount--;
            lastShot = 0;
        }

        // Auto reload once we reach 0 bullets
        if (currentBulletCount == 0 && !isReloading) {
            StartCoroutine("Reload");
        }

        lastShot += Time.deltaTime;
    }


    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log ("Player colliding:" + other.name);
        if (other.name.Contains("HealthPowerUp")) {
            this.health += 5;
            Destroy (other.gameObject);
        }

    }

    void HandleEffects(Vector2 flashPosition, Quaternion flashRotation) {
        GameObject muzzleClone = Instantiate(muzzleFlash, flashPosition, Quaternion.identity) as GameObject;
        muzzleClone.transform.rotation = flashRotation;
        Destroy(muzzleClone, MUZZLE_FLASH_LIFE_TIME);
    }

    IEnumerator Reload() {
        Debug.Log("Reloading");
        isReloading = true;
        yield return new WaitForSeconds(RELOAD_TIME);
        Debug.Log("Done reloading");
        currentBulletCount = MAX_BULLETS;
        isReloading = false;
    }

    public void TakeDamage() {
        health--;
        if (health <= 0) {
            //game.HandleGameOver();
        }
    }


}
