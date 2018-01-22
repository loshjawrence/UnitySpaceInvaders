using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public Vector3 thrust;
    public Quaternion heading;
    public Vector2 widthThreshold;
    public Vector2 heightThreshold;
    public bool fromPlayer;

    public GameObject deathExplosion;
    public AudioClip deathKnell;

    // Use this for initialization
    void Start()
    {
        widthThreshold = new Vector2(0, Camera.main.pixelWidth);
        heightThreshold = new Vector2(0, Camera.main.pixelHeight);

        //travel z axis
        thrust.z = 400.0f;

        //do not passively decelerate
        GetComponent<Rigidbody>().drag = 0;

        //set the dir it will travel
        GetComponent<Rigidbody>().MoveRotation(heading);

        //apply thust once, no need to apply again since it will not decelerate
        GetComponent<Rigidbody>().AddRelativeForce(thrust);
    }


    private void OnCollisionEnter(Collision collision) {
        //the collision contains a lot of info, but it's the colliding
        //object we're most interested in.

        Collider collider = collision.collider;//the thing we collided with
        if (collider.CompareTag("Alien") && fromPlayer) {
            //get handle to alien script and tell it to die
            Alien alien = collider.gameObject.GetComponent<Alien>();
            alien.Die();

            //Destroy the bullet which collided with the alien;
            Destroy(gameObject);
        } else if (collider.CompareTag("Alien") && !fromPlayer) {
            Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider>());

        } else if(collider.CompareTag("Cannon")) {
            //get handle to alien script and tell it to die
            Cannon cannon = collider.gameObject.GetComponent<Cannon>();
            cannon.Die();

            //Destroy the bullet which collided with the alien;
            Destroy(gameObject);
        } else if(collider.CompareTag("Bunker")) {
            //get handle to alien script and tell it to die
            Bunker bunker = collider.gameObject.GetComponent<Bunker>();
            bunker.Die();
            //Destroy the bullet which collided with the alien;
            Destroy(gameObject);
            //Debug.Log("Collided with: " + collider.tag);
        } else if(collider.CompareTag("Bullet")) {
            //get handle to alien script and tell it to die
            Bullet bullet = collider.gameObject.GetComponent<Bullet>();
            bullet.Die();
            //Destroy the bullet which collided with the alien;
            Destroy(gameObject);
            //Debug.Log("Collided with: " + collider.tag);
        }
    }


    public void Die() {
        //play sound at location
        AudioSource.PlayClipAtPoint(deathKnell, gameObject.transform.position);

        Instantiate(deathExplosion, gameObject.transform.position,
            Quaternion.AngleAxis(-90, Vector3.right));

        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update() {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.x < widthThreshold.x ||
            screenPosition.x > widthThreshold.y ||
            screenPosition.y < heightThreshold.x ||
            screenPosition.y > heightThreshold.y)
        {
            Destroy(gameObject);
        }
    }
}
