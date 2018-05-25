﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {

    Animator animator;
    public SoundManagerScript sound;

    private GameObject cObject;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private Transform[] wallPoints;
    [SerializeField]
    private LayerMask isGround;
    [SerializeField]
    private float teleDistance;
    [SerializeField]
    private GameObject shieldPoint;
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private Rigidbody2D pBullet;
    [SerializeField]
    private GameObject shield;

    //Charge Shot
    private Vector2 startPoint;
    private const float radius = 1f;
    private bool charging = false;
    private float chargeRate = 1;
    private int chargeCounter = 0;
    private bool chargeComplete = false;


    private float groundRadius = 0.2f;
    private float time;
    private float dashTime = 0.5f;

    //health
    private float startingHealth = 50f;
    private float currentHealth;
    private bool isDead;
    private bool damage;

    private FloatBug enemy;

    private Rigidbody2D myRigidBody;
    private bool isGrounded;
    private bool isFalling;

    //button inputs
    private bool jumpButton;
    private bool teleButton;
    private bool dashButton;

    //wall interaction
    private bool onWall;
    private bool wallCheck;

    private bool facingRight;

    //Power Ups
    private bool dashUp;
    private bool teleUp;

    // Use this for initialization
    void Start() {

        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        facingRight = true;
        dashUp = false;
        teleUp = false;

        //initializing starting health
        currentHealth = startingHealth;

    }




    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertVelocity = myRigidBody.velocity.y;

      
        isGrounded = IsGrounded();
        onWall = OnWall();
        Movement(horizontal, vertVelocity);


        // Debug.Log(teleUp);
        // Debug.Log(isGrounded + " Grounded");
    }


    // Update is called once per frame
    void Update () {

        float horizontal = Input.GetAxis("Horizontal");
        float vertVelocity = myRigidBody.velocity.y;
        HandleInput();

        //Animation
        animator.SetFloat("YVelocity", vertVelocity);
        Flip(horizontal);

        //Debug.Log(currentHealth);
        //Debug.Log(isFalling);

        // falling
        if (myRigidBody.velocity.y < -0.01f == true)
        {
             isFalling = true;
        }
        else
        {
            isFalling = false;
        }

        //charging
        if (charging == true)
        {
            chargeCounter++;
            if(chargeCounter >= 300)
            {
                chargeComplete = true;
            }
        }
    }

    private void LateUpdate()
    {
        animator.SetBool("Dashing", false);

    }

    private void Movement(float horizontal, float vertVelocity)
    {
        //Horizontal Movement
        if (!onWall)
        {
            myRigidBody.velocity = new Vector2(horizontal * moveSpeed, myRigidBody.velocity.y);
        }
        //Animation for walking
        if (horizontal != 0)
        {
            //sound.PlayAudio("walk");
            animator.SetBool("Walking", true);
            
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        //Jump
        if (isGrounded && jumpButton && !onWall)
        {
            myRigidBody.AddForce(new Vector2(0, jumpForce));
            jumpButton = false;
        }

        //Teleport
        if (teleButton && teleUp)
        {
            if (facingRight)
            {
                myRigidBody.AddForce(new Vector2(teleDistance, 0));
                teleButton = false;
            }
            else
            {
                myRigidBody.AddForce(new Vector2(-teleDistance, 0));
                teleButton = false;
            }
        }

        //Dashing
        if (dashButton)
        {

            animator.SetBool("Dashing", true);
            StartCoroutine(Dash());
            
        }


        //WallJump
        if (!isGrounded && jumpButton && onWall && isFalling)
        {
            

            if (facingRight)
            {
                myRigidBody.AddForce(new Vector2(-100, jumpForce+80));
                jumpButton = false;
            }
            if (!facingRight)
            {
                myRigidBody.AddForce(new Vector2(100, jumpForce+80));
                jumpButton = false;
            }
        }
        
        //WallSlide
        if (onWall && isFalling)
        {
            myRigidBody.velocity = new Vector2(0, -5f);

        }
        
    }


    private void HandleInput()
    {

        //Jump
        if (Input.GetButtonDown("Jump") && (isGrounded || onWall && isFalling))
        {
            sound.PlaySound("jump");
            jumpButton = true;

        }
        //Teleport
        if (Input.GetButtonDown("Fire2") && teleUp)
        {
            sound.PlaySound("teleport");
            teleButton = true;
        }
        //Dashing
        if (Input.GetButtonDown("Fire1") && !dashButton && dashUp)
        {
            sound.PlaySound("dash");
            dashButton = true;
        }
        //Shooting
        if (Input.GetButtonDown("Fire3"))
        {
            Shoot();
            charging = true;
        }
        //stop charging
        if (Input.GetButtonUp("Fire3"))
        {
            charging = false;
            chargeCounter = 0;
        }
        //Charge Shot
        if (Input.GetButtonUp("Fire3")&&chargeComplete==true)
        {
            ChargeShot();
            chargeComplete = false;
        }
        //Make Shield
        if (Input.GetButtonDown("Fire4"))
        {
            SpawnShield();
        }
    }

    private void Shoot()
    {
        Rigidbody2D pBulletClone = (Rigidbody2D)Instantiate(pBullet, transform.position, transform.rotation);
        if (facingRight)
        {
            pBulletClone.velocity = new Vector2(dashSpeed, 0);
            if (onWall)
            {
                pBulletClone.velocity = new Vector2(-dashSpeed, 0);
            }
        }
        if (!facingRight)
        {
            pBulletClone.velocity = new Vector2(-dashSpeed, 0);
            if (onWall)
            {
                pBulletClone.velocity = new Vector2(dashSpeed, 0);
            }


        }
    }

    private void ChargeShot()
    {
        startPoint = shieldPoint.transform.position;

        float angleStep = 360f / 20;
        float angle = 0f;

        for (int i = 0; i <= 20 - 1; i++)
        {

            //Direction calculations
            float projectileDirXPosition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileDirYPosition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            Vector2 projectileVector = new Vector2(projectileDirXPosition, projectileDirYPosition);
            Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * dashSpeed;

            Rigidbody2D pBulletClone = Instantiate(pBullet, startPoint, transform.rotation);
            pBulletClone.velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);

            angle += angleStep;

        }
    }

    private void SpawnShield()
    {
        //Vector2 spawnPoint = shieldPoint.transform.position;
        GameObject shieldClone = Instantiate(shield, shieldPoint.transform.position, transform.rotation);
    }


    IEnumerator Dash()
    {
            time = 0;

        while (time < dashTime)
            {
                if (facingRight)
                {
                    myRigidBody.velocity = new Vector2(dashSpeed, myRigidBody.velocity.y);
                    time += Time.deltaTime;
                   // Debug.Log(time);
                }
                if (!facingRight)
                {
                    myRigidBody.velocity = new Vector2(-dashSpeed, myRigidBody.velocity.y);
                    time += Time.deltaTime;
                   // Debug.Log(time);
                }       
                if (time > dashTime)
                {
                    dashButton = false;
                }

            yield return null;


        }
        // animator.SetBool("Dashing", false);

    }


    private void Flip(float horizontal)
    {
        Vector3 scale = transform.localScale;

        if (horizontal < 0 && facingRight)
        {
            scale.x *= -1;
            transform.localScale = scale;
            facingRight = false;
        }
        if (horizontal >0 && !facingRight)
        {
            scale.x *= -1;
            transform.localScale = scale;
            facingRight = true;
        }
    }
    

    private bool IsGrounded()
    {
        if(myRigidBody.velocity.y <= 0)
        {
            foreach(Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, isGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if(colliders[i].gameObject != gameObject)
                    {
                        animator.SetBool("Grounded", true);
                        return true;
                    }
                }
            }
        }
        animator.SetBool("Grounded", false);
        return false;
    }


    private bool OnWall()
    {
        //Wall
        if (!isGrounded)
        {
            foreach(Transform point in wallPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, isGround);

                for(int i = 0; i < colliders.Length; i++)
                {
                    if(colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    //taking damage
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player has health to lose
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EBullet")
        {
            cObject = collision.gameObject;
            float dmg = cObject.GetComponent<DamageController>().getDmg();

            if (cObject != null)
            {
                currentHealth -= dmg;
                // Debug.Log(currentHealth);
            }

            //player jolt back upon taking damage
            if (facingRight)
            {
                myRigidBody.velocity = new Vector2(-80, 10);
            }
            else
            {
                myRigidBody.velocity = (new Vector2(80, 10));
            }
        }

        /* taking damage from bullets
        if (collision.gameObject.tag == "EBullet")
        {
            cObject = collision.gameObject;
            float dmg = cObject.GetComponent<DamageController>().getDmg();

            if (cObject != null)
            {
                currentHealth -= dmg;
                // Debug.Log(currentHealth);

            }

        }
        */

        if (currentHealth <= 0)
        {
            //isDead = true;
            currentHealth = 0;
            Destroy(gameObject);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        cObject = null;
    }


    //health
    public float getHealth()
    {
        return currentHealth;
    }

    public float getStartingHealth()
    {
        return startingHealth;
    }

    public bool getIsGrounded()
    {
        return isGrounded;
    }

    public bool getFacingRight()
    {
        return facingRight;
    }

    public void enableTeleUp()
    {
        teleUp = true;
    }

    public void enableDashUp()
    {
        dashUp = true;
    }
}
