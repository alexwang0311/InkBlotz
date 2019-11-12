using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour {
    public float time; 

	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyAfterSeconds(time));
	}
	
	IEnumerator DestroyAfterSeconds(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(this.gameObject);
    }
}
