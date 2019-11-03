using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Manager : MonoBehaviour {

    //Make this class singleton and undestroyable
    private static Manager _instance;
    public static Manager Instance { get { return _instance; } }

    public GameObject backGround;
    public GameObject creditPic;
    public GameObject cross;
    public GameObject[] levels;

    //buttons[0] holds 'Play'
    //buttons[1] holds 'Levels'
    //buttons[2] holds 'Credits'
    //buttons[3] holds 'Back'
    public GameObject[] buttons = new GameObject[4];

    private void Awake()
    {
        Cursor.visible = true;

    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void ResumeProgress()
    {
        if (PlayerPrefs.GetInt("LevelCleared") == 4)
        {
            SceneManager.LoadScene(1);
        }
        SceneManager.LoadScene(PlayerPrefs.GetInt("LevelCleared") + 1);
    }

    public void ShowLevelCleared()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.LoadScene(0);
        }

        cross.SetActive(false);
        backGround.SetActive(false);
        buttons[0].SetActive(false);
        buttons[1].SetActive(false);
        buttons[2].SetActive(false);
        buttons[3].SetActive(true);

        Debug.Log(PlayerPrefs.GetInt("LevelCleared"));
        for (int i = 0; i <= PlayerPrefs.GetInt("LevelCleared"); i++)
        {
            levels[i].SetActive(true);
        }
    }

    public void goToMenu()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.LoadScene(0);
        }

        cross.SetActive(true);
        creditPic.SetActive(false);
        backGround.SetActive(true);
        buttons[0].SetActive(true);
        buttons[1].SetActive(true);
        buttons[2].SetActive(true);
        buttons[3].SetActive(false);

        foreach(GameObject button in levels)
        {
            button.SetActive(false);
        }
    }

    public void ShowCredits()
    {
        cross.SetActive(false);
        buttons[3].SetActive(true);
        creditPic.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }   
    
}
