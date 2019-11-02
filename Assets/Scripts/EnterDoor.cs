using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoor : MonoBehaviour {
    public GameObject openDoor;

	// Use this for initialization
	void Start () {

	}
    

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Collided with player");
            gameObject.SetActive(false);
            openDoor.SetActive(true);
            StartCoroutine(waitForSeconds(2));
        }
    }

    

    IEnumerator waitForSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Door opened");
        if (SceneManager.GetActiveScene().name == "Level0")
        {
            SceneManager.LoadScene("Level1");
        }
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            SceneManager.LoadScene("Level2");
        }
    }

}
