using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour {

    public Control player;

    [SerializeField]
    private Transform target;
    [SerializeField]
    private float smoothSpeed = 0.25f;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Vector3 offsetR;


    // Use this for initialization
    void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        moveCamera();
    }

    private void moveCamera()
    {

        bool facingRight = player.getFacingRight();


        Vector3 desiredposition;

        if (facingRight)
        {
            desiredposition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredposition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        if (!facingRight)
        {
            desiredposition = target.position + offsetR;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredposition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }



    }
}
