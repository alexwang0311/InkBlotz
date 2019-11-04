using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour {
    public GameObject LinePrefab;
    private Line activeLine;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if (InkStatus.ink.HasInk())
            {
                GameObject line = Instantiate(LinePrefab);
                // Lay 8 is the ground layer
                line.layer = 8;
                activeLine = line.GetComponent<Line>();
            }
            else
            {
                Debug.Log("No more ink");
            }
        }

        if (activeLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (InkStatus.ink.HasInk())
            {
                activeLine.UpdateLine(mousePos, InkStatus.ink);
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
