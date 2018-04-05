using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Transform childTransform;
    [SerializeField]
    private Transform transformB;

    private Vector3 posA;
    private Vector3 posB;
    private Vector3 nextPos;

	// Use this for initialization
	void Start () {

        posA = childTransform.localPosition;
        posB = transformB.localPosition;
        nextPos = posB;
		
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    private void Move()
    {
        childTransform.localPosition = Vector3.MoveTowards(childTransform.localPosition, nextPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(childTransform.localPosition,nextPos) <= 0.1)
        {
            ChangeDestination();
        }
    }

    private void ChangeDestination()
    {
        nextPos = nextPos != posA ? posA : posB;
    }


}
