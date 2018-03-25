using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour {

    [SerializeField]
    private Transform target;
    [SerializeField]
    private float smoothSpeed = 0.25f;
    [SerializeField]
    private Vector3 offset;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        Vector3 desiredposition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredposition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

    }
}
