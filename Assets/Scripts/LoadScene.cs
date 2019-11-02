using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(WaitForSeconds(1));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator WaitForSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        PlayerPrefs.SetInt("LevelCleared", SceneManager.GetActiveScene().buildIndex);
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
