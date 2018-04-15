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

    private GameObject cObject;

    //health
    private float startingHealth = 15f;
    private float currentHealth;

    // Use this for initialization
    void Start () {

        currentHealth = startingHealth;
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    //taking damage
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PBullet")
        {
            cObject = collision.gameObject;
            float dmg = cObject.GetComponent<DamageController>().getDmg();

            if (cObject != null)
            {
                currentHealth -= dmg;

            }

            if (currentHealth <= 0)
            {
                //isDead = true;
                Destroy(gameObject);
            }
        }

    }

    public void ShootAtPlayer()
    {
        Rigidbody2D eBulletClone = Instantiate(eBullet, firePoint.transform.position, transform.rotation);
        eBulletClone.velocity = new Vector2(bulletSpeed, 0);
    }
}
