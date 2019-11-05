using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolController : MonoBehaviour {
    public Transform patrolRay;
    public float patrolRayLength;
    public LayerMask patrolLayer;

    private bool isMovingLeft;
    private Rigidbody2D rb;
    public float patrolSpeed;

    public Transform groundCheck;
    public float groundCheckRayLength;
    public LayerMask groundCheckLayer;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        isMovingLeft = true;
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit2D isGrounded = Physics2D.Raycast(groundCheck.position, -groundCheck.up, groundCheckRayLength, patrolLayer);
        Debug.DrawRay(groundCheck.position, -groundCheck.up * groundCheckRayLength, Color.red);

        if (isGrounded.collider != null) {
            Patrol();
        }
        
    }

    private void Patrol()
    {
        Vector2 pos = transform.position;
        if (isMovingLeft)
        {
            pos.x += -patrolSpeed * Time.deltaTime;
        }
        else
        {
            pos.x += patrolSpeed * Time.deltaTime;
        }
        transform.position = pos;
        RaycastHit2D hit = Physics2D.Raycast(patrolRay.position, -patrolRay.up, patrolRayLength, groundCheckLayer);
        Debug.DrawRay(patrolRay.position, -patrolRay.up * patrolRayLength, Color.red);

        if (isMovingLeft && hit.collider == null)
        {
            Debug.Log("Flip to right");
            Flip();
        }
        else
        {
            if (!isMovingLeft && hit.collider == null)
            {
                Debug.Log("Flip to left");
                Flip();
            }
        }
    }

    private void Flip()
    {
        isMovingLeft = !isMovingLeft;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
