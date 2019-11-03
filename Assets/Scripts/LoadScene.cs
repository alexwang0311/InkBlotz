using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(waitForSeconds(1));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator waitForSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("LevelCleared"))
        {
            PlayerPrefs.SetInt("LevelCleared", SceneManager.GetActiveScene().buildIndex);
        }
        Debug.Log("Door opened");
        if (SceneManager.GetActiveScene().name == "Level0")
        {
            SceneManager.LoadScene("Level1");
        }
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            SceneManager.LoadScene("Level2");
        }
        if (SceneManager.GetActiveScene().name == "Level2")
        {
            SceneManager.LoadScene("Level3");
        }
        if (SceneManager.GetActiveScene().name == "Level3")
        {
            SceneManager.LoadScene("LevelClear");
        }
    }
}
