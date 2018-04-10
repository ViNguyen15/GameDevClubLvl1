using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {

    Animator animator;
    public SoundManagerScript sound;
    public Rigidbody2D bullet;

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


    private float groundRadius = 0.2f;
    private float time;
    private float dashTime = 0.5f;

    //health
    private int startingHealth = 100;
    private int currentHealth;
    private bool isDead;
    private bool damage;

    private Rigidbody2D myRigidBody;
    private bool isGrounded;

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

        //initialing starting health
        currentHealth = startingHealth;

    }

    //health ...I feel like I definitely messed up in here
    public int getHealth()
    {
        return this.currentHealth;
    }

    public void getHealth(int health)
    {
        this.currentHealth = health;
    }


    //thought i could do something
    public void TakeDamage()
    {

    }

    //death ...Feel a little off
    public void Death()
    {
        if (currentHealth <= 0)
        {
            isDead = true;
        }
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

    }

    private void LateUpdate()
    {
        animator.SetBool("Dashing", false);

    }

    private void Movement(float horizontal, float vertVelocity)
    {
        //Horizontal Movement
        myRigidBody.velocity = new Vector2(horizontal * moveSpeed, myRigidBody.velocity.y);
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
        if (!isGrounded && jumpButton && onWall)
        {
            if(facingRight)
            {
                myRigidBody.AddForce(new Vector2(-jumpForce*3, jumpForce));
                jumpButton = false;
            }
            if (!facingRight)
            {
                myRigidBody.AddForce(new Vector2(jumpForce*3, jumpForce));
                jumpButton = false;
            }
        }

        //WallSlide
        if (onWall)
        {
            myRigidBody.velocity = new Vector2(0, -3f);

        }
    }

    private void HandleInput()
    {

        //Jump
        if (Input.GetButtonDown("Jump") && (isGrounded || onWall))
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
        Rigidbody2D bulletClone = (Rigidbody2D)Instantiate(bullet, transform.position, transform.rotation);
        if (facingRight)
        {
            bulletClone.velocity = new Vector2(dashSpeed, 0);
            if (onWall)
            {
                bulletClone.velocity = new Vector2(-dashSpeed, 0);
            }
        }
        if (!facingRight)
        {
            bulletClone.velocity = new Vector2(-dashSpeed, 0);
            if (onWall)
            {
                bulletClone.velocity = new Vector2(dashSpeed, 0);
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
