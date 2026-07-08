using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 8.0f;
    public float backSpeed = 5.0f;
    public float turnSpeed = 180.0f;

    public Transform cameraPoint;

    private CharacterController controller;
    private float verticalVelocity = 0.0f;
    private float gravity = -9.8f;

    private Quaternion defaultCameraRotation;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraPoint == null)
        {
            cameraPoint = transform.Find("CameraPoint");
        }

        if (cameraPoint != null)
        {
            defaultCameraRotation = cameraPoint.localRotation;
        }
    }

    void Update()
    {
        MoveAndTurn();
    }

    void MoveAndTurn()
    {
        float turn = Input.GetAxisRaw("Horizontal");
        float move = Input.GetAxisRaw("Vertical");

        transform.Rotate(Vector3.up * turn * turnSpeed * Time.deltaTime);

        if (cameraPoint != null)
        {
            cameraPoint.localRotation = defaultCameraRotation;
        }

        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -1.0f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        float speed = move >= 0 ? moveSpeed : backSpeed;

        Vector3 velocity = transform.forward * move * speed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }
}