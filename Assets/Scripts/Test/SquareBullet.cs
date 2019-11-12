using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBullet : MonoBehaviour {
    public GameObject splatParticles;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collided = collision.gameObject;
        if (collided.layer == 8)
        {
            if (collided.tag != "Bouncy") {
                Instantiate(splatParticles, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
        
    }
}
