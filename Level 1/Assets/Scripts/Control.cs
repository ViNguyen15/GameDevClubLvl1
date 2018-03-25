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


    private Rigidbody2D myRigidBody;
    private bool isGrounded;
    private bool jumpButton;

    private bool faceingRight;





    // Use this for initialization
    void Start () {

        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        faceingRight = true;
		
	}

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Movement(horizontal);
        isGrounded = IsGrounded();
        // Debug.Log(IsGrounded());
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


        //Jump
        if(isGrounded && jumpButton)
        {
            myRigidBody.AddForce(new Vector2(0,jumpForce));
            jumpButton = false;
            animator.SetTrigger("Jump");

        }


    }

    public void HandleInput()
    {

        //Jump
        if (Input.GetButtonDown("Jump"))
        {
            jumpButton = true;
        }
    }


    private void Flip(float horizontal)
    {
        Vector3 scale = transform.localScale;

        if (horizontal < 0 && faceingRight)
        {
            scale.x *= -1;
            transform.localScale = scale;
            faceingRight = false;
        }
        if (horizontal >0 && !faceingRight)
        {
            scale.x *= -1;
            transform.localScale = scale;
            faceingRight = true;
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
                        animator.SetTrigger("Grounded");
                        return true;
                    }
                }

            }
        }
        return false;
    }
}
