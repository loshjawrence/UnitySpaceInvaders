using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour {

    public int pointValue;
    public AudioClip deathKnell;
    public GameObject deathExplosion;
    public float rotation;
    public GameObject bullet;//the GameObject to spawn
    public GameObject bulletPowerUp;//the GameObject to spawn
    public Vector2 widthThreshold;
    public Vector2 heightThreshold;
    public bool active;
    public bool hasPowerUp;

	// Use this for initialization
	void Start () {
        active = true;
        pointValue = 10;
        rotation = 180.0f;
        widthThreshold = new Vector2(0, Camera.main.pixelWidth);
        heightThreshold = new Vector2(0, Camera.main.pixelHeight);
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.x < widthThreshold.x ||
            screenPosition.x > widthThreshold.y ||
            screenPosition.y < heightThreshold.x ||
            screenPosition.y > heightThreshold.y)
        {
            Destroy(gameObject);
        }
	}

    IEnumerator Example()
    {
        yield return new WaitForSeconds(1); //this will wait 5 seconds 
        Destroy(gameObject);
    }
    public void Die() {
        //Destroy removes teh gameObject from the scene and marks
        //it for garbage collection
        //particle effects by default use the convention of Z being upwars and XY being
        //the horizontal plane, since we are looking down the Y axis, we rotate teh particle system so that it 
        //flys in the right way

        //play sound at location
        GameObject obj = GameObject.Find("GlobalObject");
        Global g = obj.GetComponent<Global>();
        if (active) {
            //g.score += pointValue;
            GameObject player = GameObject.Find("Cannon");
            Cannon c = player.GetComponent<Cannon>();
            int bonusPoints = c.powerUpTime > 0.0 ? c.bonusPoints : 0;
            g.AdjustScore(pointValue + bonusPoints);
            g.UpdateTotalKilledAliens(0);
        }
        active = false;
        AudioSource.PlayClipAtPoint(deathKnell, gameObject.transform.position);

        Instantiate(deathExplosion, gameObject.transform.position,
            Quaternion.AngleAxis(-90, Vector3.right));

        gameObject.GetComponent<Rigidbody>().useGravity = true;
        if (hasPowerUp) {
            Vector3 spawnPos = gameObject.transform.position;
            spawnPos.x += 1.5f * Mathf.Sin(rotation * Mathf.PI / 180.0f);
            spawnPos.z += 1.5f * Mathf.Cos(rotation * Mathf.PI / 180.0f);
            GameObject objPowerUp = Instantiate(bulletPowerUp, spawnPos, Quaternion.identity) as GameObject;
            Destroy(gameObject);
        }
        //Example();
    }
    private void OnCollisionEnter(Collision collision)
    {
        //the collision contains a lot of info, but it's the colliding
        //object we're most interested in.

        Collider collider = collision.collider;//the thing we collided with
        if (collider.CompareTag("Alien")&&active) {
            //get handle to alien script and tell it to die
            Alien alien = collider.gameObject.GetComponent<Alien>();
            alien.Die();
            //active = false;
            //GetComponent<Rigidbody>().useGravity = true;
            Die();

            //Destroy(gameObject);
        }
    }

    public void Fire() {
        Vector3 spawnPos = gameObject.transform.position;
        spawnPos.x += 1.5f * Mathf.Sin(rotation * Mathf.PI / 180.0f);
        spawnPos.z += 1.5f * Mathf.Cos(rotation * Mathf.PI / 180.0f);

        // instantiate the Bullet
        GameObject obj = Instantiate(bullet, spawnPos,
            Quaternion.identity) as GameObject;

        // get the Bullet Script Component of the new Bullet instance
        Bullet b = obj.GetComponent<Bullet>();
        b.fromPlayer = false;

        // set the direction the Bullet will travel in
        Quaternion rot = Quaternion.Euler(new Vector3(0, rotation, 0));
        b.heading = rot;
    }
}
