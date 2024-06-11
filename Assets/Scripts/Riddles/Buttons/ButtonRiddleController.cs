using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ButtonRiddleController : InteractableObject
{
    [SerializeField] private ButtonRiddleDoorController door;
    [SerializeField] private Vector3 activationDir = new();
    private bool isActive = false;
    public bool IsActive => isActive;
    public Vector3 ActivationDir => activationDir;

    [ServerRpc(RequireOwnership = false)]
    public void SetTimerServerRPC(float duration)
    {
        StartCoroutine(Timer(duration));
        isActive = true;
    }

    private IEnumerator Timer (float duration)
    {
        while(duration >= 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }
        ResetButton();
        StopCoroutine(nameof(Timer));
    }

    private void ResetButton()
    {
        isActive = false;
        transform.position -= activationDir;
    }
    public override void Interact(ulong playerID = default)
    {
        print("interaction!!");
        door.InteractionServerRPC(ObjectID);
    }
}
