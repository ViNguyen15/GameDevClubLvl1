using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatBug : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask isGround;

    private Rigidbody2D myRigidBody;


    private bool facingRight;
    private bool isGrounded;




    // Use this for initialization
    void Start () {

        myRigidBody = GetComponent<Rigidbody2D>();
        facingRight = true;

    }

    // Update is called once per frame
    void Update () {

		
	}

    private void FixedUpdate()
    {
        float horizontal = myRigidBody.velocity.x;
        Movement(horizontal);
        isGrounded = IsGrounded(); 
    }




    private void Movement(float horizontal)
    {
        if (facingRight)
        {
            myRigidBody.velocity = new Vector2(moveSpeed, 0);
            if (!isGrounded)
            {
                Flip(horizontal);
            }


        }
        if (!facingRight)
        {
            myRigidBody.velocity = new Vector2(-moveSpeed, 0);
            if (!isGrounded)
            {
                Flip(horizontal);
            }

        }

    }


    private void Flip(float horizontal)
    {
        Vector3 scale = transform.localScale;

        if (horizontal > 0 && facingRight)
        {
            scale.x *= -1;
            transform.localScale = scale;
            facingRight = false;
        }
        if (horizontal < 0 && !facingRight)
        {
            scale.x *= -1;
            transform.localScale = scale;
            facingRight = true;
        }

    }

    private bool IsGrounded()
    {
        if (myRigidBody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, isGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        //animator.SetBool("Grounded", true);
                        return true;
                    }
                }

            }
        }
        //animator.SetBool("Grounded", false);
        return false;
    }
}
