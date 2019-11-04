using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkStatus : MonoBehaviour {
    public float startInk;
    public static Ink ink;

	// Use this for initialization
	void Start () {
        ink = new Ink(startInk);
        Debug.Log("Start ink: " + ink.GetInk());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
