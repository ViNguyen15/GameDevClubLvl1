using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {

    public float moveForce = 365.0f;
    public float maxSpeed = 5.0f;
    public float jumpForce = 1000.0f;


    private Rigidbody2D player;


	// Use this for initialization
	void Start () {

        player = GetComponent<Rigidbody2D>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
