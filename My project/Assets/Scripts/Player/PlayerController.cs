using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public bool canMove;

    [Header("Moving")]
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;

    [Header("Rotation")]
    public float lookDensity;
    public float looklimit;

    [HideInInspector]
    public CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX;
    private bool _isMoving;
    public bool IsMoving
    {
        get { return _isMoving; }
        set
        {
            _isMoving = value;
        }
    }
    public float GetSpeed()
    {
        return walkingSpeed;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float curSpeedZ = 0;

        if (canMove)
        {
            if (isRunning)
            {
                curSpeedZ = runningSpeed;
            }
            else
            {
                curSpeedZ = walkingSpeed;
            }

            curSpeedZ *= Input.GetAxis("Vertical");
        }
        else
        {
            curSpeedZ = 0;
        }

        float curSpeedX = 0;
        if (canMove)
        {
            if (isRunning)
            {
                curSpeedX = runningSpeed;
            }
            else
            {
                curSpeedX = walkingSpeed;
            }

            curSpeedX *= Input.GetAxis("Horizontal");
        }
        else
        {
            curSpeedX = 0;
        }

        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedZ) + (right * curSpeedX);

        if (Input.GetKey(KeyCode.Space) && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpForce;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }
    private void Rotate()
    {
        if (canMove)
        {
            rotationX += Input.GetAxis("Mouse Y") * lookDensity;
            rotationX = Mathf.Clamp(rotationX, -looklimit, looklimit);
            Camera.main.transform.localRotation = Quaternion.Euler(-rotationX, 0, 0);

            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookDensity, 0);
        }
    }
}
