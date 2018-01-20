using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour {

    public int pointValue;

    public GameObject deathExplosion;
	// Use this for initialization
	void Start () {
        pointValue = 10;
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
        Instantiate(deathExplosion, gameObject.transform.position,
            Quaternion.AngleAxis(-90, Vector3.right));
        GameObject obj = GameObject.Find("GlobalObject");
        Global g = obj.GetComponent<Global>();
        g.score += pointValue;
        Destroy(gameObject);
        //Example();
    }
}
