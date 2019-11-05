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

    public Transform groundRay;
    public float groundRayLength;
    public LayerMask groundRayLayer;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;

    public float fallingSpeed;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        isMovingLeft = true;
    }
	
	// Update is called once per frame
	void Update () {
        AdjustRotation();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (isGrounded) {
            Patrol();
        }
        
        else
        {
            Vector3 pos = transform.position;
            pos.y -= fallingSpeed * Time.deltaTime;
            transform.position = pos;
        }
        

        AdjustRotation();
        
    }

    private void Patrol()
    {
        RaycastHit2D hit = Physics2D.Raycast(patrolRay.position, -patrolRay.up, patrolRayLength, patrolLayer);
        Debug.DrawRay(patrolRay.position, -patrolRay.up * patrolRayLength, Color.red);

        if (hit.collider != null)
        {

            Vector3 tangent = Vector3.Cross(hit.normal, Vector3.forward);

            if (System.Math.Abs(tangent.magnitude) < 0.000003)
            {
                tangent = Vector3.Cross(hit.normal, Vector3.up);
            }

            Vector3 pos = transform.position;
            if (isMovingLeft)
            {
                pos -= patrolSpeed * Time.deltaTime * tangent.normalized;
            }
            else
            {
                pos += patrolSpeed * Time.deltaTime * tangent.normalized;
            }
            transform.position = pos;

        }

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

    private void AdjustRotation()
    {
        RaycastHit2D hitBottom = Physics2D.Raycast(groundRay.position, -groundRay.up, groundRayLength, groundRayLayer);
        Debug.DrawRay(groundRay.position, -groundRay.up * groundRayLength, Color.red);
        if (hitBottom.collider != null)
        {
            Vector3 surfaceNormal = hitBottom.normal; // Assign the normal of the surface to surfaceNormal
            Vector3 forwardRelativeToSurfaceNormal = Vector3.Cross(transform.right, surfaceNormal);
            Quaternion targetRotation = Quaternion.LookRotation(forwardRelativeToSurfaceNormal, surfaceNormal); //check For target Rotation.
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime);
        }
    }
}
