using UnityEngine;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour {
#pragma warning disable 649

    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 11f;
    [SerializeField] float runMultiplier = 1.3f;
    private bool isRunning;
    Vector2 horizontalInput;

    [SerializeField] float jumpHeight = 3.5f;
    bool jump;

    [SerializeField] float gravity = -30f; // -9.81
    Vector3 verticalVelocity = Vector3.zero;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;

    private void Update ()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundMask);
        if (isGrounded) {
            verticalVelocity.y = 0;
        }

        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * speed;
        horizontalVelocity = isRunning ? horizontalVelocity * runMultiplier : horizontalVelocity;
        controller.Move(horizontalVelocity * Time.deltaTime);

        // Jump: v = sqrt(-2 * jumpHeight * gravity)
        if (jump) {
            if (isGrounded)
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            jump = false;
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }
    
    public void setIsRunning(bool isRunning)
    {
        this.isRunning = isRunning;
    }
    
    public void ReceiveInput (Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }

    public void OnJumpPressed ()
    {
        jump = true;
    }

}