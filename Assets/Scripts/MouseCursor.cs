using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 offset = new Vector2(1.6f, -1.6f);
        transform.position = cursorPos + offset;
	}

    private void OnEnable()
    {
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;
        Debug.Log("Switched to pen");
        Cursor.visible = false;
    }
}
