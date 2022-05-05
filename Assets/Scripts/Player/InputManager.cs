using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour {

    [SerializeField] Movement movement;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] PlayerInteractingScript playerInteracting;
    [SerializeField] PlayerWeaponScript playerShooting;

    PlayerControls controls;
    PlayerControls.GroundMovementActions groundMovement;

    Vector2 horizontalInput;
    Vector2 mouseInput;

    private void Awake ()
    {
        controls = new PlayerControls();
        groundMovement = controls.GroundMovement;

        // groundMovement.[action].performed += context => do something
        groundMovement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();

        groundMovement.Jump.started += ctx => movement.OnJumpPressed();

        groundMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        groundMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();

        groundMovement.Run.started += ctx => movement.setIsRunning(true);
        groundMovement.Run.canceled += ctx => movement.setIsRunning(false);
        
        groundMovement.Shoot.started += ctx => playerShooting.isShooting = true;
        groundMovement.Shoot.canceled += ctx => playerShooting.isShooting = false;
        
        groundMovement.Reload.started += ctx => playerShooting.Reload();
        
        groundMovement.Interact.performed += ctx => playerInteracting.InteractWithFirstInteractableObject();
    }

    private void Update ()
    {
        movement.ReceiveInput(horizontalInput);
        mouseLook.ReceiveInput(mouseInput);
    }

    private void OnEnable ()
    {
        controls.Enable();
    }

    private void OnDestroy ()
    {
        controls.Disable();
    }
}