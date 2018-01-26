using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public AudioClip sound;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        //the collision contains a lot of info, but it's the colliding
        //object we're most interested in.

        Collider collider = collision.collider;//the thing we collided with
        if (collider.CompareTag("Cannon")) {
            AudioSource.PlayClipAtPoint(sound, gameObject.transform.position);
            Cannon cannon = collider.gameObject.GetComponent<Cannon>();
            cannon.powerUpTime = 5;
            Destroy(gameObject);
        }
    }
}
