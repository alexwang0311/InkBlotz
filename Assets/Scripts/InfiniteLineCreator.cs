using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteLineCreator : MonoBehaviour {
    public GameObject LinePrefab;
    InfiniteLine activeLine;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject line = Instantiate(LinePrefab);
            line.layer = 13;
            activeLine = line.GetComponent<InfiniteLine>();
        }

        if (activeLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activeLine.UpdateLine(mousePos);

        }

        if (Input.GetMouseButtonUp(0))
        {
            activeLine = null;
        }
    }
}
