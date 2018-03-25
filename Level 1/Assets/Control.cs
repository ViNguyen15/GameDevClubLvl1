using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {

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



	// Use this for initialization
	void Start () {

        myRigidBody = GetComponent<Rigidbody2D>();
		
	}
	
	// Update is called once per frame
	void Update () {
        HandleInput();
		
	}

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Movement(horizontal);
        isGrounded = IsGrounded();
        //Debug.Log(IsGrounded());
    }

    private void Movement(float horizontal)
    {

        myRigidBody.velocity = new Vector2(horizontal * moveSpeed, myRigidBody.velocity.x);

        if(isGrounded && jumpButton)
        {
            myRigidBody.AddForce(new Vector2(0,jumpForce));
            jumpButton = false;
        }


    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpButton = true;
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
                        return true;
                    }
                }

            }
        }
        return false;
    }
}
