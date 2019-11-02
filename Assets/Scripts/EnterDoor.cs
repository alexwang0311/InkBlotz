using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoor : MonoBehaviour {
    public GameObject openDoor;
    private bool hasCollided;

	// Use this for initialization
	void Start () {
        hasCollided = false;
	}


    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (!hasCollided) {
                hasCollided = true;
                Debug.Log("Collided with player");
                gameObject.SetActive(false);
                openDoor.SetActive(true);
            }
        }
    }
}
