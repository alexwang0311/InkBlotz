using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;
    public LayerMask boarderLayer;
    private Vector3 downSpeed;


	// Use this for initialization
	void Start () {
        downSpeed = new Vector3(0f, 10f, 0f);
	}
	
	// Update is called once per frame
	void Update () {

        if (!Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer))
        {
            Debug.Log("No ground detected for " + this.name);
            transform.position -= downSpeed * Time.deltaTime;
        }

        if (Physics2D.OverlapCircle(groundCheck.position, checkRadius, boarderLayer))
        {
            Destroy(this.gameObject);
            Debug.Log("Monster destroyed");
        }
    }

}
