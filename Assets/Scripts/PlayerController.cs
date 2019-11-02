using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    public int JUMP_NUM;

    private Rigidbody2D rb;
    public float speed;
    public float jumpSpeed;
    public float jumpForce;
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;
    private int jumpTotal;
    private bool isJumping;

    public float rayDistance;

    private Animator myAnimator;

    private bool isFacingRight;

    public Transform respawnPosition;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        jumpTotal = JUMP_NUM;
        isJumping = false;
        isFacingRight = true;
	}
	
	// Update is called once per frame
	void Update () {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        AdjustRotation();

        if (isGrounded) {
            if (!isJumping) {
                if (jumpTotal < JUMP_NUM) {
                    jumpTotal = JUMP_NUM;
                    Debug.Log("Reset jump");
                }
            }
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * jumpSpeed, rb.velocity.y);
        }

        //Animation control
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            myAnimator.SetBool("isWalking", true);
        }
        else
        {
            myAnimator.SetBool("isWalking", false);
        }

        //Flip control
        if (isFacingRight && Input.GetKeyDown(KeyCode.LeftArrow)) {
            Flip();
        }
        if (!isFacingRight && Input.GetKeyDown(KeyCode.RightArrow))
        {
            Flip();
        }

        if (Input.GetKeyDown("space") && jumpTotal > 0)
        {
             myAnimator.SetTrigger("isJumping");

             jumpTotal--;
             Debug.Log("Total jumps: " + jumpTotal);
             //Debug.Log("Whitespace down. Jump with force: " + jumpForce);
             rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKeyDown("space") && jumpTotal == 0)
        {
            Debug.Log("No more jumps");
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void AdjustRotation()
    {
        RaycastHit2D hitBottom = Physics2D.Raycast(transform.position, -transform.up, rayDistance, 1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawRay(transform.position, -transform.up * rayDistance, Color.red);
        if (hitBottom.collider != null) {
            Vector3 surfaceNormal = hitBottom.normal; // Assign the normal of the surface to surfaceNormal
            Vector3 forwardRelativeToSurfaceNormal = Vector3.Cross(transform.right, surfaceNormal);
            Quaternion targetRotation = Quaternion.LookRotation(forwardRelativeToSurfaceNormal, surfaceNormal); //check For target Rotation.
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ink")
        {
            Vector3 bottleSize = collision.gameObject.GetComponent<BoxCollider2D>().bounds.size;
            float bottleArea = bottleSize.x * bottleSize.y;
            float totalInk = bottleArea * 1f;
            Debug.Log("More ink: " + totalInk);
            LineCreator.ink.AddInk(totalInk);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Boarder")
        {
            Debug.Log("Falls out of bottom");
            transform.position = respawnPosition.position;
        }
        
    }
}
