using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcher : MonoBehaviour {
    public GameObject pen;
    public GameObject lineCreator;
    public GameObject eraser;
    private bool isUsingPen;


	// Use this for initialization
	void Start () {
        isUsingPen = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isUsingPen)
            {
                pen.SetActive(false);
                lineCreator.SetActive(false);
                eraser.SetActive(true);
                isUsingPen = false;
            }
            else
            {
                pen.SetActive(true);
                lineCreator.SetActive(true);
                eraser.SetActive(false);
                isUsingPen = true;
            }
        }
	}
}
