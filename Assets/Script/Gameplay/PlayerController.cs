using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [Header("Camera")]
    [SerializeField] private Vector2 cameraVelocity = new Vector2(4f, 8f);
    [SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private float maxYAngle = 80f;
    [SerializeField] private Transform cameraTransform = null;
    [SerializeField] private Transform LeftHand = null;
    [SerializeField] private Transform RightHand = null;
    [SerializeField] private CinemachineVirtualCamera virtualCamera = null;

    private Controls controls;

    private Controls Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new Controls();
        }
    }

    private Vector2 currentRotation;
    private Vector2 Look_axis, mouse_scroll;

    [SyncVar]
    public string playerName;

    //add Texting Event here

    public override void OnStartAuthority()
    {
        virtualCamera.gameObject.SetActive(true);

        enabled = true;
        
        Controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        Controls.Player.Hand_forward.performed += ctx => Scrolling(ctx.ReadValue<Vector2>());
        Controls.Player.Hand_forward.canceled += ctx => N_Scrolling();
    }

    [ClientCallback]
    private void OnEnable() => Controls.Enable();
    [ClientCallback]
    private void OnDisbale() => Controls.Disable();
    [ClientCallback]
    private void Update() => PlayerObject_Behavior();

    [Client]
    private void Look(Vector2 lookAxis) => Look_axis = lookAxis;
    [Client]
    private void Scrolling(Vector2 scroll) => mouse_scroll = scroll;
    [Client]
    private void N_Scrolling() => mouse_scroll = Vector2.zero;

    [Client]
    private void PlayerObject_Behavior()
    {
        float deltaTime = Time.deltaTime;
        if (Mouse.current.rightButton.isPressed)
        {
            currentRotation.x += Mouse.current.delta.ReadValue().x * sensitivity;
            currentRotation.y -= Mouse.current.delta.ReadValue().y * sensitivity;
            currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
            currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
            cameraTransform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
        }

        if (Keyboard.current.leftShiftKey.isPressed)
        {
            if (Mouse.current.middleButton.isPressed)
            {
                currentRotation.x += Mouse.current.delta.ReadValue().x * sensitivity;
                currentRotation.y -= Mouse.current.delta.ReadValue().y * sensitivity;
                currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
                currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
                LeftHand.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
            }
            else if (mouse_scroll != Vector2.zero && mouse_scroll.y > 0)
                LeftHand.position += transform.forward * deltaTime * 0.5f;
            else if (mouse_scroll != Vector2.zero && mouse_scroll.y < 0)
                LeftHand.position -= transform.forward * deltaTime * 0.5f;
            else if (!Mouse.current.rightButton.isPressed)
            {
                LeftHand.position += new Vector3(0.0f, Mouse.current.delta.ReadValue().y * deltaTime * sensitivity, 0.0f);
                LeftHand.position += transform.right * Mouse.current.delta.ReadValue().x * deltaTime * sensitivity;
            }
        }

        if (Keyboard.current.spaceKey.isPressed)
        {
            if (Mouse.current.middleButton.isPressed)
            {
                currentRotation.x += Mouse.current.delta.ReadValue().x * sensitivity;
                currentRotation.y -= Mouse.current.delta.ReadValue().y * sensitivity;
                currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
                currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
                RightHand.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
            }
            else if (mouse_scroll != Vector2.zero && mouse_scroll.y > 0)
                RightHand.position += transform.forward * deltaTime * 0.5f;
            else if (mouse_scroll != Vector2.zero && mouse_scroll.y < 0)
                RightHand.position -= transform.forward * deltaTime * 0.5f;
            else if (!Mouse.current.rightButton.isPressed)
            {
                RightHand.position += new Vector3(0.0f, Mouse.current.delta.ReadValue().y * deltaTime * sensitivity, 0.0f);
                RightHand.position += transform.right * Mouse.current.delta.ReadValue().x * deltaTime * sensitivity;
            }
        }
    }

    //Add texting Command and ClientRpc here
}
