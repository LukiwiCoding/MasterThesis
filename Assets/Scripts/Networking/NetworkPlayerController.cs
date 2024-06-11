using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Vector2 minMaxRotation;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Camera playerMain;
    [SerializeField] private float interactionDistance;
    [SerializeField] private LayerMask layerMask;

    private CinemachineVirtualCamera virtualCamera;
    private PlayerInputAction playerInput;
    private float cameraAngle;
    private ulong clientID;
    public override void OnNetworkSpawn()
    {
        virtualCamera = cameraTransform.gameObject.GetComponent<CinemachineVirtualCamera>();
        if (IsOwner) virtualCamera.Priority = 1;
        else 
        { 
            virtualCamera.Priority = 0; 
            GetComponent<AudioListener>().enabled = false;
        }
        transform.position = new(-71.4f, 7.5f, 18.28f);
        clientID = GetComponent<NetworkObject>().OwnerClientId;
    }

    private void Start()
    {
        playerInput = new();
        playerInput.Enable();
        playerInput.Player.Interact.started += InteractWithObject;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if(!IsLocalPlayer) return;

        if (playerInput.Player.Movement.inProgress) 
        {
            Move(playerInput.Player.Movement.ReadValue<Vector2>());
        }

        if (playerInput.Player.Look.inProgress)
        {
            LookAround(playerInput.Player.Look.ReadValue<Vector2>());
        }
    }
    private void InteractWithObject(InputAction.CallbackContext context)
    {
        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hitInfo, interactionDistance, layerMask))
        {
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * interactionDistance, Color.red);
            IInteractable interactable = (IInteractable)hitInfo.collider.gameObject.GetComponent(typeof(InteractableObject));       
            interactable?.Interact(clientID);
        }
    }
    private void LookAround(Vector2 input)
    {
        transform.RotateAround(transform.position, transform.up, input.x * rotationSpeed * Time.deltaTime);

        cameraAngle = Vector3.SignedAngle(transform.forward, cameraTransform.forward, cameraTransform.right);
        float camRotationAngle = cameraAngle - input.y * rotationSpeed * Time.deltaTime;
        if (camRotationAngle <= minMaxRotation.x && camRotationAngle >= minMaxRotation.y)
        {
            cameraTransform.RotateAround(cameraTransform.position, cameraTransform.right, input.y * -rotationSpeed * Time.deltaTime);
        }
    }

    private void Move(Vector2 input)
    {
        Vector3 moveDir = input.x * cameraTransform.right + input.y * cameraTransform.forward;
        characterController.Move(speed * Time.deltaTime * moveDir);
        characterController.Move(Physics.gravity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(cameraTransform.position, cameraTransform.position + cameraTransform.forward * interactionDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Ray(cameraTransform.position, cameraTransform.forward * interactionDistance));
    }
}