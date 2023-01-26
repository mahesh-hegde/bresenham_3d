using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int movementSpeed = 10;
    public int rotationAnglePerSecond = 180;

    void Start() {
        
    }

bool isCtrlPressed(KeyCode keyCode)
    {
        return ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                && Input.GetKey(keyCode));
    }
    void Update() {
        // TODO: Handle special keys
        // R = Reset rotation
        // C = Put cursor in front of camera etc..

        // if one of the rotation is pressed, rotate
        // if one of the camera movement is pressed, move camera
        // else, sit down and see the cursor move

        bool moveCamera = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) ;
        bool rotate = !moveCamera && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt));
        if (!rotate && !moveCamera)
        {
            // Nothing to do
            return;
        }
        float horizontal = Input.GetAxis("Horizontal");
        float verticle = Input.GetAxis("Vertical");
        float zDirection = 0;
        if(Input.GetKey(KeyCode.I))
        {
            zDirection = Input.GetKey(KeyCode.LeftShift) ? -1 : 1;
        }
        
        if (moveCamera)
        {
            float factor = Time.deltaTime * movementSpeed;
            transform.position += new Vector3(horizontal * factor, verticle * factor, zDirection * factor);
        }
        if (rotate)
        {
            float factor = Time.deltaTime * rotationAnglePerSecond;
            transform.Rotate(new Vector3(zDirection * factor, horizontal * factor, verticle * factor));
        }
    }
}
