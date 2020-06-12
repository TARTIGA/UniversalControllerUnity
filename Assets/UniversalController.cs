using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalController : MonoBehaviour
{

    int leftFingerId, rightFingerId;
    float halfScreenWidth;
    // Start is called before the first frame update
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

            }
        }

    }
}
