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
    [SerializeField]
    private int numberOfBullets;

    private GameObject cObject;

    private Vector2 startPoint;
    private const float radius = 1f;
    private float timer = 3f;

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
    private void OnCollisionEnter2D(Collision2D collision)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            SpawnBullets(numberOfBullets);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            SpawnBullets(numberOfBullets);
            resetTime();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        resetTime();
    }

    private void resetTime()
    {
        timer = 3f;
    }

    private void ShootAtPlayer()
    {
        Rigidbody2D eBulletClone = Instantiate(eBullet, firePoint.transform.position, transform.rotation);

    }


    private void SpawnBullets(int projectiles)
    {

        startPoint = firePoint.transform.position;


        float angleStep = 360f / projectiles;
        float angle = 0f;

        for(int i = 0; i <= projectiles -1; i++)
        {
            //Direction calculations
            float projectileDirXPosition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileDirYPosition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            Vector2 projectileVector = new Vector2(projectileDirXPosition, projectileDirYPosition);
            Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * bulletSpeed;

            Rigidbody2D eBulletClone = Instantiate(eBullet, startPoint, transform.rotation);
            eBulletClone.velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);

            angle += angleStep;

        }
    }
}
