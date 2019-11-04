using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullets : MonoBehaviour {
    public GameObject bullet;
    public GameObject monsterNormal;
    public GameObject monsterAttack;
    public Transform bulletStart;

    public float fireRate;
    private float nextFire;
    private float fireTime;
    private float mouthOpenTime;

	// Use this for initialization
	void Start () {
        nextFire = Time.time;
        mouthOpenTime = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextFire)
        {
            //Debug.Log("Bullet fired");
            monsterNormal.SetActive(false);
            monsterAttack.SetActive(true);
            fireTime = Time.time;
            Instantiate(bullet, bulletStart.position, Quaternion.identity);
            nextFire = Time.time + fireRate;
        }
        else
        {
            if (Time.time - fireTime > mouthOpenTime) {
                monsterNormal.SetActive(true);
                monsterAttack.SetActive(false);
            }
        }
	}
}
