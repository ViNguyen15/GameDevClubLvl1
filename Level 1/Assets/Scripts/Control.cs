using System;
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
    private float dashSpeed;
    [SerializeField]
    private Rigidbody2D pBullet;


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
    private bool wallJumpUp;

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

    //health
    public float getHealth()
    {
        return this.currentHealth;
    }

    public void getHealth(float health)
    {
        this.currentHealth = health;
    }

    //taking damage
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player has health to lose...
        if (collision.gameObject.tag == "Enemy")
        {
            cObject = collision.gameObject;
            float dmg = cObject.GetComponent<DamageController>().getDmg();

            if(cObject != null)
            {
                currentHealth -= dmg;
               // Debug.Log(currentHealth);

            }




            /* player jolt back upon taking damage
            if (facingRight)
            {
                myRigidBody.AddForce(new Vector2(1, 1));
            }
            else
            {
                myRigidBody.AddForce(new Vector2(-1, 1));
            }
            */
        }

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

        if (currentHealth <= 0)
        {
            //isDead = true;
            Destroy(gameObject);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        cObject = null;
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
