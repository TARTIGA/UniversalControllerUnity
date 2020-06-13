using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalController : MonoBehaviour
{

    public Transform cameraTransform;
    public Transform playerBody;
    public Transform groundCheck;

    public LayerMask groundMask;
    public float groundDistance = 0.4f;
    public bool isGrounded;

    float xRotation = 0f;
    public CharacterController characterController;
    public float cameraSensitivity;
    public float moveSpeed;
    public float moveInputDeadZone;


    int leftFingerId, rightFingerId;
    float halfScreenWidth;
    // Start is called before the first frame update

    //Camera control
    Vector2 lookInput;
    float cameraPitch;

    //Player movement control
    Vector2 moveTouchStartPosition;
    Vector2 moveInput;

    Vector3 velocity;

    public float mouseSensetivity = 100f;
    public float gravity = -9.81f;

    public float jumpHeight = 3f;


    void Start()
    {
        //Mouse
        // Cursor.lockState = CursorLockMode.Locked;

        // id = -1 finger not tracked
        leftFingerId = -1;
        rightFingerId = -1;

        //get halfScreen
        halfScreenWidth = Screen.width / 2;

        //calculate the movement input dead zone
        //TODO: Deadzone????
        moveInputDeadZone = Mathf.Pow(Screen.height / moveInputDeadZone, 2);


    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //BTNS Move
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        Vector3 move = cameraTransform.right * x + cameraTransform.forward * z;
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // Mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensetivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensetivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        //

        //Handles input
        GetTouchInput();

        if (rightFingerId != -1)
        {
            //Look around method handle if rightF is tracked
            LookAround();
        }

        if (leftFingerId != -1)
        {
            //Move  handle if leftF is tracked
            Move();
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
                        //Set start position for the movement control finger
                        moveTouchStartPosition = t.position;
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
                    else if (t.fingerId == leftFingerId)
                    {
                        // calculating the position delta from the start position
                        moveInput = t.position - moveTouchStartPosition;
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
    void Move()
    {
        //Don`t move if the touch delta is shorter than the designated dead zone
        //TODO: Deadzone????
        if (moveInput.sqrMagnitude <= moveInputDeadZone) return;

        //Multiply the normalized direction by the speed
        Vector2 movementDirection = moveInput.normalized * moveSpeed * Time.deltaTime;
        //Move relatively to the local transform distance
        //TODO: Why it in method????
        characterController.Move(transform.right * movementDirection.x + transform.forward * movementDirection.y);
    }

}
