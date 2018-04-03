using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {

    Animator animator;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private Transform[] wallPoints;
    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask isGround;
    [SerializeField]
    private float teleDistance;
    [SerializeField]
    private float dashSpeed;

    private float time;
    private float dashTime = 0.5f;


    private Rigidbody2D myRigidBody;
    private bool isGrounded;
    private bool onWall;
    //button inputs
    private bool jumpButton;
    private bool teleButton;
    private bool dashButton;

    //wall prototype
    [SerializeField]
    private Transform wallCheckPoint;
    [SerializeField]
    private LayerMask wallLayerMask;

    private bool wallSliding;
    private bool wallCheck;


    private bool facingRight;

    // Use this for initialization
    void Start () {

        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        facingRight = true;
		
	}

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        isGrounded = IsGrounded();
        onWall = OnWall();
        Movement(horizontal);

       // Debug.Log(onWall + " On Wall");
       // Debug.Log(isGrounded + " Grounded");
    }


    // Update is called once per frame
    void Update () {

        float horizontal = Input.GetAxis("Horizontal");
        HandleInput();

        //Animation
        float vertVelocity = myRigidBody.velocity.y;
        animator.SetFloat("YVelocity", vertVelocity);
        Flip(horizontal);
    }

    private void LateUpdate()
    {
        animator.SetBool("Dashing", false);

    }

    private void Movement(float horizontal)
    {
        //Horizontal Movement
        myRigidBody.velocity = new Vector2(horizontal * moveSpeed, myRigidBody.velocity.y);
        //Animation for walking
        if (horizontal != 0)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }



        //Jump
        if (isGrounded && jumpButton && !wallSliding)
        {
            myRigidBody.AddForce(new Vector2(0, jumpForce));
            SoundManagerScript.PlaySound("jump");
            jumpButton = false;
        }

        //Teleport
        if (teleButton)
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
        if (Input.GetButtonDown("Jump"))
        {
            jumpButton = true;
        }
        //Teleport
        if (Input.GetButtonDown("Fire2"))
        {
            teleButton = true;
        }
        //Dashing
        if (Input.GetButtonDown("Fire1"))
        {
            dashButton = true;
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
                }
                if (!facingRight)
                {
                    myRigidBody.velocity = new Vector2(-dashSpeed, myRigidBody.velocity.y);
                    time += Time.deltaTime;
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
}
