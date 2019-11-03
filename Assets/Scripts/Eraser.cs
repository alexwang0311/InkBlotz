using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eraser : MonoBehaviour {
    public float eraserRadius;
    public LayerMask groundLayer;
    private Animator myAnimator;
    private bool isErasing;

    // Use this for initialization
    private void Awake()
    {
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        myAnimator = GetComponent<Animator>();
        isErasing = false;
        myAnimator.SetBool("isErasing", false);
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;
        Debug.Log("Switched to eraser");
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update () {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 offset = new Vector3(2f, 0f);
        transform.position = mousePos + offset;

        if (Input.GetMouseButtonDown(0))
        {
            if (!isErasing) {
                isErasing = true;
                Debug.Log("isErasing");
                myAnimator.SetBool("isErasing", true);
            }
            var erased = Physics2D.OverlapCircleAll(mousePos, eraserRadius, groundLayer);
            foreach (var obj in erased)
            {
                Debug.Log(obj.gameObject.name);
                Destroy(obj.gameObject);
            }
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isErasing) {
                isErasing = false;
                Debug.Log("Stop erasing");
                myAnimator.SetBool("isErasing", false);
            }
        }
	}
}
