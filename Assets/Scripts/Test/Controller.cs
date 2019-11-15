using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    Rigidbody2D rb;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    //private int direction;

    public Animator cam;

    public float moveSpeed;
    private bool isDashing;
    public int totalDash;
    private int dashLeft;
    public float coolDownTime;
    private float dashCoolDown;

    public float jumpSpeed;
    public int jumpTotal;
    private int jumpLeft;

    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;
    private bool isGrounded;

    public GameObject dashEffect;

    public float rayDistance;

    private bool isFacingRight;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        //direction = 0;
        isDashing = false;
        dashLeft = totalDash;
        isFacingRight = true;
	}

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (isGrounded) {
            jumpLeft = jumpTotal;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpLeft > 1)
        {
            jumpLeft = jumpLeft - 1;
            rb.velocity = Vector2.up * jumpSpeed;
        }


        if (!isDashing) {
            if (isFacingRight && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)))
            {
                Flip();
            }

            if (!isFacingRight && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)))
            {
                Flip();
            }

            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y);

            /*
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                direction = 1;
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                direction = 2;
            }
            */
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashLeft > 0)
        {
            if (!isDashing)
            {
                isDashing = true;
                dashLeft = dashLeft - 1;
                Instantiate(dashEffect, transform.position, Quaternion.identity);
                Debug.Log("Dashing. Dash left: " + dashLeft);
            }
        }

        /*
        if (Input.GetKeyDown(KeyCode.LeftShift) && direction != 0 && dashLeft > 0)
        {
            if (!isDashing)
            {
                isDashing = true;
                dashLeft = dashLeft - 1;
                Debug.Log("Dashing. Dash left: " + dashLeft);
            }
        }
        */

        if (dashLeft <= 0)
        {
            //Debug.Log("No more dash");
            if (dashCoolDown <= 0)
            {
                Debug.Log("Cool down finished");
                dashLeft = totalDash;
                dashCoolDown = coolDownTime;
            }
            else
            {
                //Debug.Log("Cooling down");
                dashCoolDown -= Time.deltaTime;
            }
        }

        if (isDashing && dashTime >= 0)
        {
            cam.SetTrigger("Shake");
            dashTime -= Time.deltaTime;
            if (isFacingRight) {
                rb.velocity = transform.right * dashSpeed;
            }
            else
            {
                rb.velocity = -transform.right * dashSpeed;
            }

            /*
            if (direction == 1)
            {
                rb.velocity = Vector2.left * dashSpeed;
            }
            if (direction == 2)
            {
                rb.velocity = Vector2.right * dashSpeed;
            }
            */
        }
        else
        {
            if (isDashing) {
                isDashing = false;
                Debug.Log("Finished dashing");
            }

            //direction = 0;
            dashTime = startDashTime;
        }

        AdjustRotation();
    }

    private void AdjustRotation()
    {
        RaycastHit2D hitBottom = Physics2D.Raycast(transform.position, -transform.up, rayDistance, groundLayer);
        Debug.DrawRay(transform.position, -transform.up * rayDistance, Color.red);
        if (hitBottom.collider != null)
        {
            Vector3 surfaceNormal = hitBottom.normal; // Assign the normal of the surface to surfaceNormal
            Vector3 forwardRelativeToSurfaceNormal = Vector3.Cross(transform.right, surfaceNormal);
            Quaternion targetRotation = Quaternion.LookRotation(forwardRelativeToSurfaceNormal, surfaceNormal); //check For target Rotation.
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 8f);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector2 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
