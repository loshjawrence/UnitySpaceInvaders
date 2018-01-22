using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour {

    public int pointValue;
    public AudioClip deathKnell;
    public GameObject deathExplosion;
    public float rotation;
    public GameObject bullet;//the GameObject to spawn
	// Use this for initialization
	void Start () {
        pointValue = 10;
        rotation = 180.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
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
        AudioSource.PlayClipAtPoint(deathKnell, gameObject.transform.position);

        Instantiate(deathExplosion, gameObject.transform.position,
            Quaternion.AngleAxis(-90, Vector3.right));

        GameObject obj = GameObject.Find("GlobalObject");
        Global g = obj.GetComponent<Global>();
        g.score += pointValue;
        g.UpdateTotalKilledAliens(0);
        Destroy(gameObject);
        //Example();
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
