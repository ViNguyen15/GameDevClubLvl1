using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float maxSpeed = 5.0f;
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
    private bool jump;



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
        ResetInput();
        Debug.Log(IsGrounded());
    }

    private void Movement(float horizontal)
    {

        myRigidBody.velocity = new Vector2(horizontal * moveSpeed, myRigidBody.velocity.y);

        if(isGrounded && jump)
        {
           // isGrounded = false;
            myRigidBody.AddForce(new Vector2(0,jumpForce));
        }


    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
    }

    public void ResetInput()
    {
        jump = false;
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
