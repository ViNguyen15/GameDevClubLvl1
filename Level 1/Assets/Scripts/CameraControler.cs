using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour {

    private GameObject player;
    private Control playerScript;
    private Transform target;

    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Vector3 offsetR;

    private float smoothSpeed = 2f;



    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = (Control)player.GetComponent(typeof(Control));
        target = player.transform;


    }

    // Update is called once per frame
    void Update () {

        moveCamera();

    }


    private void moveCamera()
    {

        bool facingRight = playerScript.getFacingRight();

        


        Vector3 desiredposition;

        if (facingRight)
        {
            desiredposition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredposition, smoothSpeed * Time.deltaTime);
          //  transform.SetPositionAndRotation(target.position + offset, Quaternion.identity);
            transform.position = smoothedPosition;
        }
        if (!facingRight)
        {
            desiredposition = target.position + offsetR;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredposition, smoothSpeed * Time.deltaTime);
           // transform.SetPositionAndRotation(target.position + offsetR, Quaternion.identity);
            transform.position = smoothedPosition;
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x,target.position.x + -10 ,target.localPosition.x + 10),Mathf.Clamp(transform.position.y, target.position.y -2, target.position.y + 2), -10);


    }
}
