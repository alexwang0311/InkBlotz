using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private float speed = 15f;
    private float rayDistance = 0.1f;

    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        GameObject target = GameObject.FindWithTag("Player");
        Vector2 movement = (target.transform.position - transform.position).normalized * speed;
        rb.velocity = movement;
	}

    private void Update()
    {
        RaycastHit2D hitFront= Physics2D.Raycast(transform.position, -transform.right, rayDistance, 1 << LayerMask.NameToLayer("Ground"));
        //Debug.DrawRay(transform.position, -transform.right * rayDistance, Color.red);
        if (hitFront.collider != null)
        {
            //Debug.Log("Hit something");
            Destroy(this.gameObject);
        }

        RaycastHit2D hitFront_1 = Physics2D.Raycast(transform.position, -transform.right, rayDistance, 1 << LayerMask.NameToLayer("Boarder"));
        //Debug.DrawRay(transform.position, -transform.right * rayDistance, Color.red);
        if (hitFront_1.collider != null)
        {
            //Debug.Log("Hit something");
            Destroy(this.gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
