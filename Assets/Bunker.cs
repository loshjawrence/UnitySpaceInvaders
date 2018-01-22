using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunker : MonoBehaviour {

    public int health;
    public GameObject deathExplosion;
    public AudioClip deathKnell;

	// Use this for initialization
	void Start () {
        health = 8;
	}
	
	// Update is called once per frame
	void Update () {
		
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

        if (--health <= 0) {
            Destroy(gameObject);
        }
    }
}
