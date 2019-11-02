using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour {
    public static float MAX_INK = 1.5f;
    public GameObject LinePrefab;
    Line activeLine;
    public static Ink ink;
	// Use this for initialization
	void Start () {
        ink = new Ink(MAX_INK);
        Debug.Log("New pen with ink: " + ink.GetInk());
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if (ink.GetInk() > 0) {
                GameObject line = Instantiate(LinePrefab);
                line.layer = 8;
                activeLine = line.GetComponent<Line>();
            }
            else
            {
                Debug.Log("No more ink" +  Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }

        if (activeLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (ink.GetInk() > 0) {
                activeLine.UpdateLine(mousePos, ink);
            }
            else
            {
                Debug.Log("No more ink");
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            activeLine = null;
        }
	}

}
