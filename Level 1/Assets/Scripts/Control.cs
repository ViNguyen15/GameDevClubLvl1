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
    private bool jumpButton;
    private bool teleButton;
    private bool dashButton;

    //wall prototype
    [SerializeField]
    private Transform wallCheckPoint;
    [SerializeField]
    private LayerMask wallLayerMask;
    [SerializeField]
    private bool wallSliding;
    [SerializeField]
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
        Movement(horizontal);
        isGrounded = IsGrounded();


        Debug.Log(dashButton);
        Debug.Log(time);
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
            StartCoroutine(Dash());
        }

        //Wall
        if (!isGrounded)
        {
            wallCheck = Physics2D.OverlapCircle(wallCheckPoint.position, 0.2f, wallLayerMask);

            if (facingRight && Input.GetAxis("Horizontal") > -0.5f || !facingRight && Input.GetAxis("Horizontal") < 0.5f)
            {
               

                if (wallCheck)
                {
                    HandleWallSliding();
                }
            }
        }

        if (wallCheck == false || isGrounded)
        {
            wallSliding = false;
        }
    }

    //Wall Sliding and Wall Jumping
    public void HandleWallSliding()
    {
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, -1.5f);

        wallSliding = true;

        if (Input.GetButtonDown("Jump"))
        {
            if (facingRight==true)
            {
                myRigidBody.AddForce(new Vector2(-3, 1.7f)*jumpForce);
            }
            else
            {
                myRigidBody.AddForce(new Vector2(3, 1.7f)*jumpForce);
            }
        }

    }

    public void HandleInput()
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
}
