using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalController : MonoBehaviour
{
    public Transform cameraTransform;
    public float cameraSensitivity;

    int leftFingerId, rightFingerId;
    float halfScreenWidth;
    // Start is called before the first frame update

    //Camera control
    Vector2 lookInput;
    float cameraPitch;

    void Start()
    {
        // id = -1 finger not tracked
        leftFingerId = -1;
        rightFingerId = -1;

        //get halfScreen
        halfScreenWidth = Screen.width / 2;

    }

    // Update is called once per frame
    void Update()
    {
        //TEST
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Debug.Log("Pressed primary button x - " + Input.mousePosition.x);
        //     if (Input.mousePosition.x < halfScreenWidth)
        //     {
        //         Debug.Log("Left Half");
        //     }
        //     else
        //     {
        //         Debug.Log("Right Half");
        //     }
        // }

        //Handles input
        GetTouchInput();

        if (rightFingerId != -1)
        {
            //Look around method handle if rightF is tracked
            LookAround();
        }

    }

    void GetTouchInput()
    {
        //Iterate all touches
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);

            //Cheach each touch`s phase
            switch (t.phase)
            {
                case TouchPhase.Began:
                    if (t.position.x < halfScreenWidth && leftFingerId == -1)
                    {
                        //Start tracking the left finger if it was not previously being traked
                        leftFingerId = t.fingerId;
                        Debug.Log("Tracking Left F");
                    }
                    else if (t.position.x > halfScreenWidth && rightFingerId == -1)
                    {
                        //Start tracking the right finger if it was not previously being traked
                        rightFingerId = t.fingerId;
                        Debug.Log("Tracking Right F");
                    }
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (t.fingerId == leftFingerId)
                    {
                        //Stop tracking Left finger
                        leftFingerId = -1;
                        Debug.Log("Stopped tracking Left finger");
                    }
                    else if (t.fingerId == rightFingerId)
                    {
                        //Stop tracking Right finger
                        rightFingerId = -1;
                        Debug.Log("Stopped tracking Right finger");
                    }
                    break;
                case TouchPhase.Moved:
                    //GET input for LookAround
                    if (t.fingerId == rightFingerId)
                    {
                        lookInput = t.deltaPosition * cameraSensitivity * Time.deltaTime;
                    }
                    break;
                case TouchPhase.Stationary:
                    //SET the look input to zero if the finger is still
                    if (t.fingerId == rightFingerId)
                    {
                        lookInput = Vector2.zero;
                    }
                    break;

            }
        }
    }
    void LookAround()
    {
        //vertical pitch rotation
        cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y, -90f, 90F);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);

        //horizontal (yaw) rotation
        transform.Rotate(transform.up, lookInput.x);

    }

}
