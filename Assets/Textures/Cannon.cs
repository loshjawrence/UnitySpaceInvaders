using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    public float turnSpeed;
    public float thrustSpeed;
    public GameObject bullet;//the GameObject to spawn
    public float rotation;
    public float bulletCoolDown;
    public float timeSinceLastBullet;
    public GameObject deathExplosion;

	// Use this for initialization
	void Start () {
        //turnSpeed = 0.5f;
        thrustSpeed = 0.10f;
        rotation = 0.0f;
        bulletCoolDown = 1.0f;
        timeSinceLastBullet = 0.0f;
	}

    private void Update() {
        timeSinceLastBullet += Time.deltaTime;
        if(Input.GetButtonDown("Fire1") && 
            timeSinceLastBullet >= bulletCoolDown) {
            timeSinceLastBullet = 0.0f;
                //Debug.Log("Fire! " + rotation);
                //spawn at the tip of the ship
                Vector3 spawnPos = gameObject.transform.position;
                spawnPos.x += 1.5f * Mathf.Sin(rotation * Mathf.PI / 180.0f);
                spawnPos.z += 1.5f * Mathf.Cos(rotation * Mathf.PI / 180.0f);

                // instantiate the Bullet
                GameObject obj = Instantiate(bullet, spawnPos,
                    Quaternion.identity) as GameObject;

                // get the Bullet Script Component of the new Bullet instance
                Bullet b = obj.GetComponent<Bullet>();

                // set the direction the Bullet will travel in
                Quaternion rot = Quaternion.Euler(new Vector3(0, rotation, 0));
                b.heading = rot;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {

        //if(Input.GetAxisRaw("Vertical") > 0) {
        //    gameObject.transform.Translate(0, 0, thrustSpeed);
        //}
        
        if(Input.GetAxisRaw("Horizontal") > 0) {
            gameObject.transform.Translate(thrustSpeed, 0, 0);
        } else if(Input.GetAxisRaw("Horizontal") < 0) {
            gameObject.transform.Translate(-thrustSpeed, 0, 0);
        }
        //Vector3 updatedPosition = gameObject.transform.position;
        //updatedPosition.x += 0.001f;
        //gameObject.transform.position = updatedPosition;
	}
    public void Die() {
        Instantiate(deathExplosion, gameObject.transform.position,
            Quaternion.AngleAxis(-90, Vector3.right));
        Destroy(gameObject);
    }
}
