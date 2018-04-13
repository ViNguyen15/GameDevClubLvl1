using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBugController : MonoBehaviour {

    [SerializeField]
    private Rigidbody2D eBullet;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private GameObject firePoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
      // ShootAtPlayer();
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ShootAtPlayer();
            Debug.Log("Player Targeted");
        }
    }

    private void ShootAtPlayer()
    {
        Rigidbody2D eBulletClone = Instantiate(eBullet, firePoint.transform.position, transform.rotation);
        eBulletClone.velocity = new Vector2(bulletSpeed, 0);
    }
}
