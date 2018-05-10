using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorberAI : MonoBehaviour {


    [SerializeField]
    private Rigidbody2D eBullet;
    [SerializeField]
    private float bulletSpeed;

    private int numBullets = 5;

    private Vector2 startPoint;
    private const float radius = 1f;
    float timer = 3f;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(numBullets);
		if(numBullets > 5)
        {
            Explode();
        }
	}

    private void Explode()
    {
        timer -= Time.deltaTime;
        Debug.Log(timer);
        if(timer < 0)
        {
            gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
            SpawnBullets(numBullets);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "PBullet" || collision.gameObject.tag == "EBullet")
        {
            numBullets++;
        }
    }

    private void SpawnBullets(int projectiles)
    {
        startPoint = transform.position;

        float angleStep = 360f / projectiles;
        float angle = 0f;

        for (int i = 0; i <= projectiles - 1; i++)
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
