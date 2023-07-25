using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public bool useOffsetValues;
    public float rotateSpeed;
    public float heading;
    public float pitch;
    public float playerDistance = 8;

    public float minViewAngle = 45;
    public float maxViewAngle = 315;

    public bool invertY;
    public Transform pivot;

    // Use this for initialization
    void Start()
    {
        if (!useOffsetValues)
        {
            offset = transform.position - target.position;
        }
        pivot.position = target.transform.position;
        pivot.parent = null;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        pivot.position = target.transform.position;
        //Get the x position of the mouse at rotate the target. 
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed; // Mouse X is horizontal.
        pivot.Rotate(0, horizontal, 0);

        //Get Y position of the mouse and rotate the pivot
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;

        //inputs
        heading += Input.GetAxis("Mouse X") * Time.deltaTime * 180;
        pitch -= Input.GetAxis("Mouse Y") * Time.deltaTime * 180;

        if (pitch < -20) pitch = -20;
        else if (pitch > 70) pitch = 70;

        Vector3 newRot = Vector3.zero;

        newRot.y = heading - (float)System.Math.PI;
        newRot.x = pitch;

        gameObject.transform.rotation = Quaternion.Euler(newRot);


        pivot.rotation = transform.rotation;
        //limit the up/down camera rotation


        /*
        //Move the camera based on the current rotation of the target and the original offset.
        float desiredYAngle = pivot.eulerAngles.y;
        float desiredXAngle = pivot.eulerAngles.x;

        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);

        transform.position = target.position - (rotation* offset);
        if (transform.position.y < target.position.y)
        {
            transform.position = new Vector3(transform.position.x, target.position.y-.5f, transform.position.z);

        }
        transform.LookAt(target);
        */
        Vector3 newPos = target.position;

        float headingRadians = heading * ((float)System.Math.PI / 180) - (float)System.Math.PI;
        float pitchRadians = pitch * ((float)System.Math.PI / 180) /*- (float)System.Math.PI*/;

        float newPosX = 0;
        float newPosZ = 0;
        newPosX = (float)System.Math.Sin(headingRadians) * playerDistance;
        newPosZ = (float)System.Math.Cos(headingRadians) * playerDistance;

        newPosX *= (float)System.Math.Cos(pitchRadians);
        newPosZ *= (float)System.Math.Cos(pitchRadians);
        newPos.y += (float)System.Math.Sin(pitchRadians) * playerDistance + 0;
        newPos.x += newPosX;
        newPos.z += newPosZ;
        gameObject.transform.position = newPos;
        pivot.rotation = transform.rotation;
    }
}
