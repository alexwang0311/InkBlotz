using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {

    private void Awake()
    {
        Cursor.visible = true;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
