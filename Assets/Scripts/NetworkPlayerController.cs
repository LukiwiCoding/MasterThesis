using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Vector2 minMaxRotation;
    [SerializeField] private Transform cameraTransform;

    private PlayerInputAction playerInput;
    private float cameraAngle;

    public override void OnNetworkSpawn()
    {
        CinemachineVirtualCamera virtualCamera = cameraTransform.gameObject.GetComponent<CinemachineVirtualCamera>();
        if (IsOwner) virtualCamera.Priority = 1;
        else virtualCamera.Priority = 0;
    }
    private void Start()
    {
        playerInput = new();
        playerInput.Enable();

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

}