using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour {
    public float coolDownTime;
    private float shootingCoolDown;
    public GameObject bullet;
    public Transform bulletStartPos;
    public float bulletSpeed;

	// Use this for initialization
	void Start () {
        shootingCoolDown = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (shootingCoolDown <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                shootingCoolDown = coolDownTime;
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dir = (mousePos - bulletStartPos.position).normalized;
                GameObject newBullet = Instantiate(bullet, bulletStartPos.position, Quaternion.identity);
                newBullet.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;
            }
        }
        else
        {
            shootingCoolDown -= Time.deltaTime;
            Debug.Log("Shooting cooling down");
        }
	}
}
